using TMPro;
using UnityEngine;







public static class v1dfe305bda8d
{
    public const float v79290b42e948 = 34f;

    
    public static void v6e45b7f0f5c2(TMP_Text vc37086856ab0, float vea10132375f8, float vc1435287eb7c)
    {
        if (vc37086856ab0 == null)
        {
            return;
        }

        float floor = Mathf.Max(v79290b42e948, vea10132375f8);
        vc37086856ab0.textWrappingMode = TextWrappingModes.NoWrap;
        vc37086856ab0.overflowMode = TextOverflowModes.Overflow;
        vc37086856ab0.enableAutoSizing = true;
        vc37086856ab0.fontSizeMin = floor;
        vc37086856ab0.fontSizeMax = Mathf.Max(floor, vc1435287eb7c);
    }

    private static readonly string v004ecea70f71 = v3fe07621f8cc.v6eac007cdb4d(0);

    
    
    
    
    
    
    
    public static void v713aa217777c(TMP_Text vc37086856ab0, Color v6afc3d41b866)
    {
        if (vc37086856ab0 == null)
        {
            return;
        }

        vc37086856ab0.color = v6afc3d41b866;
        Material material = vc37086856ab0.fontMaterial;
        if (material == null)
        {
            return;
        }

        float luminance = (0.299f * v6afc3d41b866.r) + (0.587f * v6afc3d41b866.g) + (0.114f * v6afc3d41b866.b);
        material.EnableKeyword(v004ecea70f71);
        material.SetColor(ShaderUtilities.ID_OutlineColor, luminance < 0.5f ? v5e702f77a0c6.v5bccfad8f382 : v5e702f77a0c6.vb0b8416fd431);
        material.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.22f);
        material.SetFloat(ShaderUtilities.ID_FaceDilate, 0.24f);
        vc37086856ab0.UpdateMeshPadding();
    }

    
    public static void v77f473265d32(TMP_Text vc37086856ab0, string value, Color v6afc3d41b866, float vea10132375f8, float vc1435287eb7c)
    {
        if (vc37086856ab0 == null)
        {
            return;
        }

        v6e45b7f0f5c2(vc37086856ab0, vea10132375f8, vc1435287eb7c);
        v713aa217777c(vc37086856ab0, v6afc3d41b866);
        vc37086856ab0.text = value;
    }
}

internal static class v3fe07621f8cc
{
    private const byte va692bafe4e75 = 0xEA;

    private const string v39e0adddc5e0 = "A2000000007756bae49044982e29ce7099b552";

    internal static string v6eac007cdb4d(int id)
    {
        switch (id)
        {
            case 0: return v52e2d03783a0(v39e0adddc5e0);
            default: return string.Empty;
        }
    }

    private static string v52e2d03783a0(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)va692bafe4e75 * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
        for (int i = 0; i < count; i++)
        {
            state ^= state << 13; state ^= state >> 17; state ^= state << 5;
            byte mask = (byte)(state ^ (state >> 8) ^ (state >> 16) ^ (state >> 24));
            int shift = (int)((state + (uint)i + salt) & 7u);
            byte mixed = System.Convert.ToByte(hex.Substring(18 + (count - 1 - i) * 2, 2), 16);
            byte rotated = shift == 0 ? mixed : (byte)((mixed >> shift) | (mixed << (8 - shift)));
            bytes[i] = (byte)(rotated ^ mask ^ unchecked((byte)(i * 29 + embeddedId * 17)));
        }

        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}
