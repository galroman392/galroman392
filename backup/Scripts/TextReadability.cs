using TMPro;
using UnityEngine;

/// <summary>
/// Rule C.10 in one place: every label this project creates or touches at runtime gets
/// a contrasting outline, hand-made line breaks (never engine word-wrap) and autosize
/// with a readable FLOOR - autosize shrinks to fontSizeMin, so the minimum is the size
/// that actually ships.
/// </summary>
public static class TextReadability
{
    public const float UiFontFloor = 34f;

    /// <summary>Wrapping off, autosize on, floor clamped to the readable minimum.</summary>
    public static void ApplyLayout(TMP_Text label, float minSize, float maxSize)
    {
        if (label == null)
        {
            return;
        }

        float floor = Mathf.Max(UiFontFloor, minSize);
        label.textWrappingMode = TextWrappingModes.NoWrap;
        label.overflowMode = TextOverflowModes.Overflow;
        label.enableAutoSizing = true;
        label.fontSizeMin = floor;
        label.fontSizeMax = Mathf.Max(floor, maxSize);
    }

    private const string OutlineKeyword = "OUTLINE_ON";

    /// <summary>
    /// Instance material (reading fontMaterial clones it, so the shared asset stays
    /// clean) with an outline picked against the face luminance.
    ///
    /// EnableKeyword is the step that is easy to miss: set the outline width without it
    /// and the shader compiles the no-outline variant, so nothing changes on screen.
    /// </summary>
    public static void ApplyOutline(TMP_Text label, Color face)
    {
        if (label == null)
        {
            return;
        }

        label.color = face;
        Material material = label.fontMaterial;
        if (material == null)
        {
            return;
        }

        float luminance = (0.299f * face.r) + (0.587f * face.g) + (0.114f * face.b);
        material.EnableKeyword(OutlineKeyword);
        material.SetColor(ShaderUtilities.ID_OutlineColor, luminance < 0.5f ? ShaftPalette.TextPrimary : ShaftPalette.Deep);
        material.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.22f);
        material.SetFloat(ShaderUtilities.ID_FaceDilate, 0.24f);
        label.UpdateMeshPadding();
    }

    /// <summary>Layout + outline + text in one call, for labels written from code.</summary>
    public static void Dress(TMP_Text label, string value, Color face, float minSize, float maxSize)
    {
        if (label == null)
        {
            return;
        }

        ApplyLayout(label, minSize, maxSize);
        ApplyOutline(label, face);
        label.text = value;
    }
}
