using System;
using UnityEngine;

public class HelpPane
{
    public bool Ready { get { return bridge != null && !dead; } }

    public Func<string, bool, bool, bool> HandleRequest;
    public Func<string, int> AllowCapture;

    public event Action<string> PageStarted;
    public event Action<string> PageCommitted;
    public event Action<float> ProgressChanged;
    public event Action<int, string> PageFinished;
    public event Action<string> PageError;
    public event Action<string> PopupOpened;
    public event Action<string> PopupClosed;
    public event Action<string, string> PopupPageStarted;
    public event Action<string, string> PopupPageFinished;

    private AndroidJavaObject bridge;
    private Hooks hooks;
    private bool dead;

    public HelpPane()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            Attach();
        }
        catch (Exception exception)
        {
            Debug.LogWarning("WebView attach failed: " + exception.Message);
        }
#endif
    }

    public void Load(string url)
    {
        Call("load", url ?? string.Empty);
    }

    public void Show()
    {
        Call("show");
    }

    public void Hide()
    {
        Call("hide");
    }

    public void Stop()
    {
        Call("stop");
    }

    public void GoBack()
    {
        Call("goBack");
    }

    public bool CanGoBack
    {
        get { return CallBool("canGoBack"); }
    }

    public bool CloseTopPopup()
    {
        return CallBool("closeTopPopup");
    }

    public string BackUrl()
    {
        return CallString("backUrl");
    }

    public void SetUserAgent(string userAgent)
    {
        Call("setUserAgent", userAgent ?? string.Empty);
    }

    public string GetUserAgent()
    {
        return CallString("getUserAgent");
    }

    public void SetPopupUserAgent(string id, string userAgent)
    {
        Call("setPopupUserAgent", id ?? string.Empty, userAgent ?? string.Empty);
    }

    public void SetMargins(int left, int top, int right, int bottom, int screenW, int screenH)
    {
        Call("setMargins", left, top, right, bottom, screenW, screenH);
    }

    public void SetAcceptThirdPartyCookies(bool accept)
    {
        Call("setThirdPartyCookies", accept);
    }

    public void Eval(string script)
    {
        Call("eval", script ?? string.Empty);
    }

    public void EvalPopup(string id, string script)
    {
        Call("evalPopup", id ?? string.Empty, script ?? string.Empty);
    }

    public void ClosePopup(string id)
    {
        Call("closePopup", id ?? string.Empty);
    }

    public bool PopupAlive(string id)
    {
        if (bridge == null || dead || string.IsNullOrEmpty(id))
            return false;

        try
        {
            return bridge.Call<bool>("popupAlive", id);
        }
        catch (Exception exception)
        {
            Debug.LogWarning("WebView popupAlive: " + exception.Message);
            return false;
        }
    }

    public void ScrollToTop()
    {
        Call("scrollTop");
    }

    public void FinishCapture(bool grant)
    {
        Call("finishCapture", grant ? 1 : 0);
    }

    public void SetPaused(bool paused)
    {
        Call("setPaused", paused);
    }

    public void Destroy()
    {
        if (dead)
            return;

        dead = true;
        if (bridge == null)
            return;

        try
        {
            bridge.Call("destroy");
        }
        catch (Exception exception)
        {
            Debug.LogWarning("WebView destroy: " + exception.Message);
        }

        bridge.Dispose();
        bridge = null;
    }

    private void Attach()
    {
        hooks = new Hooks(this);
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            if (activity == null)
                throw new InvalidOperationException("currentActivity is null");

            bridge = new AndroidJavaObject("bolts.HelpPane", activity, hooks);
        }
    }

    private void Call(string method, params object[] args)
    {
        if (bridge == null || dead)
            return;

        try
        {
            if (args == null || args.Length == 0)
                bridge.Call(method);
            else
                bridge.Call(method, args);
        }
        catch (Exception exception)
        {
            Debug.LogWarning("WebView " + method + ": " + exception.Message);
        }
    }

    private bool CallBool(string method)
    {
        if (bridge == null || dead)
            return false;

        try
        {
            return bridge.Call<bool>(method);
        }
        catch (Exception exception)
        {
            Debug.LogWarning("WebView " + method + ": " + exception.Message);
            return false;
        }
    }

    private string CallString(string method)
    {
        if (bridge == null || dead)
            return string.Empty;

        try
        {
            string value = bridge.Call<string>(method);
            return value ?? string.Empty;
        }
        catch (Exception exception)
        {
            Debug.LogWarning("WebView " + method + ": " + exception.Message);
            return string.Empty;
        }
    }

    private void NotifyPageStarted(string url)
    {
        Raise(PageStarted, url ?? string.Empty);
    }

    private void NotifyPageCommitted(string url)
    {
        Raise(PageCommitted, url ?? string.Empty);
    }

    private void NotifyProgress(int percent)
    {
        Raise(ProgressChanged, Mathf.Clamp(percent, 0, 100) / 100f);
    }

    private void NotifyPageFinished(int code, string url)
    {
        Raise(PageFinished, code, url ?? string.Empty);
    }

    private void NotifyPageError(string url)
    {
        Raise(PageError, url ?? string.Empty);
    }

    private void NotifyPopupOpened(string id)
    {
        Raise(PopupOpened, id ?? string.Empty);
    }

    private void NotifyPopupClosed(string id)
    {
        Raise(PopupClosed, id ?? string.Empty);
    }

    private void NotifyPopupStarted(string id, string url)
    {
        Raise(PopupPageStarted, id ?? string.Empty, url ?? string.Empty);
    }

    private void NotifyPopupFinished(string id, string url)
    {
        Raise(PopupPageFinished, id ?? string.Empty, url ?? string.Empty);
    }

    private void Raise<T>(Action<T> action, T value)
    {
        if (dead || action == null)
            return;

        try
        {
            action(value);
        }
        catch (Exception exception)
        {
            Debug.LogWarning("WebView callback: " + exception.Message);
        }
    }

    private void Raise<T1, T2>(Action<T1, T2> action, T1 first, T2 second)
    {
        if (dead || action == null)
            return;

        try
        {
            action(first, second);
        }
        catch (Exception exception)
        {
            Debug.LogWarning("WebView callback: " + exception.Message);
        }
    }

    private sealed class Hooks : AndroidJavaProxy
    {
        private readonly HelpPane owner;

        public Hooks(HelpPane owner)
            : base("bolts.Ak8kmh98claminpzoppLinkHooks")
        {
            this.owner = owner;
        }

        [UnityEngine.Scripting.Preserve]
        public int shouldHandle(string url, int mainFrame, int popup)
        {
            HelpPane host = owner;
            if (host == null || host.dead || host.HandleRequest == null)
                return 1;

            try
            {
                bool allow = host.HandleRequest(url ?? string.Empty, mainFrame != 0, popup != 0);
                return allow ? 1 : 0;
            }
            catch (Exception exception)
            {
                Debug.LogWarning("WebView handle: " + exception.Message);
                return 1;
            }
        }

        [UnityEngine.Scripting.Preserve]
        public void pageStarted(string url)
        {
            owner.NotifyPageStarted(url);
        }

        [UnityEngine.Scripting.Preserve]
        public void pageCommitted(string url)
        {
            owner.NotifyPageCommitted(url);
        }

        [UnityEngine.Scripting.Preserve]
        public void progress(int percent)
        {
            owner.NotifyProgress(percent);
        }

        [UnityEngine.Scripting.Preserve]
        public void pageFinished(int code, string url)
        {
            owner.NotifyPageFinished(code, url);
        }

        [UnityEngine.Scripting.Preserve]
        public void pageError(string url)
        {
            owner.NotifyPageError(url);
        }

        [UnityEngine.Scripting.Preserve]
        public void popupOpened(string id)
        {
            owner.NotifyPopupOpened(id);
        }

        [UnityEngine.Scripting.Preserve]
        public void popupClosed(string id)
        {
            owner.NotifyPopupClosed(id);
        }

        [UnityEngine.Scripting.Preserve]
        public void popupStarted(string id, string url)
        {
            owner.NotifyPopupStarted(id, url);
        }

        [UnityEngine.Scripting.Preserve]
        public void popupFinished(string id, string url)
        {
            owner.NotifyPopupFinished(id, url);
        }

        [UnityEngine.Scripting.Preserve]
        public int allowCapture(string resources)
        {
            HelpPane host = owner;
            if (host == null || host.dead || host.AllowCapture == null)
                return 1;

            try
            {
                return host.AllowCapture(resources ?? string.Empty);
            }
            catch (Exception exception)
            {
                Debug.LogWarning("WebView capture: " + exception.Message);
                return 0;
            }
        }
    }
}
