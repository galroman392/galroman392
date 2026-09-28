using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Opens or closes an overlay. One instance per button, with _open deciding the
/// direction, so ROUTES / HOW TO PLAY and their close buttons all share one driver.
/// </summary>
public sealed class OverlayToggleButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private UiVisibilityGate _gate;
    [SerializeField] private UiVisibilityGate _alsoClose;
    [SerializeField] private bool _open = true;

    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this.HandleClick());
        }
    }

    private void HandleClick()
    {
        if (this._alsoClose != null)
        {
            this._alsoClose.SetVisible(false);
        }

        if (this._gate != null)
        {
            this._gate.SetVisible(this._open);
        }
    }
}
