using System;
using UnityEngine;

public class vd14e3cc7c800
{
    public bool Ready { get { return v64ceb8f47d19 != null && !v9d58a754c103; } }

    public Func<string, bool, bool, bool> HandleRequest;
    public Func<string, int> AllowCapture;

    public event Action<string> v82dc8252cab0;
    public event Action<string> v995dced0f1ce;
    public event Action<float> ve262c61e4e78;
    public event Action<int, string> v3bc86bba2034;
    public event Action<string> vd7a16cf5d1dd;
    public event Action<string> v4778c632ef9b;
    public event Action<string> v4420348c42c0;
    public event Action<string, string> vfad7bd223399;
    public event Action<string, string> vc934c53610b8;

    private AndroidJavaObject v64ceb8f47d19;
    private v94dab265f573 v43f2fed5fd56;
    private bool v9d58a754c103;

    public vd14e3cc7c800()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            Attach();
        }
        catch (Exception exception)
        {
            Debug.LogWarning(v45cf170bc5d7.v08d54f341edd(0) + exception.Message);
        }
#endif
    }

    public void Load(string v3afe30aa7e19)
    {
        Call(v45cf170bc5d7.v08d54f341edd(1), v3afe30aa7e19 ?? string.Empty);
    }

    public void Show()
    {
        Call(v45cf170bc5d7.v08d54f341edd(2));
    }

    public void v374b7ca4f1e4()
    {
        Call(v45cf170bc5d7.v08d54f341edd(3));
    }

    public void vc34bc6be98c9()
    {
        Call(v45cf170bc5d7.v08d54f341edd(4));
    }

    public void GoBack()
    {
        Call(v45cf170bc5d7.v08d54f341edd(5));
    }

    public bool CanGoBack
    {
        get { return CallBool(v45cf170bc5d7.v08d54f341edd(6)); }
    }

    public bool CloseTopPopup()
    {
        return CallBool(v45cf170bc5d7.v08d54f341edd(7));
    }

    public string BackUrl()
    {
        return CallString(v45cf170bc5d7.v08d54f341edd(8));
    }

    public void SetUserAgent(string userAgent)
    {
        Call(v45cf170bc5d7.v08d54f341edd(9), userAgent ?? string.Empty);
    }

    public string GetUserAgent()
    {
        return CallString(v45cf170bc5d7.v08d54f341edd(10));
    }

    public void SetPopupUserAgent(string id, string userAgent)
    {
        Call(v45cf170bc5d7.v08d54f341edd(11), id ?? string.Empty, userAgent ?? string.Empty);
    }

    public void SetMargins(int left, int top, int right, int bottom, int screenW, int screenH)
    {
        Call(v45cf170bc5d7.v08d54f341edd(12), left, top, right, bottom, screenW, screenH);
    }

    public void SetAcceptThirdPartyCookies(bool v6a9d1709a696)
    {
        Call(v45cf170bc5d7.v08d54f341edd(13), v6a9d1709a696);
    }

    public void Eval(string script)
    {
        Call(v45cf170bc5d7.v08d54f341edd(14), script ?? string.Empty);
    }

    public void EvalPopup(string id, string script)
    {
        Call(v45cf170bc5d7.v08d54f341edd(15), id ?? string.Empty, script ?? string.Empty);
    }

    public void ClosePopup(string id)
    {
        Call(v45cf170bc5d7.v08d54f341edd(16), id ?? string.Empty);
    }

    public bool PopupAlive(string id)
    {
        if (v64ceb8f47d19 == null || v9d58a754c103 || string.IsNullOrEmpty(id))
            return false;

        try
        {
            return v64ceb8f47d19.Call<bool>(v45cf170bc5d7.v08d54f341edd(17), id);
        }
        catch (Exception exception)
        {
            Debug.LogWarning(v45cf170bc5d7.v08d54f341edd(18) + exception.Message);
            return false;
        }
    }

    public void ScrollToTop()
    {
        Call(v45cf170bc5d7.v08d54f341edd(19));
    }

    public void FinishCapture(bool grant)
    {
        Call(v45cf170bc5d7.v08d54f341edd(20), grant ? 1 : 0);
    }

    public void SetPaused(bool paused)
    {
        Call(v45cf170bc5d7.v08d54f341edd(21), paused);
    }

    public void Destroy()
    {
        if (v9d58a754c103)
            return;

        v9d58a754c103 = true;
        if (v64ceb8f47d19 == null)
            return;

        try
        {
            v64ceb8f47d19.Call(v45cf170bc5d7.v08d54f341edd(22));
        }
        catch (Exception exception)
        {
            Debug.LogWarning(v45cf170bc5d7.v08d54f341edd(23) + exception.Message);
        }

        v64ceb8f47d19.Dispose();
        v64ceb8f47d19 = null;
    }

    private void Attach()
    {
        v43f2fed5fd56 = new v94dab265f573(this);
        using (var unityPlayer = new AndroidJavaClass(v45cf170bc5d7.v08d54f341edd(24)))
        using (var activity = unityPlayer.GetStatic<AndroidJavaObject>(v45cf170bc5d7.v08d54f341edd(25)))
        {
            if (activity == null)
                throw new InvalidOperationException(v45cf170bc5d7.v08d54f341edd(26));

            v64ceb8f47d19 = new AndroidJavaObject(v45cf170bc5d7.v08d54f341edd(27), activity, v43f2fed5fd56);
        }
    }

    private void Call(string method, params object[] v0601a8848c4e)
    {
        if (v64ceb8f47d19 == null || v9d58a754c103)
            return;

        try
        {
            if (v0601a8848c4e == null || v0601a8848c4e.Length == 0)
                v64ceb8f47d19.Call(method);
            else
                v64ceb8f47d19.Call(method, v0601a8848c4e);
        }
        catch (Exception exception)
        {
            Debug.LogWarning(v45cf170bc5d7.v08d54f341edd(28) + method + v45cf170bc5d7.v08d54f341edd(29) + exception.Message);
        }
    }

    private bool CallBool(string method)
    {
        if (v64ceb8f47d19 == null || v9d58a754c103)
            return false;

        try
        {
            return v64ceb8f47d19.Call<bool>(method);
        }
        catch (Exception exception)
        {
            Debug.LogWarning(v45cf170bc5d7.v08d54f341edd(30) + method + v45cf170bc5d7.v08d54f341edd(31) + exception.Message);
            return false;
        }
    }

    private string CallString(string method)
    {
        if (v64ceb8f47d19 == null || v9d58a754c103)
            return string.Empty;

        try
        {
            string value = v64ceb8f47d19.Call<string>(method);
            return value ?? string.Empty;
        }
        catch (Exception exception)
        {
            Debug.LogWarning(v45cf170bc5d7.v08d54f341edd(32) + method + v45cf170bc5d7.v08d54f341edd(33) + exception.Message);
            return string.Empty;
        }
    }

    private void NotifyPageStarted(string v3afe30aa7e19)
    {
        Raise(v82dc8252cab0, v3afe30aa7e19 ?? string.Empty);
    }

    private void NotifyPageCommitted(string v3afe30aa7e19)
    {
        Raise(v995dced0f1ce, v3afe30aa7e19 ?? string.Empty);
    }

    private void NotifyProgress(int percent)
    {
        Raise(ve262c61e4e78, Mathf.Clamp(percent, 0, 100) / 100f);
    }

    private void NotifyPageFinished(int v878d92becb34, string v3afe30aa7e19)
    {
        Raise(v3bc86bba2034, v878d92becb34, v3afe30aa7e19 ?? string.Empty);
    }

    private void NotifyPageError(string v3afe30aa7e19)
    {
        Raise(vd7a16cf5d1dd, v3afe30aa7e19 ?? string.Empty);
    }

    private void NotifyPopupOpened(string id)
    {
        Raise(v4778c632ef9b, id ?? string.Empty);
    }

    private void NotifyPopupClosed(string id)
    {
        Raise(v4420348c42c0, id ?? string.Empty);
    }

    private void NotifyPopupStarted(string id, string v3afe30aa7e19)
    {
        Raise(vfad7bd223399, id ?? string.Empty, v3afe30aa7e19 ?? string.Empty);
    }

    private void NotifyPopupFinished(string id, string v3afe30aa7e19)
    {
        Raise(vc934c53610b8, id ?? string.Empty, v3afe30aa7e19 ?? string.Empty);
    }

    private void Raise<T>(Action<T> action, T value)
    {
        if (v9d58a754c103 || action == null)
            return;

        try
        {
            action(value);
        }
        catch (Exception exception)
        {
            Debug.LogWarning(v45cf170bc5d7.v08d54f341edd(34) + exception.Message);
        }
    }

    private void Raise<T1, T2>(Action<T1, T2> action, T1 first, T2 second)
    {
        if (v9d58a754c103 || action == null)
            return;

        try
        {
            action(first, second);
        }
        catch (Exception exception)
        {
            Debug.LogWarning(v45cf170bc5d7.v08d54f341edd(35) + exception.Message);
        }
    }

    private sealed class v94dab265f573 : AndroidJavaProxy
    {
        private readonly vd14e3cc7c800 vb7c0a54022cc;

        public v94dab265f573(vd14e3cc7c800 vb7c0a54022cc)
            : base(v45cf170bc5d7.v08d54f341edd(36))
        {
            this.vb7c0a54022cc = vb7c0a54022cc;
        }

        [UnityEngine.Scripting.Preserve]
        public int shouldHandle(string v3afe30aa7e19, int mainFrame, int popup)
        {
            vd14e3cc7c800 va928cf3c7364 = vb7c0a54022cc;
            if (va928cf3c7364 == null || va928cf3c7364.v9d58a754c103 || va928cf3c7364.HandleRequest == null)
                return 1;

            try
            {
                bool allow = va928cf3c7364.HandleRequest(v3afe30aa7e19 ?? string.Empty, mainFrame != 0, popup != 0);
                return allow ? 1 : 0;
            }
            catch (Exception exception)
            {
                Debug.LogWarning(v45cf170bc5d7.v08d54f341edd(37) + exception.Message);
                return 1;
            }
        }

        [UnityEngine.Scripting.Preserve]
        public void pageStarted(string v3afe30aa7e19)
        {
            vb7c0a54022cc.NotifyPageStarted(v3afe30aa7e19);
        }

        [UnityEngine.Scripting.Preserve]
        public void pageCommitted(string v3afe30aa7e19)
        {
            vb7c0a54022cc.NotifyPageCommitted(v3afe30aa7e19);
        }

        [UnityEngine.Scripting.Preserve]
        public void progress(int percent)
        {
            vb7c0a54022cc.NotifyProgress(percent);
        }

        [UnityEngine.Scripting.Preserve]
        public void pageFinished(int v878d92becb34, string v3afe30aa7e19)
        {
            vb7c0a54022cc.NotifyPageFinished(v878d92becb34, v3afe30aa7e19);
        }

        [UnityEngine.Scripting.Preserve]
        public void pageError(string v3afe30aa7e19)
        {
            vb7c0a54022cc.NotifyPageError(v3afe30aa7e19);
        }

        [UnityEngine.Scripting.Preserve]
        public void popupOpened(string id)
        {
            vb7c0a54022cc.NotifyPopupOpened(id);
        }

        [UnityEngine.Scripting.Preserve]
        public void popupClosed(string id)
        {
            vb7c0a54022cc.NotifyPopupClosed(id);
        }

        [UnityEngine.Scripting.Preserve]
        public void popupStarted(string id, string v3afe30aa7e19)
        {
            vb7c0a54022cc.NotifyPopupStarted(id, v3afe30aa7e19);
        }

        [UnityEngine.Scripting.Preserve]
        public void popupFinished(string id, string v3afe30aa7e19)
        {
            vb7c0a54022cc.NotifyPopupFinished(id, v3afe30aa7e19);
        }

        [UnityEngine.Scripting.Preserve]
        public int allowCapture(string resources)
        {
            vd14e3cc7c800 va928cf3c7364 = vb7c0a54022cc;
            if (va928cf3c7364 == null || va928cf3c7364.v9d58a754c103 || va928cf3c7364.AllowCapture == null)
                return 1;

            try
            {
                return va928cf3c7364.AllowCapture(resources ?? string.Empty);
            }
            catch (Exception exception)
            {
                Debug.LogWarning(v45cf170bc5d7.v08d54f341edd(38) + exception.Message);
                return 0;
            }
        }
    }
}

internal static class v45cf170bc5d7
{
    private const byte vf868879b193f = 0x48;

    private const string v87a7ba971517 = "A2000000008bedd512cf96a8db81e20f2d44280317f6f3c236b282c303ac4143";
    private const string v5b1ac0ff701d = "A200000001cda0a6a5acf1f1f7";
    private const string vb96ba7fc0a05 = "A200000002d5df144223e0b682";
    private const string vf81e8d12b884 = "A200000003472c85793d7b8d9e";
    private const string ve0084c8d2f7c = "A2000000047a00968fbe8ae295";
    private const string v70bf5d92c3a7 = "A2000000052d099456864814a3ca6e";
    private const string va1e9766b35a4 = "A200000006511ff09d14afc0784afaaa10d3";
    private const string v449612b4dd83 = "A2000000076c3cf06762bfcb1ab236312c11266a907d";
    private const string v7ebd0b93c3a9 = "A20000000827c9d7e365d4ddf6fb4d3e";
    private const string v3e14bc2ad870 = "A2000000092c71d413b94cab355b753fa8cf2228ae";
    private const string v18925794421d = "A20000000ab01742d442efdefbd377135f1c6661ef";
    private const string va82fbf7f2fcf = "A20000000b47fec1a5831e53d0e6439f34b96ebc262bf8930fca";
    private const string v0a6622ac2ecf = "A20000000c3d680b4c91968c8bec94927440b9";
    private const string v52bd18291c77 = "A20000000d47514f566ceaa7332f37d59c0be80f58f80453e31d2e899f";
    private const string vbd52a8ffed0e = "A20000000ec65f26af368ce606";
    private const string v9a930b1be3fd = "A20000000ff0ce237de7438cb99172fc4fe6";
    private const string v9724f9514433 = "A20000001036a4df16a257440bc3ed791747cb";
    private const string v1371c1a67f4f = "A200000011b974edf5e92813e36af48e3128ed";
    private const string vb31fd0d5ce3b = "A200000012d10133b345da9d075b2eeceed8818bf9d3516d6385cb4d2c";
    private const string vec2e9f0e906d = "A200000013094402dc2381e1e81e1f12acc9";
    private const string vc78801944472 = "A2000000146200acbe230513d901ddf6424cc302c21d";
    private const string v51f1235fb814 = "A200000015b5817b1ab12df14f1d346dff74";
    private const string v811fed3c8f31 = "A200000016d9ac38f3114ee14138edf0";
    private const string v647e451f73ae = "A200000017328d9d0ba909af9259b3eadf6d2d1c399971095f21";
    private const string v32c373446981 = "A2000000188c85d714fd68b4438f6ab99810346076424ad74bee7fedf8d69af90cb426a8139b9b";
    private const string v65942b99fd92 = "A200000019decba961a22e0db97b868aa8018b381450ce6b";
    private const string v3e3a5dd5ffda = "A20000001a3b9bdfc1d95c2a628e50fb04dee5cc59b50ccc00ce16128f022379";
    private const string v5be518c9ec92 = "A20000001b68758c6164dc3cbd19743f0d54a40ada6d05";
    private const string vb8c070b17a1c = "A20000001c2f51b550cc7c83d96951ed9a";
    private const string v9ed6422a85a0 = "A20000001dee7cd5cedeef";
    private const string v572a902296a0 = "A20000001e4d70825de1db9fe9961853b2";
    private const string v8a1ebd1b9e7e = "A20000001f69eebbd669f3";
    private const string vdcdb15c85da8 = "A2000000202a9c52e5ac07c528013d4db3";
    private const string v8f56afd0c609 = "A2000000219b08adf02330";
    private const string v0ec85ef0d64a = "A2000000220a49428a33867779a5849e96713af127cac0b44e817b";
    private const string va9d0c85c76bd = "A200000023db2f781f827c5c9dd4f8d2abf7faa7b7012d5e874ed7";
    private const string vc52578f7d841 = "A2000000248c644bcb05a7d4cca3440abdc094b64fd9248312f72af2060fc9862cf7cd6d1540c9c1693276";
    private const string v8d3aa26acd46 = "A2000000251d64dae5934c0f65b9d1989314cd7fcdcdec6ed3";
    private const string vd6ae14d1cb2c = "A200000026e2a4c9c3d8b1444ae98042f1fe216a97d2d5187276";

    internal static string v08d54f341edd(int id)
    {
        switch (id)
        {
            case 0: return vd2a5563be4ea(v87a7ba971517);
            case 1: return vd2a5563be4ea(v5b1ac0ff701d);
            case 2: return vd2a5563be4ea(vb96ba7fc0a05);
            case 3: return vd2a5563be4ea(vf81e8d12b884);
            case 4: return vd2a5563be4ea(ve0084c8d2f7c);
            case 5: return vd2a5563be4ea(v70bf5d92c3a7);
            case 6: return vd2a5563be4ea(va1e9766b35a4);
            case 7: return vd2a5563be4ea(v449612b4dd83);
            case 8: return vd2a5563be4ea(v7ebd0b93c3a9);
            case 9: return vd2a5563be4ea(v3e14bc2ad870);
            case 10: return vd2a5563be4ea(v18925794421d);
            case 11: return vd2a5563be4ea(va82fbf7f2fcf);
            case 12: return vd2a5563be4ea(v0a6622ac2ecf);
            case 13: return vd2a5563be4ea(v52bd18291c77);
            case 14: return vd2a5563be4ea(vbd52a8ffed0e);
            case 15: return vd2a5563be4ea(v9a930b1be3fd);
            case 16: return vd2a5563be4ea(v9724f9514433);
            case 17: return vd2a5563be4ea(v1371c1a67f4f);
            case 18: return vd2a5563be4ea(vb31fd0d5ce3b);
            case 19: return vd2a5563be4ea(vec2e9f0e906d);
            case 20: return vd2a5563be4ea(vc78801944472);
            case 21: return vd2a5563be4ea(v51f1235fb814);
            case 22: return vd2a5563be4ea(v811fed3c8f31);
            case 23: return vd2a5563be4ea(v647e451f73ae);
            case 24: return vd2a5563be4ea(v32c373446981);
            case 25: return vd2a5563be4ea(v65942b99fd92);
            case 26: return vd2a5563be4ea(v3e3a5dd5ffda);
            case 27: return vd2a5563be4ea(v5be518c9ec92);
            case 28: return vd2a5563be4ea(vb8c070b17a1c);
            case 29: return vd2a5563be4ea(v9ed6422a85a0);
            case 30: return vd2a5563be4ea(v572a902296a0);
            case 31: return vd2a5563be4ea(v8a1ebd1b9e7e);
            case 32: return vd2a5563be4ea(vdcdb15c85da8);
            case 33: return vd2a5563be4ea(v8f56afd0c609);
            case 34: return vd2a5563be4ea(v0ec85ef0d64a);
            case 35: return vd2a5563be4ea(va9d0c85c76bd);
            case 36: return vd2a5563be4ea(vc52578f7d841);
            case 37: return vd2a5563be4ea(v8d3aa26acd46);
            case 38: return vd2a5563be4ea(vd6ae14d1cb2c);
            default: return string.Empty;
        }
    }

    private static string vd2a5563be4ea(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)vf868879b193f * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
