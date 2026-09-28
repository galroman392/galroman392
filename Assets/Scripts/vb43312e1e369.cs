using TMPro;
using UnityEngine;
using UnityEngine.UI;






public sealed class vb43312e1e369 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sectionValue;
    [SerializeField] private TextMeshProUGUI _sectionLabel;
    [SerializeField] private TextMeshProUGUI _bestValue;
    [SerializeField] private TextMeshProUGUI _bestLabel;
    [SerializeField] private TextMeshProUGUI _routeLabel;
    [SerializeField] private Image[] _pips;

    private void Awake()
    {
        v1dfe305bda8d.v6e45b7f0f5c2(this._sectionValue, 96f, 148f);
        v1dfe305bda8d.v6e45b7f0f5c2(this._sectionLabel, 34f, 38f);
        v1dfe305bda8d.v6e45b7f0f5c2(this._bestValue, 40f, 56f);
        v1dfe305bda8d.v6e45b7f0f5c2(this._bestLabel, 34f, 38f);
        v1dfe305bda8d.v6e45b7f0f5c2(this._routeLabel, 38f, 46f);

        v1dfe305bda8d.v713aa217777c(this._sectionValue, v5e702f77a0c6.vdd10b8a2dad6);
        v1dfe305bda8d.v713aa217777c(this._sectionLabel, v5e702f77a0c6.vd091459a36d3);
        v1dfe305bda8d.v713aa217777c(this._bestValue, v5e702f77a0c6.vdd10b8a2dad6);
        v1dfe305bda8d.v713aa217777c(this._bestLabel, v5e702f77a0c6.vd091459a36d3);
        v1dfe305bda8d.v713aa217777c(this._routeLabel, v5e702f77a0c6.v737c0b87c3cb);
    }

    public void vf0e82162ab83(int vdef1460c2f7d)
    {
        if (this._routeLabel != null)
        {
            this._routeLabel.text = vff93c1a0dc29.v0055f10370a1(vdef1460c2f7d);
        }

        if (this._sectionLabel != null)
        {
            this._sectionLabel.text = v4140ab4e23a5.v8e648cbcca8a(0);
        }

        if (this._bestLabel != null)
        {
            this._bestLabel.text = v4140ab4e23a5.v8e648cbcca8a(1);
        }

        int best = vff93c1a0dc29.vcc07433e5d90(vdef1460c2f7d);
        if (this._bestValue != null)
        {
            this._bestValue.text = best > 0
                ? ve63ce530ab92(best) + v4140ab4e23a5.v8e648cbcca8a(2) + vff93c1a0dc29.v0ff36440fbbc.ToString()
                : v4140ab4e23a5.v8e648cbcca8a(3);
        }
    }

    public void v794261e20b7d(int v720ad0d3a1a2)
    {
        if (this._sectionValue != null)
        {
            this._sectionValue.text = ve63ce530ab92(v720ad0d3a1a2) + v4140ab4e23a5.v8e648cbcca8a(4) + vff93c1a0dc29.v0ff36440fbbc.ToString();
        }
    }

    public void v6cefef1edf2a(int v8cde3fd658e0)
    {
        if (this._pips == null)
        {
            return;
        }

        for (int i = 0; i < this._pips.Length; i++)
        {
            Image pip = this._pips[i];
            if (pip == null)
            {
                continue;
            }

            bool v24f3a2397e0b = i < v8cde3fd658e0;
            pip.color = v24f3a2397e0b ? v5e702f77a0c6.vc46f649702f0 : v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.ve698a8efbd72, 0.5f);
        }
    }

    public static string ve63ce530ab92(int value)
    {
        return value < 10 ? v4140ab4e23a5.v8e648cbcca8a(5) + value.ToString() : value.ToString();
    }
}

internal static class v4140ab4e23a5
{
    private const byte v1baf5bd034ce = 0x73;

    private const string vc4690d051daf = "A2000000001a7a0946864dd07180be1d";
    private const string v40e448ee3893 = "A20000000103a0fc9342c1484b";
    private const string v03a36770c337 = "A2000000027a292eb482562d";
    private const string ve8e82231fa6d = "A200000003c0121f40a3febc17477bdd4118";
    private const string v6ca5a7c7f573 = "A200000004960a9ee4fdf03a";
    private const string v43a655ebbe27 = "A2000000057465a73257";

    internal static string v8e648cbcca8a(int id)
    {
        switch (id)
        {
            case 0: return vfa6420d20a70(vc4690d051daf);
            case 1: return vfa6420d20a70(v40e448ee3893);
            case 2: return vfa6420d20a70(v03a36770c337);
            case 3: return vfa6420d20a70(ve8e82231fa6d);
            case 4: return vfa6420d20a70(v6ca5a7c7f573);
            case 5: return vfa6420d20a70(v43a655ebbe27);
            default: return string.Empty;
        }
    }

    private static string vfa6420d20a70(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)v1baf5bd034ce * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
