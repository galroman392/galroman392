using UnityEngine;

/// <summary>
/// Single source of truth for every colour used by the generated screens.
/// Plain static class: no scene presence, no state, nothing to serialize.
/// Text tints are all LIGHT on purpose - the shared font material carries a dark
/// outline, so a dark face colour would sink into its own outline.
/// </summary>
public static class ShaftPalette
{
    public static readonly Color Deep = new Color(0.063f, 0.086f, 0.173f, 1f);      // #10162C
    public static readonly Color Shadow = new Color(0.043f, 0.063f, 0.149f, 1f);    // #0B1026
    public static readonly Color Surface = new Color(0.094f, 0.125f, 0.275f, 1f);   // #182046
    public static readonly Color SurfaceFill = new Color(0.063f, 0.094f, 0.220f, 1f); // #101838

    public static readonly Color AccentPrimary = new Color(0.176f, 0.486f, 1f, 1f); // #2D7CFF
    public static readonly Color AccentArcane = new Color(0.545f, 0.271f, 0.827f, 1f); // #8B45D3
    public static readonly Color AccentRoyal = new Color(0.949f, 0.769f, 0.310f, 1f); // #F2C44F
    public static readonly Color Danger = new Color(0.941f, 0.294f, 0.388f, 1f);    // #F04B63
    public static readonly Color Dormant = new Color(0.200f, 0.251f, 0.420f, 1f);   // #33406B

    public static readonly Color TextPrimary = new Color(0.961f, 0.969f, 1f, 1f);   // #F5F7FF
    public static readonly Color TextGold = AccentRoyal;                            // #F2C44F
    public static readonly Color TextBlue = new Color(0.498f, 0.690f, 1f, 1f);      // #7FB0FF
    public static readonly Color TextArcane = new Color(0.725f, 0.549f, 0.941f, 1f); // #B98CF0
    public static readonly Color TextWarn = new Color(1f, 0.502f, 0.584f, 1f);      // #FF8095

    public static Color WithAlpha(Color source, float alpha)
    {
        return new Color(source.r, source.g, source.b, alpha);
    }
}
