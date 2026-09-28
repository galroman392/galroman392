using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The control is a hold, so the screen has to say so - permanently, not as a hint that
/// fades out. It pulses for the first few seconds to catch the eye, then settles at a
/// readable alpha and stays there for the rest of the run.
/// </summary>
public sealed class GesturePromptView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private Image _plate;
    [SerializeField] private float _pulseSeconds = 6f;

    private float _elapsed;

    private void Awake()
    {
        TextReadability.Dress(this._label, "HOLD TO RISE · RELEASE TO LOCK", ShaftPalette.TextPrimary, 34f, 42f);
        if (this._plate != null)
        {
            this._plate.color = ShaftPalette.WithAlpha(ShaftPalette.Shadow, 0.55f);
        }
    }

    private void Update()
    {
        if (this._label == null)
        {
            return;
        }

        this._elapsed += Time.unscaledDeltaTime;
        float alpha = 0.92f;
        if (this._elapsed < this._pulseSeconds)
        {
            alpha = 0.7f + (0.3f * Mathf.Abs(Mathf.Sin(this._elapsed * 2.4f)));
        }

        this._label.color = ShaftPalette.WithAlpha(ShaftPalette.TextPrimary, alpha);
    }
}
