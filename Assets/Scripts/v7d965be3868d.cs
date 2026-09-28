using UnityEngine;
using UnityEngine.UI;


public sealed class v7d965be3868d : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private v6966e7f417eb _director;
    [SerializeField] private v45f4b7c1454e _presenter;

    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this.v54e0514c66a2());
        }
    }

    private void v54e0514c66a2()
    {
        if (this._presenter != null)
        {
            this._presenter.v374b7ca4f1e4(v45f4b7c1454e.v7063239642b9);
        }

        if (this._director != null)
        {
            this._director.v1f216549aa97();
        }
    }
}
