using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dresses and shows the three result cards. Pops are addressed by INDEX, never by name,
/// so the obfuscator can rename every object in the scene without breaking navigation.
/// </summary>
public sealed class GamePopPresenter : MonoBehaviour
{
    public const int PopPause = 0;
    public const int PopWin = 1;
    public const int PopLose = 2;

    [SerializeField] private ShaftRunDirector _director;
    [SerializeField] private GameObject[] _popRoots;
    [SerializeField] private RectTransform[] _popCards;
    [SerializeField] private TextMeshProUGUI[] _titles;
    [SerializeField] private TextMeshProUGUI[] _bodies;
    [SerializeField] private TextMeshProUGUI[] _extras;
    [SerializeField] private Button[] _closeButtons;
    [SerializeField] private Image[] _closeIcons;
    [SerializeField] private Sprite _closeIcon;

    private int _openPop = -1;
    private float _openTime;
    private Vector3 _cardBaseScale = Vector3.one;

    private void Awake()
    {
        this._cardBaseScale = Vector3.one;

        for (int i = 0; i < this.Count; i++)
        {
            TextReadability.ApplyLayout(this.At(this._titles, i), 78f, 96f);
            TextReadability.ApplyLayout(this.At(this._bodies, i), 46f, 56f);
            TextReadability.ApplyLayout(this.At(this._extras, i), 38f, 46f);

            Image icon = this.At(this._closeIcons, i);
            if (icon != null && this._closeIcon != null)
            {
                // A missing sprite here is Unity's white rectangle, so the icon is a
                // serialized reference rather than a lookup that obfuscation could break.
                icon.sprite = this._closeIcon;
                icon.preserveAspect = true;
                icon.color = ShaftPalette.TextPrimary;
            }

            Button close = this.At(this._closeButtons, i);
            if (close != null)
            {
                int index = i;
                close.onClick.AddListener(() => this.HandleClose(index));
            }

            GameObject root = this.At(this._popRoots, i);
            if (root != null)
            {
                root.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (this._openPop < 0)
        {
            return;
        }

        RectTransform card = this.At(this._popCards, this._openPop);
        if (card == null)
        {
            return;
        }

        this._openTime += Time.unscaledDeltaTime;
        float t = Mathf.Clamp01(this._openTime / 0.28f);
        // Overshoot-and-settle, computed off the card's own base scale (never a literal).
        float eased = 0.88f + (0.16f * Mathf.Sin(t * Mathf.PI * 0.5f)) - (0.04f * (1f - t));
        card.localScale = this._cardBaseScale * Mathf.Lerp(0.88f, eased < 1f ? eased : 1f, t);
    }

    public void ShowPause(int section, int integrity)
    {
        this.Dress(PopPause,
            "HOLD PAUSED",
            "SECTION " + RunHudView.Pad(section) + " / " + ShaftRoutes.Sections.ToString()
                + " · " + integrity.ToString() + " SHIELDS",
            "HOLD TO RISE · RELEASE TO LOCK",
            ShaftPalette.TextPrimary, ShaftPalette.TextBlue, ShaftPalette.TextArcane);
        this.Open(PopPause);
    }

    public void ShowWin(int routeIndex, int integrity)
    {
        this.Dress(PopWin,
            "BEACON REACHED",
            ShaftRoutes.Chip(routeIndex),
            ShaftRoutes.Sections.ToString() + " / " + ShaftRoutes.Sections.ToString()
                + " SECTIONS · " + integrity.ToString() + " SHIELDS LEFT",
            ShaftPalette.TextGold, ShaftPalette.TextArcane, ShaftPalette.TextPrimary);
        this.Open(PopWin);
    }

    public void ShowLose(int routeIndex, int section)
    {
        int best = ShaftRoutes.Best(routeIndex);
        this.Dress(PopLose,
            "CORE LOST",
            "STOPPED AT SECTION " + RunHudView.Pad(section) + " / " + ShaftRoutes.Sections.ToString(),
            best > 0 ? "BEST " + RunHudView.Pad(best) + " / " + ShaftRoutes.Sections.ToString() : "FIRST RUN LOGGED",
            ShaftPalette.TextWarn, ShaftPalette.TextPrimary, ShaftPalette.TextGold);
        this.Open(PopLose);
    }

    public void Hide(int popIndex)
    {
        GameObject root = this.At(this._popRoots, popIndex);
        if (root != null)
        {
            root.SetActive(false);
        }

        if (this._openPop == popIndex)
        {
            this._openPop = -1;
        }
    }

    private void HandleClose(int popIndex)
    {
        this.Hide(popIndex);
        if (popIndex == PopPause && this._director != null)
        {
            this._director.Resume();
        }
    }

    private void Dress(int popIndex, string title, string body, string extra, Color titleTint, Color bodyTint, Color extraTint)
    {
        TextReadability.Dress(this.At(this._titles, popIndex), title, titleTint, 78f, 96f);
        TextReadability.Dress(this.At(this._bodies, popIndex), body, bodyTint, 46f, 56f);
        TextReadability.Dress(this.At(this._extras, popIndex), extra, extraTint, 38f, 46f);
    }

    private void Open(int popIndex)
    {
        for (int i = 0; i < this.Count; i++)
        {
            GameObject root = this.At(this._popRoots, i);
            if (root != null)
            {
                root.SetActive(i == popIndex);
            }
        }

        RectTransform card = this.At(this._popCards, popIndex);
        if (card != null)
        {
            card.localScale = this._cardBaseScale * 0.88f;
        }

        this._openPop = popIndex;
        this._openTime = 0f;
    }

    private int Count
    {
        get { return this._popRoots == null ? 0 : this._popRoots.Length; }
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
