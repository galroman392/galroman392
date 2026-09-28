using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;










public sealed class v3d32d03d2cbe : MonoBehaviour
{
    [SerializeField] private int _sceneIndex = 1;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private Image _barFill;
    [SerializeField] private Image _barTrack;
    [SerializeField] private TextMeshProUGUI _statusLabel;
    [SerializeField] private float _minimumSeconds = 0.6f;

    private bool v64d3f8ebbbf0;

    private void Awake()
    {
        if (this._barFill != null)
        {
            this._barFill.color = v5e702f77a0c6.v44add4c81f28;
        }

        if (this._barTrack != null)
        {
            this._barTrack.color = v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v87f671aca8df, 0.85f);
        }

        v1dfe305bda8d.v77f473265d32(this._statusLabel, vbaa201029a56.v6416f4e7bff8(0), v5e702f77a0c6.vd091459a36d3, 34f, 40f);
    }

    private void OnEnable()
    {
        if (this.v64d3f8ebbbf0)
        {
            return;
        }

        this.v64d3f8ebbbf0 = true;
        this.StartCoroutine(this.v9829cf5a201c());
    }

    private IEnumerator v9829cf5a201c()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(this._sceneIndex);
        if (operation == null)
        {
            yield break;
        }

        operation.allowSceneActivation = false;

        float elapsed = 0f;
        while (!operation.isDone)
        {
            elapsed += Time.unscaledDeltaTime;

            
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            float shown = Mathf.Min(progress, Mathf.Clamp01(elapsed / Mathf.Max(0.1f, this._minimumSeconds)));
            if (this._progressBar != null)
            {
                this._progressBar.value = Mathf.Lerp(this._progressBar.minValue, this._progressBar.maxValue, shown);
            }

            if (progress >= 1f && elapsed >= this._minimumSeconds)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}

internal static class vbaa201029a56
{
    private const byte v6b77c3eef2e3 = 0x56;

    private const string v1a275e585670 = "A200000000bc018a6f1df5a40b565bc91e9c9b1a59ab9ddbb69845";

    internal static string v6416f4e7bff8(int id)
    {
        switch (id)
        {
            case 0: return v56b0151910a4(v1a275e585670);
            default: return string.Empty;
        }
    }

    private static string v56b0151910a4(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)v6b77c3eef2e3 * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
