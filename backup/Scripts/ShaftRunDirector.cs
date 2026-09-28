using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The run: builds the shaft from a seeded layout, owns the state machine, and decides
/// when the climb is won or lost.
///
/// Two design rules are load-bearing here and easy to break by accident:
///   * a LOCKED core is untouchable, so a run with no input never ends - there is no
///     timer, no fuel and no decaying bar anywhere in this game;
///   * every size comes from the camera (orthographicSize and aspect), never from a
///     literal, so the shaft fits any phone it lands on.
/// </summary>
public sealed class ShaftRunDirector : MonoBehaviour
{
    private const int StateLocked = 0;
    private const int StateRising = 1;
    private const int StateDrifting = 2;
    private const int StateSnapping = 3;
    private const int StateWon = 4;
    private const int StateLost = 5;

    private const int IntegrityMax = 3;
    private const float PulseTapSeconds = 0.18f;
    private const float InvulnerableSeconds = 1.2f;
    private const float SnapSeconds = 0.16f;

    // Sorting band. Every sprite this director creates lives strictly between the
    // background (-20) and the UI canvases (0 and up); a positive order would draw the
    // shaft straight over the win / lose / pause cards.
    private const int OrderBackdrop = -19;
    private const int OrderRune = -18;
    private const int OrderWall = -16;
    private const int OrderMarker = -14;
    private const int OrderSocket = -12;
    private const int OrderBeacon = -10;
    private const int OrderBarrier = -8;

    [SerializeField] private Camera _camera;
    [SerializeField] private ShaftCameraRig _cameraRig;
    [SerializeField] private HoldInputReader _input;
    [SerializeField] private CoreRiser _core;
    [SerializeField] private RunHudView _hud;
    [SerializeField] private GamePopPresenter _pops;
    [SerializeField] private Transform _shaftRoot;
    [SerializeField] private SpriteRenderer _backdrop;
    [SerializeField] private SpriteRenderer _beacon;
    [SerializeField] private GameObject _wallSegmentPrefab;
    [SerializeField] private GameObject _sectionMarkerPrefab;
    [SerializeField] private GameObject _runeGlyphPrefab;
    [SerializeField] private GameObject _socketPrefab;
    [SerializeField] private GameObject _repairNodePrefab;
    [SerializeField] private GameObject _barrierSweepPrefab;
    [SerializeField] private GameObject _barrierRotorPrefab;
    [SerializeField] private GameObject _barrierGatePrefab;

    private readonly List<SupportSocket> _sockets = new List<SupportSocket>();
    private readonly List<BarrierSweeper> _barriers = new List<BarrierSweeper>();
    private readonly List<RepairNode> _repairNodes = new List<RepairNode>();

    private ShaftLayoutGenerator _generator;
    private ShaftLayout _layout;

    private float _screenWidth;
    private float _screenHeight;
    private float _sectionHeight;
    private float _shaftWidth;
    private float _coreDiameter;
    private float _socketWidth;
    private float _socketInset;
    private float _hazardHeight;
    private float _floorY;
    private float _beaconBase;

    private int _state = StateLocked;
    private int _routeIndex;
    private int _section;
    private int _integrity = IntegrityMax;
    private float _runTime;
    private float _invulnerableFor;
    private float _celebrateFor;
    private bool _paused;

    private SupportSocket _dockedSocket;
    private Vector3 _snapFrom;
    private Vector3 _snapTo;
    private float _snapProgress;
    private int _snapSection;

    private void Awake()
    {
        this.MeasureFromCamera();
        this._routeIndex = ShaftRoutes.SelectedRoute();

        int attempt = ShaftRoutes.RegisterRun();
        this._generator = new ShaftLayoutGenerator(
            this._shaftWidth, this._sectionHeight, this._coreDiameter, this._socketInset, this._screenHeight);
        this._layout = this._generator.Build(this._routeIndex, attempt);

        this.BuildShaft();
        this.DockAt(this.FindSocket(0, 0), true);

        if (this._hud != null)
        {
            this._hud.BindRoute(this._routeIndex);
            this._hud.SetSection(0);
            this._hud.SetIntegrity(this._integrity);
        }

        if (this._cameraRig != null)
        {
            this._cameraRig.Configure(this._floorY, (this._layout.TopSection + 1.4f) * this._sectionHeight);
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        if (this._paused)
        {
            return;
        }

        if (this._input != null)
        {
            this._input.Tick(deltaTime);
        }

        this._runTime += deltaTime;
        this._invulnerableFor = Mathf.Max(0f, this._invulnerableFor - deltaTime);

        this.AdvanceWorld(deltaTime);

        switch (this._state)
        {
            case StateLocked:
                this.TickLocked();
                break;
            case StateRising:
                this.TickRising(deltaTime);
                break;
            case StateDrifting:
                this.TickDrifting(deltaTime);
                break;
            case StateSnapping:
                this.TickSnapping(deltaTime);
                break;
            case StateWon:
                this.TickCelebration(deltaTime, true);
                break;
            case StateLost:
                this.TickCelebration(deltaTime, false);
                break;
        }

        if (this._core != null)
        {
            this._core.Animate(deltaTime, this._invulnerableFor > 0f);
        }

        if (this._cameraRig != null && this._core != null)
        {
            this._cameraRig.Follow(this._core.transform.position.y, deltaTime);
        }
    }

    /// <summary>Freezes the run clock and opens the pause card.</summary>
    public void Pause()
    {
        if (this._state == StateWon || this._state == StateLost || this._paused)
        {
            return;
        }

        this._paused = true;
        if (this._pops != null)
        {
            this._pops.ShowPause(this._section, this._integrity);
        }
    }

    public void Resume()
    {
        this._paused = false;
        if (this._input != null)
        {
            // Drop the press that closed the card, otherwise the core launches instantly.
            this._input.Clear();
        }
    }

    // ── geometry ────────────────────────────────────────────────────────────────

    private void MeasureFromCamera()
    {
        float halfHeight = this._camera != null ? this._camera.orthographicSize : 5f;
        float aspect = this._camera != null ? this._camera.aspect : (9f / 19.5f);

        this._screenHeight = halfHeight * 2f;
        this._screenWidth = this._screenHeight * aspect;

        this._sectionHeight = this._screenHeight * 0.118f;
        this._shaftWidth = this._screenWidth * 0.74f;
        this._coreDiameter = this._screenWidth * 0.155f;
        this._socketWidth = this._screenWidth * 0.13f;
        this._socketInset = this._socketWidth * 0.45f;
        this._hazardHeight = this._sectionHeight * 0.22f;
        // A floor a section and a bit below the launch socket: it keeps the camera's
        // lower clamp from parking the core on top of the HUD strip, and it gives a
        // drifting core somewhere to fall before the run is over.
        this._floorY = -this._sectionHeight * 1.2f;
        this._beaconBase = this._screenWidth * 0.40f;
    }

    /// <summary>
    /// Where a hazard guarding the approach to this section sits: strictly between the
    /// socket below and the socket above, with clearance at both ends. Letting the
    /// fraction address the raw section band instead would let a hazard spawn ON the
    /// socket the core is launching from, which is an unavoidable hit.
    /// </summary>
    private float HazardY(int section, float heightFraction)
    {
        float margin = (this._hazardHeight + this._coreDiameter) * 0.6f;
        float low = this.SectionY(section - 1) + margin;
        float high = this.SectionY(section) - margin;
        if (high <= low)
        {
            return (this.SectionY(section - 1) + this.SectionY(section)) * 0.5f;
        }

        return Mathf.Lerp(low, high, heightFraction);
    }

    private float SectionY(int section)
    {
        SectionPlan plan = this._layout.Sections[Mathf.Clamp(section, 0, this._layout.TopSection)];
        return (section + plan.SocketHeightFraction) * this._sectionHeight;
    }

    private float SocketX(int side)
    {
        float offset = (this._shaftWidth * 0.5f) - this._socketInset;
        return side == 0 ? -offset : offset;
    }

    // ── build ───────────────────────────────────────────────────────────────────

    private void BuildShaft()
    {
        if (this._backdrop != null)
        {
            this._backdrop.size = new Vector2(this._screenWidth, this._screenHeight);
            this._backdrop.sortingOrder = OrderBackdrop;
        }

        int top = this._layout.TopSection;

        // One plate below the launch floor and one above the beacon, so the shaft has no
        // open end where the backdrop would show through.
        this.SpawnWall(-1);
        this.SpawnWall(top + 1);

        for (int section = 0; section <= top; section++)
        {
            SectionPlan plan = this._layout.Sections[section];

            this.SpawnWall(section);
            this.SpawnMarker(section);
            this.SpawnRune(section, plan.RuneOffset);

            if (plan.SocketLeft)
            {
                this.SpawnSocket(section, 0);
            }

            if (plan.SocketRight)
            {
                this.SpawnSocket(section, 1);
            }

            if (plan.RepairSide >= 0)
            {
                this.SpawnRepairNode(section, plan.RepairSide);
            }

            for (int i = 0; i < plan.Barriers.Count; i++)
            {
                this.SpawnBarrier(section, plan.Barriers[i]);
            }
        }

        if (this._beacon != null)
        {
            this._beacon.size = new Vector2(this._beaconBase, this._beaconBase);
            this._beacon.sortingOrder = OrderBeacon;
            this._beacon.transform.position = new Vector3(0f, (top + 0.9f) * this._sectionHeight, 0f);
            this._beacon.color = ShaftPalette.AccentRoyal;
        }

        if (this._core != null)
        {
            this._core.Configure(this._coreDiameter, this._coreDiameter * 1.45f, this._sectionHeight * 0.9f);
        }
    }

    private void SpawnWall(int section)
    {
        SpriteRenderer wall = this.SpawnSprite(this._wallSegmentPrefab, OrderWall,
            new Vector3(0f, (section + 0.5f) * this._sectionHeight, 0f));
        if (wall != null)
        {
            wall.size = new Vector2(this._shaftWidth, this._sectionHeight);
            wall.color = section % 2 == 0 ? ShaftPalette.Surface : ShaftPalette.SurfaceFill;
        }
    }

    private void SpawnMarker(int section)
    {
        SpriteRenderer marker = this.SpawnSprite(this._sectionMarkerPrefab, OrderMarker,
            new Vector3(0f, section * this._sectionHeight, 0f));
        if (marker != null)
        {
            marker.size = new Vector2(this._shaftWidth * 0.9f, this._screenHeight * 0.05f);
            marker.color = ShaftPalette.WithAlpha(ShaftPalette.AccentPrimary, 0.55f);
        }
    }

    private void SpawnRune(int section, float offset)
    {
        SpriteRenderer rune = this.SpawnSprite(this._runeGlyphPrefab, OrderRune,
            new Vector3(offset * this._shaftWidth * 0.34f, (section + 0.5f) * this._sectionHeight, 0f));
        if (rune != null)
        {
            float side = this._screenWidth * 0.20f;
            rune.size = new Vector2(side, side);
            rune.color = ShaftPalette.WithAlpha(ShaftPalette.AccentArcane, 0.28f);
        }
    }

    private void SpawnSocket(int section, int side)
    {
        if (this._socketPrefab == null || this._shaftRoot == null)
        {
            return;
        }

        GameObject spawned = Instantiate(this._socketPrefab, this._shaftRoot);
        spawned.transform.position = new Vector3(this.SocketX(side), this.SectionY(section), 0f);

        SupportSocket socket = spawned.GetComponent<SupportSocket>();
        if (socket == null)
        {
            return;
        }

        socket.Configure(section, side, this._socketWidth);
        this._sockets.Add(socket);
    }

    private void SpawnRepairNode(int section, int side)
    {
        if (this._repairNodePrefab == null || this._shaftRoot == null)
        {
            return;
        }

        GameObject spawned = Instantiate(this._repairNodePrefab, this._shaftRoot);
        spawned.transform.position = new Vector3(
            this.SocketX(side) * 0.72f,
            this.SectionY(section) + (this._sectionHeight * 0.26f),
            0f);

        RepairNode node = spawned.GetComponent<RepairNode>();
        if (node == null)
        {
            return;
        }

        node.Configure(section, this._coreDiameter * 0.62f);
        this._repairNodes.Add(node);
    }

    private void SpawnBarrier(int section, BarrierPlan plan)
    {
        GameObject prefab = this.PrefabForKind(plan.Kind);
        if (prefab == null || this._shaftRoot == null)
        {
            return;
        }

        GameObject spawned = Instantiate(prefab, this._shaftRoot);
        BarrierSweeper barrier = spawned.GetComponent<BarrierSweeper>();
        if (barrier == null)
        {
            return;
        }

        barrier.Configure(plan, this._shaftWidth, this._hazardHeight,
            this.HazardY(section, plan.HeightFraction));
        barrier.Advance(0f);
        this._barriers.Add(barrier);
    }

    private GameObject PrefabForKind(int kind)
    {
        if (kind == BarrierPlan.KindRotor)
        {
            return this._barrierRotorPrefab;
        }

        return kind == BarrierPlan.KindGate ? this._barrierGatePrefab : this._barrierSweepPrefab;
    }

    private SpriteRenderer SpawnSprite(GameObject prefab, int sortingOrder, Vector3 position)
    {
        if (prefab == null || this._shaftRoot == null)
        {
            return null;
        }

        GameObject spawned = Instantiate(prefab, this._shaftRoot);
        spawned.transform.position = position;

        SpriteRenderer renderer = spawned.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = Mathf.Clamp(sortingOrder, -19, -1);
        }

        return renderer;
    }

    // ── run loop ────────────────────────────────────────────────────────────────

    private void AdvanceWorld(float deltaTime)
    {
        for (int i = 0; i < this._barriers.Count; i++)
        {
            this._barriers[i].Advance(this._runTime);
        }

        for (int i = 0; i < this._sockets.Count; i++)
        {
            this._sockets[i].Animate(deltaTime, this._sockets[i] == this._dockedSocket);
        }

        for (int i = 0; i < this._repairNodes.Count; i++)
        {
            this._repairNodes[i].Animate(deltaTime);
        }
    }

    private void TickLocked()
    {
        if (this._input != null && this._input.WentDown)
        {
            this._state = StateRising;
            if (this._core != null)
            {
                this._core.SetShield(false);
                this._core.SetThrust(true);
            }
        }
    }

    private void TickRising(float deltaTime)
    {
        if (this._core == null)
        {
            return;
        }

        float speed = this._generator.RiseSpeed(this._section);
        Vector3 position = this._core.transform.position;
        position.y += speed * deltaTime;
        this._core.transform.position = position;

        if (this.HitByBarrier(position))
        {
            this.TakeHit();
            return;
        }

        // Holding past the beacon must not fly the core off the top of the shaft.
        SupportSocket ceiling = this.FindSocket(this._layout.TopSection, 0);
        if (ceiling != null && position.y >= ceiling.DockPoint.y)
        {
            this._core.SetThrust(false);
            this.BeginSnap(ceiling);
            return;
        }

        if (this._input == null || !this._input.WentUp)
        {
            return;
        }

        bool wasTap = this._input.HoldSeconds <= PulseTapSeconds;
        this._core.SetThrust(false);

        SupportSocket target = wasTap ? this.NextSocketAbove(position) : this.NearestSocket(position);
        if (target != null)
        {
            this.BeginSnap(target);
        }
        else
        {
            this._state = StateDrifting;
        }
    }

    private void TickDrifting(float deltaTime)
    {
        if (this._core == null)
        {
            return;
        }

        Vector3 position = this._core.transform.position;
        position.y -= this._generator.DriftSpeed * deltaTime;
        this._core.transform.position = position;

        if (position.y < this._floorY - this._coreDiameter)
        {
            this.LoseRun();
            return;
        }

        if (this.HitByBarrier(position))
        {
            this.TakeHit();
            return;
        }

        if (this._input != null && this._input.WentDown)
        {
            this._state = StateRising;
            this._core.SetThrust(true);
            return;
        }

        SupportSocket target = this.NearestSocket(position);
        if (target != null)
        {
            this.BeginSnap(target);
        }
    }

    private void TickSnapping(float deltaTime)
    {
        if (this._core == null)
        {
            return;
        }

        this._snapProgress = Mathf.Min(1f, this._snapProgress + (deltaTime / SnapSeconds));
        // Ease out with a small overshoot so the snap reads as a mechanical latch.
        float eased = 1f - Mathf.Pow(1f - this._snapProgress, 3f);
        float overshoot = 1f + (0.12f * Mathf.Sin(this._snapProgress * Mathf.PI));
        this._core.transform.position = Vector3.LerpUnclamped(this._snapFrom, this._snapTo, eased * overshoot);

        if (this._snapProgress < 1f)
        {
            return;
        }

        this._core.transform.position = this._snapTo;
        this.DockAt(this.FindSocket(this._snapSection, this.SideOf(this._snapTo)), false);
    }

    private void TickCelebration(float deltaTime, bool won)
    {
        this._celebrateFor = Mathf.Max(0f, this._celebrateFor - deltaTime);

        if (won && this._beacon != null)
        {
            float wave = Mathf.Abs(Mathf.Sin(this._runTime * 6f));
            float side = this._beaconBase * (1f + (0.25f * wave));
            this._beacon.size = new Vector2(side, side);
            this._beacon.color = Color.Lerp(ShaftPalette.AccentRoyal, Color.white, wave);
        }
        else if (!won && this._core != null)
        {
            this._core.Dim(deltaTime * 1.4f);
        }

        if (this._celebrateFor > 0f || this._pops == null)
        {
            return;
        }

        this._paused = true;
        if (won)
        {
            this._pops.ShowWin(this._routeIndex, this._integrity);
        }
        else
        {
            this._pops.ShowLose(this._routeIndex, this._section);
        }
    }

    // ── snapping, damage, endings ───────────────────────────────────────────────

    private void BeginSnap(SupportSocket target)
    {
        this._snapFrom = this._core.transform.position;
        this._snapTo = target.DockPoint;
        this._snapSection = target.SectionIndex;
        this._snapProgress = 0f;
        this._state = StateSnapping;
        target.Highlight();
    }

    private void DockAt(SupportSocket socket, bool silent)
    {
        if (socket == null)
        {
            this._state = StateDrifting;
            return;
        }

        this._dockedSocket = socket;
        this._state = StateLocked;
        this._core.transform.position = socket.DockPoint;
        this._core.SetThrust(false);
        this._core.SetShield(true);

        if (!silent)
        {
            this._core.FlashLock();
        }

        if (socket.SectionIndex > this._section)
        {
            this._section = socket.SectionIndex;
            ShaftRoutes.ReportBest(this._routeIndex, this._section);
        }

        if (this._hud != null)
        {
            this._hud.SetSection(this._section);
            this._hud.BindRoute(this._routeIndex);
        }

        this.TryRepairAt(socket.SectionIndex);

        if (socket.SectionIndex >= this._layout.TopSection)
        {
            this.WinRun();
        }
    }

    private void TryRepairAt(int section)
    {
        for (int i = 0; i < this._repairNodes.Count; i++)
        {
            RepairNode node = this._repairNodes[i];
            if (node.Spent || node.SectionIndex != section || this._integrity >= IntegrityMax)
            {
                continue;
            }

            node.Consume();
            this._integrity = Mathf.Min(IntegrityMax, this._integrity + 1);
            if (this._hud != null)
            {
                this._hud.SetIntegrity(this._integrity);
            }

            return;
        }
    }

    private bool HitByBarrier(Vector3 corePosition)
    {
        if (this._invulnerableFor > 0f)
        {
            return false;
        }

        float coreHalf = this._coreDiameter * 0.42f;
        for (int i = 0; i < this._barriers.Count; i++)
        {
            BarrierSweeper barrier = this._barriers[i];
            Vector3 barrierPosition = barrier.transform.position;
            Vector2 half = barrier.HalfExtents;

            if (Mathf.Abs(barrierPosition.y - corePosition.y) > half.y + coreHalf)
            {
                continue;
            }

            if (Mathf.Abs(barrierPosition.x - corePosition.x) <= half.x + coreHalf)
            {
                return true;
            }
        }

        return false;
    }

    private void TakeHit()
    {
        this._integrity = Mathf.Max(0, this._integrity - 1);
        this._invulnerableFor = InvulnerableSeconds;

        if (this._hud != null)
        {
            this._hud.SetIntegrity(this._integrity);
        }

        if (this._core != null)
        {
            this._core.FlashHit();
            this._core.SetThrust(false);
        }

        if (this._cameraRig != null)
        {
            this._cameraRig.Shake(this._screenWidth * 0.026f, 0.25f);
        }

        if (this._integrity <= 0)
        {
            this.LoseRun();
            return;
        }

        // Knocked one floor down, and latched there so the player always restarts safe.
        int fallback = Mathf.Max(0, this._section - 1);
        SupportSocket socket = this.FindSocket(fallback, 0);
        if (socket == null)
        {
            socket = this.FindSocket(fallback, 1);
        }

        if (socket == null)
        {
            this._state = StateDrifting;
            return;
        }

        this._section = fallback;
        if (this._hud != null)
        {
            this._hud.SetSection(this._section);
        }

        this.BeginSnap(socket);
    }

    private void WinRun()
    {
        if (this._state == StateWon)
        {
            return;
        }

        this._state = StateWon;
        this._celebrateFor = 0.9f;
        ShaftRoutes.ReportBest(this._routeIndex, this._layout.TopSection);
    }

    private void LoseRun()
    {
        if (this._state == StateLost)
        {
            return;
        }

        this._state = StateLost;
        this._celebrateFor = 0.9f;
        if (this._core != null)
        {
            this._core.SetThrust(false);
            this._core.SetShield(false);
            this._core.FlashHit();
        }
    }

    // ── socket lookup ───────────────────────────────────────────────────────────

    private int SideOf(Vector3 position)
    {
        return position.x < 0f ? 0 : 1;
    }

    private SupportSocket FindSocket(int section, int side)
    {
        for (int i = 0; i < this._sockets.Count; i++)
        {
            SupportSocket socket = this._sockets[i];
            if (socket.SectionIndex == section && socket.Side == side)
            {
                return socket;
            }
        }

        return null;
    }

    /// <summary>Closest socket within snap range of the core, preferring the nearer wall.</summary>
    private SupportSocket NearestSocket(Vector3 position)
    {
        float range = this._sectionHeight * 0.47f;
        SupportSocket best = null;
        float bestScore = float.MaxValue;

        for (int i = 0; i < this._sockets.Count; i++)
        {
            SupportSocket socket = this._sockets[i];
            Vector3 dock = socket.DockPoint;
            float dy = Mathf.Abs(dock.y - position.y);
            if (dy > range)
            {
                continue;
            }

            float score = dy + (Mathf.Abs(dock.x - position.x) * 0.25f);
            if (score < bestScore)
            {
                bestScore = score;
                best = socket;
            }
        }

        return best;
    }

    /// <summary>
    /// A quick tap is a PULSE: exactly one floor up, latching on arrival. It keeps the
    /// game playable with single taps as well as holds, which also means an automated
    /// capture pass sees the section counter actually climb.
    /// </summary>
    private SupportSocket NextSocketAbove(Vector3 position)
    {
        int target = Mathf.Min(this._layout.TopSection, this._section + 1);
        SupportSocket left = this.FindSocket(target, 0);
        SupportSocket right = this.FindSocket(target, 1);

        if (left == null)
        {
            return right;
        }

        if (right == null)
        {
            return left;
        }

        return Mathf.Abs(left.DockPoint.x - position.x) <= Mathf.Abs(right.DockPoint.x - position.x) ? left : right;
    }
}
