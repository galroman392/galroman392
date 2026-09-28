using UnityEngine;
using UnityEngine.UI;

/// <summary>Drives one row of the route list. Three instances, one per route.</summary>
public sealed class RouteTileButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private RouteBoardController _board;
    [SerializeField] private int _routeIndex;

    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this.HandleClick());
        }
    }

    private void HandleClick()
    {
        if (this._board != null)
        {
            this._board.Select(this._routeIndex);
        }
    }
}
