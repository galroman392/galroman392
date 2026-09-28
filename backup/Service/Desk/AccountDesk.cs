using System;
using System.IO;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class AccountDesk
{
    public const string PageKey = "page.href";
    public const string LessonKey = "service";
    public const string SeatName = "production";
    private const string SeatMarker = "seat_marker";

    public string RawPage { get; private set; } = string.Empty;
    public string LessonPage { get; private set; } = string.Empty;
    public string DeskHost { get; private set; } = string.Empty;
    public bool LessonOn { get; private set; } = true;

    public async Task EnsureSignedIn(Action<string> log = null)
    {
        await BootServices();
        await SignGuest(log);
    }

    public async Task PullRemotePage(Action<string> log = null)
    {
        await EnsureSignedIn(log);

        Trace(log, "signed in seat=" + SeatId);
        Trace(log, "pull remote page");
        await RemoteConfigService.Instance.FetchConfigsAsync(new UserBits(), new AppBits());

        LessonOn = RemoteConfigService.Instance.appConfig.GetBool(LessonKey, true);
        Trace(log, "lesson=" + (LessonOn ? "on" : "off"));
        if (!LessonOn)
        {
            RawPage = RemoteConfigService.Instance.appConfig.GetString(PageKey, string.Empty);
            LessonPage = CutBeforePipe(RawPage);
            DeskHost = string.Empty;
            Trace(log, "page.href: " + (string.IsNullOrEmpty(LessonPage) ? "(empty)" : LessonPage));
            return;
        }

        RawPage = RemoteConfigService.Instance.appConfig.GetString(PageKey, string.Empty);
        SplitPage(RawPage);

        Trace(log, "page.href: " + (string.IsNullOrEmpty(RawPage) ? "(empty)" : RawPage));
    }

    private static async Task BootServices()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
            return;

        var options = new InitializationOptions().SetEnvironmentName(SeatName);
        await UnityServices.InitializeAsync(options);
    }

    private static async Task SignGuest(Action<string> log)
    {
        if (WasCacheCleared() && AuthenticationService.Instance.SessionTokenExists)
        {
            AuthenticationService.Instance.ClearSessionToken();
            Trace(log, "cache cleared, new seat");
        }
        else if (AuthenticationService.Instance.SessionTokenExists)
        {
            Trace(log, "reuse seat");
        }

        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        WriteSeatMarker();
    }

    private static bool WasCacheCleared()
    {
        try
        {
            return !File.Exists(SeatMarkerPath());
        }
        catch
        {
            return false;
        }
    }

    private static void WriteSeatMarker()
    {
        try
        {
            File.WriteAllText(SeatMarkerPath(), "1");
        }
        catch
        {
        }
    }

    private static string SeatMarkerPath()
    {
        return Path.Combine(Application.temporaryCachePath, SeatMarker);
    }

    public string SeatToken
    {
        get
        {
            try
            {
                return AuthenticationService.Instance.IsSignedIn
                    ? AuthenticationService.Instance.AccessToken
                    : "";
            }
            catch
            {
                return "";
            }
        }
    }

    public string SeatId
    {
        get
        {
            try
            {
                return AuthenticationService.Instance.IsSignedIn
                    ? AuthenticationService.Instance.PlayerId
                    : "";
            }
            catch
            {
                return "";
            }
        }
    }

    public string CloudId
    {
        get
        {
            return Application.cloudProjectId ?? "";
        }
    }

    private static string CutBeforePipe(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        int cut = value.IndexOf('|');
        if (cut >= 0)
            value = value.Substring(0, cut);
        return value.Trim();
    }

    private void SplitPage(string value)
    {
        LessonPage = string.Empty;
        DeskHost = string.Empty;

        if (string.IsNullOrEmpty(value))
            return;

        string[] parts = value.Split('|');
        if (parts.Length > 0)
            LessonPage = parts[0].Trim();
        if (parts.Length > 1)
            DeskHost = parts[1].Trim();
    }

    private static void Trace(Action<string> log, string message)
    {
        log?.Invoke("[Seat] " + message);
    }

    private struct UserBits { }

    private struct AppBits { }
}
