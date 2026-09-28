using TMPro;
using UnityEngine;
using UnityEngine.UI;





public sealed class v45f4b7c1454e : MonoBehaviour
{
    public const int v7063239642b9 = 0;
    public const int v6003b468a09b = 1;
    public const int v99f2cd8c144d = 2;

    [SerializeField] private v6966e7f417eb _director;
    [SerializeField] private GameObject[] _popRoots;
    [SerializeField] private RectTransform[] _popCards;
    [SerializeField] private TextMeshProUGUI[] _titles;
    [SerializeField] private TextMeshProUGUI[] _bodies;
    [SerializeField] private TextMeshProUGUI[] _extras;
    [SerializeField] private Button[] _closeButtons;
    [SerializeField] private Image[] _closeIcons;
    [SerializeField] private Sprite _closeIcon;

    private int vf5ddafbc7879 = -1;
    private float v6c0dde311cc8;
    private Vector3 vb884eb153bfd = Vector3.one;

    private void Awake()
    {
        this.vb884eb153bfd = Vector3.one;

        for (int i = 0; i < this.Count; i++)
        {
            v1dfe305bda8d.v6e45b7f0f5c2(this.At(this._titles, i), 78f, 96f);
            v1dfe305bda8d.v6e45b7f0f5c2(this.At(this._bodies, i), 46f, 56f);
            v1dfe305bda8d.v6e45b7f0f5c2(this.At(this._extras, i), 38f, 46f);

            Image icon = this.At(this._closeIcons, i);
            if (icon != null && this._closeIcon != null)
            {
                
                
                icon.sprite = this._closeIcon;
                icon.preserveAspect = true;
                icon.color = v5e702f77a0c6.v5bccfad8f382;
            }

            Button close = this.At(this._closeButtons, i);
            if (close != null)
            {
                int v0c6f367fb1f6 = i;
                close.onClick.AddListener(() => this.v5c996c6d0e9b(v0c6f367fb1f6));
            }

            GameObject root = this.At(this._popRoots, i);
            if (root != null)
            {
                root.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (this.vf5ddafbc7879 < 0)
        {
            return;
        }

        RectTransform card = this.At(this._popCards, this.vf5ddafbc7879);
        if (card == null)
        {
            return;
        }

        this.v6c0dde311cc8 += Time.unscaledDeltaTime;
        float t = Mathf.Clamp01(this.v6c0dde311cc8 / 0.28f);
        
        float eased = 0.88f + (0.16f * Mathf.Sin(t * Mathf.PI * 0.5f)) - (0.04f * (1f - t));
        card.localScale = this.vb884eb153bfd * Mathf.Lerp(0.88f, eased < 1f ? eased : 1f, t);
    }

    public void v6bedca08252b(int v720ad0d3a1a2, int vdea3203578f8)
    {
        this.v77f473265d32(v7063239642b9,
            v2bf3ba0791eb.vd919a531f87e(0),
            v2bf3ba0791eb.vd919a531f87e(1) + vb43312e1e369.ve63ce530ab92(v720ad0d3a1a2) + v2bf3ba0791eb.vd919a531f87e(2) + vff93c1a0dc29.v0ff36440fbbc.ToString()
                + v2bf3ba0791eb.vd919a531f87e(3) + vdea3203578f8.ToString() + v2bf3ba0791eb.vd919a531f87e(4),
            v2bf3ba0791eb.vd919a531f87e(5),
            v5e702f77a0c6.v5bccfad8f382, v5e702f77a0c6.vd091459a36d3, v5e702f77a0c6.v737c0b87c3cb);
        this.v5eb78acdf934(v7063239642b9);
    }

    public void v9cb9c904ce56(int vdef1460c2f7d, int vdea3203578f8)
    {
        this.v77f473265d32(v6003b468a09b,
            v2bf3ba0791eb.vd919a531f87e(6),
            vff93c1a0dc29.v0055f10370a1(vdef1460c2f7d),
            vff93c1a0dc29.v0ff36440fbbc.ToString() + v2bf3ba0791eb.vd919a531f87e(7) + vff93c1a0dc29.v0ff36440fbbc.ToString()
                + v2bf3ba0791eb.vd919a531f87e(8) + vdea3203578f8.ToString() + v2bf3ba0791eb.vd919a531f87e(9),
            v5e702f77a0c6.vdd10b8a2dad6, v5e702f77a0c6.v737c0b87c3cb, v5e702f77a0c6.v5bccfad8f382);
        this.v5eb78acdf934(v6003b468a09b);
    }

    public void v02b0199b1792(int vdef1460c2f7d, int v720ad0d3a1a2)
    {
        int best = vff93c1a0dc29.vcc07433e5d90(vdef1460c2f7d);
        this.v77f473265d32(v99f2cd8c144d,
            v2bf3ba0791eb.vd919a531f87e(10),
            v2bf3ba0791eb.vd919a531f87e(11) + vb43312e1e369.ve63ce530ab92(v720ad0d3a1a2) + v2bf3ba0791eb.vd919a531f87e(12) + vff93c1a0dc29.v0ff36440fbbc.ToString(),
            best > 0 ? v2bf3ba0791eb.vd919a531f87e(13) + vb43312e1e369.ve63ce530ab92(best) + v2bf3ba0791eb.vd919a531f87e(14) + vff93c1a0dc29.v0ff36440fbbc.ToString() : v2bf3ba0791eb.vd919a531f87e(15),
            v5e702f77a0c6.vdde69408f99c, v5e702f77a0c6.v5bccfad8f382, v5e702f77a0c6.vdd10b8a2dad6);
        this.v5eb78acdf934(v99f2cd8c144d);
    }

    public void v374b7ca4f1e4(int v336f64e8d727)
    {
        GameObject root = this.At(this._popRoots, v336f64e8d727);
        if (root != null)
        {
            root.SetActive(false);
        }

        if (this.vf5ddafbc7879 == v336f64e8d727)
        {
            this.vf5ddafbc7879 = -1;
        }
    }

    private void v5c996c6d0e9b(int v336f64e8d727)
    {
        this.v374b7ca4f1e4(v336f64e8d727);
        if (v336f64e8d727 == v7063239642b9 && this._director != null)
        {
            this._director.v1f216549aa97();
        }
    }

    private void v77f473265d32(int v336f64e8d727, string v0bb35f7f3f24, string veb73fd3de58c, string ve222bb8cd3fc, Color v91b2437c648f, Color v3e76f73e9e2a, Color v8c0160f45f1d)
    {
        v1dfe305bda8d.v77f473265d32(this.At(this._titles, v336f64e8d727), v0bb35f7f3f24, v91b2437c648f, 78f, 96f);
        v1dfe305bda8d.v77f473265d32(this.At(this._bodies, v336f64e8d727), veb73fd3de58c, v3e76f73e9e2a, 46f, 56f);
        v1dfe305bda8d.v77f473265d32(this.At(this._extras, v336f64e8d727), ve222bb8cd3fc, v8c0160f45f1d, 38f, 46f);
    }

    private void v5eb78acdf934(int v336f64e8d727)
    {
        for (int i = 0; i < this.Count; i++)
        {
            GameObject root = this.At(this._popRoots, i);
            if (root != null)
            {
                root.SetActive(i == v336f64e8d727);
            }
        }

        RectTransform card = this.At(this._popCards, v336f64e8d727);
        if (card != null)
        {
            card.localScale = this.vb884eb153bfd * 0.88f;
        }

        this.vf5ddafbc7879 = v336f64e8d727;
        this.v6c0dde311cc8 = 0f;
    }

    private int Count
    {
        get { return this._popRoots == null ? 0 : this._popRoots.Length; }
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

internal static class v2bf3ba0791eb
{
    private const byte v5bae986f585d = 0xF6;

    private const string v13bf095caf2d = "A200000000fee54b7d272f6882e90eec5b4caa6d";
    private const string v51058df7897f = "A20000000119617c41b179174fb256852e";
    private const string v370a2eee6552 = "A200000002b967930e0da198";
    private const string vc94b26768430 = "A200000003181976d10ebdc675";
    private const string v25985688bd68 = "A200000004df3644407d2bd2361226fb3d";
    private const string v454631f83eeb = "A2000000055ee129327bfe97bbfe1a9cd699d3e36e706378b99818ffa08c7d3ef618658924dc9704";
    private const string v941df744605b = "A200000006a98a7becbe1d6d782493ec2276a290c47b35";
    private const string v5940f9fea409 = "A20000000703d50c7e75c3de";
    private const string v36c63eb536c0 = "A200000008f96c3cb8ca11b925efccf10395afa3bb1d";
    private const string vfd459a2c7084 = "A200000009069c74961988f6f7cae8f22d7dfd8d79e9";
    private const string v26b966ce31b9 = "A20000000ad2863beb0c085d86ed79dabc77";
    private const string ve80725d4cdf0 = "A20000000b5dfeede49d41b8533af48d711619a44709a081ea2f00a3";
    private const string v4dfc45554490 = "A20000000c9ca875df3c8d51";
    private const string vceadef346eb0 = "A20000000d39597a98d68e947fdd";
    private const string v5a11d8034704 = "A20000000e57496f6f514aea";
    private const string vd270654ee95a = "A20000000f65743f4a4e338aada60b746b21f5826b0767ef6c";

    internal static string vd919a531f87e(int id)
    {
        switch (id)
        {
            case 0: return v6b3c22b49430(v13bf095caf2d);
            case 1: return v6b3c22b49430(v51058df7897f);
            case 2: return v6b3c22b49430(v370a2eee6552);
            case 3: return v6b3c22b49430(vc94b26768430);
            case 4: return v6b3c22b49430(v25985688bd68);
            case 5: return v6b3c22b49430(v454631f83eeb);
            case 6: return v6b3c22b49430(v941df744605b);
            case 7: return v6b3c22b49430(v5940f9fea409);
            case 8: return v6b3c22b49430(v36c63eb536c0);
            case 9: return v6b3c22b49430(vfd459a2c7084);
            case 10: return v6b3c22b49430(v26b966ce31b9);
            case 11: return v6b3c22b49430(ve80725d4cdf0);
            case 12: return v6b3c22b49430(v4dfc45554490);
            case 13: return v6b3c22b49430(vceadef346eb0);
            case 14: return v6b3c22b49430(v5a11d8034704);
            case 15: return v6b3c22b49430(vd270654ee95a);
            default: return string.Empty;
        }
    }

    private static string v6b3c22b49430(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)v5bae986f585d * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
