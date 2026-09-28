using UnityEngine;

/// <summary>
/// The look of the royal core: shield ring while locked, thrust beam while rising,
/// spark burst on impact. It owns no rules - ShaftRunDirector drives it.
///
/// Everything is sized through SpriteRenderer.size (draw mode Sliced) and the transform
/// scale is left at one: two competing scaling mechanisms is how a sphere ends up an egg.
/// The base sizes come from the camera, so the "breathe" and "burst" animations are
/// written as multiples of that base rather than as literals.
/// </summary>
public sealed class CoreRiser : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private SpriteRenderer _shieldRing;
    [SerializeField] private SpriteRenderer _thrustBeam;
    [SerializeField] private SpriteRenderer _sparks;

    private float _coreBase = 1f;
    private float _shieldBase = 1f;
    private float _beamBase = 1f;
    private float _pulse;
    private float _flashTimer;
    private float _flashSpan = 0.32f;
    private Color _flashColor = ShaftPalette.AccentRoyal;
    private bool _shieldUp;
    private bool _thrustOn;
    private float _sparkTimer;

    /// <summary>Sizes every part off the camera-derived core diameter. Called once, on build.</summary>
    public void Configure(float coreDiameter, float shieldDiameter, float beamHeight)
    {
        this._coreBase = coreDiameter;
        this._shieldBase = shieldDiameter;
        this._beamBase = beamHeight;

        Square(this._body, this._coreBase);
        Square(this._shieldRing, this._shieldBase);
        Square(this._sparks, this._coreBase * 0.9f);

        if (this._sparks != null)
        {
            this._sparks.color = ShaftPalette.WithAlpha(ShaftPalette.AccentRoyal, 0f);
        }

        if (this._thrustBeam != null)
        {
            this._thrustBeam.size = new Vector2(this._coreBase * 0.55f, this._beamBase);
            this._thrustBeam.transform.localPosition = new Vector3(0f, -((this._beamBase * 0.5f) + (coreDiameter * 0.35f)), 0f);
            this._thrustBeam.color = ShaftPalette.WithAlpha(ShaftPalette.AccentPrimary, 0f);
        }

        this.SetShield(true);
        this.SetThrust(false);
    }

    public void SetShield(bool up)
    {
        this._shieldUp = up;
        if (this._shieldRing != null)
        {
            this._shieldRing.enabled = up;
        }
    }

    public void SetThrust(bool on)
    {
        this._thrustOn = on;
    }

    /// <summary>Red flash plus a spark burst; the camera shake lives on the rig.</summary>
    public void FlashHit()
    {
        this._flashColor = ShaftPalette.Danger;
        this._flashSpan = 0.32f;
        this._flashTimer = this._flashSpan;
        this._sparkTimer = 0.40f;
    }

    /// <summary>Gold flash when the core snaps into a socket.</summary>
    public void FlashLock()
    {
        this._flashColor = ShaftPalette.AccentRoyal;
        this._flashSpan = 0.22f;
        this._flashTimer = this._flashSpan;
        this._sparkTimer = 0.24f;
    }

    /// <summary>Ticked by the director, so pausing the run also freezes the animation.</summary>
    public void Animate(float deltaTime, bool invulnerableBlink)
    {
        this._pulse += deltaTime;

        if (this._shieldRing != null && this._shieldUp)
        {
            float wave = 0.5f + (0.5f * Mathf.Sin(this._pulse * 3.4f));
            this._shieldRing.color = ShaftPalette.WithAlpha(ShaftPalette.AccentPrimary, 0.5f + (0.35f * wave));
            Square(this._shieldRing, this._shieldBase * (1f + (0.04f * wave)));
        }

        if (this._thrustBeam != null)
        {
            float target = this._thrustOn ? 0.85f : 0f;
            float alpha = Mathf.MoveTowards(this._thrustBeam.color.a, target, deltaTime * 4.5f);
            this._thrustBeam.color = ShaftPalette.WithAlpha(ShaftPalette.AccentPrimary, alpha);
            float jitter = 1f + (0.06f * Mathf.Sin(this._pulse * 18f));
            this._thrustBeam.size = new Vector2(this._coreBase * 0.55f, this._beamBase * jitter);
        }

        if (this._body != null)
        {
            Color face = ShaftPalette.TextPrimary;
            if (this._flashTimer > 0f)
            {
                this._flashTimer = Mathf.Max(0f, this._flashTimer - deltaTime);
                face = Color.Lerp(face, this._flashColor, Mathf.Clamp01(this._flashTimer / this._flashSpan));
            }

            if (invulnerableBlink)
            {
                face = ShaftPalette.WithAlpha(face, 0.45f + (0.55f * Mathf.Abs(Mathf.Sin(this._pulse * 12f))));
            }

            this._body.color = face;
            Square(this._body, this._coreBase * (1f + (0.03f * Mathf.Sin(this._pulse * 2.2f))));
        }

        if (this._sparks == null)
        {
            return;
        }

        if (this._sparkTimer > 0f)
        {
            this._sparkTimer = Mathf.Max(0f, this._sparkTimer - deltaTime);
            float life = Mathf.Clamp01(this._sparkTimer / 0.40f);
            this._sparks.color = ShaftPalette.WithAlpha(this._flashColor, life);
            Square(this._sparks, this._coreBase * (0.9f + (1.3f * (1f - life))));
        }
        else if (this._sparks.color.a > 0f)
        {
            this._sparks.color = ShaftPalette.WithAlpha(this._flashColor, 0f);
        }
    }

    /// <summary>Slow fade used when the run is lost.</summary>
    public void Dim(float amount)
    {
        if (this._body != null)
        {
            this._body.color = Color.Lerp(this._body.color, ShaftPalette.Dormant, Mathf.Clamp01(amount));
        }
    }

    private static void Square(SpriteRenderer renderer, float side)
    {
        if (renderer != null)
        {
            renderer.size = new Vector2(side, side);
        }
    }
}
