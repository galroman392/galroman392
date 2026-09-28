using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The route list and the selection it persists. Picking a route has to be visible on a
/// still screenshot, not just in a save file, so one tap changes four things at once:
/// the row frame and fill, a punch on the row, the difficulty pips, and the artwork and
/// chip above the sheet.
/// </summary>
public sealed class RouteBoardController : MonoBehaviour
{
    [SerializeField] private MenuShaftPresenter _presenter;
    [SerializeField] private MenuStatsView _stats;
    [SerializeField] private TextMeshProUGUI _routeChip;
    [SerializeField] private TextMeshProUGUI _objective;
    [SerializeField] private TextMeshProUGUI _emptyLabel;
    [SerializeField] private Image[] _rowPlates;
    [SerializeField] private Image[] _rowFrames;
    [SerializeField] private RectTransform[] _rowRoots;
    [SerializeField] private TextMeshProUGUI[] _rowTitles;
    [SerializeField] private TextMeshProUGUI[] _rowSummaries;
    [SerializeField] private TextMeshProUGUI[] _rowBests;
    /// <summary>Three difficulty pips per row, laid out row-major.</summary>
    [SerializeField] private Image[] _rowPips;

    private const int PipsPerRow = 3;

    private int _selected;
    private int _punchRow = -1;
    private float _punchTimer;
    private Vector3 _rowBaseScale = Vector3.one;

    private void Awake()
    {
        this._rowBaseScale = Vector3.one;

        for (int row = 0; row < this.RowCount; row++)
        {
            TextReadability.ApplyLayout(this.At(this._rowTitles, row), 46f, 56f);
            TextReadability.ApplyLayout(this.At(this._rowSummaries, row), 38f, 46f);
            TextReadability.ApplyLayout(this.At(this._rowBests, row), 38f, 46f);
        }

        TextReadability.ApplyLayout(this._routeChip, 38f, 46f);
        TextReadability.ApplyLayout(this._objective, 38f, 46f);
        TextReadability.ApplyLayout(this._emptyLabel, 38f, 46f);
    }

    private void Start()
    {
        this.Select(ShaftRoutes.SelectedRoute());
    }

    private void Update()
    {
        if (this._punchTimer <= 0f)
        {
            return;
        }

        this._punchTimer = Mathf.Max(0f, this._punchTimer - Time.unscaledDeltaTime);
        RectTransform row = this.At(this._rowRoots, this._punchRow);
        if (row == null)
        {
            return;
        }

        // Punch computed from the row's own base scale, never from a literal size.
        float life = this._punchTimer / 0.30f;
        row.localScale = this._rowBaseScale * (1f + (0.06f * Mathf.Sin(life * Mathf.PI)));
    }

    public void Select(int routeIndex)
    {
        this._selected = Mathf.Clamp(routeIndex, 0, ShaftRoutes.Count - 1);
        ShaftRoutes.SelectRoute(this._selected);

        for (int row = 0; row < this.RowCount; row++)
        {
            bool active = row == this._selected;
            this.PaintRow(row, active);
        }

        this._punchRow = this._selected;
        this._punchTimer = 0.30f;

        TextReadability.Dress(this._routeChip, ShaftRoutes.Chip(this._selected), ShaftPalette.TextArcane, 38f, 46f);
        TextReadability.Dress(this._objective, "REACH THE BEACON · " + ShaftRoutes.Sections.ToString() + " SECTIONS",
            ShaftPalette.TextPrimary, 38f, 46f);

        if (this._emptyLabel != null)
        {
            bool empty = this.RowCount == 0;
            TextReadability.Dress(this._emptyLabel, empty ? "NOTHING UNLOCKED YET" : string.Empty, ShaftPalette.TextBlue, 38f, 46f);
            this._emptyLabel.gameObject.SetActive(empty);
        }

        if (this._stats != null)
        {
            this._stats.Refresh(this._selected);
        }

        if (this._presenter != null)
        {
            this._presenter.ApplyRoute(this._selected);
        }
    }

    private void PaintRow(int row, bool active)
    {
        Image plate = this.At(this._rowPlates, row);
        if (plate != null)
        {
            plate.color = active
                ? new Color(0.125f, 0.188f, 0.420f, 1f)   // #20306B, the lifted row
                : ShaftPalette.WithAlpha(ShaftPalette.Surface, 0.55f);
        }

        Image frame = this.At(this._rowFrames, row);
        if (frame != null)
        {
            frame.color = active
                ? ShaftPalette.AccentRoyal
                : ShaftPalette.WithAlpha(ShaftPalette.AccentArcane, 0.35f);
        }

        TextReadability.Dress(this.At(this._rowTitles, row), ShaftRoutes.Title(row),
            active ? ShaftPalette.TextPrimary : ShaftPalette.WithAlpha(ShaftPalette.TextPrimary, 0.7f), 46f, 56f);
        TextReadability.Dress(this.At(this._rowSummaries, row), ShaftRoutes.Summary(row), ShaftPalette.TextBlue, 38f, 46f);

        int best = ShaftRoutes.Best(row);
        TextReadability.Dress(this.At(this._rowBests, row),
            best > 0 ? "BEST " + best.ToString() + " / " + ShaftRoutes.Sections.ToString() : "BEST --",
            ShaftPalette.TextGold, 38f, 46f);

        for (int pip = 0; pip < PipsPerRow; pip++)
        {
            Image mark = this.At(this._rowPips, (row * PipsPerRow) + pip);
            if (mark == null)
            {
                continue;
            }

            bool lit = pip <= row;
            mark.color = lit ? ShaftPalette.AccentRoyal : ShaftPalette.WithAlpha(ShaftPalette.Dormant, 0.7f);
        }

        RectTransform root = this.At(this._rowRoots, row);
        if (root != null)
        {
            root.localScale = this._rowBaseScale;
        }
    }

    private int RowCount
    {
        get { return this._rowPlates == null ? 0 : this._rowPlates.Length; }
    }

    private T At<T>(T[] source, int index) where T : class
    {
        if (source == null || index < 0 || index >= source.Length)
        {
            return null;
        }

        return source[index];
    }
}
