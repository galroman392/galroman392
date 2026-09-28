using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Drives BACK / MENU / RETRY / RISE AGAIN. Scenes are addressed by BUILD INDEX, never
/// by name: obfuscation renames the scene files, and a name-based load would die in the
/// shipped build while working perfectly in the editor.
/// </summary>
public sealed class SceneNavButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private int _sceneIndex;
    [SerializeField] private bool _reloadCurrent;

    private void Start()
    {
        if (this._button != null)
        {
            this._button.onClick.AddListener(() => this.HandleClick());
        }
    }

    private void HandleClick()
    {
        int target = this._reloadCurrent ? SceneManager.GetActiveScene().buildIndex : this._sceneIndex;
        SceneManager.LoadScene(target);
    }
}
