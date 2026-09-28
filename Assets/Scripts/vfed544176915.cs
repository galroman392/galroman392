using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;






public sealed class vfed544176915 : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private int _sceneIndex;
    [SerializeField] private bool _reloadCurrent;

    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this.v54e0514c66a2());
        }
    }

    private void v54e0514c66a2()
    {
        int v2132923cb930 = this._reloadCurrent ? SceneManager.GetActiveScene().buildIndex : this._sceneIndex;
        SceneManager.LoadScene(v2132923cb930);
    }
}
