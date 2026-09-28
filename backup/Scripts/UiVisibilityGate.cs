using UnityEngine;

/// <summary>
/// Shows and hides one piece of THIS game's UI. Deliberately holds a serialized
/// reference to its own content rather than looking anything up by name, so an overlay
/// still opens after the project has been renamed end to end.
/// </summary>
public sealed class UiVisibilityGate : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private bool _visibleOnStart;

    private void Start()
    {
        this.SetVisible(this._visibleOnStart);
    }

    public void SetVisible(bool visible)
    {
        if (this._content != null)
        {
            this._content.SetActive(visible);
        }
    }
}
