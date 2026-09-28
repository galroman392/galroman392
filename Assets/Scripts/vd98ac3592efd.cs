using TMPro;
using UnityEngine;
using UnityEngine.UI;







public sealed class vd98ac3592efd : MonoBehaviour
{
    [SerializeField] private v60ed5081ca52 _presenter;
    [SerializeField] private v8e4e0056c0f9 _stats;
    [SerializeField] private TextMeshProUGUI _routeChip;
    [SerializeField] private TextMeshProUGUI _objective;
    [SerializeField] private TextMeshProUGUI _emptyLabel;
    [SerializeField] private Image[] _rowPlates;
    [SerializeField] private Image[] _rowFrames;
    [SerializeField] private RectTransform[] _rowRoots;
    [SerializeField] private TextMeshProUGUI[] _rowTitles;
    [SerializeField] private TextMeshProUGUI[] _rowSummaries;
    [SerializeField] private TextMeshProUGUI[] _rowBests;
    
    [SerializeField] private Image[] _rowPips;

    private const int v47d6566930c5 = 3;

    private int vb524c47add51;
    private int v09b29cb99d4c = -1;
    private float vc3c680ba5048;
    private Vector3 v499ed660592d = Vector3.one;

    private void Awake()
    {
        this.v499ed660592d = Vector3.one;

        for (int v0c2eb0976dd9 = 0; v0c2eb0976dd9 < this.v382b4179969d; v0c2eb0976dd9++)
        {
            v1dfe305bda8d.v6e45b7f0f5c2(this.At(this._rowTitles, v0c2eb0976dd9), 46f, 56f);
            v1dfe305bda8d.v6e45b7f0f5c2(this.At(this._rowSummaries, v0c2eb0976dd9), 38f, 46f);
            v1dfe305bda8d.v6e45b7f0f5c2(this.At(this._rowBests, v0c2eb0976dd9), 38f, 46f);
        }

        v1dfe305bda8d.v6e45b7f0f5c2(this._routeChip, 38f, 46f);
        v1dfe305bda8d.v6e45b7f0f5c2(this._objective, 38f, 46f);
        v1dfe305bda8d.v6e45b7f0f5c2(this._emptyLabel, 38f, 46f);
    }

    private void Start()
    {
        this.v250e953b26b5(vff93c1a0dc29.vd54499223391());
    }

    private void Update()
    {
        if (this.vc3c680ba5048 <= 0f)
        {
            return;
        }

        this.vc3c680ba5048 = Mathf.Max(0f, this.vc3c680ba5048 - Time.unscaledDeltaTime);
        RectTransform v0c2eb0976dd9 = this.At(this._rowRoots, this.v09b29cb99d4c);
        if (v0c2eb0976dd9 == null)
        {
            return;
        }

        
        float life = this.vc3c680ba5048 / 0.30f;
        v0c2eb0976dd9.localScale = this.v499ed660592d * (1f + (0.06f * Mathf.Sin(life * Mathf.PI)));
    }

    public void v250e953b26b5(int vdef1460c2f7d)
    {
        this.vb524c47add51 = Mathf.Clamp(vdef1460c2f7d, 0, vff93c1a0dc29.Count - 1);
        vff93c1a0dc29.v7155f77d776f(this.vb524c47add51);

        for (int v0c2eb0976dd9 = 0; v0c2eb0976dd9 < this.v382b4179969d; v0c2eb0976dd9++)
        {
            bool vca4cefa117cf = v0c2eb0976dd9 == this.vb524c47add51;
            this.v3b7a24e94b29(v0c2eb0976dd9, vca4cefa117cf);
        }

        this.v09b29cb99d4c = this.vb524c47add51;
        this.vc3c680ba5048 = 0.30f;

        v1dfe305bda8d.v77f473265d32(this._routeChip, vff93c1a0dc29.v0055f10370a1(this.vb524c47add51), v5e702f77a0c6.v737c0b87c3cb, 38f, 46f);
        v1dfe305bda8d.v77f473265d32(this._objective, vd4da4103c714.ve3f6ebeb5c34(0) + vff93c1a0dc29.v0ff36440fbbc.ToString() + vd4da4103c714.ve3f6ebeb5c34(1),
            v5e702f77a0c6.v5bccfad8f382, 38f, 46f);

        if (this._emptyLabel != null)
        {
            bool empty = this.v382b4179969d == 0;
            v1dfe305bda8d.v77f473265d32(this._emptyLabel, empty ? vd4da4103c714.ve3f6ebeb5c34(2) : string.Empty, v5e702f77a0c6.vd091459a36d3, 38f, 46f);
            this._emptyLabel.gameObject.SetActive(empty);
        }

        if (this._stats != null)
        {
            this._stats.v4da8899525f8(this.vb524c47add51);
        }

        if (this._presenter != null)
        {
            this._presenter.v5b954a36b88d(this.vb524c47add51);
        }
    }

    private void v3b7a24e94b29(int v0c2eb0976dd9, bool vca4cefa117cf)
    {
        Image plate = this.At(this._rowPlates, v0c2eb0976dd9);
        if (plate != null)
        {
            plate.color = vca4cefa117cf
                ? new Color(0.125f, 0.188f, 0.420f, 1f)   
                : v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.va37c9d296cc7, 0.55f);
        }

        Image frame = this.At(this._rowFrames, v0c2eb0976dd9);
        if (frame != null)
        {
            frame.color = vca4cefa117cf
                ? v5e702f77a0c6.v44add4c81f28
                : v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v3893738f411c, 0.35f);
        }

        v1dfe305bda8d.v77f473265d32(this.At(this._rowTitles, v0c2eb0976dd9), vff93c1a0dc29.vdae8b34c339d(v0c2eb0976dd9),
            vca4cefa117cf ? v5e702f77a0c6.v5bccfad8f382 : v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v5bccfad8f382, 0.7f), 46f, 56f);
        v1dfe305bda8d.v77f473265d32(this.At(this._rowSummaries, v0c2eb0976dd9), vff93c1a0dc29.vc14d60c25918(v0c2eb0976dd9), v5e702f77a0c6.vd091459a36d3, 38f, 46f);

        int best = vff93c1a0dc29.vcc07433e5d90(v0c2eb0976dd9);
        v1dfe305bda8d.v77f473265d32(this.At(this._rowBests, v0c2eb0976dd9),
            best > 0 ? vd4da4103c714.ve3f6ebeb5c34(3) + best.ToString() + vd4da4103c714.ve3f6ebeb5c34(4) + vff93c1a0dc29.v0ff36440fbbc.ToString() : vd4da4103c714.ve3f6ebeb5c34(5),
            v5e702f77a0c6.vdd10b8a2dad6, 38f, 46f);

        for (int pip = 0; pip < v47d6566930c5; pip++)
        {
            Image mark = this.At(this._rowPips, (v0c2eb0976dd9 * v47d6566930c5) + pip);
            if (mark == null)
            {
                continue;
            }

            bool lit = pip <= v0c2eb0976dd9;
            mark.color = lit ? v5e702f77a0c6.v44add4c81f28 : v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.ve698a8efbd72, 0.7f);
        }

        RectTransform root = this.At(this._rowRoots, v0c2eb0976dd9);
        if (root != null)
        {
            root.localScale = this.v499ed660592d;
        }
    }

    private int v382b4179969d
    {
        get { return this._rowPlates == null ? 0 : this._rowPlates.Length; }
    }

    private T At<T>(T[] vf8b9bc3ba2b0, int v0c6f367fb1f6) where T : class
    {
        if (vf8b9bc3ba2b0 == null || v0c6f367fb1f6 < 0 || v0c6f367fb1f6 >= vf8b9bc3ba2b0.Length)
        {
            return null;
        }

        return vf8b9bc3ba2b0[v0c6f367fb1f6];
    }
}

internal static class vd4da4103c714
{
    private const byte vd0cb56db1cfd = 0x7E;

    private const string vba440bf09460 = "A2000000002c3dea8cfd5d12da49405c8fffd6453dceb02f8ef7846942";
    private const string v631217f12ec3 = "A200000001a56a849b0ba2e3d2e6d8349a6d";
    private const string v1300a5a34773 = "A20000000283ba27ca10c2ea5e0252654781440893bb6a3b7c49f99bde";
    private const string v6d5e959bce93 = "A2000000036cff1e1a17f1f89796";
    private const string v112018ea9a10 = "A200000004117be344038970";
    private const string vbae2079725de = "A2000000058391a551c50aa38eb368a6";

    internal static string ve3f6ebeb5c34(int id)
    {
        switch (id)
        {
            case 0: return vee97cac762a1(vba440bf09460);
            case 1: return vee97cac762a1(v631217f12ec3);
            case 2: return vee97cac762a1(v1300a5a34773);
            case 3: return vee97cac762a1(v6d5e959bce93);
            case 4: return vee97cac762a1(v112018ea9a10);
            case 5: return vee97cac762a1(vbae2079725de);
            default: return string.Empty;
        }
    }

    private static string vee97cac762a1(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)vd0cb56db1cfd * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
