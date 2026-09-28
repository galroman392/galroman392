using UnityEngine;
using UnityEngine.UI;





public sealed class v27ab52ceb84f : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private va40fdbd46ce2 _gate;
    [SerializeField] private va40fdbd46ce2 _alsoClose;
    [SerializeField] private bool _open = true;

    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this.v54e0514c66a2());
        }
    }

    private void v54e0514c66a2()
    {
        if (this._alsoClose != null)
        {
            this._alsoClose.v7d801e945b7e(false);
        }

        if (this._gate != null)
        {
            this._gate.v7d801e945b7e(this._open);
        }
    }
}
