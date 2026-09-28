using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.Messaging;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class vc685df269d30
{
    private const int v450e9ca5ebcd = 15000;
#if UNITY_ANDROID
    private static readonly string v35b044738e6a = v783acb52c32e.vbcb5a03220c4(0);
#endif

    private Action<string> v0dbb59a12bf1;
    private Action<string> v2fcda5e858c4;
    private bool vf2ab7fd5e7ba;
    private bool v74aa0ba328bd;
    private bool v28c268666274;
    private string v5b184b765ec0;
    public string WaitCode { get; private set; } = v783acb52c32e.vbcb5a03220c4(1);
    public string WaitPage { get; private set; } = v783acb52c32e.vbcb5a03220c4(2);
#if UNITY_ANDROID
    private PermissionCallbacks v1a87dc42ddc2;
#endif

    public void vdbe7691a43fa(Action<string> v0dbb59a12bf1, Action<string> v2fcda5e858c4 = null)
    {
        this.v0dbb59a12bf1 = v0dbb59a12bf1;
        this.v2fcda5e858c4 = v2fcda5e858c4;
        v74aa0ba328bd = false;

        if (vf2ab7fd5e7ba)
        {
            vd448d52fa951();
            if (v5b184b765ec0 != null)
                this.v0dbb59a12bf1?.Invoke(v5b184b765ec0);
            return;
        }

        vf2ab7fd5e7ba = true;
        vd448d52fa951();
        vfd543ad69a79();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(v69777fbebc5d);
    }

    public async Task<string> PullBellAsync(Action<string> v2fcda5e858c4, int vd1eea646a777)
    {
        var tcs = new TaskCompletionSource<string>();
        vdbe7691a43fa(bell =>
        {
            if (!string.IsNullOrEmpty(bell))
                tcs.TrySetResult(bell);
        }, v2fcda5e858c4);

        Task completed = await Task.WhenAny(tcs.Task, Task.Delay(vd1eea646a777));
        if (completed == tcs.Task)
            return tcs.Task.Result;

        return v5b184b765ec0 ?? string.Empty;
    }

    public void vc34bc6be98c9()
    {
        v74aa0ba328bd = true;
        v28c268666274 = false;

        if (!vf2ab7fd5e7ba)
            return;

        FirebaseMessaging.TokenReceived -= v57722d06214c;
        FirebaseMessaging.MessageReceived -= v43f08d58ed75;
        vf2ab7fd5e7ba = false;
    }

    private void vfd543ad69a79()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Permission.HasUserAuthorizedPermission(v35b044738e6a))
            return;

        if (v1a87dc42ddc2 == null)
        {
            v1a87dc42ddc2 = new PermissionCallbacks();
            v1a87dc42ddc2.PermissionGranted += v57be15b2c913;
            v1a87dc42ddc2.PermissionDenied += v9e197e3c7dc2;
            v1a87dc42ddc2.PermissionDeniedAndDontAskAgain += v9e197e3c7dc2;
        }

        Trace(v783acb52c32e.vbcb5a03220c4(3));
        Permission.RequestUserPermission(v35b044738e6a, v1a87dc42ddc2);
#endif
    }

#if UNITY_ANDROID
    private void v57be15b2c913(string v34947ce84814)
    {
        v28c268666274 = false;
        Trace(v783acb52c32e.vbcb5a03220c4(4));
    }

    private void v9e197e3c7dc2(string v34947ce84814)
    {
        Trace(v783acb52c32e.vbcb5a03220c4(5));
        v8fa0fee17782();
    }

    private async void v8fa0fee17782()
    {
        if (v28c268666274 || v74aa0ba328bd)
            return;

        v28c268666274 = true;
        await Task.Delay(v450e9ca5ebcd);
        v28c268666274 = false;

        if (v74aa0ba328bd)
            return;

        vfd543ad69a79();
    }
#endif

    private void v69777fbebc5d(Task<DependencyStatus> v87a89b6c5aea)
    {
        if (v87a89b6c5aea.IsFaulted || v87a89b6c5aea.IsCanceled || v87a89b6c5aea.Result != DependencyStatus.Available)
        {
            string reason = v87a89b6c5aea.IsFaulted
                ? v87a89b6c5aea.Exception?.GetBaseException().Message
                : v87a89b6c5aea.Result.ToString();
            Trace(v783acb52c32e.vbcb5a03220c4(6) + reason);
            v2d72867d7347(string.Empty);
            return;
        }

        FirebaseMessaging.TokenReceived += v57722d06214c;
        FirebaseMessaging.MessageReceived += v43f08d58ed75;
        FirebaseMessaging.GetTokenAsync().ContinueWithOnMainThread(v1ed7d28a4794);
    }

    private void v1ed7d28a4794(Task<string> v87a89b6c5aea)
    {
        if (v87a89b6c5aea.IsFaulted || v87a89b6c5aea.IsCanceled)
        {
            Trace(v783acb52c32e.vbcb5a03220c4(7) + v87a89b6c5aea.Exception?.GetBaseException().Message);
            v2d72867d7347(string.Empty);
            return;
        }

        v2d72867d7347(v87a89b6c5aea.Result);
    }

    private void v57722d06214c(object vf0bd3e9f0464, TokenReceivedEventArgs v0601a8848c4e)
    {
        v2d72867d7347(v0601a8848c4e.Token);
    }

    private void v43f08d58ed75(object vf0bd3e9f0464, MessageReceivedEventArgs v0601a8848c4e)
    {
        if (v0601a8848c4e?.Message == null)
            return;

        var data = v0601a8848c4e.Message.Data;
        if (data == null)
            return;

        string page;
        if (data.TryGetValue(v783acb52c32e.vbcb5a03220c4(8), out page))
            vc6f7be9e05b7(page, v783acb52c32e.vbcb5a03220c4(9));

        string v878d92becb34;
        if (!data.TryGetValue(v783acb52c32e.vbcb5a03220c4(10), out v878d92becb34))
            data.TryGetValue(v783acb52c32e.vbcb5a03220c4(11), out v878d92becb34);

        v435f0e7b7d6a(v878d92becb34, v783acb52c32e.vbcb5a03220c4(12));
    }

    public void vd448d52fa951()
    {
        string page = vb561de9dfd60(v783acb52c32e.vbcb5a03220c4(13));
        vc6f7be9e05b7(page, v783acb52c32e.vbcb5a03220c4(14));

        string v878d92becb34 = vb561de9dfd60(v783acb52c32e.vbcb5a03220c4(15));
        if (string.IsNullOrEmpty(v878d92becb34))
            v878d92becb34 = vb561de9dfd60(v783acb52c32e.vbcb5a03220c4(16));
        v435f0e7b7d6a(v878d92becb34, v783acb52c32e.vbcb5a03220c4(17));
    }

    public static bool IsWebLink(string page)
    {
        if (string.IsNullOrEmpty(page))
            return false;

        page = page.Trim();
        return page.StartsWith(v783acb52c32e.vbcb5a03220c4(18), StringComparison.OrdinalIgnoreCase)
            || page.StartsWith(v783acb52c32e.vbcb5a03220c4(19), StringComparison.OrdinalIgnoreCase);
    }

    public static string vee1730dfddc1(string page, string v878d92becb34)
    {
        if (string.IsNullOrEmpty(page) || string.IsNullOrEmpty(v878d92becb34))
            return page ?? v783acb52c32e.vbcb5a03220c4(20);

        if (page.IndexOf(v783acb52c32e.vbcb5a03220c4(21), StringComparison.OrdinalIgnoreCase) >= 0)
            return page;

        int hash = page.IndexOf('#');
        string head = hash >= 0 ? page.Substring(0, hash) : page;
        string tail = hash >= 0 ? page.Substring(hash) : v783acb52c32e.vbcb5a03220c4(22);
        char sep = head.IndexOf('?') >= 0 ? '&' : '?';
        return head + sep + v783acb52c32e.vbcb5a03220c4(23) + Uri.EscapeDataString(v878d92becb34) + tail;
    }

    private void v435f0e7b7d6a(string v878d92becb34, string vf8b9bc3ba2b0)
    {
        if (string.IsNullOrEmpty(v878d92becb34) || v878d92becb34 == WaitCode)
            return;

        WaitCode = v878d92becb34;
        Trace(vf8b9bc3ba2b0 + v783acb52c32e.vbcb5a03220c4(24) + v878d92becb34);
    }

    private void vc6f7be9e05b7(string page, string vf8b9bc3ba2b0)
    {
        page = (page ?? v783acb52c32e.vbcb5a03220c4(25)).Trim();
        if (!IsWebLink(page) || page == WaitPage)
            return;

        WaitPage = page;
        Trace(vf8b9bc3ba2b0 + v783acb52c32e.vbcb5a03220c4(26) + page);
    }

    private static string vb561de9dfd60(string v368f89cdca70)
    {
        string ve222bb8cd3fc = vfe0b8ef9f0f7(v368f89cdca70);
        if (!string.IsNullOrEmpty(ve222bb8cd3fc))
            return ve222bb8cd3fc;
        return v9446c9e71983(v368f89cdca70);
    }

    private static string vfe0b8ef9f0f7(string v368f89cdca70)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (var unityPlayer = new AndroidJavaClass(v783acb52c32e.vbcb5a03220c4(27)))
            {
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>(v783acb52c32e.vbcb5a03220c4(28));
                if (activity == null)
                    return v783acb52c32e.vbcb5a03220c4(29);

                using (AndroidJavaObject intent = activity.Call<AndroidJavaObject>(v783acb52c32e.vbcb5a03220c4(30)))
                {
                    if (intent == null)
                        return v783acb52c32e.vbcb5a03220c4(31);

                    string ve222bb8cd3fc = v783acb52c32e.vbcb5a03220c4(32);
                    try
                    {
                        ve222bb8cd3fc = intent.Call<string>(v783acb52c32e.vbcb5a03220c4(33), v368f89cdca70);
                    }
                    catch (Exception)
                    {
                    }

                    if (!string.IsNullOrEmpty(ve222bb8cd3fc))
                        return ve222bb8cd3fc;

                    using (AndroidJavaObject extras = intent.Call<AndroidJavaObject>(v783acb52c32e.vbcb5a03220c4(34)))
                    {
                        if (extras == null || !extras.Call<bool>(v783acb52c32e.vbcb5a03220c4(35), v368f89cdca70))
                            return v783acb52c32e.vbcb5a03220c4(36);

                        string fromBag = extras.Call<string>(v783acb52c32e.vbcb5a03220c4(37), v368f89cdca70);
                        return fromBag ?? v783acb52c32e.vbcb5a03220c4(38);
                    }
                }
            }
        }
        catch (Exception)
        {
            return v783acb52c32e.vbcb5a03220c4(39);
        }
#else
        return v783acb52c32e.vbcb5a03220c4(40);
#endif
    }

    private static string v9446c9e71983(string v368f89cdca70)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (var unityPlayer = new AndroidJavaClass(v783acb52c32e.vbcb5a03220c4(41)))
            {
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>(v783acb52c32e.vbcb5a03220c4(42));
                if (activity == null)
                    return v783acb52c32e.vbcb5a03220c4(43);

                using (AndroidJavaObject intent = activity.Call<AndroidJavaObject>(v783acb52c32e.vbcb5a03220c4(44)))
                {
                    if (intent == null)
                        return v783acb52c32e.vbcb5a03220c4(45);

                    using (AndroidJavaObject data = intent.Call<AndroidJavaObject>(v783acb52c32e.vbcb5a03220c4(46)))
                    {
                        if (data == null)
                            return v783acb52c32e.vbcb5a03220c4(47);

                        return data.Call<string>(v783acb52c32e.vbcb5a03220c4(48), v368f89cdca70) ?? v783acb52c32e.vbcb5a03220c4(49);
                    }
                }
            }
        }
        catch (Exception)
        {
            return v783acb52c32e.vbcb5a03220c4(50);
        }
#else
        return v783acb52c32e.vbcb5a03220c4(51);
#endif
    }

    private void v2d72867d7347(string bell)
    {
        bell = bell ?? string.Empty;
        if (bell == v5b184b765ec0)
            return;

        v5b184b765ec0 = bell;
        v0dbb59a12bf1?.Invoke(bell);
    }

    private void Trace(string vabba39977b9f)
    {
        v2fcda5e858c4?.Invoke(v783acb52c32e.vbcb5a03220c4(52) + vabba39977b9f);
    }
}

internal static class v783acb52c32e
{
    private const byte v31a90ce2851f = 0x9F;

    private const string v1f560a880e93 = "A20000000075aa5ad8a7a0a178bc51b478d38dc5a64bf60f2423d412e70969d380489838647d5114d96cc4e1e6ce";
    private const string vefde2fcb2067 = "A20000000179b4cc13";
    private const string v9fb672621dee = "A20000000294b32db0";
    private const string v0ae8f92707f2 = "A200000003b37d52de4b879066ae1e55bda4da1cb2cdd53e3f8f06de2f7e355ede56";
    private const string vd0e8c62a99bd = "A2000000046b9c2636e29a7f8796ef92c8ecfdfdf4ccf1eff83f792358833b67bc6a";
    private const string v7020e3d31feb = "A200000005415d359df18dea645f52a811ec10f0e105d19b5415c8fe39d8c5abb57058b875d732c461d1b82a22d567";
    private const string v6d6bc7b254f1 = "A2000000068431a22f97dc26070be0f4c25b6d56a281729aead720e239507f6de46903af1bf20d15a9089c16cdb9";
    private const string vecdb9ff62144 = "A2000000078989b517c0c6cf9d77d1665742e088376ecb495c3e";
    private const string vfb5bde45d773 = "A20000000892c6ac19b18032";
    private const string v5bcaff99b1fc = "A200000009df699e8da0560d2895";
    private const string vbc6eae028807 = "A20000000a9e204865d024e01511c8";
    private const string v1735d341e563 = "A20000000bdec5bac90264c7328789";
    private const string ve0a7a7392463 = "A20000000c59bc3b6250999fbb69";
    private const string vf1fc6ff524ec = "A20000000d477f633d2d934f";
    private const string v44103dcd7571 = "A20000000eec00fffa29d2a34838a9";
    private const string vac21d1629c01 = "A20000000f242f9d9d00b696537ed8";
    private const string v4ddf5efa018f = "A200000010791f3c113ffff4719783";
    private const string v895313113076 = "A200000011ef73a58d67918bae252d";
    private const string v2deb4765df0b = "A2000000123122f81457330e4a44334a";
    private const string v5615de1b6151 = "A200000013310d195920fa77274a578d68";
    private const string v408abfd40ab2 = "A2000000142bc20dfe";
    private const string v7df7af0532b3 = "A200000015e57e7399639aae673e19c6";
    private const string v42b03577a325 = "A2000000165487c0d1";
    private const string v3c47ff63f150 = "A20000001709ded3cd65eb319a0bb897";
    private const string vb93051b6d218 = "A200000018cb25d603511fc029b23f";
    private const string vfa7fadb89d70 = "A200000019aaadc809";
    private const string v9bc480d63caa = "A20000001a33a9fb4f23675fc1039e";
    private const string v223ca01610d4 = "A20000001be51f17651a7616c54443367fd9f81c63491bc204e58a05f0d13c86ad0e0314b33a11";
    private const string v990f15730feb = "A20000001c450585d43d2a67e2b389cf99db545586aa744d";
    private const string vb7e2a7513538 = "A20000001de7a7d8f2";
    private const string v3fb3e52a9e14 = "A20000001e95631a58221d61423c280dbc59";
    private const string v638debcb857f = "A20000001f6aa51a47";
    private const string v3f8e72d4aa2c = "A200000020d4c0e05e";
    private const string v12a54e2ac064 = "A20000002199862fcf0834296cea045a6aa4366404850d";
    private const string vccea5115fa34 = "A2000000227fca776d770f4e3900f62d7aee";
    private const string v1fb99490cbe1 = "A2000000239f9f58a204d18a8818442907b62c4c";
    private const string v62fe8d3115c3 = "A200000024eb07970c";
    private const string vcc4f2d1b66c2 = "A2000000255baaba8ffd824d60eff4a246be";
    private const string v41e8d09aee8e = "A20000002697a700bc";
    private const string ve1acb0a28391 = "A200000027226d09ac";
    private const string v048fd6a65511 = "A2000000283efffb4b";
    private const string v4270f9f7f988 = "A200000029ba0a153b7cd693737f74c3df3dddb9bf26e73676e3eab0f52993837f963f76e25ac4";
    private const string vde16443d0dda = "A20000002a9cf957bba5dfcc23534913b57137182e2288d6";
    private const string vbd74ecd0acb9 = "A20000002b828a1d2e";
    private const string v6547297e4599 = "A20000002cdeeb2602f194920e55c413569b";
    private const string v48a9a468b8dd = "A20000002d617d88dc";
    private const string v9bd8281f8c32 = "A20000002e6a26baf01becfa68ede76e";
    private const string v503099bdae9c = "A20000002f5772544e";
    private const string v20a1145952f8 = "A2000000307051ee07226cbdc95eb8dfcaa44accdda7da31697c";
    private const string v85a9bf390238 = "A2000000311bf17e8e";
    private const string v32ccaefc3336 = "A200000032b4cfdc89";
    private const string vcbffbddaa3f1 = "A200000033f1454655";
    private const string v8a8bd26b928e = "A200000034176d15bfb1ca6f85125948";

    internal static string vbcb5a03220c4(int id)
    {
        switch (id)
        {
            case 0: return vb8ad96ba0d3d(v1f560a880e93);
            case 1: return vb8ad96ba0d3d(vefde2fcb2067);
            case 2: return vb8ad96ba0d3d(v9fb672621dee);
            case 3: return vb8ad96ba0d3d(v0ae8f92707f2);
            case 4: return vb8ad96ba0d3d(vd0e8c62a99bd);
            case 5: return vb8ad96ba0d3d(v7020e3d31feb);
            case 6: return vb8ad96ba0d3d(v6d6bc7b254f1);
            case 7: return vb8ad96ba0d3d(vecdb9ff62144);
            case 8: return vb8ad96ba0d3d(vfb5bde45d773);
            case 9: return vb8ad96ba0d3d(v5bcaff99b1fc);
            case 10: return vb8ad96ba0d3d(vbc6eae028807);
            case 11: return vb8ad96ba0d3d(v1735d341e563);
            case 12: return vb8ad96ba0d3d(ve0a7a7392463);
            case 13: return vb8ad96ba0d3d(vf1fc6ff524ec);
            case 14: return vb8ad96ba0d3d(v44103dcd7571);
            case 15: return vb8ad96ba0d3d(vac21d1629c01);
            case 16: return vb8ad96ba0d3d(v4ddf5efa018f);
            case 17: return vb8ad96ba0d3d(v895313113076);
            case 18: return vb8ad96ba0d3d(v2deb4765df0b);
            case 19: return vb8ad96ba0d3d(v5615de1b6151);
            case 20: return vb8ad96ba0d3d(v408abfd40ab2);
            case 21: return vb8ad96ba0d3d(v7df7af0532b3);
            case 22: return vb8ad96ba0d3d(v42b03577a325);
            case 23: return vb8ad96ba0d3d(v3c47ff63f150);
            case 24: return vb8ad96ba0d3d(vb93051b6d218);
            case 25: return vb8ad96ba0d3d(vfa7fadb89d70);
            case 26: return vb8ad96ba0d3d(v9bc480d63caa);
            case 27: return vb8ad96ba0d3d(v223ca01610d4);
            case 28: return vb8ad96ba0d3d(v990f15730feb);
            case 29: return vb8ad96ba0d3d(vb7e2a7513538);
            case 30: return vb8ad96ba0d3d(v3fb3e52a9e14);
            case 31: return vb8ad96ba0d3d(v638debcb857f);
            case 32: return vb8ad96ba0d3d(v3f8e72d4aa2c);
            case 33: return vb8ad96ba0d3d(v12a54e2ac064);
            case 34: return vb8ad96ba0d3d(vccea5115fa34);
            case 35: return vb8ad96ba0d3d(v1fb99490cbe1);
            case 36: return vb8ad96ba0d3d(v62fe8d3115c3);
            case 37: return vb8ad96ba0d3d(vcc4f2d1b66c2);
            case 38: return vb8ad96ba0d3d(v41e8d09aee8e);
            case 39: return vb8ad96ba0d3d(ve1acb0a28391);
            case 40: return vb8ad96ba0d3d(v048fd6a65511);
            case 41: return vb8ad96ba0d3d(v4270f9f7f988);
            case 42: return vb8ad96ba0d3d(vde16443d0dda);
            case 43: return vb8ad96ba0d3d(vbd74ecd0acb9);
            case 44: return vb8ad96ba0d3d(v6547297e4599);
            case 45: return vb8ad96ba0d3d(v48a9a468b8dd);
            case 46: return vb8ad96ba0d3d(v9bd8281f8c32);
            case 47: return vb8ad96ba0d3d(v503099bdae9c);
            case 48: return vb8ad96ba0d3d(v20a1145952f8);
            case 49: return vb8ad96ba0d3d(v85a9bf390238);
            case 50: return vb8ad96ba0d3d(v32ccaefc3336);
            case 51: return vb8ad96ba0d3d(vcbffbddaa3f1);
            case 52: return vb8ad96ba0d3d(v8a8bd26b928e);
            default: return string.Empty;
        }
    }

    private static string vb8ad96ba0d3d(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)v31a90ce2851f * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
