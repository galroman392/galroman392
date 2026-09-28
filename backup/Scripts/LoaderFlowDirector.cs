using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Drives the splash panel and hands over to the menu. The template shipped this job on
/// a MonoBehaviour whose script was missing from the package, so the loading bar never
/// filled and the app never left the splash - this is its replacement.
///
/// The hand-over goes through the template's own PanelsFlow so the menu controller keeps
/// working exactly as it was written.
/// </summary>
public sealed class LoaderFlowDirector : MonoBehaviour
{
    [SerializeField] private PanelsFlow _panelsFlow;
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private Image _barFill;
    [SerializeField] private Image _barTrack;
    [SerializeField] private RectTransform _emblem;
    [SerializeField] private Image _emblemGlow;
    [SerializeField] private TextMeshProUGUI _statusLabel;
    [SerializeField] private float _durationSeconds = 2.6f;

    private float _elapsed;
    private bool _handedOver;
    private Vector2 _emblemBaseSize = new Vector2(360f, 360f);

    private void Awake()
    {
        if (this._emblem != null)
        {
            this._emblemBaseSize = this._emblem.sizeDelta;
        }

        if (this._barFill != null)
        {
            this._barFill.color = ShaftPalette.AccentRoyal;
        }

        if (this._barTrack != null)
        {
            this._barTrack.color = ShaftPalette.WithAlpha(ShaftPalette.Shadow, 0.85f);
        }

        TextReadability.Dress(this._statusLabel, "LOADING", ShaftPalette.TextBlue, 34f, 38f);

        if (this._loadingBar != null)
        {
            this._loadingBar.value = this._loadingBar.minValue;
        }
    }

    private void Update()
    {
        if (this._handedOver)
        {
            return;
        }

        this._elapsed += Time.unscaledDeltaTime;
        float progress = Mathf.Clamp01(this._elapsed / Mathf.Max(0.1f, this._durationSeconds));

        if (this._loadingBar != null)
        {
            this._loadingBar.value = Mathf.Lerp(this._loadingBar.minValue, this._loadingBar.maxValue, progress);
        }

        if (this._emblem != null)
        {
            // Settle in from 86% of the authored size, then breathe. Scaled off the
            // emblem's own base size so it behaves the same on every reference resolution.
            float settle = Mathf.Clamp01(this._elapsed / 0.6f);
            float overshoot = 0.86f + (0.16f * settle) - (0.02f * Mathf.Cos(settle * Mathf.PI));
            float breathe = 1f + (0.02f * Mathf.Sin(this._elapsed * 2.6f));
            this._emblem.sizeDelta = this._emblemBaseSize * Mathf.Min(1f, overshoot) * breathe;
        }

        if (this._emblemGlow != null)
        {
            float wave = 0.5f + (0.5f * Mathf.Sin(this._elapsed * 2.6f));
            this._emblemGlow.color = ShaftPalette.WithAlpha(ShaftPalette.AccentPrimary, 0.45f + (0.45f * wave));
        }

        if (progress < 1f)
        {
            return;
        }

        this._handedOver = true;
        if (this._panelsFlow != null)
        {
            this._panelsFlow.PanelTransition(this._panelsFlow.panel_menu, this._panelsFlow.panel_loader);
        }
    }
}
