using UnityEngine;
using UnityEngine.UI;






public sealed class v60ed5081ca52 : MonoBehaviour
{
    [SerializeField] private RectTransform _core;
    [SerializeField] private RectTransform[] _sockets;
    [SerializeField] private Image[] _hazardSilhouettes;
    [SerializeField] private Image _coreGlow;
    [SerializeField] private float _loopSeconds = 4.2f;

    private float v5b52b8f4a4c7;
    private Vector2 v262ac351c356 = new Vector2(96f, 96f);

    private void Awake()
    {
        if (this._core != null)
        {
            this.v262ac351c356 = this._core.sizeDelta;
        }
    }

    
    public void v5b954a36b88d(int vdef1460c2f7d)
    {
        if (this._hazardSilhouettes == null)
        {
            return;
        }

        int[] kinds = vff93c1a0dc29.v806f0f5eaf5e(vdef1460c2f7d);
        for (int i = 0; i < this._hazardSilhouettes.Length; i++)
        {
            Image mark = this._hazardSilhouettes[i];
            if (mark == null)
            {
                continue;
            }

            bool used = i < kinds.Length;
            mark.gameObject.SetActive(used);
            if (!used)
            {
                continue;
            }

            mark.color = v171ff206be14(kinds[i]);
        }
    }

    private void Update()
    {
        if (this._core == null || this._sockets == null || this._sockets.Length < 2)
        {
            return;
        }

        this.v5b52b8f4a4c7 += Time.unscaledDeltaTime;
        float phase = (this.v5b52b8f4a4c7 % this._loopSeconds) / this._loopSeconds;

        int legs = this._sockets.Length - 1;
        float scaled = phase * legs;
        int leg = Mathf.Clamp(Mathf.FloorToInt(scaled), 0, legs - 1);
        float t = scaled - leg;

        RectTransform from = this._sockets[leg];
        RectTransform to = this._sockets[leg + 1];
        if (from == null || to == null)
        {
            return;
        }

        
        
        float rise = Mathf.Clamp01(t / 0.78f);
        float eased = rise * rise * (3f - (2f * rise));
        this._core.anchoredPosition = Vector2.Lerp(from.anchoredPosition, to.anchoredPosition, eased);

        bool latched = t >= 0.78f;
        this._core.sizeDelta = this.v262ac351c356 * (latched ? 1.06f : 1f);

        if (this._coreGlow != null)
        {
            this._coreGlow.color = v5e702f77a0c6.v35ea66314f38(
                latched ? v5e702f77a0c6.v44add4c81f28 : v5e702f77a0c6.v8cce7ccc9ec7,
                latched ? 0.9f : 0.55f);
        }
    }

    private static Color v171ff206be14(int v2cba01959503)
    {
        if (v2cba01959503 == v4434d66e509d.v30559dd3082e)
        {
            return v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v3893738f411c, 0.85f);
        }

        if (v2cba01959503 == v4434d66e509d.v2a418c016828)
        {
            return v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.v8cce7ccc9ec7, 0.85f);
        }

        return v5e702f77a0c6.v35ea66314f38(v5e702f77a0c6.vc46f649702f0, 0.85f);
    }
}
