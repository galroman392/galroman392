using System.Collections.Generic;

/// <summary>One floor of the shaft: where the sockets are and what patrols it.</summary>
public sealed class SectionPlan
{
    public SectionPlan()
    {
        this.Barriers = new List<BarrierPlan>();
    }

    /// <summary>A socket exists on the left wall.</summary>
    public bool SocketLeft;

    /// <summary>A socket exists on the right wall.</summary>
    public bool SocketRight;

    /// <summary>Socket height inside the section, as a fraction of section height.</summary>
    public float SocketHeightFraction;

    /// <summary>-1 none, 0 left wall, 1 right wall.</summary>
    public int RepairSide = -1;

    /// <summary>Decorative rune offset across the shaft (-1..1), purely cosmetic.</summary>
    public float RuneOffset;

    public List<BarrierPlan> Barriers;
}
