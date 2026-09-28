using TMPro;
using UnityEngine;








public sealed class v8e4e0056c0f9 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bestValue;
    [SerializeField] private TextMeshProUGUI _bestLabel;
    [SerializeField] private TextMeshProUGUI _runsValue;
    [SerializeField] private TextMeshProUGUI _runsLabel;
    [SerializeField] private TextMeshProUGUI _emptyHint;

    private void Awake()
    {
        v1dfe305bda8d.v77f473265d32(this._bestLabel, v9f175eab1497.v02b94d5205a7(0), v5e702f77a0c6.vd091459a36d3, 34f, 38f);
        v1dfe305bda8d.v77f473265d32(this._runsLabel, v9f175eab1497.v02b94d5205a7(1), v5e702f77a0c6.vd091459a36d3, 34f, 38f);
        v1dfe305bda8d.v6e45b7f0f5c2(this._bestValue, 62f, 96f);
        v1dfe305bda8d.v6e45b7f0f5c2(this._runsValue, 62f, 96f);
        v1dfe305bda8d.v6e45b7f0f5c2(this._emptyHint, 34f, 38f);
    }

    public void v4da8899525f8(int vdef1460c2f7d)
    {
        int best = vff93c1a0dc29.vcc07433e5d90(vdef1460c2f7d);
        int runs = vff93c1a0dc29.vd7d70315f1fb();

        v1dfe305bda8d.v77f473265d32(this._bestValue,
            best > 0 ? best.ToString() + v9f175eab1497.v02b94d5205a7(2) + vff93c1a0dc29.v0ff36440fbbc.ToString() : v9f175eab1497.v02b94d5205a7(3),
            v5e702f77a0c6.vdd10b8a2dad6, 62f, 96f);

        v1dfe305bda8d.v77f473265d32(this._runsValue, runs.ToString(), v5e702f77a0c6.v5bccfad8f382, 62f, 96f);

        if (this._emptyHint != null)
        {
            bool blank = best <= 0 && runs <= 0;
            v1dfe305bda8d.v77f473265d32(this._emptyHint, blank ? v9f175eab1497.v02b94d5205a7(4) : string.Empty, v5e702f77a0c6.vd091459a36d3, 34f, 38f);
            this._emptyHint.gameObject.SetActive(blank);
        }
    }
}

internal static class v9f175eab1497
{
    private const byte vd03aa4959130 = 0x9E;

    private const string vd9503cc4b8a2 = "A2000000001caecade17d08e56";
    private const string v161c86a125e7 = "A20000000160683c522537f990";
    private const string v7e65acb5af18 = "A2000000023919dc49c8af7d";
    private const string vd1aaa69f12f8 = "A20000000356e80d365740";
    private const string v3b21225ccc10 = "A20000000486a3e5e3ba7df8c228880c11fd16";

    internal static string v02b94d5205a7(int id)
    {
        switch (id)
        {
            case 0: return v53b139871180(vd9503cc4b8a2);
            case 1: return v53b139871180(v161c86a125e7);
            case 2: return v53b139871180(v7e65acb5af18);
            case 3: return v53b139871180(vd1aaa69f12f8);
            case 4: return v53b139871180(v3b21225ccc10);
            default: return string.Empty;
        }
    }

    private static string v53b139871180(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)vd03aa4959130 * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
