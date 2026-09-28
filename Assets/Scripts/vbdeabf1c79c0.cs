using TMPro;
using UnityEngine;
using UnityEngine.UI;






public sealed class vbdeabf1c79c0 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private Image _plate;
    [SerializeField] private float _pulseSeconds = 6f;

    private float va333e9225d67;

    private void Awake()
    {
        v1dfe305bda8d.v77f473265d32(this._label, v3047d2c43d06.vd66fd8e9306f(0), v5e702f77a0c6.v5bccfad8f382, 34f, 42f);
        if (this._plate != null)
        {
            this._plate.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v87f671aca8df, 0.55f);
        }
    }

    private void Update()
    {
        if (this._label == null)
        {
            return;
        }

        this.va333e9225d67 += Time.unscaledDeltaTime;
        float v875a2b2d2dc9 = 0.92f;
        if (this.va333e9225d67 < this._pulseSeconds)
        {
            v875a2b2d2dc9 = 0.7f + (0.3f * Mathf.Abs(Mathf.Sin(this.va333e9225d67 * 2.4f)));
        }

        this._label.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v5bccfad8f382, v875a2b2d2dc9);
    }
}

internal static class v3047d2c43d06
{
    private const byte vdabda8ea385f = 0xCF;

    private const string ved46d2b34fd7 = "A2000000005bade2492ff054330f6928aef6aac0c60c32840a476b029fb10f1c1fe8f2c00935abc6";

    internal static string vd66fd8e9306f(int id)
    {
        switch (id)
        {
            case 0: return v2c3cd65d23c8(ved46d2b34fd7);
            default: return string.Empty;
        }
    }

    private static string v2c3cd65d23c8(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)vdabda8ea385f * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
