using TMPro;
using UnityEngine;
using UnityEngine.UI;









public sealed class v5b8c389f8d5e : MonoBehaviour
{
    [SerializeField] private v0ac98812b868 _panelsFlow;
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private Image _barFill;
    [SerializeField] private Image _barTrack;
    [SerializeField] private RectTransform _emblem;
    [SerializeField] private Image _emblemGlow;
    [SerializeField] private TextMeshProUGUI _statusLabel;
    [SerializeField] private float _durationSeconds = 2.6f;

    private float va333e9225d67;
    private bool v316f257ad0c6;
    private Vector2 vaf2c41adf244 = new Vector2(360f, 360f);

    private void Awake()
    {
        if (this._emblem != null)
        {
            this.vaf2c41adf244 = this._emblem.sizeDelta;
        }

        if (this._barFill != null)
        {
            this._barFill.color = v5e702f77a0c6.v44add4c81f28;
        }

        if (this._barTrack != null)
        {
            this._barTrack.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v87f671aca8df, 0.85f);
        }

        v1dfe305bda8d.v77f473265d32(this._statusLabel, ve770991b12c8.v5caa6199493f(0), v5e702f77a0c6.vd091459a36d3, 34f, 38f);

        if (this._loadingBar != null)
        {
            this._loadingBar.value = this._loadingBar.minValue;
        }
    }

    private void Update()
    {
        if (this.v316f257ad0c6)
        {
            return;
        }

        this.va333e9225d67 += Time.unscaledDeltaTime;
        float progress = Mathf.Clamp01(this.va333e9225d67 / Mathf.Max(0.1f, this._durationSeconds));

        if (this._loadingBar != null)
        {
            this._loadingBar.value = Mathf.Lerp(this._loadingBar.minValue, this._loadingBar.maxValue, progress);
        }

        if (this._emblem != null)
        {
            
            
            float settle = Mathf.Clamp01(this.va333e9225d67 / 0.6f);
            float overshoot = 0.86f + (0.16f * settle) - (0.02f * Mathf.Cos(settle * Mathf.PI));
            float breathe = 1f + (0.02f * Mathf.Sin(this.va333e9225d67 * 2.6f));
            this._emblem.sizeDelta = this.vaf2c41adf244 * Mathf.Min(1f, overshoot) * breathe;
        }

        if (this._emblemGlow != null)
        {
            float wave = 0.5f + (0.5f * Mathf.Sin(this.va333e9225d67 * 2.6f));
            this._emblemGlow.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v8cce7ccc9ec7, 0.45f + (0.45f * wave));
        }

        if (progress < 1f)
        {
            return;
        }

        this.v316f257ad0c6 = true;
        if (this._panelsFlow != null)
        {
            this._panelsFlow.vdd33ad2f57a5(this._panelsFlow.panel_menu, this._panelsFlow.panel_loader);
        }
    }
}

internal static class ve770991b12c8
{
    private const byte v35709106b49d = 0x6A;

    private const string v75313c600c3d = "A2000000002495dcf82ec323d42090ec";

    internal static string v5caa6199493f(int id)
    {
        switch (id)
        {
            case 0: return v8a1560769d2d(v75313c600c3d);
            default: return string.Empty;
        }
    }

    private static string v8a1560769d2d(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)v35709106b49d * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
