using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The living preview above the menu sheet: a core climbing three sockets on a loop,
/// with hazard silhouettes that change with the selected route. This is the part that
/// makes route selection visible at a glance instead of being a number in a save file.
/// </summary>
public sealed class MenuShaftPresenter : MonoBehaviour
{
    [SerializeField] private RectTransform _core;
    [SerializeField] private RectTransform[] _sockets;
    [SerializeField] private Image[] _hazardSilhouettes;
    [SerializeField] private Image _coreGlow;
    [SerializeField] private float _loopSeconds = 4.2f;

    private float _clock;
    private Vector2 _coreBaseSize = new Vector2(96f, 96f);

    private void Awake()
    {
        if (this._core != null)
        {
            this._coreBaseSize = this._core.sizeDelta;
        }
    }

    /// <summary>Repaints the silhouettes for the hazards this route actually spawns.</summary>
    public void ApplyRoute(int routeIndex)
    {
        if (this._hazardSilhouettes == null)
        {
            return;
        }

        int[] kinds = ShaftRoutes.BarrierKinds(routeIndex);
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

            mark.color = Tint(kinds[i]);
        }
    }

    private void Update()
    {
        if (this._core == null || this._sockets == null || this._sockets.Length < 2)
        {
            return;
        }

        this._clock += Time.unscaledDeltaTime;
        float phase = (this._clock % this._loopSeconds) / this._loopSeconds;

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

        // Rise for most of the leg, then latch and hold - the loop reads as the real
        // hold-and-lock rhythm rather than a constant glide.
        float rise = Mathf.Clamp01(t / 0.78f);
        float eased = rise * rise * (3f - (2f * rise));
        this._core.anchoredPosition = Vector2.Lerp(from.anchoredPosition, to.anchoredPosition, eased);

        bool latched = t >= 0.78f;
        this._core.sizeDelta = this._coreBaseSize * (latched ? 1.06f : 1f);

        if (this._coreGlow != null)
        {
            this._coreGlow.color = ShaftPalette.WithAlpha(
                latched ? ShaftPalette.AccentRoyal : ShaftPalette.AccentPrimary,
                latched ? 0.9f : 0.55f);
        }
    }

    private static Color Tint(int kind)
    {
        if (kind == BarrierPlan.KindRotor)
        {
            return ShaftPalette.WithAlpha(ShaftPalette.AccentArcane, 0.85f);
        }

        if (kind == BarrierPlan.KindGate)
        {
            return ShaftPalette.WithAlpha(ShaftPalette.AccentPrimary, 0.85f);
        }

        return ShaftPalette.WithAlpha(ShaftPalette.Danger, 0.85f);
    }
}
