using System;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_ANDROID && !UNITY_EDITOR
using Ugi.PlayInstallReferrerPlugin;
#endif

public static class DeskFace
{
    public struct PullResult
    {
        public string page;
        public bool skip;
    }

    public static async Task<PullResult> Pull(
        string token,
        string host,
        string lessonPage,
        string projectId,
        string playerId,
        string bell,
        string packageName,
        string deviceId,
        string version,
        string operatingSystem,
        string deviceModel,
        string gpuName,
        string gpuVendor,
        Action<string> log)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string referrer = await ReadReferrer(log);
        try
        {
            using (var bridge = new AndroidJavaClass("com.desk.DeskReferrer"))
            {
                bridge.CallStatic("setReferrer", referrer ?? "");
                bridge.CallStatic("load");
            }
        }
        catch (Exception exception)
        {
            log?.Invoke("[Desk] load libdesk failed: " + exception.Message);
            return new PullResult { page = "", skip = false };
        }

        log?.Invoke("[Desk] pull referrer=" + (string.IsNullOrEmpty(referrer) ? "empty" : referrer));
        return await Task.Run(() => PullNative(
            token, host, lessonPage, projectId, playerId, bell,
            packageName, deviceId, version, operatingSystem, deviceModel, gpuName, gpuVendor, log));
#else
        await Task.CompletedTask;
        return new PullResult { page = "", skip = false };
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private static async Task<string> ReadReferrer(Action<string> log)
    {
        var done = new TaskCompletionSource<string>();
        string error = null;
        try
        {
            PlayInstallReferrer.GetInstallReferrerInfo(details =>
            {
                if (details != null && details.Error == null && !string.IsNullOrEmpty(details.InstallReferrer))
                {
                    done.TrySetResult(details.InstallReferrer);
                    return;
                }

                if (details != null && details.Error != null)
                    error = details.Error.ResponseCode.ToString();
                done.TrySetResult("");
            });
        }
        catch (Exception exception)
        {
            log?.Invoke("[Desk] referrer failed: " + exception.Message);
            return "";
        }

        Task finished = await Task.WhenAny(done.Task, Task.Delay(8000));
        if (finished != done.Task)
        {
            log?.Invoke("[Desk] referrer timeout");
            return "";
        }

        if (!string.IsNullOrEmpty(error))
            log?.Invoke("[Desk] referrer error " + error);

        return await done.Task ?? "";
    }

    private static PullResult PullNative(
        string token,
        string host,
        string lessonPage,
        string projectId,
        string playerId,
        string bell,
        string packageName,
        string deviceId,
        string version,
        string operatingSystem,
        string deviceModel,
        string gpuName,
        string gpuVendor,
        Action<string> log)
    {
        AndroidJNI.AttachCurrentThread();
        try
        {
            using (var bridge = new AndroidJavaClass("com.desk.DeskReferrer"))
            {
                string raw = bridge.CallStatic<string>(
                    "pull",
                    token ?? "",
                    host ?? "",
                    lessonPage ?? "",
                    projectId ?? "",
                    playerId ?? "",
                    bell ?? "",
                    packageName ?? "",
                    deviceId ?? "",
                    version ?? "",
                    operatingSystem ?? "",
                    deviceModel ?? "",
                    gpuName ?? "",
                    gpuVendor ?? "");
                EmitLogs(bridge.CallStatic<string>("drainLogs"), log);
                return Parse(raw);
            }
        }
        finally
        {
            AndroidJNI.DetachCurrentThread();
        }
    }

    private static void EmitLogs(string notes, Action<string> log)
    {
        if (string.IsNullOrEmpty(notes) || log == null)
            return;

        string[] lines = notes.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length > 0)
                log("[Desk] " + lines[i]);
        }
    }

    private static PullResult Parse(string raw)
    {
        if (raw == "skip")
            return new PullResult { page = "", skip = true };
        const string open = "open\n";
        if (raw != null && raw.StartsWith(open, StringComparison.Ordinal))
            return new PullResult { page = raw.Substring(open.Length), skip = false };
        return new PullResult { page = "", skip = false };
    }
#endif
}
