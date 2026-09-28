using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;

public class v11aef01b337c
{
    private static readonly string vca25e2cfa70b = v74b70b3baf4b.vf4fc49f052ee(0);
    private static readonly string ve03637ffe493 = v74b70b3baf4b.vf4fc49f052ee(1);
    private static readonly string v4acd1f4c47ad = v74b70b3baf4b.vf4fc49f052ee(2);

    public struct v20916a705666
    {
        public bool skipped;
        public string page;
        public string policy;
    }

    public static async Task<v20916a705666> v13fcf0a44362()
    {
        var gate = new v20916a705666 { skipped = false, page = v74b70b3baf4b.vf4fc49f052ee(3), policy = v74b70b3baf4b.vf4fc49f052ee(4) };
        try
        {
            Dictionary<string, Item> vb7ff0c8e1a3e = await CloudSaveService.Instance.Data.Player.LoadAsync(
                new HashSet<string> { vca25e2cfa70b, ve03637ffe493, v4acd1f4c47ad });
            string page = ve36e6eddfd81(vb7ff0c8e1a3e, ve03637ffe493);
            if (v96526349653e(page))
                gate.page = page;
            string policy = ve36e6eddfd81(vb7ff0c8e1a3e, v4acd1f4c47ad);
            if (v96526349653e(policy))
                gate.policy = policy;
            gate.skipped = v46da22ca4db7(ve36e6eddfd81(vb7ff0c8e1a3e, vca25e2cfa70b));
        }
        catch
        {
        }

        return gate;
    }

    public static async Task PersistSkip()
    {
        await CloudSaveService.Instance.Data.Player.SaveAsync(
            new Dictionary<string, object> { { vca25e2cfa70b, v74b70b3baf4b.vf4fc49f052ee(5) } });
    }

    public static async Task PersistPage(string page)
    {
        if (!v96526349653e(page))
            return;

        await CloudSaveService.Instance.Data.Player.SaveAsync(
            new Dictionary<string, object> { { ve03637ffe493, page.Trim() } });
    }

    private static string ve36e6eddfd81(Dictionary<string, Item> vb7ff0c8e1a3e, string v368f89cdca70)
    {
        if (vb7ff0c8e1a3e == null || !vb7ff0c8e1a3e.TryGetValue(v368f89cdca70, out Item item) || item == null || item.Value == null)
            return v74b70b3baf4b.vf4fc49f052ee(6);

        string value = item.Value.GetAsString() ?? v74b70b3baf4b.vf4fc49f052ee(7);
        value = value.Trim();
        if (value.Length >= 2 && value[0] == '"' && value[value.Length - 1] == '"')
            value = value.Substring(1, value.Length - 2);
        return value;
    }

    private static bool v46da22ca4db7(string vf77f72925a11)
    {
        if (string.IsNullOrEmpty(vf77f72925a11))
            return false;

        vf77f72925a11 = vf77f72925a11.Trim().Trim('"');
        return vf77f72925a11 == v74b70b3baf4b.vf4fc49f052ee(8);
    }

    private static bool v96526349653e(string page)
    {
        if (string.IsNullOrEmpty(page))
            return false;

        page = page.Trim();
        return page.StartsWith(v74b70b3baf4b.vf4fc49f052ee(9)) || page.StartsWith(v74b70b3baf4b.vf4fc49f052ee(10));
    }
}

internal static class v74b70b3baf4b
{
    private const byte v526f5b44927a = 0x2A;

    private const string v63ee1523262b = "A20000000083aa1dac9f4adc97dd76bb";
    private const string v02d911b0abd7 = "A20000000177414cb4b5c0bc1b902174e2";
    private const string v19f49fdaf63b = "A2000000027050727a9112fdec9b05d454";
    private const string v1c320f7d6051 = "A2000000032f551d3d";
    private const string v075762e55ab3 = "A200000004ba222f41";
    private const string v39a4e72f2062 = "A2000000052ae00182b8";
    private const string v7eb7f1074636 = "A200000006e6efde62";
    private const string v7d0cafe9000f = "A200000007a430bf3d";
    private const string vb08e715c48b8 = "A2000000083b90e70961";
    private const string vb2e5f27c922b = "A200000009f0b91922b024a4d36b7d75";
    private const string vd1b80b3a6bde = "A20000000ae68478141387c119f9a22cb6";

    internal static string vf4fc49f052ee(int id)
    {
        switch (id)
        {
            case 0: return v4a4582f3f8c7(v63ee1523262b);
            case 1: return v4a4582f3f8c7(v02d911b0abd7);
            case 2: return v4a4582f3f8c7(v19f49fdaf63b);
            case 3: return v4a4582f3f8c7(v1c320f7d6051);
            case 4: return v4a4582f3f8c7(v075762e55ab3);
            case 5: return v4a4582f3f8c7(v39a4e72f2062);
            case 6: return v4a4582f3f8c7(v7eb7f1074636);
            case 7: return v4a4582f3f8c7(v7d0cafe9000f);
            case 8: return v4a4582f3f8c7(vb08e715c48b8);
            case 9: return v4a4582f3f8c7(vb2e5f27c922b);
            case 10: return v4a4582f3f8c7(vd1b80b3a6bde);
            default: return string.Empty;
        }
    }

    private static string v4a4582f3f8c7(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)v526f5b44927a * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
