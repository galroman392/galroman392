using UnityEngine;
using UnityEngine.UI;


public sealed class v237f431f51f7 : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private v6966e7f417eb _director;

    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this.v54e0514c66a2());
        }
    }

    private void v54e0514c66a2()
    {
        if (this._director != null)
        {
            this._director.v256d77a60108();
        }
    }
}
