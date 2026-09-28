using UnityEngine;






public static class vff93c1a0dc29
{
    public const int Count = 3;
    public const int v0ff36440fbbc = 18;

    private static readonly string vf05da945df88 = vb2ff55209fc0.v1be0b516f582(0);
    private static readonly string v495eca606e2f = vb2ff55209fc0.v1be0b516f582(1);
    private static readonly string v41985950d727 = vb2ff55209fc0.v1be0b516f582(2);

    
    public static int[] v806f0f5eaf5e(int vdef1460c2f7d)
    {
        switch (Mathf.Clamp(vdef1460c2f7d, 0, Count - 1))
        {
            case 0: return new[] { v4434d66e509d.v6224c3d1d228 };
            case 1: return new[] { v4434d66e509d.v6224c3d1d228, v4434d66e509d.v30559dd3082e };
            default: return new[] { v4434d66e509d.v6224c3d1d228, v4434d66e509d.v30559dd3082e, v4434d66e509d.v2a418c016828 };
        }
    }

    public static float vae459b99a45b(int vdef1460c2f7d)
    {
        switch (Mathf.Clamp(vdef1460c2f7d, 0, Count - 1))
        {
            case 0: return 1.00f;
            case 1: return 1.15f;
            default: return 1.30f;
        }
    }

    public static string vdae8b34c339d(int vdef1460c2f7d)
    {
        switch (Mathf.Clamp(vdef1460c2f7d, 0, Count - 1))
        {
            case 0: return vb2ff55209fc0.v1be0b516f582(3);
            case 1: return vb2ff55209fc0.v1be0b516f582(4);
            default: return vb2ff55209fc0.v1be0b516f582(5);
        }
    }

    public static string vc14d60c25918(int vdef1460c2f7d)
    {
        switch (Mathf.Clamp(vdef1460c2f7d, 0, Count - 1))
        {
            case 0: return vb2ff55209fc0.v1be0b516f582(6);
            case 1: return vb2ff55209fc0.v1be0b516f582(7);
            default: return vb2ff55209fc0.v1be0b516f582(8);
        }
    }

    public static string v0055f10370a1(int vdef1460c2f7d)
    {
        int shown = Mathf.Clamp(vdef1460c2f7d, 0, Count - 1) + 1;
        return vb2ff55209fc0.v1be0b516f582(9) + shown.ToString() + vb2ff55209fc0.v1be0b516f582(10) + vdae8b34c339d(vdef1460c2f7d);
    }

    public static int vd54499223391()
    {
        return Mathf.Clamp(PlayerPrefs.GetInt(v41985950d727, 0), 0, Count - 1);
    }

    public static void v7155f77d776f(int vdef1460c2f7d)
    {
        PlayerPrefs.SetInt(v41985950d727, Mathf.Clamp(vdef1460c2f7d, 0, Count - 1));
        PlayerPrefs.Save();
    }

    public static int vcc07433e5d90(int vdef1460c2f7d)
    {
        return Mathf.Clamp(PlayerPrefs.GetInt(vf05da945df88 + Mathf.Clamp(vdef1460c2f7d, 0, Count - 1).ToString(), 0), 0, v0ff36440fbbc);
    }

    public static void v6fe279d232b4(int vdef1460c2f7d, int v720ad0d3a1a2)
    {
        int clamped = Mathf.Clamp(vdef1460c2f7d, 0, Count - 1);
        if (v720ad0d3a1a2 > vcc07433e5d90(clamped))
        {
            PlayerPrefs.SetInt(vf05da945df88 + clamped.ToString(), Mathf.Clamp(v720ad0d3a1a2, 0, v0ff36440fbbc));
            PlayerPrefs.Save();
        }
    }

    public static int vd7d70315f1fb()
    {
        return Mathf.Max(0, PlayerPrefs.GetInt(v495eca606e2f, 0));
    }

    
    public static int vf5880f526d6d()
    {
        int next = vd7d70315f1fb() + 1;
        PlayerPrefs.SetInt(v495eca606e2f, next);
        PlayerPrefs.Save();
        return next;
    }
}

internal static class vb2ff55209fc0
{
    private const byte v11e87c1c91dd = 0x96;

    private const string vb920c48576f1 = "A200000000cc043ab8db1d800d2ac58b27";
    private const string v0243559f2953 = "A20000000134146048a88dc04632f83e";
    private const string v7196e69ac117 = "A20000000230b9eb4267ca21679d5a6014";
    private const string vcc1dbe0b5004 = "A200000003e996329cf006748e80bd652180";
    private const string v9d0b3dbd1da8 = "A200000004e9678a461962049958f6a769dc";
    private const string vd4bc2093ad28 = "A200000005599680d8b9062ec2edcad9189d64";
    private const string vd1c735260cec = "A200000006e294da4ee86c282323ae595a352ec4317b6cd419fea18a60";
    private const string vad68731afb0b = "A20000000787099ca283485fa89a338edb6016b7fd0e82ccc1357867f0a9ec844154d27d07";
    private const string vc0a1f98d07c9 = "A20000000884d9cca766bf3d581e72a7854f943ddd4818e910c09c64052b98ab6b025b453b5c20f1768293b1";
    private const string vee14ae051a37 = "A200000009f3667ee1bd7dd9154e3f";
    private const string v58760deb4edd = "A20000000a330c805533db7145";

    internal static string v1be0b516f582(int id)
    {
        switch (id)
        {
            case 0: return v540e0ad97b01(vb920c48576f1);
            case 1: return v540e0ad97b01(v0243559f2953);
            case 2: return v540e0ad97b01(v7196e69ac117);
            case 3: return v540e0ad97b01(vcc1dbe0b5004);
            case 4: return v540e0ad97b01(v9d0b3dbd1da8);
            case 5: return v540e0ad97b01(vd4bc2093ad28);
            case 6: return v540e0ad97b01(vd1c735260cec);
            case 7: return v540e0ad97b01(vad68731afb0b);
            case 8: return v540e0ad97b01(vc0a1f98d07c9);
            case 9: return v540e0ad97b01(vee14ae051a37);
            case 10: return v540e0ad97b01(v58760deb4edd);
            default: return string.Empty;
        }
    }

    private static string v540e0ad97b01(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)v11e87c1c91dd * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
