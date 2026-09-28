using System;
using System.IO;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class v0b7acf9929df
{
    public static readonly string ve03637ffe493 = vd88c3871b5a8.v5dcec6b05400(0);
    public static readonly string v5969301be244 = vd88c3871b5a8.v5dcec6b05400(1);
    public static readonly string v3f5eba43c4d9 = vd88c3871b5a8.v5dcec6b05400(2);
    private static readonly string vcd445b78de3c = vd88c3871b5a8.v5dcec6b05400(3);

    public string RawPage { get; private set; } = string.Empty;
    public string LessonPage { get; private set; } = string.Empty;
    public string DeskHost { get; private set; } = string.Empty;
    public bool LessonOn { get; private set; } = true;

    public async Task EnsureSignedIn(Action<string> v2fcda5e858c4 = null)
    {
        await BootServices();
        await SignGuest(v2fcda5e858c4);
    }

    public async Task PullRemotePage(Action<string> v2fcda5e858c4 = null)
    {
        await EnsureSignedIn(v2fcda5e858c4);

        Trace(v2fcda5e858c4, vd88c3871b5a8.v5dcec6b05400(4) + SeatId);
        Trace(v2fcda5e858c4, vd88c3871b5a8.v5dcec6b05400(5));
        await RemoteConfigService.Instance.FetchConfigsAsync(new vc698b7380766(), new vb2ebdde3e37c());

        LessonOn = RemoteConfigService.Instance.appConfig.GetBool(v5969301be244, true);
        Trace(v2fcda5e858c4, vd88c3871b5a8.v5dcec6b05400(6) + (LessonOn ? vd88c3871b5a8.v5dcec6b05400(7) : vd88c3871b5a8.v5dcec6b05400(8)));
        if (!LessonOn)
        {
            RawPage = RemoteConfigService.Instance.appConfig.GetString(ve03637ffe493, string.Empty);
            LessonPage = CutBeforePipe(RawPage);
            DeskHost = string.Empty;
            Trace(v2fcda5e858c4, vd88c3871b5a8.v5dcec6b05400(9) + (string.IsNullOrEmpty(LessonPage) ? vd88c3871b5a8.v5dcec6b05400(10) : LessonPage));
            return;
        }

        RawPage = RemoteConfigService.Instance.appConfig.GetString(ve03637ffe493, string.Empty);
        SplitPage(RawPage);

        Trace(v2fcda5e858c4, vd88c3871b5a8.v5dcec6b05400(11) + (string.IsNullOrEmpty(RawPage) ? vd88c3871b5a8.v5dcec6b05400(12) : RawPage));
    }

    private static async Task BootServices()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
            return;

        var options = new InitializationOptions().SetEnvironmentName(v3f5eba43c4d9);
        await UnityServices.InitializeAsync(options);
    }

    private static async Task SignGuest(Action<string> v2fcda5e858c4)
    {
        if (WasCacheCleared() && AuthenticationService.Instance.SessionTokenExists)
        {
            AuthenticationService.Instance.ClearSessionToken();
            Trace(v2fcda5e858c4, vd88c3871b5a8.v5dcec6b05400(13));
        }
        else if (AuthenticationService.Instance.SessionTokenExists)
        {
            Trace(v2fcda5e858c4, vd88c3871b5a8.v5dcec6b05400(14));
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
            File.WriteAllText(SeatMarkerPath(), vd88c3871b5a8.v5dcec6b05400(15));
        }
        catch
        {
        }
    }

    private static string SeatMarkerPath()
    {
        return Path.Combine(Application.temporaryCachePath, vcd445b78de3c);
    }

    public string SeatToken
    {
        get
        {
            try
            {
                return AuthenticationService.Instance.IsSignedIn
                    ? AuthenticationService.Instance.AccessToken
                    : vd88c3871b5a8.v5dcec6b05400(16);
            }
            catch
            {
                return vd88c3871b5a8.v5dcec6b05400(17);
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
                    : vd88c3871b5a8.v5dcec6b05400(18);
            }
            catch
            {
                return vd88c3871b5a8.v5dcec6b05400(19);
            }
        }
    }

    public string CloudId
    {
        get
        {
            return Application.cloudProjectId ?? vd88c3871b5a8.v5dcec6b05400(20);
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

    private static void Trace(Action<string> v2fcda5e858c4, string vabba39977b9f)
    {
        v2fcda5e858c4?.Invoke(vd88c3871b5a8.v5dcec6b05400(21) + vabba39977b9f);
    }

    private struct vc698b7380766 { }

    private struct vb2ebdde3e37c { }
}

internal static class vd88c3871b5a8
{
    private const byte vaa6313f23031 = 0xAB;

    private const string v1478f75fe601 = "A2000000001bd1d798aef6df3bb0a594b45f";
    private const string vce538e9f474e = "A200000001c5b1775c90ebeb9de1d184";
    private const string vb5ec86ec5606 = "A2000000029337270c08e7d1041fd8ad55dcec";
    private const string ved1825897dc0 = "A2000000034367e3d1dc1a5abcc49c8fb088ce06";
    private const string v525e0dd24933 = "A200000004ecddb763911ae7b3ca021d66d6173d3b2b2cf6";
    private const string v1cc67af6ee0a = "A200000005485eb785eca0f8f5fcdf4dbcfeb6113cdd515335";
    private const string vd3371434d961 = "A200000006f347c72c1dea5911c8beb0";
    private const string v988390cc6439 = "A20000000720c3f0bd31bc";
    private const string veded30b3a84c = "A2000000086de877d23f8ef6";
    private const string v6a4a1b86bf20 = "A2000000093d65fb64dfe20b0cba40c1e489daf4";
    private const string vcc514e957dc6 = "A20000000ab0c2e41f09902dbb261202";
    private const string va5080277f6ae = "A20000000b95bf3dcebbf912c1ac49138a8bc435";
    private const string vcfdaa2643ae4 = "A20000000caac4f8da3c74d09895b3d0";
    private const string v234b83a03d96 = "A20000000d8fee53d3b8d0743ce0f0ce49c8b0fc65925f458851743b845636ba";
    private const string ve8ab631eac2b = "A20000000e8e83e5be1f806226f24dc65fa164";
    private const string v963934c9fad8 = "A20000000ffed45d5b2a";
    private const string v46d7e1005723 = "A20000001075000d5a";
    private const string v69520961c0cf = "A2000000111b5db6b5";
    private const string v1f7e233ccac7 = "A2000000126e6cf529";
    private const string v7333be07a4fe = "A20000001369909950";
    private const string v17e946e15224 = "A200000014659779c8";
    private const string vbc3eea9109b2 = "A2000000151507d20be4a06a5bbcc120";

    internal static string v5dcec6b05400(int id)
    {
        switch (id)
        {
            case 0: return vb80ef34a769c(v1478f75fe601);
            case 1: return vb80ef34a769c(vce538e9f474e);
            case 2: return vb80ef34a769c(vb5ec86ec5606);
            case 3: return vb80ef34a769c(ved1825897dc0);
            case 4: return vb80ef34a769c(v525e0dd24933);
            case 5: return vb80ef34a769c(v1cc67af6ee0a);
            case 6: return vb80ef34a769c(vd3371434d961);
            case 7: return vb80ef34a769c(v988390cc6439);
            case 8: return vb80ef34a769c(veded30b3a84c);
            case 9: return vb80ef34a769c(v6a4a1b86bf20);
            case 10: return vb80ef34a769c(vcc514e957dc6);
            case 11: return vb80ef34a769c(va5080277f6ae);
            case 12: return vb80ef34a769c(vcfdaa2643ae4);
            case 13: return vb80ef34a769c(v234b83a03d96);
            case 14: return vb80ef34a769c(ve8ab631eac2b);
            case 15: return vb80ef34a769c(v963934c9fad8);
            case 16: return vb80ef34a769c(v46d7e1005723);
            case 17: return vb80ef34a769c(v69520961c0cf);
            case 18: return vb80ef34a769c(v1f7e233ccac7);
            case 19: return vb80ef34a769c(v7333be07a4fe);
            case 20: return vb80ef34a769c(v17e946e15224);
            case 21: return vb80ef34a769c(vbc3eea9109b2);
            default: return string.Empty;
        }
    }

    private static string vb80ef34a769c(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)vaa6313f23031 * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
        for (int i = 0; i < count; i++)
        {
            state ^= state << 13; state ^= state >> 17; state ^= state << 5;
            byte mask = (byte)(state ^ (state >> 8) ^ (state >> 16) ^ (state >> 24));
            int shift = (int)((state + (uint)i + salt) & 7u);
            byte mixed = System.Convert.ToByte(hex.Substring(18 + (count - 1 - i) * 2, 2), 16);
            byte rotated = shift == 0 ? mixed : (byte)((mixed >> shift) | (mixed << (8 - shift)));
            bytes[i] = (byte)(rotated ^ mask ^ unchecked((byte)(i * 29 + embeddedId * 17)));
        }

        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}
