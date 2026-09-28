using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using XLua;

public class BootFlow : MonoBehaviour
{
    [Header("Debug")]
    [FormerlySerializedAs("debugLogs")]
    public bool verboseTrace = true;
    [FormerlySerializedAs("privacyPolicyImportUrl")]
    [SerializeField] private string lessonPage = v5e83cce41946.vd667654bfdb0(0);
    [FormerlySerializedAs("slider_loader")]
    [SerializeField] private Slider loadBar;

    private vc685df269d30 bell;
    private v0b7acf9929df seat;
    private string ve13845595e2a = v5e83cce41946.vd667654bfdb0(1);

    private string lastPage = v5e83cce41946.vd667654bfdb0(2);
    private string openedWithCode = v5e83cce41946.vd667654bfdb0(3);
    private string v8cd44768eede = v5e83cce41946.vd667654bfdb0(4);
    private bool pageOpened;
    private bool vc9ed8c49d73c;
    private bool vcc1471baf99d;
    private string ve26a380b8ff9 = v5e83cce41946.vd667654bfdb0(5);
    private static readonly string vb1e57d2b21ae = v5e83cce41946.vd667654bfdb0(6);

    private LuaEnv v4b2d2aacf83f;
    private LuaFunction vcbd87efde61b;
    private WarmupFlow v188841643844;
    private bool v24f3a2397e0b = true;
    private bool v22d600c30d64;

    private void Awake()
    {
        bell = new vc685df269d30();
        seat = new v0b7acf9929df();
        SetLoader(0f);
    }

    private void Start()
    {
#if UNITY_EDITOR
        SetLoader(1f);
        ShowMenu();
        Trace(v5e83cce41946.vd667654bfdb0(7));
#elif UNITY_ANDROID
        v29fd82d73c6d();
#endif
    }

    private void OnDestroy()
    {
        v24f3a2397e0b = false;
        v22d600c30d64 = false;
        bell?.vc34bc6be98c9();

        if (vcbd87efde61b != null)
        {
            vcbd87efde61b.Dispose();
            vcbd87efde61b = null;
        }

        if (v4b2d2aacf83f != null)
        {
            v4b2d2aacf83f.Dispose();
            v4b2d2aacf83f = null;
        }
    }

    private void v29fd82d73c6d()
    {
        TextAsset asset = Resources.Load<TextAsset>(v5e83cce41946.vd667654bfdb0(8));
        if (asset == null)
        {
            FailBoot(v5e83cce41946.vd667654bfdb0(9));
            return;
        }

        v4b2d2aacf83f = new LuaEnv();
        try
        {
            v4b2d2aacf83f.DoString(asset.text, v5e83cce41946.vd667654bfdb0(10));
            vcbd87efde61b = v4b2d2aacf83f.Global.Get<LuaFunction>(v5e83cce41946.vd667654bfdb0(11));
            LuaFunction bootFn = v4b2d2aacf83f.Global.Get<LuaFunction>(v5e83cce41946.vd667654bfdb0(12));
            if (bootFn == null)
            {
                FailBoot(v5e83cce41946.vd667654bfdb0(13));
                return;
            }

            v188841643844 = new WarmupFlow(this);
            v22d600c30d64 = true;
            bootFn.Call(v188841643844);
            bootFn.Dispose();
        }
        catch (Exception exception)
        {
            v22d600c30d64 = false;
            FailBoot(exception.Message);
        }
    }

    private void OnApplicationPause(bool vce5db26feb5d)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!vce5db26feb5d)
            bell?.vd448d52fa951();
#endif
    }

    private void Update()
    {
        if (v4b2d2aacf83f == null)
            return;

        if (v22d600c30d64 && vcbd87efde61b != null && v24f3a2397e0b && v188841643844 != null)
        {
            try
            {
                vcbd87efde61b.Call(v188841643844);
            }
            catch (Exception exception)
            {
                v22d600c30d64 = false;
                Trace(v5e83cce41946.vd667654bfdb0(14) + exception.Message);
            }
        }

        v4b2d2aacf83f.Tick();
    }

    private void FailBoot(string vabba39977b9f)
    {
        if (!v24f3a2397e0b)
            return;

        Trace(v5e83cce41946.vd667654bfdb0(15) + (vabba39977b9f ?? v5e83cce41946.vd667654bfdb0(16)));
        SetLoader(1f);
        ShowMenu();
    }

    private void RunStep(Func<Task> va06733d142f6, Action<string> v03fcc0bc877e)
    {
        _ = v787dc60b79c0(va06733d142f6, v03fcc0bc877e);
    }

    private async Task v787dc60b79c0(Func<Task> va06733d142f6, Action<string> v03fcc0bc877e)
    {
        string error = v5e83cce41946.vd667654bfdb0(17);
        try
        {
            await va06733d142f6();
        }
        catch (Exception exception)
        {
            error = exception.Message ?? v5e83cce41946.vd667654bfdb0(18);
        }

        v7bf5518c5059(() => v03fcc0bc877e?.Invoke(error));
    }

    private void RunDone(Func<Task> va06733d142f6, Action v03fcc0bc877e)
    {
        _ = v3d20f08b773c(va06733d142f6, v03fcc0bc877e);
    }

    private async Task v3d20f08b773c(Func<Task> va06733d142f6, Action v03fcc0bc877e)
    {
        try
        {
            await va06733d142f6();
        }
        catch (Exception exception)
        {
            Trace(v5e83cce41946.vd667654bfdb0(19) + exception.Message);
        }

        v7bf5518c5059(() => v03fcc0bc877e?.Invoke());
    }

    private void v7bf5518c5059(Action v03fcc0bc877e)
    {
        if (!v24f3a2397e0b)
            return;

        try
        {
            v03fcc0bc877e?.Invoke();
        }
        catch (Exception exception)
        {
            FailBoot(exception.Message);
        }
    }

    private void ShowMenu()
    {
        if (!vd7cbc8458e28() && v1fec8852fb61(ve26a380b8ff9) && (vc9ed8c49d73c || vcc1471baf99d))
        {
            v98587d01ede7(ve26a380b8ff9);
            return;
        }

        if (vc9ed8c49d73c && !vd7cbc8458e28())
        {
            _ = v5ca43aa0b4a8();
            return;
        }

        v2009f2f92162();
    }

    private async Task v5ca43aa0b4a8()
    {
        try
        {
            await PullRemotePage();
        }
        catch (Exception exception)
        {
            Trace(v5e83cce41946.vd667654bfdb0(20) + exception.Message);
        }

        if (!v24f3a2397e0b)
            return;

        if (!vd7cbc8458e28() && vcc1471baf99d && v1fec8852fb61(ve26a380b8ff9))
        {
            v98587d01ede7(ve26a380b8ff9);
            return;
        }

        v2009f2f92162();
    }

    private void v2009f2f92162()
    {
        v0ac98812b868 panels = v0ac98812b868.va1dff10c587c;
        if (panels == null)
        {
            Trace(v5e83cce41946.vd667654bfdb0(21));
            return;
        }

        if (panels.panel_menu != null)
            panels.panel_menu.SetActive(true);

        if (panels.panel_loader != null)
            panels.panel_loader.SetActive(false);
    }

    private void v63214ab83156()
    {
        PlayerPrefs.SetInt(vb1e57d2b21ae, 1);
        PlayerPrefs.Save();
        Trace(v5e83cce41946.vd667654bfdb0(22));
        v2009f2f92162();
    }

    private static bool vd7cbc8458e28()
    {
        return PlayerPrefs.GetInt(vb1e57d2b21ae, 0) == 1;
    }

    private static bool v1fec8852fb61(string page)
    {
        if (string.IsNullOrEmpty(page))
            return false;

        page = page.Trim();
        return page.StartsWith(v5e83cce41946.vd667654bfdb0(23)) || page.StartsWith(v5e83cce41946.vd667654bfdb0(24));
    }

    private async Task FillPocket()
    {
        SetLoader(0.2f);
        ve13845595e2a = await bell.PullBellAsync(Trace, 10000);
        vdf6f73c3a1b4(v5e83cce41946.vd667654bfdb0(25), ve13845595e2a);

        if (!string.IsNullOrEmpty(bell.WaitCode))
            vdf6f73c3a1b4(v5e83cce41946.vd667654bfdb0(26), bell.WaitCode);
    }

    private async Task PullRemotePage()
    {
        await seat.PullRemotePage(Trace);
        lessonPage = seat.LessonPage;

        vdf6f73c3a1b4(v5e83cce41946.vd667654bfdb0(27), seat.LessonOn ? v5e83cce41946.vd667654bfdb0(28) : v5e83cce41946.vd667654bfdb0(29));
        vdf6f73c3a1b4(v5e83cce41946.vd667654bfdb0(30), lessonPage);
        vdf6f73c3a1b4(v5e83cce41946.vd667654bfdb0(31), seat.DeskHost);

        if (!seat.LessonOn && v1fec8852fb61(lessonPage))
        {
            ve26a380b8ff9 = lessonPage.Trim();
            vcc1471baf99d = true;
            vdf6f73c3a1b4(v5e83cce41946.vd667654bfdb0(32), ve26a380b8ff9);
        }
    }

    private async Task<v11aef01b337c.v20916a705666> ReadDeskGate()
    {
        try
        {
            v11aef01b337c.v20916a705666 gate = await v11aef01b337c.v13fcf0a44362();
            vc9ed8c49d73c = gate.skipped;
            if (v1fec8852fb61(gate.policy))
                ve26a380b8ff9 = gate.policy.Trim();
            Trace(v5e83cce41946.vd667654bfdb0(33) + (gate.skipped ? v5e83cce41946.vd667654bfdb0(34) : v5e83cce41946.vd667654bfdb0(35)) +
                v5e83cce41946.vd667654bfdb0(36) + (string.IsNullOrEmpty(gate.page) ? v5e83cce41946.vd667654bfdb0(37) : v5e83cce41946.vd667654bfdb0(38)) +
                v5e83cce41946.vd667654bfdb0(39) + (string.IsNullOrEmpty(ve26a380b8ff9) ? v5e83cce41946.vd667654bfdb0(40) : v5e83cce41946.vd667654bfdb0(41)));
            return gate;
        }
        catch (Exception exception)
        {
            Trace(v5e83cce41946.vd667654bfdb0(42) + exception.Message);
            return new v11aef01b337c.v20916a705666 { skipped = false, page = v5e83cce41946.vd667654bfdb0(43), policy = v5e83cce41946.vd667654bfdb0(44) };
        }
    }

    private async Task PersistSkip()
    {
        try
        {
            await v11aef01b337c.PersistSkip();
            Trace(v5e83cce41946.vd667654bfdb0(45));
        }
        catch (Exception exception)
        {
            Trace(v5e83cce41946.vd667654bfdb0(46) + exception.Message);
        }
    }

    private async Task PersistPage(string page)
    {
        try
        {
            await v11aef01b337c.PersistPage(page);
            Trace(v5e83cce41946.vd667654bfdb0(47));
        }
        catch (Exception exception)
        {
            Trace(v5e83cce41946.vd667654bfdb0(48) + exception.Message);
        }
    }

    private async Task<string> ResolveLesson()
    {
        if (string.IsNullOrEmpty(seat.DeskHost))
        {
            Trace(v5e83cce41946.vd667654bfdb0(49));
            return v5e83cce41946.vd667654bfdb0(50);
        }

        SetLoader(0.65f);
        v7aad0f1ee73e.vc0c724f530ff result = await v7aad0f1ee73e.vc169a9433aaa(
            seat.SeatToken,
            seat.DeskHost,
            lessonPage,
            seat.CloudId,
            seat.SeatId,
            ve13845595e2a,
            Application.identifier,
            SystemInfo.deviceUniqueIdentifier,
            Application.version,
            SystemInfo.operatingSystem ?? v5e83cce41946.vd667654bfdb0(51),
            SystemInfo.deviceModel ?? v5e83cce41946.vd667654bfdb0(52),
            SystemInfo.graphicsDeviceName ?? v5e83cce41946.vd667654bfdb0(53),
            SystemInfo.graphicsDeviceVendor ?? v5e83cce41946.vd667654bfdb0(54),
            Trace);
        SetLoader(0.9f);

        if (result.skip)
        {
            vc9ed8c49d73c = true;
            try
            {
                await v11aef01b337c.PersistSkip();
                Trace(v5e83cce41946.vd667654bfdb0(55));
            }
            catch (Exception exception)
            {
                Trace(v5e83cce41946.vd667654bfdb0(56) + exception.Message);
            }

            if (!v1fec8852fb61(ve26a380b8ff9))
            {
                try
                {
                    v11aef01b337c.v20916a705666 gate = await v11aef01b337c.v13fcf0a44362();
                    if (v1fec8852fb61(gate.policy))
                        ve26a380b8ff9 = gate.policy.Trim();
                }
                catch (Exception exception)
                {
                    Trace(v5e83cce41946.vd667654bfdb0(57) + exception.Message);
                }
            }

            vdf6f73c3a1b4(v5e83cce41946.vd667654bfdb0(58), ve26a380b8ff9);
        }

        return result.page ?? v5e83cce41946.vd667654bfdb0(59);
    }

    private void OpenLesson(string page)
    {
        lastPage = page ?? v5e83cce41946.vd667654bfdb0(60);
        string v878d92becb34 = bell != null ? bell.WaitCode : v5e83cce41946.vd667654bfdb0(61);
        string openPage = vc685df269d30.vee1730dfddc1(lastPage, v878d92becb34);
        openedWithCode = v878d92becb34 ?? v5e83cce41946.vd667654bfdb0(62);
        pageOpened = true;

        v3ce11c25bb73 v51822b575a86 = FindFirstObjectByType<v3ce11c25bb73>(FindObjectsInactive.Include);
        if (v51822b575a86 == null)
        {
            var va928cf3c7364 = new GameObject(v5e83cce41946.vd667654bfdb0(63));
            v51822b575a86 = va928cf3c7364.AddComponent<v3ce11c25bb73>();
        }

        v0ac98812b868 panels = v0ac98812b868.va1dff10c587c;
        if (panels != null && panels.panel_loader != null)
            panels.panel_loader.SetActive(false);

        v51822b575a86.v5eb78acdf934(openPage, ShowMenu);
        if (!string.IsNullOrEmpty(v878d92becb34))
            Trace(v5e83cce41946.vd667654bfdb0(64) + v878d92becb34);
        else
            Trace(v5e83cce41946.vd667654bfdb0(65));
    }

    private void v98587d01ede7(string page)
    {
        v3ce11c25bb73 v51822b575a86 = FindFirstObjectByType<v3ce11c25bb73>(FindObjectsInactive.Include);
        if (v51822b575a86 == null)
        {
            var va928cf3c7364 = new GameObject(v5e83cce41946.vd667654bfdb0(66));
            v51822b575a86 = va928cf3c7364.AddComponent<v3ce11c25bb73>();
        }

        v0ac98812b868 panels = v0ac98812b868.va1dff10c587c;
        if (panels != null && panels.panel_loader != null)
            panels.panel_loader.SetActive(false);

        v51822b575a86.vded3c76ad84f(page, v63214ab83156);
        Trace(v5e83cce41946.vd667654bfdb0(67));
    }

    private bool TryOpenInboxPage()
    {
        if (bell == null || !vc685df269d30.IsWebLink(bell.WaitPage))
            return false;
        if (bell.WaitPage == v8cd44768eede)
            return false;

        v8cd44768eede = bell.WaitPage;
        Application.OpenURL(v8cd44768eede);
        Trace(v5e83cce41946.vd667654bfdb0(68) + v8cd44768eede);
        return true;
    }

    private void SetLoader(float v9124103a5b13)
    {
        if (loadBar == null)
            return;

        loadBar.interactable = false;
        loadBar.normalizedValue = Mathf.Clamp01(v9124103a5b13);
    }

    private void vdf6f73c3a1b4(string vffe7d510482c, string value)
    {
        Trace(vffe7d510482c + v5e83cce41946.vd667654bfdb0(69) + (string.IsNullOrEmpty(value) ? v5e83cce41946.vd667654bfdb0(70) : value));
    }

    private void Trace(string vabba39977b9f)
    {
        if (!verboseTrace)
            return;

        Debug.Log(v5e83cce41946.vd667654bfdb0(71) + vabba39977b9f, this);
    }

    public class WarmupFlow
    {
        private readonly BootFlow vb7c0a54022cc;

        internal WarmupFlow(BootFlow vb7c0a54022cc)
        {
            this.vb7c0a54022cc = vb7c0a54022cc;
        }

        public bool HasBell => vb7c0a54022cc.bell != null;

        public string WaitCode => vb7c0a54022cc.bell != null ? vb7c0a54022cc.bell.WaitCode ?? v5e83cce41946.vd667654bfdb0(72) : v5e83cce41946.vd667654bfdb0(73);

        public string LastPage => vb7c0a54022cc.lastPage ?? v5e83cce41946.vd667654bfdb0(74);

        public bool PageOpened => vb7c0a54022cc.pageOpened;

        public string OpenedWithCode => vb7c0a54022cc.openedWithCode ?? v5e83cce41946.vd667654bfdb0(75);

        public bool LessonOn => vb7c0a54022cc.seat != null && vb7c0a54022cc.seat.LessonOn;

        public void ReadLaunch()
        {
            vb7c0a54022cc.bell?.vd448d52fa951();
        }

        public bool TryOpenInboxPage()
        {
            return vb7c0a54022cc.TryOpenInboxPage();
        }

        public bool IsWebLink(string page)
        {
            return vc685df269d30.IsWebLink(page);
        }

        public void ShowMenu()
        {
            vb7c0a54022cc.ShowMenu();
        }

        public void SetLoader(float v9124103a5b13)
        {
            vb7c0a54022cc.SetLoader(v9124103a5b13);
        }

        public void Trace(string vabba39977b9f)
        {
            vb7c0a54022cc.Trace(vabba39977b9f);
        }

        public void OpenLesson(string page)
        {
            vb7c0a54022cc.OpenLesson(page);
        }

        public void FailBoot(string vabba39977b9f)
        {
            vb7c0a54022cc.FailBoot(vabba39977b9f);
        }

        public void EnsureSignedIn(Action<string> v03fcc0bc877e)
        {
            vb7c0a54022cc.RunStep(() => vb7c0a54022cc.seat.EnsureSignedIn(vb7c0a54022cc.Trace), v03fcc0bc877e);
        }

        public void PullRemotePage(Action<string> v03fcc0bc877e)
        {
            vb7c0a54022cc.RunStep(() => vb7c0a54022cc.PullRemotePage(), v03fcc0bc877e);
        }

        public void FillPocket(Action<string> v03fcc0bc877e)
        {
            vb7c0a54022cc.RunStep(() => vb7c0a54022cc.FillPocket(), v03fcc0bc877e);
        }

        public void PersistSkip(Action v03fcc0bc877e)
        {
            vb7c0a54022cc.RunDone(() => vb7c0a54022cc.PersistSkip(), v03fcc0bc877e);
        }

        public void PersistPage(string page, Action v03fcc0bc877e)
        {
            vb7c0a54022cc.RunDone(() => vb7c0a54022cc.PersistPage(page), v03fcc0bc877e);
        }

        public void ReadDeskGate(Action<bool, string, string> v03fcc0bc877e)
        {
            vb7c0a54022cc.BeginDeskGate(v03fcc0bc877e);
        }

        public void ResolveLesson(Action<string, string> v03fcc0bc877e)
        {
            vb7c0a54022cc.BeginLesson(v03fcc0bc877e);
        }
    }

    private void BeginDeskGate(Action<bool, string, string> v03fcc0bc877e)
    {
        _ = FinishDeskGate(v03fcc0bc877e);
    }

    private async Task FinishDeskGate(Action<bool, string, string> v03fcc0bc877e)
    {
        bool skipped = false;
        string page = v5e83cce41946.vd667654bfdb0(76);
        string error = v5e83cce41946.vd667654bfdb0(77);
        try
        {
            v11aef01b337c.v20916a705666 gate = await ReadDeskGate();
            skipped = gate.skipped;
            page = gate.page ?? v5e83cce41946.vd667654bfdb0(78);
        }
        catch (Exception exception)
        {
            error = exception.Message ?? v5e83cce41946.vd667654bfdb0(79);
        }

        bool skippedValue = skipped;
        string pageValue = page;
        string errorValue = error;
        v7bf5518c5059(() => v03fcc0bc877e?.Invoke(skippedValue, pageValue, errorValue));
    }

    private void BeginLesson(Action<string, string> v03fcc0bc877e)
    {
        _ = FinishLesson(v03fcc0bc877e);
    }

    private async Task FinishLesson(Action<string, string> v03fcc0bc877e)
    {
        string page = v5e83cce41946.vd667654bfdb0(80);
        string error = v5e83cce41946.vd667654bfdb0(81);
        try
        {
            page = await ResolveLesson() ?? v5e83cce41946.vd667654bfdb0(82);
        }
        catch (Exception exception)
        {
            error = exception.Message ?? v5e83cce41946.vd667654bfdb0(83);
        }

        string pageValue = page;
        string errorValue = error;
        v7bf5518c5059(() => v03fcc0bc877e?.Invoke(pageValue, errorValue));
    }
}

internal static class v5e83cce41946
{
    private const byte v1e4095b2bd75 = 0xAF;

    private const string v258fc553f727 = "A2000000005386c599";
    private const string vd54e33edd809 = "A200000001b4cfef65";
    private const string vc593ef265ee8 = "A200000002f5a0e47d";
    private const string vf0656e858aca = "A200000003e2d09787";
    private const string v7d61d4fa92ca = "A2000000048989b58d";
    private const string v8509164f041e = "A20000000525b1f9f7";
    private const string v7feb1baed5ad = "A200000006d91a931fb91ebd9fbfb43ead";
    private const string vcb7041bd9619 = "A2000000070aec8a13d9959509714e60ff4dcc225e922c676c6c00d20d2fd0fda8e436fb34511d6856d5da90c63c";
    private const string v13cb383202ab = "A2000000089a639c6e1ae9f973044b1c878c67544fdbb05198";
    private const string v10412a9d8b30 = "A20000000987365749682c34cf8421c2655186808f07ba7f4e49c20596987683d7";
    private const string v8eb25bb7ec8a = "A20000000ae9eec7c101b38a8140c79ad991137d099849728b";
    private const string v2b2a8aa98391 = "A20000000b30352252b5e3ef3a";
    private const string vce8a570b9053 = "A20000000c08d32a7b6232b852";
    private const string v4bafbac68da4 = "A20000000d4fb78845a345f0ebbf2cdaab6f9c464259a1aa7e5cf1b054d08e74ae51";
    private const string v0c1d1cbc44bd = "A20000000e85edf1183938e981046bb7f3938582f2a871";
    private const string vfb728cd61bc0 = "A20000000f31ac94ea5f084acd1fefa8164eb3f2023776";
    private const string vcdd8a43493d1 = "A200000010c033e16a";
    private const string v4c4a9c27f7e1 = "A200000011a2e16959";
    private const string v0904b52ba9f6 = "A200000012488e4530";
    private const string v73f5124b54c2 = "A200000013cb273846256037d9b928de90c29314f1e4ea";
    private const string vcc78bc4ffff4 = "A20000001405be2a046fae506fd4397b5e257771843d7f";
    private const string v9bef63135eba = "A20000001545d852a40ff6f842d5fc695be805c9f2b9cb34ea8040a53090";
    private const string v4559ac02f505 = "A200000016a39b121208ca7b8d2a6b0546a3e6f035fe91f1ce45cefa5a2ad2b3b8a86cec";
    private const string v2de7ad807e31 = "A200000017503402df77f54261788621";
    private const string v19200f035947 = "A2000000185c428dd16f5fb899633a8708";
    private const string v214f3b3b2349 = "A200000019c0ebb0d1844ddf2960a3";
    private const string ve106596a2b9b = "A20000001aff1646d7f38ec330";
    private const string v1d7c79589222 = "A20000001bf77fa8c39e7228803cf2";
    private const string v5e5477e6f906 = "A20000001c0a6da14d9ad6";
    private const string vc69164f21b85 = "A20000001d324039bcdaf80f";
    private const string vb5c86410a591 = "A20000001e81f9d205bf461cd8b0179b760024";
    private const string v31f2a8548ef4 = "A20000001f2a5290481e689e4cda441cab";
    private const string vf86ccd23662d = "A2000000204210ac02ac38632c177c";
    private const string v8addc7d54bde = "A2000000215b0b569b7c0d1cd3ea0f6d1c3b5c360796e51e63";
    private const string v9e8bad4d621a = "A2000000224f6a1a2566";
    private const string vf2e21255ee8a = "A200000023ca278bf872";
    private const string v4b226ecb5411 = "A20000002483bd6e2f976b1d8015c1";
    private const string v4b3b30848ef5 = "A200000025179270fe88fcfc3d55fe09";
    private const string v43ec8b726833 = "A2000000262a7fb5d23061a7";
    private const string vbb1698315db5 = "A200000027a3c3cd651952fcd9d07cf1f7";
    private const string v0d425fb761bb = "A20000002892bf4f74fba35633f6dcf4";
    private const string vcd9d2ba0c823 = "A2000000296716ccd4e88d96";
    private const string v350b97481607 = "A20000002ab6ca789b745ca5bfafdd553f75223c82346faecae4a3f2245722";
    private const string v280bcdd8245a = "A20000002b2f18b563";
    private const string vc9e0f8a4f29b = "A20000002c58e1140d";
    private const string v8ec6c834a693 = "A20000002d794dc5d04f75a1c16579502eb95b55b79354165703e8338527ae8c4f5ef1";
    private const string v27ef1eb6d4eb = "A20000002eed676fce25e7a03b59e69f4bd697b969edae3fc67401ff3ab410a93d5a884b07";
    private const string v503194e68898 = "A20000002f60189b1704257d01d9e6627f8a3d0214d9298b76bff038351af7f52cad";
    private const string vbc2c4684ae7a = "A200000030d1ca8bea5f1735124a8670067b8cb70b55d7300b20e2b526f2e72ea13a6170c0";
    private const string v46d17e34ff78 = "A2000000318579831d2520b52a5e614b8b21062366b7aff35e34";
    private const string v4ecc68939036 = "A2000000324c3cd3a6";
    private const string v0cfc51bf84ae = "A20000003351a9f4c4";
    private const string v9196030c469e = "A2000000340a0b24c0";
    private const string v91f4b76fcd35 = "A2000000356d65e72f";
    private const string va9e021975dad = "A200000036e16b8924";
    private const string v53c91b8e926a = "A2000000377ff190c214b4aa39deb9f38ba52c6c1558f9b5294eee53b323a5a800ba7b";
    private const string v00d771793310 = "A2000000386d5b97767456a706f60a68f47c46e99015f9845683954e055af40d";
    private const string vb826152b0ec6 = "A200000039386a8566288341cf34ace5965e99166cc49fc4f9815c14c022dc8ec4";
    private const string vdc9c916f7af3 = "A20000003a12d4c9544a867de239db";
    private const string v427f3e6fc87e = "A20000003b75e434f1";
    private const string v9415b2a2b6f3 = "A20000003c651136be";
    private const string vfd7692f662ba = "A20000003d2c78ac0c";
    private const string v442584a506d8 = "A20000003ece5c8f4e";
    private const string v8b9bebe58cee = "A20000003f2b6bce62854769e33cb27f5d";
    private const string vc760dfb268d4 = "A20000004092bbd21c3c04aaa104eee9f78aa8ff204f1c79f89f07ed";
    private const string vadb5e87a7460 = "A200000041fca057a7bb2b525587590492dc859f729d";
    private const string va08149c4f22a = "A200000042590f13bdda0d59819f60f219";
    private const string vbd90d5511a65 = "A200000043ce100b6308727dd2d5e8f2ce8ef2e3aa71";
    private const string vf49955c21997 = "A200000044c937301a285b5468af69b53bc2d9af6e79eaf886f204dbc5c2c5646a722dd2aae31d";
    private const string v655778885bd5 = "A200000045102df766d401";
    private const string v85f0a2b815ad = "A20000004669ab72872c36111e8163c6";
    private const string v6dd80d5ad21e = "A20000004743b5b8e78f68d3b4e926";
    private const string ve213a4d67fc8 = "A2000000485f2b93dc";
    private const string vefa1b3e332e1 = "A2000000494ab642bc";
    private const string v8f7dc8cf64c6 = "A20000004a2441c0c8";
    private const string v7be46bd939d4 = "A20000004b0a63a034";
    private const string v958fd4107274 = "A20000004c12e1a46b";
    private const string v965f74e6c570 = "A20000004d5484d293";
    private const string v63d0939e29e0 = "A20000004e6c71a558";
    private const string v81141a69c85f = "A20000004f322c384d";
    private const string v3d6e011e8b14 = "A200000050bce4a097";
    private const string vd464d7221fa8 = "A200000051f492d966";
    private const string v6780319f3938 = "A200000052652beca3";
    private const string v680bd6426e79 = "A200000053f8f8c5c6";

    internal static string vd667654bfdb0(int id)
    {
        switch (id)
        {
            case 0: return v7ab0c8d04c35(v258fc553f727);
            case 1: return v7ab0c8d04c35(vd54e33edd809);
            case 2: return v7ab0c8d04c35(vc593ef265ee8);
            case 3: return v7ab0c8d04c35(vf0656e858aca);
            case 4: return v7ab0c8d04c35(v7d61d4fa92ca);
            case 5: return v7ab0c8d04c35(v8509164f041e);
            case 6: return v7ab0c8d04c35(v7feb1baed5ad);
            case 7: return v7ab0c8d04c35(vcb7041bd9619);
            case 8: return v7ab0c8d04c35(v13cb383202ab);
            case 9: return v7ab0c8d04c35(v10412a9d8b30);
            case 10: return v7ab0c8d04c35(v8eb25bb7ec8a);
            case 11: return v7ab0c8d04c35(v2b2a8aa98391);
            case 12: return v7ab0c8d04c35(vce8a570b9053);
            case 13: return v7ab0c8d04c35(v4bafbac68da4);
            case 14: return v7ab0c8d04c35(v0c1d1cbc44bd);
            case 15: return v7ab0c8d04c35(vfb728cd61bc0);
            case 16: return v7ab0c8d04c35(vcdd8a43493d1);
            case 17: return v7ab0c8d04c35(v4c4a9c27f7e1);
            case 18: return v7ab0c8d04c35(v0904b52ba9f6);
            case 19: return v7ab0c8d04c35(v73f5124b54c2);
            case 20: return v7ab0c8d04c35(vcc78bc4ffff4);
            case 21: return v7ab0c8d04c35(v9bef63135eba);
            case 22: return v7ab0c8d04c35(v4559ac02f505);
            case 23: return v7ab0c8d04c35(v2de7ad807e31);
            case 24: return v7ab0c8d04c35(v19200f035947);
            case 25: return v7ab0c8d04c35(v214f3b3b2349);
            case 26: return v7ab0c8d04c35(ve106596a2b9b);
            case 27: return v7ab0c8d04c35(v1d7c79589222);
            case 28: return v7ab0c8d04c35(v5e5477e6f906);
            case 29: return v7ab0c8d04c35(vc69164f21b85);
            case 30: return v7ab0c8d04c35(vb5c86410a591);
            case 31: return v7ab0c8d04c35(v31f2a8548ef4);
            case 32: return v7ab0c8d04c35(vf86ccd23662d);
            case 33: return v7ab0c8d04c35(v8addc7d54bde);
            case 34: return v7ab0c8d04c35(v9e8bad4d621a);
            case 35: return v7ab0c8d04c35(vf2e21255ee8a);
            case 36: return v7ab0c8d04c35(v4b226ecb5411);
            case 37: return v7ab0c8d04c35(v4b3b30848ef5);
            case 38: return v7ab0c8d04c35(v43ec8b726833);
            case 39: return v7ab0c8d04c35(vbb1698315db5);
            case 40: return v7ab0c8d04c35(v0d425fb761bb);
            case 41: return v7ab0c8d04c35(vcd9d2ba0c823);
            case 42: return v7ab0c8d04c35(v350b97481607);
            case 43: return v7ab0c8d04c35(v280bcdd8245a);
            case 44: return v7ab0c8d04c35(vc9e0f8a4f29b);
            case 45: return v7ab0c8d04c35(v8ec6c834a693);
            case 46: return v7ab0c8d04c35(v27ef1eb6d4eb);
            case 47: return v7ab0c8d04c35(v503194e68898);
            case 48: return v7ab0c8d04c35(vbc2c4684ae7a);
            case 49: return v7ab0c8d04c35(v46d17e34ff78);
            case 50: return v7ab0c8d04c35(v4ecc68939036);
            case 51: return v7ab0c8d04c35(v0cfc51bf84ae);
            case 52: return v7ab0c8d04c35(v9196030c469e);
            case 53: return v7ab0c8d04c35(v91f4b76fcd35);
            case 54: return v7ab0c8d04c35(va9e021975dad);
            case 55: return v7ab0c8d04c35(v53c91b8e926a);
            case 56: return v7ab0c8d04c35(v00d771793310);
            case 57: return v7ab0c8d04c35(vb826152b0ec6);
            case 58: return v7ab0c8d04c35(vdc9c916f7af3);
            case 59: return v7ab0c8d04c35(v427f3e6fc87e);
            case 60: return v7ab0c8d04c35(v9415b2a2b6f3);
            case 61: return v7ab0c8d04c35(vfd7692f662ba);
            case 62: return v7ab0c8d04c35(v442584a506d8);
            case 63: return v7ab0c8d04c35(v8b9bebe58cee);
            case 64: return v7ab0c8d04c35(vc760dfb268d4);
            case 65: return v7ab0c8d04c35(vadb5e87a7460);
            case 66: return v7ab0c8d04c35(va08149c4f22a);
            case 67: return v7ab0c8d04c35(vbd90d5511a65);
            case 68: return v7ab0c8d04c35(vf49955c21997);
            case 69: return v7ab0c8d04c35(v655778885bd5);
            case 70: return v7ab0c8d04c35(v85f0a2b815ad);
            case 71: return v7ab0c8d04c35(v6dd80d5ad21e);
            case 72: return v7ab0c8d04c35(ve213a4d67fc8);
            case 73: return v7ab0c8d04c35(vefa1b3e332e1);
            case 74: return v7ab0c8d04c35(v8f7dc8cf64c6);
            case 75: return v7ab0c8d04c35(v7be46bd939d4);
            case 76: return v7ab0c8d04c35(v958fd4107274);
            case 77: return v7ab0c8d04c35(v965f74e6c570);
            case 78: return v7ab0c8d04c35(v63d0939e29e0);
            case 79: return v7ab0c8d04c35(v81141a69c85f);
            case 80: return v7ab0c8d04c35(v3d6e011e8b14);
            case 81: return v7ab0c8d04c35(vd464d7221fa8);
            case 82: return v7ab0c8d04c35(v6780319f3938);
            case 83: return v7ab0c8d04c35(v680bd6426e79);
            default: return string.Empty;
        }
    }

    private static string v7ab0c8d04c35(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)v1e4095b2bd75 * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
