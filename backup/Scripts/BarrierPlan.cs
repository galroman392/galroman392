/// <summary>
/// One hazard on one section, as the generator decided it. Plain data - the scene
/// object that plays it back is BarrierSweeper.
/// </summary>
public sealed class BarrierPlan
{
    public const int KindSweep = 0;
    public const int KindRotor = 1;
    public const int KindGate = 2;

    /// <summary>KindSweep / KindRotor / KindGate.</summary>
    public int Kind;

    /// <summary>Height inside its section, as a fraction of the section height (0..1).</summary>
    public float HeightFraction;

    /// <summary>Travel speed in world units per second.</summary>
    public float Speed;

    /// <summary>Where in its ping-pong cycle it starts (0..1).</summary>
    public float Phase;

    /// <summary>+1 starts moving right, -1 starts moving left.</summary>
    public int Direction;

    /// <summary>Hazard width as a fraction of the shaft width.</summary>
    public float WidthFraction;

    /// <summary>Seconds of contiguous clear lane this hazard leaves the climber.</summary>
    public float ClearWindowSeconds;
}
