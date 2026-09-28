using TMPro;
using UnityEngine;

/// <summary>
/// The BEST / RUNS plates on the menu sheet. Both are objects of this game - the
/// template's own score placeholder stays switched off, because this game has no
/// currency and a coin counter would read as one.
///
/// An empty record is never an empty plate: the hint takes over instead.
/// </summary>
public sealed class MenuStatsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bestValue;
    [SerializeField] private TextMeshProUGUI _bestLabel;
    [SerializeField] private TextMeshProUGUI _runsValue;
    [SerializeField] private TextMeshProUGUI _runsLabel;
    [SerializeField] private TextMeshProUGUI _emptyHint;

    private void Awake()
    {
        TextReadability.Dress(this._bestLabel, "BEST", ShaftPalette.TextBlue, 34f, 38f);
        TextReadability.Dress(this._runsLabel, "RUNS", ShaftPalette.TextBlue, 34f, 38f);
        TextReadability.ApplyLayout(this._bestValue, 62f, 96f);
        TextReadability.ApplyLayout(this._runsValue, 62f, 96f);
        TextReadability.ApplyLayout(this._emptyHint, 34f, 38f);
    }

    public void Refresh(int routeIndex)
    {
        int best = ShaftRoutes.Best(routeIndex);
        int runs = ShaftRoutes.Runs();

        TextReadability.Dress(this._bestValue,
            best > 0 ? best.ToString() + " / " + ShaftRoutes.Sections.ToString() : "--",
            ShaftPalette.TextGold, 62f, 96f);

        TextReadability.Dress(this._runsValue, runs.ToString(), ShaftPalette.TextPrimary, 62f, 96f);

        if (this._emptyHint != null)
        {
            bool blank = best <= 0 && runs <= 0;
            TextReadability.Dress(this._emptyHint, blank ? "NO RUN YET" : string.Empty, ShaftPalette.TextBlue, 34f, 38f);
            this._emptyHint.gameObject.SetActive(blank);
        }
    }
}
