using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ControllerPrivacyPolicy : MonoBehaviour
{
    public static bool IsOpen { get; private set; }

    private const int ToolbarHeight = 112;

    private HelpPane pane;
    private GameObject chrome;
    private RectTransform bar;
    private Rect lastSafeArea;
    private Vector2 lastScreenSize;

    public void OpenPrivacyPolicy(string href)
    {
        if (string.IsNullOrWhiteSpace(href))
        {
            Debug.LogError("Privacy Policy URL is empty.", this);
            return;
        }

        ClosePrivacyPolicy();

#if UNITY_ANDROID && !UNITY_EDITOR
        pane = new HelpPane();
        if (!pane.Ready)
        {
            pane.Destroy();
            pane = null;
            Application.OpenURL(href);
            return;
        }

        pane.SetAcceptThirdPartyCookies(true);
        ApplyFrame();
        pane.Load(href);
        pane.Show();
        BuildChrome();
        IsOpen = true;
#else
        Application.OpenURL(href);
#endif
    }

    private void Update()
    {
        if (!IsOpen || pane == null)
            return;

        if (lastSafeArea != Screen.safeArea ||
            lastScreenSize.x != Screen.width ||
            lastScreenSize.y != Screen.height)
        {
            ApplyFrame();
        }

        if (!BackReleased())
            return;

        if (pane.CanGoBack)
            pane.GoBack();
        else
            ClosePrivacyPolicy();
    }

    private void ClosePrivacyPolicy()
    {
        IsOpen = false;

        if (pane != null)
        {
            pane.Hide();
            pane.Destroy();
            pane = null;
        }

        if (chrome != null)
        {
            Destroy(chrome);
            chrome = null;
            bar = null;
        }
    }

    private void ApplyFrame()
    {
        Rect area = Screen.safeArea;
        int left = Mathf.Max(0, Mathf.RoundToInt(area.xMin));
        int right = Mathf.Max(0, Mathf.RoundToInt(Screen.width - area.xMax));
        int bottom = Mathf.Max(0, Mathf.RoundToInt(area.yMin));
        int top = Mathf.Max(0, Mathf.RoundToInt(Screen.height - area.yMax)) + ToolbarHeight;

        if (pane != null)
            pane.SetMargins(left, top, right, bottom, Screen.width, Screen.height);

        if (bar != null)
        {
            bar.sizeDelta = new Vector2(0f, top);
            bar.anchoredPosition = Vector2.zero;
        }

        lastSafeArea = area;
        lastScreenSize = new Vector2(Screen.width, Screen.height);
    }

    private void BuildChrome()
    {
        chrome = new GameObject("[Privacy Policy] Chrome");
        var canvas = chrome.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 5000;
        chrome.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        chrome.AddComponent<GraphicRaycaster>();
        EnsureEventSystem();

        int top = Mathf.Max(0, Mathf.RoundToInt(Screen.height - Screen.safeArea.yMax)) + ToolbarHeight;
        var barObject = new GameObject("Bar", typeof(RectTransform), typeof(Image));
        bar = barObject.GetComponent<RectTransform>();
        bar.SetParent(chrome.transform, false);
        bar.anchorMin = new Vector2(0f, 1f);
        bar.anchorMax = new Vector2(1f, 1f);
        bar.pivot = new Vector2(0.5f, 1f);
        bar.sizeDelta = new Vector2(0f, top);
        bar.anchoredPosition = Vector2.zero;
        barObject.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.12f, 1f);

        Sprite uiSprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
        var buttonObject = new GameObject("Close", typeof(RectTransform), typeof(Image), typeof(Button));
        var buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.SetParent(bar, false);
        buttonRect.anchorMin = new Vector2(1f, 0f);
        buttonRect.anchorMax = new Vector2(1f, 0f);
        buttonRect.pivot = new Vector2(1f, 0f);
        buttonRect.sizeDelta = new Vector2(180f, 72f);
        buttonRect.anchoredPosition = new Vector2(-24f, 16f);
        Image image = buttonObject.GetComponent<Image>();
        image.sprite = uiSprite;
        image.type = Image.Type.Sliced;
        image.color = new Color(0.95f, 0.96f, 0.98f, 1f);
        buttonObject.GetComponent<Button>().onClick.AddListener(ClosePrivacyPolicy);

        var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
        var labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.SetParent(buttonRect, false);
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = labelRect.offsetMax = Vector2.zero;
        var label = labelObject.GetComponent<Text>();
        label.text = "Close";
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.fontSize = 28;
        label.alignment = TextAnchor.MiddleCenter;
        label.color = new Color(0.12f, 0.12f, 0.15f, 1f);
    }

    private static void EnsureEventSystem()
    {
        if (EventSystem.current != null)
            return;

        var events = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        events.GetComponent<EventSystem>();
    }

    private static bool BackReleased()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null && keyboard.escapeKey.wasReleasedThisFrame)
            return true;

#if ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKeyUp(KeyCode.Escape);
#else
        return false;
#endif
    }

    private void OnDestroy()
    {
        ClosePrivacyPolicy();
    }
}
