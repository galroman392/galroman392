using UnityEngine;

/// <summary>
/// Builds a shaft for one attempt. Every position, side, speed, phase and hazard type
/// comes out of a seeded System.Random. The engine-global random generator is avoided on
/// purpose: it shares state with anything else that touches it, so "the same seed" would
/// stop meaning the same run.
///
/// Solvability is CONSTRUCTIVE rather than hopeful: hazard speed is derived from the
/// clear window the section owes the climber, so a generated layout is climbable by
/// construction. IsClimbable re-checks it, and a hand-written fallback closes the
/// last gap.
/// </summary>
public sealed class ShaftLayoutGenerator
{
    private const float EasyWindowSeconds = 1.40f;   // sections 1..3
    private const float NormalWindowSeconds = 0.85f; // sections 4..top
    private const int EasySectionCount = 3;
    private const int RepairSectionLow = 6;
    private const int RepairSectionHigh = 12;
    private const int MaxRegenerationAttempts = 20;

    private readonly float _shaftWidth;
    private readonly float _sectionHeight;
    private readonly float _coreDiameter;
    private readonly float _socketInset;
    private readonly float _screenHeight;

    public ShaftLayoutGenerator(float shaftWidth, float sectionHeight, float coreDiameter, float socketInset, float screenHeight)
    {
        this._shaftWidth = shaftWidth;
        this._sectionHeight = sectionHeight;
        this._coreDiameter = coreDiameter;
        this._socketInset = socketInset;
        this._screenHeight = screenHeight;
    }

    /// <summary>Climb speed for a section, in world units per second, scaled off the camera.</summary>
    public float RiseSpeed(int section)
    {
        float scaled = (0.1300f + (0.0024f * section)) * this._screenHeight;
        return Mathf.Min(0.1700f * this._screenHeight, scaled);
    }

    public float DriftSpeed
    {
        get { return 0.0900f * this._screenHeight; }
    }

    /// <summary>Generate, verify, retry, and only then fall back to the hand-written shaft.</summary>
    public ShaftLayout Build(int routeIndex, int attempt)
    {
        int seed = (routeIndex * 7919) ^ (attempt * 104729);
        System.Random rng = new System.Random(seed);

        ShaftLayout layout = this.Generate(rng, routeIndex, seed);
        int tries = 0;
        while (tries < MaxRegenerationAttempts && !this.IsClimbable(layout))
        {
            layout = this.Generate(rng, routeIndex, seed);
            tries++;
        }

        if (!this.IsClimbable(layout))
        {
            layout = this.Fallback(routeIndex, seed);
        }

        Debug.Log("[shaft] seed=" + seed.ToString() + " route=" + routeIndex.ToString()
            + " attempt=" + attempt.ToString() + " regenerated=" + tries.ToString());
        return layout;
    }

    public ShaftLayout Generate(System.Random rng, int routeIndex, int seed)
    {
        ShaftLayout layout = new ShaftLayout(ShaftRoutes.Sections);
        layout.Seed = seed;
        layout.RouteIndex = routeIndex;

        int[] kinds = ShaftRoutes.BarrierKinds(routeIndex);
        float speedMultiplier = ShaftRoutes.SpeedMultiplier(routeIndex);

        // Section 0 is the launch pad: both sockets, never any hazard.
        SectionPlan ground = layout.Sections[0];
        ground.SocketLeft = true;
        ground.SocketRight = true;
        ground.SocketHeightFraction = 0.5f;

        int top = layout.TopSection;
        for (int section = 1; section <= top; section++)
        {
            SectionPlan plan = layout.Sections[section];

            // Sides: one guaranteed, the second by chance, so the climb zigzags.
            bool left = rng.Next(0, 2) == 0;
            plan.SocketLeft = left;
            plan.SocketRight = !left;
            if (rng.NextDouble() < 0.34d)
            {
                plan.SocketLeft = true;
                plan.SocketRight = true;
            }

            plan.SocketHeightFraction = 0.5f + (float)((rng.NextDouble() - 0.5d) * 0.44d);
            plan.RuneOffset = (float)((rng.NextDouble() * 2d) - 1d);

            if (section == RepairSectionLow || section == RepairSectionHigh)
            {
                plan.RepairSide = plan.SocketLeft ? 0 : 1;
            }

            // The beacon floor stays clear so the finish is always reachable.
            if (section == top)
            {
                plan.SocketLeft = true;
                plan.SocketRight = true;
                continue;
            }

            int hazards = this.HazardCount(section, rng);
            for (int i = 0; i < hazards; i++)
            {
                plan.Barriers.Add(this.BuildHazard(rng, kinds, speedMultiplier, section, i, hazards));
            }
        }

        return layout;
    }

    private int HazardCount(int section, System.Random rng)
    {
        if (section <= EasySectionCount)
        {
            return 1;
        }

        if (section <= 11)
        {
            return rng.Next(0, 2) == 0 ? 1 : 2;
        }

        return 2;
    }

    private BarrierPlan BuildHazard(System.Random rng, int[] kinds, float speedMultiplier, int section, int index, int total)
    {
        BarrierPlan plan = new BarrierPlan();
        plan.Kind = kinds[rng.Next(0, kinds.Length)];
        plan.Direction = rng.Next(0, 2) == 0 ? 1 : -1;
        plan.Phase = (float)rng.NextDouble();

        // Spread hazards across the height of the section so they never stack.
        float slot = (index + 1f) / (total + 1f);
        plan.HeightFraction = Mathf.Clamp(slot + (float)((rng.NextDouble() - 0.5d) * 0.10d), 0.16f, 0.84f);

        plan.WidthFraction = plan.Kind == BarrierPlan.KindRotor ? 0.26f
            : (plan.Kind == BarrierPlan.KindGate ? 0.34f : 0.58f);

        // CONSTRUCTIVE PART: derive the speed from the window this section owes the
        // climber, instead of rolling a speed and hoping the window survives it.
        float minSpeed = 0.0550f * this._screenHeight * speedMultiplier;
        float hardCap = 0.1350f * this._screenHeight * speedMultiplier;
        float maxSpeed = Mathf.Min(hardCap, this.SpeedCeiling(plan, section));
        if (maxSpeed < minSpeed)
        {
            // Too wide to move slowly enough here - thin it down until it fits. For the
            // rotor this also shrinks its collision band, since it is round.
            plan.WidthFraction *= 0.72f;
            maxSpeed = Mathf.Min(hardCap, this.SpeedCeiling(plan, section));
        }

        maxSpeed = Mathf.Max(minSpeed, maxSpeed);
        plan.Speed = minSpeed + ((maxSpeed - minSpeed) * (float)rng.NextDouble());
        plan.ClearWindowSeconds = this.ClearTravel(plan.WidthFraction) / Mathf.Max(0.0001f, plan.Speed);
        return plan;
    }

    /// <summary>
    /// The fastest this hazard may move and still leave a clear lane for as long as the
    /// core needs to cross it.
    /// </summary>
    private float SpeedCeiling(BarrierPlan plan, int section)
    {
        float need = Mathf.Max(this.RequiredWindow(section), this.TransitSeconds(plan, section) * 1.15f);
        return this.ClearTravel(plan.WidthFraction) / Mathf.Max(0.0001f, need);
    }

    /// <summary>
    /// A bar and a shutter are thin strips; a rotor is round, so its collision band is as
    /// tall as it is wide. Measuring every hazard as a thin strip is how a rotor section
    /// ends up genuinely impassable while the checker still calls it fine.
    /// </summary>
    private float HazardHeight(BarrierPlan plan)
    {
        return plan.Kind == BarrierPlan.KindRotor
            ? this._shaftWidth * plan.WidthFraction
            : this._sectionHeight * 0.22f;
    }

    /// <summary>How long the core spends inside this hazard's band on the way up.</summary>
    private float TransitSeconds(BarrierPlan plan, int section)
    {
        float rise = this.RiseSpeed(Mathf.Max(0, section - 1));
        return (this.HazardHeight(plan) + this._coreDiameter) / Mathf.Max(0.0001f, rise);
    }

    /// <summary>
    /// How far a hazard travels while the socket lane stays clear. Sockets hug the walls,
    /// so the blocked strip sits at one end of the sweep and the clear part is contiguous.
    /// </summary>
    private float ClearTravel(float widthFraction)
    {
        float hazardWidth = this._shaftWidth * widthFraction;
        float travel = Mathf.Max(0.0001f, this._shaftWidth - hazardWidth);
        float laneCentre = (this._shaftWidth * 0.5f) - this._socketInset;
        float blockedHalf = (hazardWidth + this._coreDiameter) * 0.5f;
        float blockedFrom = Mathf.Max(-travel * 0.5f, laneCentre - blockedHalf);
        float blockedTo = Mathf.Min(travel * 0.5f, laneCentre + blockedHalf);
        float blocked = Mathf.Max(0f, blockedTo - blockedFrom);
        return Mathf.Max(0f, travel - blocked);
    }

    private float RequiredWindow(int section)
    {
        return section <= EasySectionCount ? EasyWindowSeconds : NormalWindowSeconds;
    }

    /// <summary>
    /// Simulated check: for every section, is there a contiguous stretch of time in which
    /// the core - rising at that section's speed - crosses every hazard band untouched?
    /// </summary>
    public bool IsClimbable(ShaftLayout layout)
    {
        if (layout == null)
        {
            return false;
        }

        int top = layout.TopSection;
        for (int section = 1; section <= top; section++)
        {
            SectionPlan plan = layout.Sections[section];
            if (!plan.SocketLeft && !plan.SocketRight)
            {
                return false;
            }

            float required = this.RequiredWindow(section);

            for (int i = 0; i < plan.Barriers.Count; i++)
            {
                BarrierPlan hazard = plan.Barriers[i];
                float window = this.ClearTravel(hazard.WidthFraction) / Mathf.Max(0.0001f, hazard.Speed);
                if (window < required || window < this.TransitSeconds(hazard, section))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// The only hand-written shaft in the project, and it is a safety net rather than the
    /// game: one slow sweep per section, sockets alternating sides.
    /// </summary>
    public ShaftLayout Fallback(int routeIndex, int seed)
    {
        ShaftLayout layout = new ShaftLayout(ShaftRoutes.Sections);
        layout.Seed = seed;
        layout.RouteIndex = routeIndex;

        SectionPlan ground = layout.Sections[0];
        ground.SocketLeft = true;
        ground.SocketRight = true;
        ground.SocketHeightFraction = 0.5f;

        int top = layout.TopSection;
        for (int section = 1; section <= top; section++)
        {
            SectionPlan plan = layout.Sections[section];
            plan.SocketLeft = (section % 2) == 1;
            plan.SocketRight = !plan.SocketLeft;
            plan.SocketHeightFraction = 0.5f;
            plan.RuneOffset = ((section % 5) * 0.4f) - 0.8f;
            if (section == RepairSectionLow || section == RepairSectionHigh)
            {
                plan.RepairSide = plan.SocketLeft ? 0 : 1;
            }

            if (section == top)
            {
                plan.SocketLeft = true;
                plan.SocketRight = true;
                continue;
            }

            BarrierPlan hazard = new BarrierPlan();
            hazard.Kind = BarrierPlan.KindSweep;
            hazard.HeightFraction = 0.5f;
            hazard.WidthFraction = 0.44f;
            hazard.Direction = (section % 2) == 0 ? 1 : -1;
            hazard.Phase = (section % 7) / 7f;
            hazard.Speed = 0.0600f * this._screenHeight;
            hazard.ClearWindowSeconds = this.ClearTravel(hazard.WidthFraction) / hazard.Speed;
            plan.Barriers.Add(hazard);
        }

        return layout;
    }
}
