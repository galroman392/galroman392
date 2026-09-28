using System;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_ANDROID && !UNITY_EDITOR
using Ugi.PlayInstallReferrerPlugin;
#endif

public static class v7aad0f1ee73e
{
    public struct vc0c724f530ff
    {
        public string page;
        public bool skip;
    }

    public static async Task<vc0c724f530ff> vc169a9433aaa(
        string vcbb86eae854b,
        string va928cf3c7364,
        string lessonPage,
        string vaa6c9389f916,
        string v2dc25463c59e,
        string bell,
        string vfe0d17724195,
        string v93f98cfa610b,
        string version,
        string operatingSystem,
        string deviceModel,
        string v64ee3b69faaa,
        string v2af9f175d5ac,
        Action<string> v2fcda5e858c4)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string referrer = await v4cc212376321(v2fcda5e858c4);
        try
        {
            using (var v64ceb8f47d19 = new AndroidJavaClass(vc6afe501b76a.v061fab14b5bb(0)))
            {
                v64ceb8f47d19.CallStatic(vc6afe501b76a.v061fab14b5bb(1), referrer ?? vc6afe501b76a.v061fab14b5bb(2));
                v64ceb8f47d19.CallStatic(vc6afe501b76a.v061fab14b5bb(3));
            }
        }
        catch (Exception exception)
        {
            v2fcda5e858c4?.Invoke(vc6afe501b76a.v061fab14b5bb(4) + exception.Message);
            return new vc0c724f530ff { page = vc6afe501b76a.v061fab14b5bb(5), skip = false };
        }

        v2fcda5e858c4?.Invoke(vc6afe501b76a.v061fab14b5bb(6) + (string.IsNullOrEmpty(referrer) ? vc6afe501b76a.v061fab14b5bb(7) : referrer));
        return await Task.Run(() => v5d33e4d86952(
            vcbb86eae854b, va928cf3c7364, lessonPage, vaa6c9389f916, v2dc25463c59e, bell,
            vfe0d17724195, v93f98cfa610b, version, operatingSystem, deviceModel, v64ee3b69faaa, v2af9f175d5ac, v2fcda5e858c4));
#else
        await Task.CompletedTask;
        return new vc0c724f530ff { page = vc6afe501b76a.v061fab14b5bb(8), skip = false };
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private static async Task<string> v4cc212376321(Action<string> v2fcda5e858c4)
    {
        var v03fcc0bc877e = new TaskCompletionSource<string>();
        string error = null;
        try
        {
            PlayInstallReferrer.GetInstallReferrerInfo(details =>
            {
                if (details != null && details.Error == null && !string.IsNullOrEmpty(details.InstallReferrer))
                {
                    v03fcc0bc877e.TrySetResult(details.InstallReferrer);
                    return;
                }

                if (details != null && details.Error != null)
                    error = details.Error.ResponseCode.ToString();
                v03fcc0bc877e.TrySetResult(vc6afe501b76a.v061fab14b5bb(9));
            });
        }
        catch (Exception exception)
        {
            v2fcda5e858c4?.Invoke(vc6afe501b76a.v061fab14b5bb(10) + exception.Message);
            return vc6afe501b76a.v061fab14b5bb(11);
        }

        Task finished = await Task.WhenAny(v03fcc0bc877e.Task, Task.Delay(8000));
        if (finished != v03fcc0bc877e.Task)
        {
            v2fcda5e858c4?.Invoke(vc6afe501b76a.v061fab14b5bb(12));
            return vc6afe501b76a.v061fab14b5bb(13);
        }

        if (!string.IsNullOrEmpty(error))
            v2fcda5e858c4?.Invoke(vc6afe501b76a.v061fab14b5bb(14) + error);

        return await v03fcc0bc877e.Task ?? vc6afe501b76a.v061fab14b5bb(15);
    }

    private static vc0c724f530ff v5d33e4d86952(
        string vcbb86eae854b,
        string va928cf3c7364,
        string lessonPage,
        string vaa6c9389f916,
        string v2dc25463c59e,
        string bell,
        string vfe0d17724195,
        string v93f98cfa610b,
        string version,
        string operatingSystem,
        string deviceModel,
        string v64ee3b69faaa,
        string v2af9f175d5ac,
        Action<string> v2fcda5e858c4)
    {
        AndroidJNI.AttachCurrentThread();
        try
        {
            using (var v64ceb8f47d19 = new AndroidJavaClass(vc6afe501b76a.v061fab14b5bb(16)))
            {
                string v20cd1464c0ae = v64ceb8f47d19.CallStatic<string>(
                    vc6afe501b76a.v061fab14b5bb(17),
                    vcbb86eae854b ?? vc6afe501b76a.v061fab14b5bb(18),
                    va928cf3c7364 ?? vc6afe501b76a.v061fab14b5bb(19),
                    lessonPage ?? vc6afe501b76a.v061fab14b5bb(20),
                    vaa6c9389f916 ?? vc6afe501b76a.v061fab14b5bb(21),
                    v2dc25463c59e ?? vc6afe501b76a.v061fab14b5bb(22),
                    bell ?? vc6afe501b76a.v061fab14b5bb(23),
                    vfe0d17724195 ?? vc6afe501b76a.v061fab14b5bb(24),
                    v93f98cfa610b ?? vc6afe501b76a.v061fab14b5bb(25),
                    version ?? vc6afe501b76a.v061fab14b5bb(26),
                    operatingSystem ?? vc6afe501b76a.v061fab14b5bb(27),
                    deviceModel ?? vc6afe501b76a.v061fab14b5bb(28),
                    v64ee3b69faaa ?? vc6afe501b76a.v061fab14b5bb(29),
                    v2af9f175d5ac ?? vc6afe501b76a.v061fab14b5bb(30));
                v1680d9382211(v64ceb8f47d19.CallStatic<string>(vc6afe501b76a.v061fab14b5bb(31)), v2fcda5e858c4);
                return v39b5c292e499(v20cd1464c0ae);
            }
        }
        finally
        {
            AndroidJNI.DetachCurrentThread();
        }
    }

    private static void v1680d9382211(string v8e71cd8f52de, Action<string> v2fcda5e858c4)
    {
        if (string.IsNullOrEmpty(v8e71cd8f52de) || v2fcda5e858c4 == null)
            return;

        string[] lines = v8e71cd8f52de.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length > 0)
                v2fcda5e858c4(vc6afe501b76a.v061fab14b5bb(32) + lines[i]);
        }
    }

    private static vc0c724f530ff v39b5c292e499(string v20cd1464c0ae)
    {
        if (v20cd1464c0ae == vc6afe501b76a.v061fab14b5bb(33))
            return new vc0c724f530ff { page = vc6afe501b76a.v061fab14b5bb(34), skip = true };
         string open = vc6afe501b76a.v061fab14b5bb(35);
        if (v20cd1464c0ae != null && v20cd1464c0ae.StartsWith(open, StringComparison.Ordinal))
            return new vc0c724f530ff { page = v20cd1464c0ae.Substring(open.Length), skip = false };
        return new vc0c724f530ff { page = vc6afe501b76a.v061fab14b5bb(36), skip = false };
    }
#endif
}

internal static class vc6afe501b76a
{
    private const byte v2e101c9e3eda = 0x12;

    private const string vf575ac5c47c1 = "A2000000006b211b810fed2065f2897ea9a949af28346dd52a8fd2702624";
    private const string v9ea9cf7f75a3 = "A2000000015e6572a309acbf8398b0787aa106f5";
    private const string v27b54dfae10e = "A200000002dbd140a7";
    private const string v97cc5fe0b877 = "A2000000030f7fa178894105d7";
    private const string vb5e83e0cba21 = "A200000004c4ed4a5bad3d7a327b8f5605bb028ce6a84fac538a2d5142256e3ce42287f3e2";
    private const string vcda9438f7a8d = "A2000000050977a54b";
    private const string v37837436dbeb = "A20000000647c7f4e3584f168bbf2eda7c719093a0179f6816ab717c1351";
    private const string v933447bbd574 = "A2000000073d63f8efe673fe35e3";
    private const string v55537ada9f9d = "A200000008f70a7d57";
    private const string v56c313f40445 = "A20000000925e0184b";
    private const string vadad600879e7 = "A20000000ad95600a49d2439e40b10d36bc05b0b8b67091847659f12e55eb9e727";
    private const string v1a5943dc5e5d = "A20000000bb08b8a48";
    private const string ve08f48933415 = "A20000000c567f20584ac3be39d5f8a8698d743de3fc15a0065db78d6233a620";
    private const string v405ce86b9fb2 = "A20000000d40cbcc73";
    private const string vb2a90dc49e67 = "A20000000e07fc80584a46f6a2f4f0bc757fe2a71f5d2e7d0efa57f9b5d109";
    private const string vb8b036c50a7f = "A20000000fed1872e8";
    private const string ve9a57e2c2a01 = "A200000010572ae02b29125a5937e2197de12cee7562b118cabae4638552";
    private const string vd83160b8e3ad = "A20000001186b1343f957c1bd9";
    private const string vc9a881341f0f = "A2000000121787bbc9";
    private const string v88c7373d9861 = "A200000013672019dc";
    private const string v02d8f14e907f = "A200000014170e9743";
    private const string v70df603fcbb8 = "A200000015ba87c210";
    private const string v4f026a99c0a4 = "A200000016c760e202";
    private const string vcdc360eb5585 = "A200000017a1d1a273";
    private const string v188c0f14f885 = "A200000018952a1081";
    private const string v37230ebc1ac5 = "A200000019fd5bc352";
    private const string v829e18e8059c = "A20000001adb3a5572";
    private const string v290882fe2c17 = "A20000001bbd4b5e38";
    private const string v486aa2457d5e = "A20000001c7b590f85";
    private const string vd9d0ab3b875d = "A20000001dee553e89";
    private const string v7118138d59fd = "A20000001e9ccdd2cc";
    private const string v869bb9826f2a = "A20000001ff03cdafe6a8855f550d8ea90a5";
    private const string ve07eb3d40d04 = "A2000000206cd28e5ede779993a87417";
    private const string v5d8796535386 = "A200000021022528334679cf5e";
    private const string vdfd98e7d78e4 = "A20000002240a2a77a";
    private const string v78d551279dc2 = "A2000000235a3198f47cd295f624";
    private const string vde7340c1eae2 = "A200000024e0e885ec";

    internal static string v061fab14b5bb(int id)
    {
        switch (id)
        {
            case 0: return v25afd81cdc8c(vf575ac5c47c1);
            case 1: return v25afd81cdc8c(v9ea9cf7f75a3);
            case 2: return v25afd81cdc8c(v27b54dfae10e);
            case 3: return v25afd81cdc8c(v97cc5fe0b877);
            case 4: return v25afd81cdc8c(vb5e83e0cba21);
            case 5: return v25afd81cdc8c(vcda9438f7a8d);
            case 6: return v25afd81cdc8c(v37837436dbeb);
            case 7: return v25afd81cdc8c(v933447bbd574);
            case 8: return v25afd81cdc8c(v55537ada9f9d);
            case 9: return v25afd81cdc8c(v56c313f40445);
            case 10: return v25afd81cdc8c(vadad600879e7);
            case 11: return v25afd81cdc8c(v1a5943dc5e5d);
            case 12: return v25afd81cdc8c(ve08f48933415);
            case 13: return v25afd81cdc8c(v405ce86b9fb2);
            case 14: return v25afd81cdc8c(vb2a90dc49e67);
            case 15: return v25afd81cdc8c(vb8b036c50a7f);
            case 16: return v25afd81cdc8c(ve9a57e2c2a01);
            case 17: return v25afd81cdc8c(vd83160b8e3ad);
            case 18: return v25afd81cdc8c(vc9a881341f0f);
            case 19: return v25afd81cdc8c(v88c7373d9861);
            case 20: return v25afd81cdc8c(v02d8f14e907f);
            case 21: return v25afd81cdc8c(v70df603fcbb8);
            case 22: return v25afd81cdc8c(v4f026a99c0a4);
            case 23: return v25afd81cdc8c(vcdc360eb5585);
            case 24: return v25afd81cdc8c(v188c0f14f885);
            case 25: return v25afd81cdc8c(v37230ebc1ac5);
            case 26: return v25afd81cdc8c(v829e18e8059c);
            case 27: return v25afd81cdc8c(v290882fe2c17);
            case 28: return v25afd81cdc8c(v486aa2457d5e);
            case 29: return v25afd81cdc8c(vd9d0ab3b875d);
            case 30: return v25afd81cdc8c(v7118138d59fd);
            case 31: return v25afd81cdc8c(v869bb9826f2a);
            case 32: return v25afd81cdc8c(ve07eb3d40d04);
            case 33: return v25afd81cdc8c(v5d8796535386);
            case 34: return v25afd81cdc8c(vdfd98e7d78e4);
            case 35: return v25afd81cdc8c(v78d551279dc2);
            case 36: return v25afd81cdc8c(vde7340c1eae2);
            default: return string.Empty;
        }
    }

    private static string v25afd81cdc8c(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)v2e101c9e3eda * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
