using UnityEngine;
using UnityEngine.UI;

/// <summary>Drives the PAUSE slot in the top strip: freezes the run and opens the pause card.</summary>
public sealed class PauseHoldButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private ShaftRunDirector _director;

    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this.HandleClick());
        }
    }

    private void HandleClick()
    {
        if (this._director != null)
        {
            this._director.Pause();
        }
    }
}
