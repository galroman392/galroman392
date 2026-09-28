using System;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PageHost : MonoBehaviour
{
    private GameObject loader;
    private HelpPane view;
    private Canvas rootCanvas;
    private RectTransform safeAreaRect;
    private Rect lastSafeArea;
    private Vector2 lastScreenSize;
    private bool eventsRegistered;
    private bool prewarming;
    private bool firstLoadShown;
    private bool googleUserAgentApplied;
    private bool paymentUserAgentApplied;
    private bool webViewVisible;
    private Action onCloseCallback;
    private GameObject exitRoot;
    private Text exitText;
    private Button stayButton;
    private Button leaveButton;
    private PermissionCallbacks captureCallbacks;
    private int captureWaits;
    private bool captureVideoPending;
    private bool captureAudioPending;
    private bool holdPanels;
    private bool consent;
    private Action onAccept;
    private GameObject consentRoot;

    private readonly string[] exitLines =
    {
        "The next spin could change everything – stay a little longer!",
        "Your lucky moment may be closer than you think.",
        "Great moments happen when you stay in the game.",
        "One more round could be the one you’ve been waiting for.",
        "Luck is still on your side – don’t leave just yet.",
        "Don’t break the flow now – keep playing.",
        "The energy is high – stay in and keep it going.",
        "You’re already here – keep going and see what happens next.",
        "Winners don’t stop halfway – continue your run.",
        "The game is still moving – your chance is not over.",
        "Stay focused – your next chance is coming up.",
        "The action is heating up – keep your momentum going.",
        "One more spin, one more chance – don’t miss it.",
    };

    private static string ProductAgent()
    {
        return $"{Application.productName}/{Application.version} (Unity; {SystemInfo.operatingSystem}; {SystemInfo.deviceModel}; {Application.systemLanguage})";
    }

    private void Awake()
    {
        Input.backButtonLeavesApp = false;
        EnsureCanvas();
        EnsureSafeArea();
        EnsureWebViewCreated();
        if (view == null || !view.Ready)
            return;

        prewarming = true;
        view.Load("about:blank");
        DebugMessage("WebView prewarm started");
    }

    public void Open(string url)
    {
        consent = false;
        OpenInternal(url, null);
    }

    public void Open(string url, Action onClose)
    {
        consent = false;
        OpenInternal(url, onClose);
    }

    public void Open(string url, GameObject suppliedLoader, Action onClose)
    {
        consent = false;
        if (suppliedLoader != null)
            loader = suppliedLoader;

        OpenInternal(url, onClose);
    }

    public void OpenConsent(string url, Action accept)
    {
        consent = true;
        onAccept = accept;
        OpenInternal(url, null);
        EnsureConsentBar();
        ApplySafeArea();
    }

    private void OpenInternal(string url, Action onClose)
    {
        if (!IsHttp(url))
        {
            DebugMessage("WebView open rejected: URL is not HTTP/HTTPS");
            return;
        }

        DebugMessage("WebView open requested");
        if (!consent)
        {
            onAccept = null;
            HideConsentBar();
        }

        onCloseCallback = onClose;
        EnsureWebViewCreated();
        if (view == null || !view.Ready)
        {
            Application.OpenURL(url);
            return;
        }

        prewarming = false;
        firstLoadShown = false;
        googleUserAgentApplied = paymentUserAgentApplied = false;
        view.SetUserAgent("");
        ApplySafeArea();
        HoldPanels();

        view.Stop();
        view.Load(url);
        view.Show();
        webViewVisible = true;
        DebugMessage("WebView navigation started");
    }

    private void EnsureWebViewCreated()
    {
        if (view == null)
        {
            DebugMessage("Creating HelpPane instance");
            view = new HelpPane();
#if UNITY_ANDROID && !UNITY_EDITOR
            view.SetAcceptThirdPartyCookies(true);
#endif
            view.SetUserAgent("");
        }

        RegisterEvents();
        ApplySafeArea();
    }

    private void RegisterEvents()
    {
        if (eventsRegistered || view == null)
            return;

        eventsRegistered = true;
        view.AllowCapture = resources =>
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            bool video = string.IsNullOrEmpty(resources) || Contains(resources, "VIDEO_CAPTURE");
            bool audio = Contains(resources, "AUDIO_CAPTURE");
            const string microphone = "android.permission.RECORD_AUDIO";
            bool needVideo = video && !Permission.HasUserAuthorizedPermission(Permission.Camera);
            bool needAudio = audio && !Permission.HasUserAuthorizedPermission(microphone);
            if (!needVideo && !needAudio)
                return 1;

            captureVideoPending = needVideo;
            captureAudioPending = needAudio;
            return 2;
#else
            return 1;
#endif
        };

        view.HandleRequest = (url, mainFrame, isPopup) =>
        {
            if (string.IsNullOrEmpty(url))
                return true;

            if (isPopup)
                return !TryOpenExternal(url);

            if (Contains(url, "pay.widget") || TryOpenExternal(url))
            {
                if (loader != null)
                    loader.SetActive(false);

                return false;
            }

            if (mainFrame &&
                ((IsGoogleAuth(url) && !googleUserAgentApplied) ||
                 (IsPayment(url) && !paymentUserAgentApplied)))
            {
                if (IsGoogleAuth(url))
                    googleUserAgentApplied = true;
                else
                    paymentUserAgentApplied = true;

                view.SetUserAgent(ProductAgent());
                view.Load(url);
                DebugMessage("Product user agent applied");
                return false;
            }

            return true;
        };

        view.PageStarted += url =>
        {
            if (prewarming && IsAboutBlank(url))
                return;

            DebugMessage("WebView page started");
            if (Contains(url, "pay.widget") || Contains(url, "diia.app") || TryOpenExternal(url))
            {
                view.Stop();
                OpenExternalIfHttp(url);
            }
        };

        view.PageCommitted += url =>
        {
            if (!prewarming && !firstLoadShown && IsHttp(url))
                ShowLoaded("commit");
        };

        view.ProgressChanged += progress =>
        {
            if (!prewarming && !firstLoadShown && progress >= 0.65f)
                ShowLoaded("progress");
        };

        view.PageFinished += (status, url) =>
        {
            if (prewarming && IsAboutBlank(url))
            {
                prewarming = false;
                DebugMessage("WebView prewarm finished");
                return;
            }

            DebugMessage("WebView page finished; status=" + status);
            if (!firstLoadShown)
                ShowLoaded("finish");
            else if (loader != null)
                loader.SetActive(false);

            RestoreUserAgent(url);
        };

        view.PageError += failing =>
        {
            DebugMessage("WebView loading error; url=" + failing);
            if (!string.IsNullOrEmpty(failing) && !IsAboutBlank(failing))
            {
                view.Stop();
                TryOpenExternal(failing);
            }
        };

        view.PopupOpened += id =>
        {
            view.ScrollToTop();
            DebugMessage("WebView popup opened: " + id);
        };

        view.PopupClosed += id => DebugMessage("WebView popup closed: " + id);
    }

    private void EnsureCaptureCallbacks()
    {
        if (captureCallbacks != null)
            return;

        captureCallbacks = new PermissionCallbacks();
        captureCallbacks.PermissionGranted += permission => OnCaptureAnswer(true);
        captureCallbacks.PermissionDenied += permission => OnCaptureAnswer(false);
        captureCallbacks.PermissionDeniedAndDontAskAgain += permission => OnCaptureAnswer(false);
    }

    private void AskPendingCapture()
    {
        if (!captureVideoPending && !captureAudioPending)
            return;

        EnsureCaptureCallbacks();
        captureWaits = 0;
        if (captureVideoPending)
        {
            captureVideoPending = false;
            captureWaits++;
            Permission.RequestUserPermission(Permission.Camera, captureCallbacks);
        }

        if (captureAudioPending)
        {
            captureAudioPending = false;
            captureWaits++;
            Permission.RequestUserPermission("android.permission.RECORD_AUDIO", captureCallbacks);
        }
    }

    private void OnCaptureAnswer(bool granted)
    {
        if (!granted)
        {
            captureWaits = 0;
            captureVideoPending = false;
            captureAudioPending = false;
            if (view != null)
                view.FinishCapture(false);
            return;
        }

        captureWaits--;
        if (captureWaits <= 0 && view != null)
        {
            captureWaits = 0;
            view.FinishCapture(true);
        }
    }

    private void ShowLoaded(string source)
    {
        firstLoadShown = true;
        HoldPanels();
        if (loader != null)
            loader.SetActive(false);

        ApplySafeArea();
        if (view != null)
            view.Show();

        webViewVisible = true;
        DebugMessage("WebView shown by " + source);
    }

    public void ShowNative()
    {
        DebugMessage("Switching to native content");
        if (view != null)
            view.Hide();

        ReleasePanels();
        webViewVisible = false;
        if (loader != null)
            loader.SetActive(false);

        if (exitRoot != null)
            exitRoot.SetActive(false);

        consent = false;
        onAccept = null;
        HideConsentBar();

        Action done = onCloseCallback;
        onCloseCallback = null;
        if (done != null)
            done.Invoke();
    }

    private void Update()
    {
        AskPendingCapture();
        SuppressPanels();

        if (!consent && BackReleased() && view != null && webViewVisible && !ControllerPrivacyPolicy.IsOpen)
        {
            if (exitRoot == null || !exitRoot.activeSelf)
                HandleBackPressed();

            return;
        }

        if (lastSafeArea != Screen.safeArea ||
            lastScreenSize.x != Screen.width ||
            lastScreenSize.y != Screen.height)
        {
            ApplySafeArea();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            ApplySafeArea();
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
            ApplySafeArea();
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

    private void HandleBackPressed()
    {
        try
        {
            if (view.CloseTopPopup())
            {
                DebugMessage("Android Back: closed popup");
                return;
            }

            string backUrl = view.CanGoBack ? view.BackUrl() : null;
            if (!string.IsNullOrWhiteSpace(backUrl) && !IsAboutBlank(backUrl))
            {
                DebugMessage("Android Back: navigating WebView history");
                view.GoBack();
                return;
            }
        }
        catch (Exception e)
        {
            DebugMessage("WebView history check failed: " + e.Message);
        }

        DebugMessage("Android Back: no real history, opening exit dialog");
        view.Hide();
        ShowExitDialog();
    }

    private void HoldPanels()
    {
        holdPanels = true;
        SuppressPanels();
    }

    private void ReleasePanels()
    {
        holdPanels = false;
    }

    private void SuppressPanels()
    {
        if (!holdPanels)
            return;

        PanelsFlow panels = PanelsFlow.instance;
        if (panels == null)
            return;

        SuppressPanel(panels.panel_loader);
        SuppressPanel(panels.panel_menu);
        SuppressPanel(panels.panel_game);
    }

    private static void SuppressPanel(GameObject panel)
    {
        if (panel != null && panel.activeSelf)
            panel.SetActive(false);
    }

    private Canvas EnsureCanvas()
    {
        if (rootCanvas != null)
            return rootCanvas;

        var go = new GameObject("HelpCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        go.transform.SetParent(transform, false);
        rootCanvas = go.GetComponent<Canvas>();
        rootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        rootCanvas.sortingOrder = short.MaxValue;
        rootCanvas.overrideSorting = true;
        EnsurePointer();
        return rootCanvas;
    }

    private void EnsurePointer()
    {
        EventSystem live = null;
        EventSystem[] all = Resources.FindObjectsOfTypeAll<EventSystem>();
        for (int i = 0; i < all.Length; i++)
        {
            EventSystem candidate = all[i];
            if (candidate == null || !candidate.isActiveAndEnabled)
                continue;
            if (candidate.gameObject.hideFlags != HideFlags.None)
                continue;

            live = candidate;
            break;
        }

        if (live == null)
        {
            var events = new GameObject("HelpPointer");
            events.transform.SetParent(transform, false);
            live = events.AddComponent<EventSystem>();
            events.AddComponent<InputSystemUIInputModule>();
            return;
        }

        if (live.GetComponent<InputSystemUIInputModule>() == null)
        {
            StandaloneInputModule oldModule = live.GetComponent<StandaloneInputModule>();
            if (oldModule != null)
                Destroy(oldModule);

            live.gameObject.AddComponent<InputSystemUIInputModule>();
        }
    }

    private void EnsureSafeArea()
    {
        if (safeAreaRect != null)
            return;

        var go = new GameObject("HelpSafeArea", typeof(RectTransform));
        safeAreaRect = go.GetComponent<RectTransform>();
        safeAreaRect.SetParent(EnsureCanvas().transform, false);
        safeAreaRect.anchorMin = Vector2.zero;
        safeAreaRect.anchorMax = Vector2.one;
        safeAreaRect.offsetMin = safeAreaRect.offsetMax = Vector2.zero;
    }

    private void ApplySafeArea()
    {
        EnsureSafeArea();
        if (Screen.width <= 0 || Screen.height <= 0)
            return;

        Rect area = Screen.safeArea;
        safeAreaRect.anchorMin = new Vector2(area.xMin / Screen.width, area.yMin / Screen.height);
        safeAreaRect.anchorMax = new Vector2(area.xMax / Screen.width, area.yMax / Screen.height);
        safeAreaRect.offsetMin = safeAreaRect.offsetMax = Vector2.zero;

        if (view != null && view.Ready)
        {
            int left = Mathf.Max(0, Mathf.RoundToInt(area.xMin));
            int right = Mathf.Max(0, Mathf.RoundToInt(Screen.width - area.xMax));
            int bottom = Mathf.Max(0, Mathf.RoundToInt(area.yMin));
            if (consent)
                bottom += ConsentBarPixels();
            int top = Mathf.Max(0, Mathf.RoundToInt(Screen.height - area.yMax));
            view.SetMargins(left, top, right, bottom, Screen.width, Screen.height);
            LayoutConsentBar(area);
        }

        lastSafeArea = Screen.safeArea;
        lastScreenSize = new Vector2(Screen.width, Screen.height);
        DebugMessage("WebView frame updated: " + Screen.width + "x" + Screen.height);
    }

    private void ShowExitDialog()
    {
        webViewVisible = false;
        EnsureCanvas();
        rootCanvas.gameObject.SetActive(true);
        EnsureExitDialog();
        exitText.text = exitLines[UnityEngine.Random.Range(0, exitLines.Length)];
        stayButton.onClick.RemoveAllListeners();
        leaveButton.onClick.RemoveAllListeners();
        stayButton.onClick.AddListener(() =>
        {
            exitRoot.SetActive(false);
            if (view != null)
                view.Show();
            webViewVisible = true;
            DebugMessage("Exit dialog: player stayed");
        });
        leaveButton.onClick.AddListener(() =>
        {
            exitRoot.SetActive(false);
            if (view != null)
                view.Hide();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
        exitRoot.SetActive(true);
        exitRoot.transform.SetAsLastSibling();
        DebugMessage("WebView exit dialog shown");
    }

    private void EnsureExitDialog()
    {
        if (exitRoot != null)
            return;

        EnsureSafeArea();
        Sprite uiSprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
        exitRoot = new GameObject("HelpExitDialog", typeof(RectTransform), typeof(Image));
        var root = exitRoot.GetComponent<RectTransform>();
        root.SetParent(safeAreaRect, false);
        root.anchorMin = Vector2.zero;
        root.anchorMax = Vector2.one;
        root.offsetMin = root.offsetMax = Vector2.zero;
        Image dim = exitRoot.GetComponent<Image>();
        dim.sprite = uiSprite;
        dim.type = Image.Type.Sliced;
        dim.color = new Color(0f, 0f, 0f, 0.65f);

        var panel = new GameObject("Panel", typeof(RectTransform), typeof(Image));
        var panelRect = panel.GetComponent<RectTransform>();
        panelRect.SetParent(root, false);
        panelRect.anchorMin = panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(Mathf.Min(Screen.width * 0.86f, 720f), 380f);
        Image panelImage = panel.GetComponent<Image>();
        panelImage.sprite = uiSprite;
        panelImage.type = Image.Type.Sliced;
        panelImage.color = Color.white;

        var textObject = new GameObject("Text", typeof(RectTransform), typeof(Text));
        var textRect = textObject.GetComponent<RectTransform>();
        textRect.SetParent(panelRect, false);
        textRect.anchorMin = new Vector2(0.08f, 0.42f);
        textRect.anchorMax = new Vector2(0.92f, 0.92f);
        textRect.offsetMin = textRect.offsetMax = Vector2.zero;
        exitText = textObject.GetComponent<Text>();
        exitText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        exitText.fontSize = 32;
        exitText.fontStyle = FontStyle.Bold;
        exitText.alignment = TextAnchor.MiddleCenter;
        exitText.color = new Color(0.12f, 0.12f, 0.15f);

        stayButton = CreateExitButton("Stay", panelRect, new Vector2(0.08f, 0.1f), new Vector2(0.48f, 0.34f), new Color(0.13f, 0.77f, 0.37f), uiSprite);
        leaveButton = CreateExitButton("Exit", panelRect, new Vector2(0.52f, 0.1f), new Vector2(0.92f, 0.34f), new Color(0.95f, 0.96f, 0.98f), uiSprite);
        exitRoot.SetActive(false);
    }

    private static Button CreateExitButton(string label, Transform parent, Vector2 min, Vector2 max, Color color, Sprite sprite)
    {
        var buttonObject = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button));
        var rect = buttonObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        Image image = buttonObject.GetComponent<Image>();
        image.sprite = sprite;
        image.type = Image.Type.Sliced;
        image.color = color;

        var textObject = new GameObject("Text", typeof(RectTransform), typeof(Text));
        var textRect = textObject.GetComponent<RectTransform>();
        textRect.SetParent(rect, false);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = textRect.offsetMax = Vector2.zero;
        var text = textObject.GetComponent<Text>();
        text.text = label;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 28;
        text.alignment = TextAnchor.MiddleCenter;
        return buttonObject.GetComponent<Button>();
    }

    private void RestoreUserAgent(string url)
    {
        bool keepGoogle = googleUserAgentApplied &&
                          (IsGoogleAuth(url) || Contains(url, "googleusercontent.com") || Contains(url, "gstatic.com"));
        bool keepPayment = paymentUserAgentApplied && IsPayment(url);
        if ((googleUserAgentApplied || paymentUserAgentApplied) && !keepGoogle && !keepPayment)
        {
            googleUserAgentApplied = paymentUserAgentApplied = false;
            if (view != null)
                view.SetUserAgent("");
            DebugMessage("Default user agent restored");
        }
    }

    private void OpenExternalIfHttp(string url)
    {
        if (IsHttp(url))
            Application.OpenURL(url);
    }

    private bool TryOpenExternal(string url)
    {
        if (string.IsNullOrEmpty(url))
            return false;

#if UNITY_ANDROID && !UNITY_EDITOR
        if (url.StartsWith("intent://", StringComparison.OrdinalIgnoreCase)
            || IsMarket(url)
            || (!IsHttp(url) && !IsAboutBlank(url)))
        {
            return OpenAndroidUrl(url);
        }
#endif
        return false;
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private bool OpenAndroidUrl(string url)
    {
        try
        {
            using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = player.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var uriClass = new AndroidJavaClass("android.net.Uri"))
            using (var uri = uriClass.CallStatic<AndroidJavaObject>("parse", url))
            using (var intent = new AndroidJavaObject("android.content.Intent", "android.intent.action.VIEW", uri))
            {
                intent.Call<AndroidJavaObject>("addCategory", "android.intent.category.BROWSABLE");
                intent.Call<AndroidJavaObject>("addFlags", 0x10000000);
                activity.Call("startActivity", intent);
                return true;
            }
        }
        catch (Exception e)
        {
            DebugMessage("External URL failed: " + e.Message);
            Application.OpenURL(url);
            return true;
        }
    }
#endif

    private static bool IsHttp(string value)
    {
        return Uri.TryCreate(value, UriKind.Absolute, out Uri uri)
               && (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp);
    }

    private static bool IsAboutBlank(string value)
    {
        return !string.IsNullOrEmpty(value)
               && value.StartsWith("about:blank", StringComparison.OrdinalIgnoreCase);
    }

    private static bool Contains(string source, string value)
    {
        return !string.IsNullOrEmpty(source)
               && source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private static bool IsGoogleAuth(string url)
    {
        return Contains(url, "accounts.google.");
    }

    private static bool IsPayment(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            return false;

        return Contains(uri.Host, "pay")
               || Contains(uri.Host, "checkout")
               || Contains(uri.Host, "billing")
               || Contains(uri.AbsolutePath, "/pay")
               || Contains(uri.AbsolutePath, "/checkout")
               || Contains(uri.AbsolutePath, "/payment")
               || Contains(uri.AbsolutePath, "/order")
               || Contains(uri.AbsolutePath, "/billing");
    }

    private static bool IsMarket(string url)
    {
        return url.StartsWith("market://", StringComparison.OrdinalIgnoreCase)
               || url.StartsWith("https://play.google.com/", StringComparison.OrdinalIgnoreCase)
               || url.StartsWith("http://play.google.com/", StringComparison.OrdinalIgnoreCase);
    }

    private void EnsureConsentBar()
    {
        if (consentRoot != null)
            return;

        EnsureCanvas();
        EnsureSafeArea();
        Sprite uiSprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
        consentRoot = new GameObject("PolicyChoice", typeof(RectTransform), typeof(Image));
        var root = consentRoot.GetComponent<RectTransform>();
        root.SetParent(safeAreaRect, false);
        Image bar = consentRoot.GetComponent<Image>();
        bar.sprite = uiSprite;
        bar.type = Image.Type.Sliced;
        bar.color = new Color(0.08f, 0.08f, 0.1f, 1f);

        CreateChoiceButton("Decline", root, new Vector2(0.04f, 0.12f), new Vector2(0.48f, 0.88f), new Color(0.95f, 0.96f, 0.98f), uiSprite, DeclinePolicy);
        CreateChoiceButton("Accept", root, new Vector2(0.52f, 0.12f), new Vector2(0.96f, 0.88f), new Color(0.13f, 0.77f, 0.37f), uiSprite, AcceptPolicy);
        LayoutConsentBar(Screen.safeArea);
    }

    private void LayoutConsentBar(Rect area)
    {
        if (consentRoot == null || Screen.width <= 0 || Screen.height <= 0)
            return;

        int bar = ConsentBarPixels();
        var root = consentRoot.GetComponent<RectTransform>();
        root.anchorMin = new Vector2(0f, 0f);
        root.anchorMax = new Vector2(1f, 0f);
        root.pivot = new Vector2(0.5f, 0f);
        root.sizeDelta = new Vector2(0f, bar);
        root.anchoredPosition = Vector2.zero;
        consentRoot.SetActive(consent);
        consentRoot.transform.SetAsLastSibling();
    }

    private void HideConsentBar()
    {
        if (consentRoot != null)
            consentRoot.SetActive(false);
    }

    private void AcceptPolicy()
    {
        Action accept = onAccept;
        consent = false;
        onAccept = null;
        HideConsentBar();
        if (view != null)
            view.Hide();
        ReleasePanels();
        webViewVisible = false;
        if (loader != null)
            loader.SetActive(false);
        accept?.Invoke();
    }

    private void DeclinePolicy()
    {
        DebugMessage("Policy declined");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private static int ConsentBarPixels()
    {
        float dpi = Screen.dpi > 0f ? Screen.dpi : 160f;
        return Mathf.Max(64, Mathf.RoundToInt(64f * dpi / 160f));
    }

    private static void CreateChoiceButton(string label, Transform parent, Vector2 min, Vector2 max, Color color, Sprite sprite, UnityEngine.Events.UnityAction click)
    {
        var buttonObject = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button));
        var rect = buttonObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        Image image = buttonObject.GetComponent<Image>();
        image.sprite = sprite;
        image.type = Image.Type.Sliced;
        image.color = color;
        buttonObject.GetComponent<Button>().onClick.AddListener(click);

        var textObject = new GameObject("Text", typeof(RectTransform), typeof(Text));
        var textRect = textObject.GetComponent<RectTransform>();
        textRect.SetParent(rect, false);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = textRect.offsetMax = Vector2.zero;
        var text = textObject.GetComponent<Text>();
        text.text = label;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 28;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = new Color(0.12f, 0.12f, 0.15f, 1f);
    }

    private void DebugMessage(string text)
    {
        Debug.Log("[Debug] " + text);
    }

    private void OnDestroy()
    {
        if (view == null)
            return;

        view.Destroy();
        view = null;
    }
}
