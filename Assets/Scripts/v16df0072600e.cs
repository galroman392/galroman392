using UnityEngine;
using UnityEngine.UI;


public sealed class v16df0072600e : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private vd98ac3592efd _board;
    [SerializeField] private int _routeIndex;

    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this.v54e0514c66a2());
        }
    }

    private void v54e0514c66a2()
    {
        if (this._board != null)
        {
            this._board.v250e953b26b5(this._routeIndex);
        }
    }
}
