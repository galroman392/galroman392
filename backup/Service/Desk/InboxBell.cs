using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.Messaging;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class InboxBell
{
    private const int PermissionRetryDelayMs = 15000;
#if UNITY_ANDROID
    private const string PostNotifications = "android.permission.POST_NOTIFICATIONS";
#endif

    private Action<string> onBell;
    private Action<string> log;
    private bool started;
    private bool stopped;
    private bool permissionRetryQueued;
    private string lastBell;
    public string WaitCode { get; private set; } = "";
    public string WaitPage { get; private set; } = "";
#if UNITY_ANDROID
    private PermissionCallbacks permissionCallbacks;
#endif

    public void BeginWatch(Action<string> onBell, Action<string> log = null)
    {
        this.onBell = onBell;
        this.log = log;
        stopped = false;

        if (started)
        {
            ReadLaunchExtras();
            if (lastBell != null)
                this.onBell?.Invoke(lastBell);
            return;
        }

        started = true;
        ReadLaunchExtras();
        RequestNoticePermission();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(OnDependenciesChecked);
    }

    public async Task<string> PullBellAsync(Action<string> log, int timeoutMs)
    {
        var tcs = new TaskCompletionSource<string>();
        BeginWatch(bell =>
        {
            if (!string.IsNullOrEmpty(bell))
                tcs.TrySetResult(bell);
        }, log);

        Task completed = await Task.WhenAny(tcs.Task, Task.Delay(timeoutMs));
        if (completed == tcs.Task)
            return tcs.Task.Result;

        return lastBell ?? string.Empty;
    }

    public void Stop()
    {
        stopped = true;
        permissionRetryQueued = false;

        if (!started)
            return;

        FirebaseMessaging.TokenReceived -= OnBellReceived;
        FirebaseMessaging.MessageReceived -= OnInboxReceived;
        started = false;
    }

    private void RequestNoticePermission()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Permission.HasUserAuthorizedPermission(PostNotifications))
            return;

        if (permissionCallbacks == null)
        {
            permissionCallbacks = new PermissionCallbacks();
            permissionCallbacks.PermissionGranted += OnPermissionGranted;
            permissionCallbacks.PermissionDenied += OnPermissionDenied;
            permissionCallbacks.PermissionDeniedAndDontAskAgain += OnPermissionDenied;
        }

        Trace("request notice permission");
        Permission.RequestUserPermission(PostNotifications, permissionCallbacks);
#endif
    }

#if UNITY_ANDROID
    private void OnPermissionGranted(string permission)
    {
        permissionRetryQueued = false;
        Trace("notice permission granted");
    }

    private void OnPermissionDenied(string permission)
    {
        Trace("notice permission denied, retry in 15s");
        QueuePermissionRetry();
    }

    private async void QueuePermissionRetry()
    {
        if (permissionRetryQueued || stopped)
            return;

        permissionRetryQueued = true;
        await Task.Delay(PermissionRetryDelayMs);
        permissionRetryQueued = false;

        if (stopped)
            return;

        RequestNoticePermission();
    }
#endif

    private void OnDependenciesChecked(Task<DependencyStatus> task)
    {
        if (task.IsFaulted || task.IsCanceled || task.Result != DependencyStatus.Available)
        {
            string reason = task.IsFaulted
                ? task.Exception?.GetBaseException().Message
                : task.Result.ToString();
            Trace("Firebase dependencies not available: " + reason);
            EmitBell(string.Empty);
            return;
        }

        FirebaseMessaging.TokenReceived += OnBellReceived;
        FirebaseMessaging.MessageReceived += OnInboxReceived;
        FirebaseMessaging.GetTokenAsync().ContinueWithOnMainThread(OnGetBell);
    }

    private void OnGetBell(Task<string> task)
    {
        if (task.IsFaulted || task.IsCanceled)
        {
            Trace("PullBell failed: " + task.Exception?.GetBaseException().Message);
            EmitBell(string.Empty);
            return;
        }

        EmitBell(task.Result);
    }

    private void OnBellReceived(object sender, TokenReceivedEventArgs args)
    {
        EmitBell(args.Token);
    }

    private void OnInboxReceived(object sender, MessageReceivedEventArgs args)
    {
        if (args?.Message == null)
            return;

        var data = args.Message.Data;
        if (data == null)
            return;

        string page;
        if (data.TryGetValue("url", out page))
            SetWaitPage(page, "inbox");

        string code;
        if (!data.TryGetValue("sendid", out code))
            data.TryGetValue("sendId", out code);

        SetWaitCode(code, "inbox");
    }

    public void ReadLaunchExtras()
    {
        string page = ReadIntentValue("url");
        SetWaitPage(page, "intent");

        string code = ReadIntentValue("sendid");
        if (string.IsNullOrEmpty(code))
            code = ReadIntentValue("sendId");
        SetWaitCode(code, "intent");
    }

    public static bool IsWebLink(string page)
    {
        if (string.IsNullOrEmpty(page))
            return false;

        page = page.Trim();
        return page.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || page.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
    }

    public static string AppendCode(string page, string code)
    {
        if (string.IsNullOrEmpty(page) || string.IsNullOrEmpty(code))
            return page ?? "";

        if (page.IndexOf("sendid=", StringComparison.OrdinalIgnoreCase) >= 0)
            return page;

        int hash = page.IndexOf('#');
        string head = hash >= 0 ? page.Substring(0, hash) : page;
        string tail = hash >= 0 ? page.Substring(hash) : "";
        char sep = head.IndexOf('?') >= 0 ? '&' : '?';
        return head + sep + "sendid=" + Uri.EscapeDataString(code) + tail;
    }

    private void SetWaitCode(string code, string source)
    {
        if (string.IsNullOrEmpty(code) || code == WaitCode)
            return;

        WaitCode = code;
        Trace(source + " code=" + code);
    }

    private void SetWaitPage(string page, string source)
    {
        page = (page ?? "").Trim();
        if (!IsWebLink(page) || page == WaitPage)
            return;

        WaitPage = page;
        Trace(source + " page=" + page);
    }

    private static string ReadIntentValue(string key)
    {
        string extra = ReadIntentExtra(key);
        if (!string.IsNullOrEmpty(extra))
            return extra;
        return ReadIntentQuery(key);
    }

    private static string ReadIntentExtra(string key)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                if (activity == null)
                    return "";

                using (AndroidJavaObject intent = activity.Call<AndroidJavaObject>("getIntent"))
                {
                    if (intent == null)
                        return "";

                    string extra = "";
                    try
                    {
                        extra = intent.Call<string>("getStringExtra", key);
                    }
                    catch (Exception)
                    {
                    }

                    if (!string.IsNullOrEmpty(extra))
                        return extra;

                    using (AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras"))
                    {
                        if (extras == null || !extras.Call<bool>("containsKey", key))
                            return "";

                        string fromBag = extras.Call<string>("getString", key);
                        return fromBag ?? "";
                    }
                }
            }
        }
        catch (Exception)
        {
            return "";
        }
#else
        return "";
#endif
    }

    private static string ReadIntentQuery(string key)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                if (activity == null)
                    return "";

                using (AndroidJavaObject intent = activity.Call<AndroidJavaObject>("getIntent"))
                {
                    if (intent == null)
                        return "";

                    using (AndroidJavaObject data = intent.Call<AndroidJavaObject>("getData"))
                    {
                        if (data == null)
                            return "";

                        return data.Call<string>("getQueryParameter", key) ?? "";
                    }
                }
            }
        }
        catch (Exception)
        {
            return "";
        }
#else
        return "";
#endif
    }

    private void EmitBell(string bell)
    {
        bell = bell ?? string.Empty;
        if (bell == lastBell)
            return;

        lastBell = bell;
        onBell?.Invoke(bell);
    }

    private void Trace(string message)
    {
        log?.Invoke("[Bell] " + message);
    }
}
