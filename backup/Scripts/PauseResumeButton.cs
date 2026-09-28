using UnityEngine;
using UnityEngine.UI;

/// <summary>Drives RESUME on the pause card: closes the card and restarts the run clock.</summary>
public sealed class PauseResumeButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private ShaftRunDirector _director;
    [SerializeField] private GamePopPresenter _presenter;

    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this.HandleClick());
        }
    }

    private void HandleClick()
    {
        if (this._presenter != null)
        {
            this._presenter.Hide(GamePopPresenter.PopPause);
        }

        if (this._director != null)
        {
            this._director.Resume();
        }
    }
}
