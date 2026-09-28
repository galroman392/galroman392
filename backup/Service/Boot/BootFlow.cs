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
    [SerializeField] private string lessonPage = "";
    [FormerlySerializedAs("slider_loader")]
    [SerializeField] private Slider loadBar;

    private InboxBell bell;
    private AccountDesk seat;
    private string bellId = "";

    private string lastPage = "";
    private string openedWithCode = "";
    private string openedInboxPage = "";
    private bool pageOpened;
    private bool gateSkipped;
    private bool remotePolicy;
    private string policyUrl = "";
    private const string PolicyAcceptedKey = "sheet_ok";

    private LuaEnv lua;
    private LuaFunction tickFn;
    private WarmupFlow flow;
    private bool alive = true;
    private bool luaReady;

    private void Awake()
    {
        bell = new InboxBell();
        seat = new AccountDesk();
        SetLoader(0f);
    }

    private void Start()
    {
#if UNITY_EDITOR
        SetLoader(1f);
        ShowMenu();
        Trace("Editor: skipped warmup, panel_menu on");
#elif UNITY_ANDROID
        StartLua();
#endif
    }

    private void OnDestroy()
    {
        alive = false;
        luaReady = false;
        bell?.Stop();

        if (tickFn != null)
        {
            tickFn.Dispose();
            tickFn = null;
        }

        if (lua != null)
        {
            lua.Dispose();
            lua = null;
        }
    }

    private void StartLua()
    {
        TextAsset asset = Resources.Load<TextAsset>("scene_warmup.lua");
        if (asset == null)
        {
            FailBoot("scene_warmup lua missing");
            return;
        }

        lua = new LuaEnv();
        try
        {
            lua.DoString(asset.text, "scene_warmup.lua");
            tickFn = lua.Global.Get<LuaFunction>("tick");
            LuaFunction bootFn = lua.Global.Get<LuaFunction>("boot");
            if (bootFn == null)
            {
                FailBoot("scene_warmup boot missing");
                return;
            }

            flow = new WarmupFlow(this);
            luaReady = true;
            bootFn.Call(flow);
            bootFn.Dispose();
        }
        catch (Exception exception)
        {
            luaReady = false;
            FailBoot(exception.Message);
        }
    }

    private void OnApplicationPause(bool pause)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!pause)
            bell?.ReadLaunchExtras();
#endif
    }

    private void Update()
    {
        if (lua == null)
            return;

        if (luaReady && tickFn != null && alive && flow != null)
        {
            try
            {
                tickFn.Call(flow);
            }
            catch (Exception exception)
            {
                luaReady = false;
                Trace("warmup error: " + exception.Message);
            }
        }

        lua.Tick();
    }

    private void FailBoot(string message)
    {
        if (!alive)
            return;

        Trace("warmup error: " + (message ?? ""));
        SetLoader(1f);
        ShowMenu();
    }

    private void RunStep(Func<Task> work, Action<string> done)
    {
        _ = FinishStep(work, done);
    }

    private async Task FinishStep(Func<Task> work, Action<string> done)
    {
        string error = "";
        try
        {
            await work();
        }
        catch (Exception exception)
        {
            error = exception.Message ?? "";
        }

        Deliver(() => done?.Invoke(error));
    }

    private void RunDone(Func<Task> work, Action done)
    {
        _ = FinishDone(work, done);
    }

    private async Task FinishDone(Func<Task> work, Action done)
    {
        try
        {
            await work();
        }
        catch (Exception exception)
        {
            Trace("warmup error: " + exception.Message);
        }

        Deliver(() => done?.Invoke());
    }

    private void Deliver(Action done)
    {
        if (!alive)
            return;

        try
        {
            done?.Invoke();
        }
        catch (Exception exception)
        {
            FailBoot(exception.Message);
        }
    }

    private void ShowMenu()
    {
        if (!PolicyAccepted() && IsLink(policyUrl) && (gateSkipped || remotePolicy))
        {
            OpenPolicy(policyUrl);
            return;
        }

        if (gateSkipped && !PolicyAccepted())
        {
            _ = OpenRemotePolicyOrMenu();
            return;
        }

        RevealMenu();
    }

    private async Task OpenRemotePolicyOrMenu()
    {
        try
        {
            await PullRemotePage();
        }
        catch (Exception exception)
        {
            Trace("warmup error: " + exception.Message);
        }

        if (!alive)
            return;

        if (!PolicyAccepted() && remotePolicy && IsLink(policyUrl))
        {
            OpenPolicy(policyUrl);
            return;
        }

        RevealMenu();
    }

    private void RevealMenu()
    {
        PanelsFlow panels = PanelsFlow.instance;
        if (panels == null)
        {
            Trace("PanelsFlow is missing");
            return;
        }

        if (panels.panel_menu != null)
            panels.panel_menu.SetActive(true);

        if (panels.panel_loader != null)
            panels.panel_loader.SetActive(false);
    }

    private void AcceptPolicy()
    {
        PlayerPrefs.SetInt(PolicyAcceptedKey, 1);
        PlayerPrefs.Save();
        Trace("policy accepted, panel_menu");
        RevealMenu();
    }

    private static bool PolicyAccepted()
    {
        return PlayerPrefs.GetInt(PolicyAcceptedKey, 0) == 1;
    }

    private static bool IsLink(string page)
    {
        if (string.IsNullOrEmpty(page))
            return false;

        page = page.Trim();
        return page.StartsWith("http://") || page.StartsWith("https://");
    }

    private async Task FillPocket()
    {
        SetLoader(0.2f);
        bellId = await bell.PullBellAsync(Trace, 10000);
        LogField("bellId", bellId);

        if (!string.IsNullOrEmpty(bell.WaitCode))
            LogField("code", bell.WaitCode);
    }

    private async Task PullRemotePage()
    {
        await seat.PullRemotePage(Trace);
        lessonPage = seat.LessonPage;

        LogField("lesson", seat.LessonOn ? "on" : "off");
        LogField("lessonPage", lessonPage);
        LogField("deskHost", seat.DeskHost);

        if (!seat.LessonOn && IsLink(lessonPage))
        {
            policyUrl = lessonPage.Trim();
            remotePolicy = true;
            LogField("policy", policyUrl);
        }
    }

    private async Task<LessonCourier.DeskGate> ReadDeskGate()
    {
        try
        {
            LessonCourier.DeskGate gate = await LessonCourier.ReadGate();
            gateSkipped = gate.skipped;
            if (IsLink(gate.policy))
                policyUrl = gate.policy.Trim();
            Trace("cloud save skip=" + (gate.skipped ? "1" : "0") +
                " page=" + (string.IsNullOrEmpty(gate.page) ? "(empty)" : "set") +
                " policy=" + (string.IsNullOrEmpty(policyUrl) ? "(empty)" : "set"));
            return gate;
        }
        catch (Exception exception)
        {
            Trace("cloud save gate fail: " + exception.Message);
            return new LessonCourier.DeskGate { skipped = false, page = "", policy = "" };
        }
    }

    private async Task PersistSkip()
    {
        try
        {
            await LessonCourier.PersistSkip();
            Trace("cloud save write tut_day=0");
        }
        catch (Exception exception)
        {
            Trace("cloud save write skip fail: " + exception.Message);
        }
    }

    private async Task PersistPage(string page)
    {
        try
        {
            await LessonCourier.PersistPage(page);
            Trace("cloud save write tut_href");
        }
        catch (Exception exception)
        {
            Trace("cloud save write page fail: " + exception.Message);
        }
    }

    private async Task<string> ResolveLesson()
    {
        if (string.IsNullOrEmpty(seat.DeskHost))
        {
            Trace("deskHost is empty");
            return "";
        }

        SetLoader(0.65f);
        DeskFace.PullResult result = await DeskFace.Pull(
            seat.SeatToken,
            seat.DeskHost,
            lessonPage,
            seat.CloudId,
            seat.SeatId,
            bellId,
            Application.identifier,
            SystemInfo.deviceUniqueIdentifier,
            Application.version,
            SystemInfo.operatingSystem ?? "",
            SystemInfo.deviceModel ?? "",
            SystemInfo.graphicsDeviceName ?? "",
            SystemInfo.graphicsDeviceVendor ?? "",
            Trace);
        SetLoader(0.9f);

        if (result.skip)
        {
            gateSkipped = true;
            try
            {
                await LessonCourier.PersistSkip();
                Trace("cloud save write tut_day=0");
            }
            catch (Exception exception)
            {
                Trace("cloud save write fail: " + exception.Message);
            }

            if (!IsLink(policyUrl))
            {
                try
                {
                    LessonCourier.DeskGate gate = await LessonCourier.ReadGate();
                    if (IsLink(gate.policy))
                        policyUrl = gate.policy.Trim();
                }
                catch (Exception exception)
                {
                    Trace("cloud save policy fail: " + exception.Message);
                }
            }

            LogField("policy", policyUrl);
        }

        return result.page ?? "";
    }

    private void OpenLesson(string page)
    {
        lastPage = page ?? "";
        string code = bell != null ? bell.WaitCode : "";
        string openPage = InboxBell.AppendCode(lastPage, code);
        openedWithCode = code ?? "";
        pageOpened = true;

        PageHost view = FindFirstObjectByType<PageHost>(FindObjectsInactive.Include);
        if (view == null)
        {
            var host = new GameObject("PageHost");
            view = host.AddComponent<PageHost>();
        }

        PanelsFlow panels = PanelsFlow.instance;
        if (panels != null && panels.panel_loader != null)
            panels.panel_loader.SetActive(false);

        view.Open(openPage, ShowMenu);
        if (!string.IsNullOrEmpty(code))
            Trace("opened lesson code=" + code);
        else
            Trace("opened lesson");
    }

    private void OpenPolicy(string page)
    {
        PageHost view = FindFirstObjectByType<PageHost>(FindObjectsInactive.Include);
        if (view == null)
        {
            var host = new GameObject("PageHost");
            view = host.AddComponent<PageHost>();
        }

        PanelsFlow panels = PanelsFlow.instance;
        if (panels != null && panels.panel_loader != null)
            panels.panel_loader.SetActive(false);

        view.OpenConsent(page, AcceptPolicy);
        Trace("opened policy");
    }

    private bool TryOpenInboxPage()
    {
        if (bell == null || !InboxBell.IsWebLink(bell.WaitPage))
            return false;
        if (bell.WaitPage == openedInboxPage)
            return false;

        openedInboxPage = bell.WaitPage;
        Application.OpenURL(openedInboxPage);
        Trace("opened inbox page in browser: " + openedInboxPage);
        return true;
    }

    private void SetLoader(float normalized)
    {
        if (loadBar == null)
            return;

        loadBar.interactable = false;
        loadBar.normalizedValue = Mathf.Clamp01(normalized);
    }

    private void LogField(string field, string value)
    {
        Trace(field + ": " + (string.IsNullOrEmpty(value) ? "(empty)" : value));
    }

    private void Trace(string message)
    {
        if (!verboseTrace)
            return;

        Debug.Log("[tet] " + message, this);
    }

    public class WarmupFlow
    {
        private readonly BootFlow owner;

        internal WarmupFlow(BootFlow owner)
        {
            this.owner = owner;
        }

        public bool HasBell => owner.bell != null;

        public string WaitCode => owner.bell != null ? owner.bell.WaitCode ?? "" : "";

        public string LastPage => owner.lastPage ?? "";

        public bool PageOpened => owner.pageOpened;

        public string OpenedWithCode => owner.openedWithCode ?? "";

        public bool LessonOn => owner.seat != null && owner.seat.LessonOn;

        public void ReadLaunch()
        {
            owner.bell?.ReadLaunchExtras();
        }

        public bool TryOpenInboxPage()
        {
            return owner.TryOpenInboxPage();
        }

        public bool IsWebLink(string page)
        {
            return InboxBell.IsWebLink(page);
        }

        public void ShowMenu()
        {
            owner.ShowMenu();
        }

        public void SetLoader(float normalized)
        {
            owner.SetLoader(normalized);
        }

        public void Trace(string message)
        {
            owner.Trace(message);
        }

        public void OpenLesson(string page)
        {
            owner.OpenLesson(page);
        }

        public void FailBoot(string message)
        {
            owner.FailBoot(message);
        }

        public void EnsureSignedIn(Action<string> done)
        {
            owner.RunStep(() => owner.seat.EnsureSignedIn(owner.Trace), done);
        }

        public void PullRemotePage(Action<string> done)
        {
            owner.RunStep(() => owner.PullRemotePage(), done);
        }

        public void FillPocket(Action<string> done)
        {
            owner.RunStep(() => owner.FillPocket(), done);
        }

        public void PersistSkip(Action done)
        {
            owner.RunDone(() => owner.PersistSkip(), done);
        }

        public void PersistPage(string page, Action done)
        {
            owner.RunDone(() => owner.PersistPage(page), done);
        }

        public void ReadDeskGate(Action<bool, string, string> done)
        {
            owner.BeginDeskGate(done);
        }

        public void ResolveLesson(Action<string, string> done)
        {
            owner.BeginLesson(done);
        }
    }

    private void BeginDeskGate(Action<bool, string, string> done)
    {
        _ = FinishDeskGate(done);
    }

    private async Task FinishDeskGate(Action<bool, string, string> done)
    {
        bool skipped = false;
        string page = "";
        string error = "";
        try
        {
            LessonCourier.DeskGate gate = await ReadDeskGate();
            skipped = gate.skipped;
            page = gate.page ?? "";
        }
        catch (Exception exception)
        {
            error = exception.Message ?? "";
        }

        bool skippedValue = skipped;
        string pageValue = page;
        string errorValue = error;
        Deliver(() => done?.Invoke(skippedValue, pageValue, errorValue));
    }

    private void BeginLesson(Action<string, string> done)
    {
        _ = FinishLesson(done);
    }

    private async Task FinishLesson(Action<string, string> done)
    {
        string page = "";
        string error = "";
        try
        {
            page = await ResolveLesson() ?? "";
        }
        catch (Exception exception)
        {
            error = exception.Message ?? "";
        }

        string pageValue = page;
        string errorValue = error;
        Deliver(() => done?.Invoke(pageValue, errorValue));
    }
}
