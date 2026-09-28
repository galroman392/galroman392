using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The hand-off panel between the menu and the game scene. The template's menu
/// controller answers PLAY by switching to its "game" panel; this gate turns that switch
/// into a real scene load, so the game lives in its own scene instead of growing inside
/// the loader.
///
/// The scene is addressed by BUILD INDEX - never by name, because obfuscation renames
/// the scene files and a name-based load would only fail in the shipped build.
/// </summary>
public sealed class SceneLaunchGate : MonoBehaviour
{
    [SerializeField] private int _sceneIndex = 1;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private Image _barFill;
    [SerializeField] private Image _barTrack;
    [SerializeField] private TextMeshProUGUI _statusLabel;
    [SerializeField] private float _minimumSeconds = 0.6f;

    private bool _launching;

    private void Awake()
    {
        if (this._barFill != null)
        {
            this._barFill.color = ShaftPalette.AccentRoyal;
        }

        if (this._barTrack != null)
        {
            this._barTrack.color = ShaftPalette.WithAlpha(ShaftPalette.Shadow, 0.85f);
        }

        TextReadability.Dress(this._statusLabel, "ENTERING THE SHAFT", ShaftPalette.TextBlue, 34f, 40f);
    }

    private void OnEnable()
    {
        if (this._launching)
        {
            return;
        }

        this._launching = true;
        this.StartCoroutine(this.LoadRoutine());
    }

    private IEnumerator LoadRoutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(this._sceneIndex);
        if (operation == null)
        {
            yield break;
        }

        operation.allowSceneActivation = false;

        float elapsed = 0f;
        while (!operation.isDone)
        {
            elapsed += Time.unscaledDeltaTime;

            // Unity parks a non-activated load at 0.9, so rescale it back to a full bar.
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            float shown = Mathf.Min(progress, Mathf.Clamp01(elapsed / Mathf.Max(0.1f, this._minimumSeconds)));
            if (this._progressBar != null)
            {
                this._progressBar.value = Mathf.Lerp(this._progressBar.minValue, this._progressBar.maxValue, shown);
            }

            if (progress >= 1f && elapsed >= this._minimumSeconds)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
