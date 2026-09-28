using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The floating strip at the bottom of the game screen: how high the core is, how many
/// shield pips are left, the route and the record. Every element here is an object of
/// this game - nothing is hung on a template placeholder.
/// </summary>
public sealed class RunHudView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sectionValue;
    [SerializeField] private TextMeshProUGUI _sectionLabel;
    [SerializeField] private TextMeshProUGUI _bestValue;
    [SerializeField] private TextMeshProUGUI _bestLabel;
    [SerializeField] private TextMeshProUGUI _routeLabel;
    [SerializeField] private Image[] _pips;

    private void Awake()
    {
        TextReadability.ApplyLayout(this._sectionValue, 96f, 148f);
        TextReadability.ApplyLayout(this._sectionLabel, 34f, 38f);
        TextReadability.ApplyLayout(this._bestValue, 40f, 56f);
        TextReadability.ApplyLayout(this._bestLabel, 34f, 38f);
        TextReadability.ApplyLayout(this._routeLabel, 38f, 46f);

        TextReadability.ApplyOutline(this._sectionValue, ShaftPalette.TextGold);
        TextReadability.ApplyOutline(this._sectionLabel, ShaftPalette.TextBlue);
        TextReadability.ApplyOutline(this._bestValue, ShaftPalette.TextGold);
        TextReadability.ApplyOutline(this._bestLabel, ShaftPalette.TextBlue);
        TextReadability.ApplyOutline(this._routeLabel, ShaftPalette.TextArcane);
    }

    public void BindRoute(int routeIndex)
    {
        if (this._routeLabel != null)
        {
            this._routeLabel.text = ShaftRoutes.Chip(routeIndex);
        }

        if (this._sectionLabel != null)
        {
            this._sectionLabel.text = "SECTION";
        }

        if (this._bestLabel != null)
        {
            this._bestLabel.text = "BEST";
        }

        int best = ShaftRoutes.Best(routeIndex);
        if (this._bestValue != null)
        {
            this._bestValue.text = best > 0
                ? Pad(best) + " / " + ShaftRoutes.Sections.ToString()
                : "FIRST RUN";
        }
    }

    public void SetSection(int section)
    {
        if (this._sectionValue != null)
        {
            this._sectionValue.text = Pad(section) + " / " + ShaftRoutes.Sections.ToString();
        }
    }

    public void SetIntegrity(int remaining)
    {
        if (this._pips == null)
        {
            return;
        }

        for (int i = 0; i < this._pips.Length; i++)
        {
            Image pip = this._pips[i];
            if (pip == null)
            {
                continue;
            }

            bool alive = i < remaining;
            pip.color = alive ? ShaftPalette.Danger : ShaftPalette.WithAlpha(ShaftPalette.Dormant, 0.5f);
        }
    }

    public static string Pad(int value)
    {
        return value < 10 ? "0" + value.ToString() : value.ToString();
    }
}
