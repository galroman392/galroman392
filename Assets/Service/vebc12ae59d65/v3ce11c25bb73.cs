using System;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class v3ce11c25bb73 : MonoBehaviour
{
    private GameObject vb7c99eb4273c;
    private vd14e3cc7c800 v51822b575a86;
    private Canvas vd8cde1fd5d58;
    private RectTransform v09a58c6eb02a;
    private Rect v15b25fd566ea;
    private Vector2 ve58b7587b1e9;
    private bool vcc6d1f48a72c;
    private bool v305e0fb5e355;
    private bool vac89a5d3421f;
    private bool ve1b163a0fc7c;
    private bool vd77e89f0e246;
    private bool v655eaf210e19;
    private Action vebfad412445c;
    private GameObject v8892c4878280;
    private Text v187023bd0cd1;
    private Button v1e35f84933d3;
    private Button vd7d52c61f8c5;
    private PermissionCallbacks v726459a8ee57;
    private int v8667ab25f39c;
    private bool v2dfacb77c5d9;
    private bool v4e223cc15b31;
    private bool v917385db711e;
    private bool v915df17b37e1;
    private Action v037942739148;
    private GameObject v2fde38ea85e3;

    private readonly string[] vc62081b756b4 =
    {
        ve1d4833e86de.v92a94600e486(0),
        ve1d4833e86de.v92a94600e486(1),
        ve1d4833e86de.v92a94600e486(2),
        ve1d4833e86de.v92a94600e486(3),
        ve1d4833e86de.v92a94600e486(4),
        ve1d4833e86de.v92a94600e486(5),
        ve1d4833e86de.v92a94600e486(6),
        ve1d4833e86de.v92a94600e486(7),
        ve1d4833e86de.v92a94600e486(8),
        ve1d4833e86de.v92a94600e486(9),
        ve1d4833e86de.v92a94600e486(10),
        ve1d4833e86de.v92a94600e486(11),
        ve1d4833e86de.v92a94600e486(12),
    };

    private static string va523521c04ae()
    {
        return Application.productName + ve1d4833e86de.v92a94600e486(13) + Application.version + ve1d4833e86de.v92a94600e486(14) + SystemInfo.operatingSystem + ve1d4833e86de.v92a94600e486(15) + SystemInfo.deviceModel + ve1d4833e86de.v92a94600e486(16) + Application.systemLanguage + ve1d4833e86de.v92a94600e486(17);
    }

    private void Awake()
    {
        Input.backButtonLeavesApp = false;
        v3f646832145b();
        v9a2d79455cee();
        v6b53c728fab8();
        if (v51822b575a86 == null || !v51822b575a86.Ready)
            return;

        v305e0fb5e355 = true;
        v51822b575a86.Load(ve1d4833e86de.v92a94600e486(18));
        v9d02a20686d3(ve1d4833e86de.v92a94600e486(19));
    }

    public void v5eb78acdf934(string v3afe30aa7e19)
    {
        v915df17b37e1 = false;
        v0a52c2a3864a(v3afe30aa7e19, null);
    }

    public void v5eb78acdf934(string v3afe30aa7e19, Action v510b7ece13e6)
    {
        v915df17b37e1 = false;
        v0a52c2a3864a(v3afe30aa7e19, v510b7ece13e6);
    }

    public void v5eb78acdf934(string v3afe30aa7e19, GameObject v399ab806eccb, Action v510b7ece13e6)
    {
        v915df17b37e1 = false;
        if (v399ab806eccb != null)
            vb7c99eb4273c = v399ab806eccb;

        v0a52c2a3864a(v3afe30aa7e19, v510b7ece13e6);
    }

    public void vded3c76ad84f(string v3afe30aa7e19, Action v6a9d1709a696)
    {
        v915df17b37e1 = true;
        v037942739148 = v6a9d1709a696;
        v0a52c2a3864a(v3afe30aa7e19, null);
        v9d8bb2b96405();
        vb9e2c08a3f7f();
    }

    private void v0a52c2a3864a(string v3afe30aa7e19, Action v510b7ece13e6)
    {
        if (!vdf8ecb272e0c(v3afe30aa7e19))
        {
            v9d02a20686d3(ve1d4833e86de.v92a94600e486(20));
            return;
        }

        v9d02a20686d3(ve1d4833e86de.v92a94600e486(21));
        if (!v915df17b37e1)
        {
            v037942739148 = null;
            v68dc426efe88();
        }

        vebfad412445c = v510b7ece13e6;
        v6b53c728fab8();
        if (v51822b575a86 == null || !v51822b575a86.Ready)
        {
            Application.OpenURL(v3afe30aa7e19);
            return;
        }

        v305e0fb5e355 = false;
        vac89a5d3421f = false;
        ve1b163a0fc7c = vd77e89f0e246 = false;
        v51822b575a86.SetUserAgent(ve1d4833e86de.v92a94600e486(22));
        vb9e2c08a3f7f();
        vd3063a8f993e();

        v51822b575a86.vc34bc6be98c9();
        v51822b575a86.Load(v3afe30aa7e19);
        v51822b575a86.Show();
        v655eaf210e19 = true;
        v9d02a20686d3(ve1d4833e86de.v92a94600e486(23));
    }

    private void v6b53c728fab8()
    {
        if (v51822b575a86 == null)
        {
            v9d02a20686d3(ve1d4833e86de.v92a94600e486(24));
            v51822b575a86 = new vd14e3cc7c800();
#if UNITY_ANDROID && !UNITY_EDITOR
            v51822b575a86.SetAcceptThirdPartyCookies(true);
#endif
            v51822b575a86.SetUserAgent(ve1d4833e86de.v92a94600e486(25));
        }

        ved03870a02ad();
        vb9e2c08a3f7f();
    }

    private void ved03870a02ad()
    {
        if (vcc6d1f48a72c || v51822b575a86 == null)
            return;

        vcc6d1f48a72c = true;
        v51822b575a86.AllowCapture = resources =>
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            bool video = string.IsNullOrEmpty(resources) || Contains(resources, ve1d4833e86de.v92a94600e486(26));
            bool audio = Contains(resources, ve1d4833e86de.v92a94600e486(27));
             string microphone = ve1d4833e86de.v92a94600e486(28);
            bool needVideo = video && !Permission.HasUserAuthorizedPermission(Permission.Camera);
            bool needAudio = audio && !Permission.HasUserAuthorizedPermission(microphone);
            if (!needVideo && !needAudio)
                return 1;

            v2dfacb77c5d9 = needVideo;
            v4e223cc15b31 = needAudio;
            return 2;
#else
            return 1;
#endif
        };

        v51822b575a86.HandleRequest = (v3afe30aa7e19, mainFrame, isPopup) =>
        {
            if (string.IsNullOrEmpty(v3afe30aa7e19))
                return true;

            if (isPopup)
                return !v513eb76fd31e(v3afe30aa7e19);

            if (Contains(v3afe30aa7e19, ve1d4833e86de.v92a94600e486(29)) || v513eb76fd31e(v3afe30aa7e19))
            {
                if (vb7c99eb4273c != null)
                    vb7c99eb4273c.SetActive(false);

                return false;
            }

            if (mainFrame &&
                ((vbb0f92668d73(v3afe30aa7e19) && !ve1b163a0fc7c) ||
                 (v14c6ff1a25a3(v3afe30aa7e19) && !vd77e89f0e246)))
            {
                if (vbb0f92668d73(v3afe30aa7e19))
                    ve1b163a0fc7c = true;
                else
                    vd77e89f0e246 = true;

                v51822b575a86.SetUserAgent(va523521c04ae());
                v51822b575a86.Load(v3afe30aa7e19);
                v9d02a20686d3(ve1d4833e86de.v92a94600e486(30));
                return false;
            }

            return true;
        };

        v51822b575a86.v82dc8252cab0 += v3afe30aa7e19 =>
        {
            if (v305e0fb5e355 && v2f5d2e12273d(v3afe30aa7e19))
                return;

            v9d02a20686d3(ve1d4833e86de.v92a94600e486(31));
            if (Contains(v3afe30aa7e19, ve1d4833e86de.v92a94600e486(32)) || Contains(v3afe30aa7e19, ve1d4833e86de.v92a94600e486(33)) || v513eb76fd31e(v3afe30aa7e19))
            {
                v51822b575a86.vc34bc6be98c9();
                v046efda91089(v3afe30aa7e19);
            }
        };

        v51822b575a86.v995dced0f1ce += v3afe30aa7e19 =>
        {
            if (!v305e0fb5e355 && !vac89a5d3421f && vdf8ecb272e0c(v3afe30aa7e19))
                v904f034d9c67(ve1d4833e86de.v92a94600e486(34));
        };

        v51822b575a86.ve262c61e4e78 += progress =>
        {
            if (!v305e0fb5e355 && !vac89a5d3421f && progress >= 0.65f)
                v904f034d9c67(ve1d4833e86de.v92a94600e486(35));
        };

        v51822b575a86.v3bc86bba2034 += (status, v3afe30aa7e19) =>
        {
            if (v305e0fb5e355 && v2f5d2e12273d(v3afe30aa7e19))
            {
                v305e0fb5e355 = false;
                v9d02a20686d3(ve1d4833e86de.v92a94600e486(36));
                return;
            }

            v9d02a20686d3(ve1d4833e86de.v92a94600e486(37) + status);
            if (!vac89a5d3421f)
                v904f034d9c67(ve1d4833e86de.v92a94600e486(38));
            else if (vb7c99eb4273c != null)
                vb7c99eb4273c.SetActive(false);

            v4087abaf049c(v3afe30aa7e19);
        };

        v51822b575a86.vd7a16cf5d1dd += failing =>
        {
            v9d02a20686d3(ve1d4833e86de.v92a94600e486(39) + failing);
            if (!string.IsNullOrEmpty(failing) && !v2f5d2e12273d(failing))
            {
                v51822b575a86.vc34bc6be98c9();
                v513eb76fd31e(failing);
            }
        };

        v51822b575a86.v4778c632ef9b += id =>
        {
            v51822b575a86.ScrollToTop();
            v9d02a20686d3(ve1d4833e86de.v92a94600e486(40) + id);
        };

        v51822b575a86.v4420348c42c0 += id => v9d02a20686d3(ve1d4833e86de.v92a94600e486(41) + id);
    }

    private void v43b265ead9b6()
    {
        if (v726459a8ee57 != null)
            return;

        v726459a8ee57 = new PermissionCallbacks();
        v726459a8ee57.PermissionGranted += v34947ce84814 => v4a6527ecc73b(true);
        v726459a8ee57.PermissionDenied += v34947ce84814 => v4a6527ecc73b(false);
        v726459a8ee57.PermissionDeniedAndDontAskAgain += v34947ce84814 => v4a6527ecc73b(false);
    }

    private void v24ab025ced36()
    {
        if (!v2dfacb77c5d9 && !v4e223cc15b31)
            return;

        v43b265ead9b6();
        v8667ab25f39c = 0;
        if (v2dfacb77c5d9)
        {
            v2dfacb77c5d9 = false;
            v8667ab25f39c++;
            Permission.RequestUserPermission(Permission.Camera, v726459a8ee57);
        }

        if (v4e223cc15b31)
        {
            v4e223cc15b31 = false;
            v8667ab25f39c++;
            Permission.RequestUserPermission(ve1d4833e86de.v92a94600e486(42), v726459a8ee57);
        }
    }

    private void v4a6527ecc73b(bool vbe703ded55e4)
    {
        if (!vbe703ded55e4)
        {
            v8667ab25f39c = 0;
            v2dfacb77c5d9 = false;
            v4e223cc15b31 = false;
            if (v51822b575a86 != null)
                v51822b575a86.FinishCapture(false);
            return;
        }

        v8667ab25f39c--;
        if (v8667ab25f39c <= 0 && v51822b575a86 != null)
        {
            v8667ab25f39c = 0;
            v51822b575a86.FinishCapture(true);
        }
    }

    private void v904f034d9c67(string vf8b9bc3ba2b0)
    {
        vac89a5d3421f = true;
        vd3063a8f993e();
        if (vb7c99eb4273c != null)
            vb7c99eb4273c.SetActive(false);

        vb9e2c08a3f7f();
        if (v51822b575a86 != null)
            v51822b575a86.Show();

        v655eaf210e19 = true;
        v9d02a20686d3(ve1d4833e86de.v92a94600e486(43) + vf8b9bc3ba2b0);
    }

    public void vf1a623d3c872()
    {
        v9d02a20686d3(ve1d4833e86de.v92a94600e486(44));
        if (v51822b575a86 != null)
            v51822b575a86.v374b7ca4f1e4();

        v3ae4875803fb();
        v655eaf210e19 = false;
        if (vb7c99eb4273c != null)
            vb7c99eb4273c.SetActive(false);

        if (v8892c4878280 != null)
            v8892c4878280.SetActive(false);

        v915df17b37e1 = false;
        v037942739148 = null;
        v68dc426efe88();

        Action v03fcc0bc877e = vebfad412445c;
        vebfad412445c = null;
        if (v03fcc0bc877e != null)
            v03fcc0bc877e.Invoke();
    }

    private void Update()
    {
        v24ab025ced36();
        v4151ad07d18d();

        if (!v915df17b37e1 && v3957dfd83d44() && v51822b575a86 != null && v655eaf210e19 && !va834df695281.v53d6cf1cf001)
        {
            if (v8892c4878280 == null || !v8892c4878280.activeSelf)
                v8698059ecc76();

            return;
        }

        if (v15b25fd566ea != Screen.safeArea ||
            ve58b7587b1e9.x != Screen.width ||
            ve58b7587b1e9.y != Screen.height)
        {
            vb9e2c08a3f7f();
        }
    }

    private void OnApplicationFocus(bool v40fd6a75a781)
    {
        if (v40fd6a75a781)
            vb9e2c08a3f7f();
    }

    private void OnApplicationPause(bool vce5db26feb5d)
    {
        if (!vce5db26feb5d)
            vb9e2c08a3f7f();
    }

    private static bool v3957dfd83d44()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null && keyboard.escapeKey.wasReleasedThisFrame)
            return true;

#if ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKeyUp(KeyCode.Escape);
#else
        return false;
#endif
    }

    private void v8698059ecc76()
    {
        try
        {
            if (v51822b575a86.CloseTopPopup())
            {
                v9d02a20686d3(ve1d4833e86de.v92a94600e486(45));
                return;
            }

            string backUrl = v51822b575a86.CanGoBack ? v51822b575a86.BackUrl() : null;
            if (!string.IsNullOrWhiteSpace(backUrl) && !v2f5d2e12273d(backUrl))
            {
                v9d02a20686d3(ve1d4833e86de.v92a94600e486(46));
                v51822b575a86.GoBack();
                return;
            }
        }
        catch (Exception e)
        {
            v9d02a20686d3(ve1d4833e86de.v92a94600e486(47) + e.Message);
        }

        v9d02a20686d3(ve1d4833e86de.v92a94600e486(48));
        v51822b575a86.v374b7ca4f1e4();
        v52ad2d191583();
    }

    private void vd3063a8f993e()
    {
        v917385db711e = true;
        v4151ad07d18d();
    }

    private void v3ae4875803fb()
    {
        v917385db711e = false;
    }

    private void v4151ad07d18d()
    {
        if (!v917385db711e)
            return;

        v0ac98812b868 panels = v0ac98812b868.va1dff10c587c;
        if (panels == null)
            return;

        vb0b5c8d8cb01(panels.panel_loader);
        vb0b5c8d8cb01(panels.panel_menu);
        vb0b5c8d8cb01(panels.panel_game);
    }

    private static void vb0b5c8d8cb01(GameObject v87ec8244344c)
    {
        if (v87ec8244344c != null && v87ec8244344c.activeSelf)
            v87ec8244344c.SetActive(false);
    }

    private Canvas v3f646832145b()
    {
        if (vd8cde1fd5d58 != null)
            return vd8cde1fd5d58;

        var go = new GameObject(ve1d4833e86de.v92a94600e486(49), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        go.transform.SetParent(transform, false);
        vd8cde1fd5d58 = go.GetComponent<Canvas>();
        vd8cde1fd5d58.renderMode = RenderMode.ScreenSpaceOverlay;
        vd8cde1fd5d58.sortingOrder = short.MaxValue;
        vd8cde1fd5d58.overrideSorting = true;
        vf8052d7a7fa0();
        return vd8cde1fd5d58;
    }

    private void vf8052d7a7fa0()
    {
        EventSystem live = null;
        EventSystem[] all = Resources.FindObjectsOfTypeAll<EventSystem>();
        for (int i = 0; i < all.Length; i++)
        {
            EventSystem candidate = all[i];
            if (candidate == null || !candidate.isActiveAndEnabled)
                continue;
            if (candidate.gameObject.hideFlags != HideFlags.None)
                continue;

            live = candidate;
            break;
        }

        if (live == null)
        {
            var events = new GameObject(ve1d4833e86de.v92a94600e486(50));
            events.transform.SetParent(transform, false);
            live = events.AddComponent<EventSystem>();
            events.AddComponent<InputSystemUIInputModule>();
            return;
        }

        if (live.GetComponent<InputSystemUIInputModule>() == null)
        {
            StandaloneInputModule oldModule = live.GetComponent<StandaloneInputModule>();
            if (oldModule != null)
                Destroy(oldModule);

            live.gameObject.AddComponent<InputSystemUIInputModule>();
        }
    }

    private void v9a2d79455cee()
    {
        if (v09a58c6eb02a != null)
            return;

        var go = new GameObject(ve1d4833e86de.v92a94600e486(51), typeof(RectTransform));
        v09a58c6eb02a = go.GetComponent<RectTransform>();
        v09a58c6eb02a.SetParent(v3f646832145b().transform, false);
        v09a58c6eb02a.anchorMin = Vector2.zero;
        v09a58c6eb02a.anchorMax = Vector2.one;
        v09a58c6eb02a.offsetMin = v09a58c6eb02a.offsetMax = Vector2.zero;
    }

    private void vb9e2c08a3f7f()
    {
        v9a2d79455cee();
        if (Screen.width <= 0 || Screen.height <= 0)
            return;

        Rect v1eeb22f60420 = Screen.safeArea;
        v09a58c6eb02a.anchorMin = new Vector2(v1eeb22f60420.xMin / Screen.width, v1eeb22f60420.yMin / Screen.height);
        v09a58c6eb02a.anchorMax = new Vector2(v1eeb22f60420.xMax / Screen.width, v1eeb22f60420.yMax / Screen.height);
        v09a58c6eb02a.offsetMin = v09a58c6eb02a.offsetMax = Vector2.zero;

        if (v51822b575a86 != null && v51822b575a86.Ready)
        {
            int left = Mathf.Max(0, Mathf.RoundToInt(v1eeb22f60420.xMin));
            int right = Mathf.Max(0, Mathf.RoundToInt(Screen.width - v1eeb22f60420.xMax));
            int bottom = Mathf.Max(0, Mathf.RoundToInt(v1eeb22f60420.yMin));
            if (v915df17b37e1)
                bottom += va6dc00296d7a();
            int top = Mathf.Max(0, Mathf.RoundToInt(Screen.height - v1eeb22f60420.yMax));
            v51822b575a86.SetMargins(left, top, right, bottom, Screen.width, Screen.height);
            v70c73ea92256(v1eeb22f60420);
        }

        v15b25fd566ea = Screen.safeArea;
        ve58b7587b1e9 = new Vector2(Screen.width, Screen.height);
        v9d02a20686d3(ve1d4833e86de.v92a94600e486(52) + Screen.width + ve1d4833e86de.v92a94600e486(53) + Screen.height);
    }

    private void v52ad2d191583()
    {
        v655eaf210e19 = false;
        v3f646832145b();
        vd8cde1fd5d58.gameObject.SetActive(true);
        ve95f8f44f08a();
        v187023bd0cd1.text = vc62081b756b4[UnityEngine.Random.Range(0, vc62081b756b4.Length)];
        v1e35f84933d3.onClick.RemoveAllListeners();
        vd7d52c61f8c5.onClick.RemoveAllListeners();
        v1e35f84933d3.onClick.AddListener(() =>
        {
            v8892c4878280.SetActive(false);
            if (v51822b575a86 != null)
                v51822b575a86.Show();
            v655eaf210e19 = true;
            v9d02a20686d3(ve1d4833e86de.v92a94600e486(54));
        });
        vd7d52c61f8c5.onClick.AddListener(() =>
        {
            v8892c4878280.SetActive(false);
            if (v51822b575a86 != null)
                v51822b575a86.v374b7ca4f1e4();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
        v8892c4878280.SetActive(true);
        v8892c4878280.transform.SetAsLastSibling();
        v9d02a20686d3(ve1d4833e86de.v92a94600e486(55));
    }

    private void ve95f8f44f08a()
    {
        if (v8892c4878280 != null)
            return;

        v9a2d79455cee();
        Sprite uiSprite = Resources.GetBuiltinResource<Sprite>(ve1d4833e86de.v92a94600e486(56));
        v8892c4878280 = new GameObject(ve1d4833e86de.v92a94600e486(57), typeof(RectTransform), typeof(Image));
        var root = v8892c4878280.GetComponent<RectTransform>();
        root.SetParent(v09a58c6eb02a, false);
        root.anchorMin = Vector2.zero;
        root.anchorMax = Vector2.one;
        root.offsetMin = root.offsetMax = Vector2.zero;
        Image dim = v8892c4878280.GetComponent<Image>();
        dim.sprite = uiSprite;
        dim.type = Image.Type.Sliced;
        dim.color = new Color(0f, 0f, 0f, 0.65f);

        var v87ec8244344c = new GameObject(ve1d4833e86de.v92a94600e486(58), typeof(RectTransform), typeof(Image));
        var panelRect = v87ec8244344c.GetComponent<RectTransform>();
        panelRect.SetParent(root, false);
        panelRect.anchorMin = panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(Mathf.Min(Screen.width * 0.86f, 720f), 380f);
        Image panelImage = v87ec8244344c.GetComponent<Image>();
        panelImage.sprite = uiSprite;
        panelImage.type = Image.Type.Sliced;
        panelImage.color = Color.white;

        var textObject = new GameObject(ve1d4833e86de.v92a94600e486(59), typeof(RectTransform), typeof(Text));
        var textRect = textObject.GetComponent<RectTransform>();
        textRect.SetParent(panelRect, false);
        textRect.anchorMin = new Vector2(0.08f, 0.42f);
        textRect.anchorMax = new Vector2(0.92f, 0.92f);
        textRect.offsetMin = textRect.offsetMax = Vector2.zero;
        v187023bd0cd1 = textObject.GetComponent<Text>();
        v187023bd0cd1.font = Resources.GetBuiltinResource<Font>(ve1d4833e86de.v92a94600e486(60));
        v187023bd0cd1.fontSize = 32;
        v187023bd0cd1.fontStyle = FontStyle.Bold;
        v187023bd0cd1.alignment = TextAnchor.MiddleCenter;
        v187023bd0cd1.color = new Color(0.12f, 0.12f, 0.15f);

        v1e35f84933d3 = vcd1fe3a80cf2(ve1d4833e86de.v92a94600e486(61), panelRect, new Vector2(0.08f, 0.1f), new Vector2(0.48f, 0.34f), new Color(0.13f, 0.77f, 0.37f), uiSprite);
        vd7d52c61f8c5 = vcd1fe3a80cf2(ve1d4833e86de.v92a94600e486(62), panelRect, new Vector2(0.52f, 0.1f), new Vector2(0.92f, 0.34f), new Color(0.95f, 0.96f, 0.98f), uiSprite);
        v8892c4878280.SetActive(false);
    }

    private static Button vcd1fe3a80cf2(string vc37086856ab0, Transform parent, Vector2 v9eec84cdbda9, Vector2 vb85b9e3eb107, Color color, Sprite sprite)
    {
        var buttonObject = new GameObject(vc37086856ab0, typeof(RectTransform), typeof(Image), typeof(Button));
        var rect = buttonObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = v9eec84cdbda9;
        rect.anchorMax = vb85b9e3eb107;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        Image image = buttonObject.GetComponent<Image>();
        image.sprite = sprite;
        image.type = Image.Type.Sliced;
        image.color = color;

        var textObject = new GameObject(ve1d4833e86de.v92a94600e486(63), typeof(RectTransform), typeof(Text));
        var textRect = textObject.GetComponent<RectTransform>();
        textRect.SetParent(rect, false);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = textRect.offsetMax = Vector2.zero;
        var text = textObject.GetComponent<Text>();
        text.text = vc37086856ab0;
        text.font = Resources.GetBuiltinResource<Font>(ve1d4833e86de.v92a94600e486(64));
        text.fontSize = 28;
        text.alignment = TextAnchor.MiddleCenter;
        return buttonObject.GetComponent<Button>();
    }

    private void v4087abaf049c(string v3afe30aa7e19)
    {
        bool keepGoogle = ve1b163a0fc7c &&
                          (vbb0f92668d73(v3afe30aa7e19) || Contains(v3afe30aa7e19, ve1d4833e86de.v92a94600e486(65)) || Contains(v3afe30aa7e19, ve1d4833e86de.v92a94600e486(66)));
        bool keepPayment = vd77e89f0e246 && v14c6ff1a25a3(v3afe30aa7e19);
        if ((ve1b163a0fc7c || vd77e89f0e246) && !keepGoogle && !keepPayment)
        {
            ve1b163a0fc7c = vd77e89f0e246 = false;
            if (v51822b575a86 != null)
                v51822b575a86.SetUserAgent(ve1d4833e86de.v92a94600e486(67));
            v9d02a20686d3(ve1d4833e86de.v92a94600e486(68));
        }
    }

    private void v046efda91089(string v3afe30aa7e19)
    {
        if (vdf8ecb272e0c(v3afe30aa7e19))
            Application.OpenURL(v3afe30aa7e19);
    }

    private bool v513eb76fd31e(string v3afe30aa7e19)
    {
        if (string.IsNullOrEmpty(v3afe30aa7e19))
            return false;

#if UNITY_ANDROID && !UNITY_EDITOR
        if (v3afe30aa7e19.StartsWith(ve1d4833e86de.v92a94600e486(69), StringComparison.OrdinalIgnoreCase)
            || v1813068a6b87(v3afe30aa7e19)
            || (!vdf8ecb272e0c(v3afe30aa7e19) && !v2f5d2e12273d(v3afe30aa7e19)))
        {
            return v6d30c295408b(v3afe30aa7e19);
        }
#endif
        return false;
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private bool v6d30c295408b(string v3afe30aa7e19)
    {
        try
        {
            using (var player = new AndroidJavaClass(ve1d4833e86de.v92a94600e486(70)))
            using (var activity = player.GetStatic<AndroidJavaObject>(ve1d4833e86de.v92a94600e486(71)))
            using (var uriClass = new AndroidJavaClass(ve1d4833e86de.v92a94600e486(72)))
            using (var uri = uriClass.CallStatic<AndroidJavaObject>(ve1d4833e86de.v92a94600e486(73), v3afe30aa7e19))
            using (var intent = new AndroidJavaObject(ve1d4833e86de.v92a94600e486(74), ve1d4833e86de.v92a94600e486(75), uri))
            {
                intent.Call<AndroidJavaObject>(ve1d4833e86de.v92a94600e486(76), ve1d4833e86de.v92a94600e486(77));
                intent.Call<AndroidJavaObject>(ve1d4833e86de.v92a94600e486(78), 0x10000000);
                activity.Call(ve1d4833e86de.v92a94600e486(79), intent);
                return true;
            }
        }
        catch (Exception e)
        {
            v9d02a20686d3(ve1d4833e86de.v92a94600e486(80) + e.Message);
            Application.OpenURL(v3afe30aa7e19);
            return true;
        }
    }
#endif

    private static bool vdf8ecb272e0c(string value)
    {
        return Uri.TryCreate(value, UriKind.Absolute, out Uri uri)
               && (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp);
    }

    private static bool v2f5d2e12273d(string value)
    {
        return !string.IsNullOrEmpty(value)
               && value.StartsWith(ve1d4833e86de.v92a94600e486(81), StringComparison.OrdinalIgnoreCase);
    }

    private static bool Contains(string vf8b9bc3ba2b0, string value)
    {
        return !string.IsNullOrEmpty(vf8b9bc3ba2b0)
               && vf8b9bc3ba2b0.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private static bool vbb0f92668d73(string v3afe30aa7e19)
    {
        return Contains(v3afe30aa7e19, ve1d4833e86de.v92a94600e486(82));
    }

    private static bool v14c6ff1a25a3(string v3afe30aa7e19)
    {
        if (!Uri.TryCreate(v3afe30aa7e19, UriKind.Absolute, out Uri uri))
            return false;

        return Contains(uri.Host, ve1d4833e86de.v92a94600e486(83))
               || Contains(uri.Host, ve1d4833e86de.v92a94600e486(84))
               || Contains(uri.Host, ve1d4833e86de.v92a94600e486(85))
               || Contains(uri.AbsolutePath, ve1d4833e86de.v92a94600e486(86))
               || Contains(uri.AbsolutePath, ve1d4833e86de.v92a94600e486(87))
               || Contains(uri.AbsolutePath, ve1d4833e86de.v92a94600e486(88))
               || Contains(uri.AbsolutePath, ve1d4833e86de.v92a94600e486(89))
               || Contains(uri.AbsolutePath, ve1d4833e86de.v92a94600e486(90));
    }

    private static bool v1813068a6b87(string v3afe30aa7e19)
    {
        return v3afe30aa7e19.StartsWith(ve1d4833e86de.v92a94600e486(91), StringComparison.OrdinalIgnoreCase)
               || v3afe30aa7e19.StartsWith(ve1d4833e86de.v92a94600e486(92), StringComparison.OrdinalIgnoreCase)
               || v3afe30aa7e19.StartsWith(ve1d4833e86de.v92a94600e486(93), StringComparison.OrdinalIgnoreCase);
    }

    private void v9d8bb2b96405()
    {
        if (v2fde38ea85e3 != null)
            return;

        v3f646832145b();
        v9a2d79455cee();
        Sprite uiSprite = Resources.GetBuiltinResource<Sprite>(ve1d4833e86de.v92a94600e486(94));
        v2fde38ea85e3 = new GameObject(ve1d4833e86de.v92a94600e486(95), typeof(RectTransform), typeof(Image));
        var root = v2fde38ea85e3.GetComponent<RectTransform>();
        root.SetParent(v09a58c6eb02a, false);
        Image v74866789eca0 = v2fde38ea85e3.GetComponent<Image>();
        v74866789eca0.sprite = uiSprite;
        v74866789eca0.type = Image.Type.Sliced;
        v74866789eca0.color = new Color(0.08f, 0.08f, 0.1f, 1f);

        v4c5421db8d8f(ve1d4833e86de.v92a94600e486(96), root, new Vector2(0.04f, 0.12f), new Vector2(0.48f, 0.88f), new Color(0.95f, 0.96f, 0.98f), uiSprite, va54084b8a157);
        v4c5421db8d8f(ve1d4833e86de.v92a94600e486(97), root, new Vector2(0.52f, 0.12f), new Vector2(0.96f, 0.88f), new Color(0.13f, 0.77f, 0.37f), uiSprite, v63214ab83156);
        v70c73ea92256(Screen.safeArea);
    }

    private void v70c73ea92256(Rect v1eeb22f60420)
    {
        if (v2fde38ea85e3 == null || Screen.width <= 0 || Screen.height <= 0)
            return;

        int v74866789eca0 = va6dc00296d7a();
        var root = v2fde38ea85e3.GetComponent<RectTransform>();
        root.anchorMin = new Vector2(0f, 0f);
        root.anchorMax = new Vector2(1f, 0f);
        root.pivot = new Vector2(0.5f, 0f);
        root.sizeDelta = new Vector2(0f, v74866789eca0);
        root.anchoredPosition = Vector2.zero;
        v2fde38ea85e3.SetActive(v915df17b37e1);
        v2fde38ea85e3.transform.SetAsLastSibling();
    }

    private void v68dc426efe88()
    {
        if (v2fde38ea85e3 != null)
            v2fde38ea85e3.SetActive(false);
    }

    private void v63214ab83156()
    {
        Action v6a9d1709a696 = v037942739148;
        v915df17b37e1 = false;
        v037942739148 = null;
        v68dc426efe88();
        if (v51822b575a86 != null)
            v51822b575a86.v374b7ca4f1e4();
        v3ae4875803fb();
        v655eaf210e19 = false;
        if (vb7c99eb4273c != null)
            vb7c99eb4273c.SetActive(false);
        v6a9d1709a696?.Invoke();
    }

    private void va54084b8a157()
    {
        v9d02a20686d3(ve1d4833e86de.v92a94600e486(98));
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private static int va6dc00296d7a()
    {
        float dpi = Screen.dpi > 0f ? Screen.dpi : 160f;
        return Mathf.Max(64, Mathf.RoundToInt(64f * dpi / 160f));
    }

    private static void v4c5421db8d8f(string vc37086856ab0, Transform parent, Vector2 v9eec84cdbda9, Vector2 vb85b9e3eb107, Color color, Sprite sprite, UnityEngine.Events.UnityAction v38f7474fbc65)
    {
        var buttonObject = new GameObject(vc37086856ab0, typeof(RectTransform), typeof(Image), typeof(Button));
        var rect = buttonObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = v9eec84cdbda9;
        rect.anchorMax = vb85b9e3eb107;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        Image image = buttonObject.GetComponent<Image>();
        image.sprite = sprite;
        image.type = Image.Type.Sliced;
        image.color = color;
        buttonObject.GetComponent<Button>().onClick.AddListener(v38f7474fbc65);

        var textObject = new GameObject(ve1d4833e86de.v92a94600e486(99), typeof(RectTransform), typeof(Text));
        var textRect = textObject.GetComponent<RectTransform>();
        textRect.SetParent(rect, false);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = textRect.offsetMax = Vector2.zero;
        var text = textObject.GetComponent<Text>();
        text.text = vc37086856ab0;
        text.font = Resources.GetBuiltinResource<Font>(ve1d4833e86de.v92a94600e486(100));
        text.fontSize = 28;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = new Color(0.12f, 0.12f, 0.15f, 1f);
    }

    private void v9d02a20686d3(string text)
    {
        Debug.Log(ve1d4833e86de.v92a94600e486(101) + text);
    }

    private void OnDestroy()
    {
        if (v51822b575a86 == null)
            return;

        v51822b575a86.Destroy();
        v51822b575a86 = null;
    }
}

internal static class ve1d4833e86de
{
    private const byte v354566e49003 = 0xC4;

    private const string vb29ff280edcb = "A200000000701cf8ba9e2c0566443ce907022ee404673a0acaa0cbf2875f1f12323f5a1fe0f88d9b8b105c7b0109b8960fb2a8a828fcdd5d0d24ad6f00a6337a25344efb87aeb675";
    private const string v1f49518ae927 = "A200000001b6833855e3e7f3d5599382b279b4dd607eb77fc8ff85c192e15b54af43719d22e5f9f33b976d07d4334bb65e511b2152190f44";
    private const string v0f4c5d91e139 = "A20000000267ced4dd2238d00abd82b87659bf8c446600a7fa0d6f2121d6ec1820f0634dd087572c7aa017163446e52fc66e81e26b4c40fb";
    private const string vf5908c209cd6 = "A2000000033b79065e70321c18c2252095b02894fd5f0a98bfbeecd1dd3d5e51c439c0e5535dae52dfc02d1a67c8a6fe6b5b370e63e0cdc267e54393314660c06c4018";
    private const string v8528aede3766 = "A2000000048750a9f5fec96a0949ab425d7d362202f77bbfe3fdb1acf0b41c26daf2adf2cd40223558783979389b108cb9c98251a5ea9a728e13ec90515fe0";
    private const string v34b84a454db8 = "A2000000054cf08d8fff6eccf9d4f36ed6931e13bcee4857a5b49e9e2769e6071b0668604951b2973099a06a237d4b9d7635321a01";
    private const string v920503c0100d = "A20000000643c14e86b1b95597a39aa347ccff724bc623b83c415eabe66600c4e28ad0a55ae090f3073729f46057be97c726dc50d0a9a3b66072";
    private const string v438cfcda78c5 = "A2000000076a2d92df560a066249a5b8320cc231f5874c0ed0c90c893a024ba85672bc18eee0ff9e50c7305c2751fd4351a8ca8ee8367a458146d778c69933f714b2462a7a08b084";
    private const string v6a11bd4ecb9c = "A200000008bb7c9036270cf00a2adac800df58a0b9be65e60dc4e8ddfca81c97532d1447ce25c6b6d5031b3cc03756b8f4d713ce357e2ae747b1ebae";
    private const string v48fdacb2602e = "A200000009a0ce6b5ef96925fcce6b11c3e51648cd00f88a7a1ac261534cea3152fb0d84242b89fecb5fc831d7c02f392376b3c2c6ffcbf12e096bfccf5b";
    private const string v11bb33cf6874 = "A20000000ae68844a93fa2bc07452cabf22eddc891a87cc0de7318847730d3f414472a3401d87c9cfb933d6531f8adb7d42b4b16ae24fb1a";
    private const string v4581ae2f9048 = "A20000000b0829c94148704557376261e51651929a0c5f7f0d921930b57c6be9b4ddb19ebfa42119c64a4c9205784faf89bd53f24e66de50af3cd80342428f";
    private const string v16a72c2d8c25 = "A20000000cd5c0b1f55163c8932de41b6ed2e284195a8129850de0182ff724979910bef89522ffa1417a45f62636a761e622e77a4687f6f39c852fc9";
    private const string v3f98823664ae = "A20000000db65c321a45";
    private const string v1909c4bfd475 = "A20000000e5646bf0f24c60e610f6ae9e6d3";
    private const string vb18474499e34 = "A20000000f9a59ad75ac86";
    private const string vc390ca1a0825 = "A2000000104bf6d38352e9";
    private const string v66c214555d10 = "A2000000116eed09e55f";
    private const string v222b57b9ac04 = "A200000012e7610f58feefbba0769567907241c7";
    private const string vb0818a9ee63f = "A20000001313c9e9e8ea4be5a74f042176f021c02e435ef535f4f660cdcb8e6a";
    private const string v502f116378a6 = "A200000014e004ca48153e200c849ea42339058b90a15c6f05298139692d35db1c4ad29747587e373218f275f4430f56c65f54f771";
    private const string vdcd28cb2d692 = "A200000015cae0f882230ccd03e89146b31cb5b9248ffc6e91f20f852e9733";
    private const string v93a525b22640 = "A2000000167f36cb50";
    private const string v9892fedcb895 = "A200000017c0a8b488c90735d008f4c55d3bf1b39c2c29a4602b62824c514c4f98565c";
    private const string v9155a5de70d0 = "A200000018138fecf5f2836c3f02cb11b38a72b76f5f807e6ca04cc873c26d05226d1e";
    private const string v20b6dacecf59 = "A20000001933640478";
    private const string vca09447eb11f = "A20000001ac865a52f185285ec10671257ec05b72a30";
    private const string v6f28203c5273 = "A20000001b6caa4f6f49d35e7818ac935d33449c5736";
    private const string vc33a069b80eb = "A20000001cbe5b2aa63740b83b84f8c164a41b978611c36dd4f55484933b3c8f128b83c9a8fb307e";
    private const string vadaf7ccaef72 = "A20000001d5dffb452a124635c5e36a0fdb47e";
    private const string vbaa520a70c30 = "A20000001ecd0a3986ff46df8540155918d380bb26153af81022e9006a6de6717c5936";
    private const string v8ecdd2332fcf = "A20000001f6794ac5a101ab436a1dd3a65efb15ce5c7dbf9f7b7889c10";
    private const string v24520e01aeb1 = "A200000020a5b6d90443198d6790c6f2a1a494";
    private const string v2aecde27afbc = "A2000000211a2c45a541fba193b88618f7";
    private const string vac44456dddf7 = "A20000002247648cf661f46100b510";
    private const string v8a7f5298aba7 = "A2000000238e7848f1c0769e76f8bb091c";
    private const string v932f955eb8ad = "A200000024d501ba8416651c8350b3b2be3157334ac156e4fda4b468b726bcf687";
    private const string v60400aa06763 = "A200000025c6ba9ecef117be47915faae56d51dd767c1ba19172053646945df492a84cc511488e";
    private const string v919eecee9494 = "A200000026eece63647a5d16830c98";
    private const string v519b85902b68 = "A200000027894bd309790f4b56df65ee13ca297e3881c6e3a251ebc94af4314da7558d89";
    private const string v919b4ea45c44 = "A20000002820099ef5e576f8d4671acccadd3e483ca47180c8b6ab0820493b";
    private const string v72757675e767 = "A2000000299e0ab934d6ce38f8771f4f79c20b584987a52dbbf9c08815ac04";
    private const string vf2488e8ca66c = "A20000002a9f87978c7e6316bf3ef60e8ff09cb3834346369fcf0e590d5f7ac5cf651e7c5b482d2c";
    private const string v4a225ed97766 = "A20000002b45a5fea8bf92e7989312e85058d673e6c51712f4a6";
    private const string v07b516673609 = "A20000002c83fb7848253f1c2ea5453788b577612e62997bc71249ae309313693852568f";
    private const string vc8a16cf9441a = "A20000002d1d4ad39e62ed4003e918a7ec54076c6c1730bdd8e3d7a2e1e7093e085d30";
    private const string v42f17036dab5 = "A20000002ec33ab00c0d045e0012f18eab9caec9b4a777e6ddb7b764fe316dfc2a729724a89eebfc60c88bc4ec0a4ac4d0";
    private const string vae5ecfaca689 = "A20000002fa94e57b7c1c4f888981fb0dfeeade6f3e3f1a093dcde7e278a361966ceafc63d7ca7";
    private const string v1e0d27e36c1f = "A200000030b9c708f4ebe0edce673f121ee9421f13f76bc21938e0c2daaa90b0f95c98c656c960c21eb5abcb557e36ec75886294aca43c341a481b";
    private const string v407cdce147e1 = "A2000000317d30d6c453c4aa65da4bbdde0585";
    private const string va1d7f8f2e478 = "A20000003297b830864c9ef478e358b34eb1a4eb";
    private const string v3991dd43280d = "A2000000337027da312c1661a6e862256a80824938";
    private const string v7e5963319b8d = "A200000034b56e78c480780cce7555903031313402bbb876884e410a05ea3a87";
    private const string v5ba3808edc14 = "A200000035e99489d5c0";
    private const string v285c0caee48a = "A200000036b9b6e1f996835e761fee2095defb3b865ae3602875086c38388c6e92b5c4";
    private const string vba20669b16fb = "A20000003778f2c71084f2910b6b58086d25c72a648dd862a3100f2bc5e0fae31988";
    private const string v5b114177ad04 = "A20000003814b50acc83516f5a6c9d2777fed3c9ebde6739c74ff04c5c";
    private const string vb94b6ee6adf3 = "A200000039aca282b70aa74918fbc4c8ff76ed06c3d63e";
    private const string va1e37863f536 = "A20000003a13c89f4c49d048a804";
    private const string v3faf30885e72 = "A20000003b3183329ac7e563f4";
    private const string v889cade9fe37 = "A20000003c88a8d3b94659d0e4bcd8eb6ccfdbe0e7e7ba3efdf4";
    private const string vf64234d30524 = "A20000003dc22a64b93f38b8d1";
    private const string vc04982c3f651 = "A20000003ece4dae16af25c6cb";
    private const string v8d1ed7f1bad8 = "A20000003f751c145957fe45a5";
    private const string v06f270f79dad = "A2000000405404882318dd958b9c8e112a34e31f3db0b8ea9497";
    private const string v37575bfbd6e5 = "A2000000412abc7f3951b91884ec277ff5556febc8f9602745773b71a87e";
    private const string vec0ffa52baf0 = "A20000004278c40aaa4dd91644d31f02c9cd8f37";
    private const string v311ae5674cc7 = "A2000000439127b372";
    private const string vcbd9cc8cbc6d = "A200000044f4d0c9f4c5c4e30f2a8dc724b5941114541daa3a18da560a5d42ecca14554d";
    private const string v30ad4a4e2ed2 = "A2000000453ec44c12aacf56ee9ae36de2a4";
    private const string vb6628a1bee3f = "A200000046f477fd259dd5ed2ffdb18ff757a1caa846c96ce9211f8a47d49a9e5e0d4b21534b2a";
    private const string v92bc9a8a219c = "A200000047f555a94804b3cb08c28117cb60c6d6b878fb45";
    private const string vc471c4f34b86 = "A2000000486d5c0fbd12d16ea7e05d9ede2bad609a4d3c3e";
    private const string v6a353e89a764 = "A200000049577f7931a85d94fb63";
    private const string v13bfe1bcc7fd = "A20000004ac2155ba9dd58c9c722e5c6272db41d4f16deacfcdb6eec7e8a44";
    private const string v92ac3fd52d79 = "A20000004b88a5d3860383e027069bfac9a73d8fb82154032771e3df82ffbef1bc3a22";
    private const string vc918ec040b27 = "A20000004ccdc808fda7b5f746eccf04dcb0a7ce";
    private const string v4a8b960a81a8 = "A20000004d10cdb11b16e4b186e2dddac81af9f034f14a724647fc4a6d318c8ffe8362fdedb220ab9fe5";
    private const string v7985d67debea = "A20000004eb0b1a1084e1cc08f9fa3dc07";
    private const string v3c9156d62d57 = "A20000004fc8cb64b51672a92e0aa013171454475294";
    private const string v459831e1e44d = "A200000050c8faf7ffb424d684382b7e0d0b4ebf33fb59b17b96ad403ab4";
    private const string v2919b6071322 = "A200000051f4bff92bc3c4819b198c58cb82096b";
    private const string v22c27ff0e9aa = "A200000052062b53409858e168e6392255ec558dfa0da316c0";
    private const string v485dc4e80e01 = "A20000005304b04361feae75";
    private const string v5419dc01778f = "A2000000549f3551039e38c60b252332d9";
    private const string v658f8bc1dffe = "A200000055fe7bcfd75c83d81eafcc4c";
    private const string v00710da519bb = "A20000005635d58b8d4b771dad";
    private const string v72fa8439fafe = "A2000000574feeb82167073e2c8618ed43b0";
    private const string vf4326eb95cb7 = "A200000058dda1807e771ac136ab951341";
    private const string v3d456f2720bc = "A2000000599c8a65375c03f4a2ef61";
    private const string v523025144d56 = "A20000005ad5f3704c1a6c0822f5bdb914";
    private const string vb463be2c78f1 = "A20000005b5ec1fddcbcdfc7e686cf2f7718";
    private const string v6967500ceadc = "A20000005c630f3181840fef032b1740da5c0589dd6f444461b72129c1ba81a68c";
    private const string v95ce70758234 = "A20000005dfa8b0dc37e4d1029c0fd0fa051b11b767c0a99e475b2593a57a9f8";
    private const string v034e207aa3f1 = "A20000005e5e8ca3fa730090b6963adc702f856146915e61edb893a364";
    private const string v1ada46e72f30 = "A20000005f122c986d071809084cf30e74c325b223";
    private const string vb3d9ab79358d = "A200000060153b188501c0d436c7483f";
    private const string vaaaff1ce7b11 = "A2000000613cb4d5a4247d78dc7624";
    private const string v05031c45de0f = "A200000062ffc2a5bf4efce18b22607d09da36fb8a187b31";
    private const string v8c43668024c8 = "A200000063e93309b450cc8de2";
    private const string v698b638a747f = "A2000000649051c510300a796d7d98ce1fed03bb8e96f4c5b45e";
    private const string vaff12e06599f = "A2000000659af05152fb20d133328edadc";

    internal static string v92a94600e486(int id)
    {
        switch (id)
        {
            case 0: return v6d4ba077e331(vb29ff280edcb);
            case 1: return v6d4ba077e331(v1f49518ae927);
            case 2: return v6d4ba077e331(v0f4c5d91e139);
            case 3: return v6d4ba077e331(vf5908c209cd6);
            case 4: return v6d4ba077e331(v8528aede3766);
            case 5: return v6d4ba077e331(v34b84a454db8);
            case 6: return v6d4ba077e331(v920503c0100d);
            case 7: return v6d4ba077e331(v438cfcda78c5);
            case 8: return v6d4ba077e331(v6a11bd4ecb9c);
            case 9: return v6d4ba077e331(v48fdacb2602e);
            case 10: return v6d4ba077e331(v11bb33cf6874);
            case 11: return v6d4ba077e331(v4581ae2f9048);
            case 12: return v6d4ba077e331(v16a72c2d8c25);
            case 13: return v6d4ba077e331(v3f98823664ae);
            case 14: return v6d4ba077e331(v1909c4bfd475);
            case 15: return v6d4ba077e331(vb18474499e34);
            case 16: return v6d4ba077e331(vc390ca1a0825);
            case 17: return v6d4ba077e331(v66c214555d10);
            case 18: return v6d4ba077e331(v222b57b9ac04);
            case 19: return v6d4ba077e331(vb0818a9ee63f);
            case 20: return v6d4ba077e331(v502f116378a6);
            case 21: return v6d4ba077e331(vdcd28cb2d692);
            case 22: return v6d4ba077e331(v93a525b22640);
            case 23: return v6d4ba077e331(v9892fedcb895);
            case 24: return v6d4ba077e331(v9155a5de70d0);
            case 25: return v6d4ba077e331(v20b6dacecf59);
            case 26: return v6d4ba077e331(vca09447eb11f);
            case 27: return v6d4ba077e331(v6f28203c5273);
            case 28: return v6d4ba077e331(vc33a069b80eb);
            case 29: return v6d4ba077e331(vadaf7ccaef72);
            case 30: return v6d4ba077e331(vbaa520a70c30);
            case 31: return v6d4ba077e331(v8ecdd2332fcf);
            case 32: return v6d4ba077e331(v24520e01aeb1);
            case 33: return v6d4ba077e331(v2aecde27afbc);
            case 34: return v6d4ba077e331(vac44456dddf7);
            case 35: return v6d4ba077e331(v8a7f5298aba7);
            case 36: return v6d4ba077e331(v932f955eb8ad);
            case 37: return v6d4ba077e331(v60400aa06763);
            case 38: return v6d4ba077e331(v919eecee9494);
            case 39: return v6d4ba077e331(v519b85902b68);
            case 40: return v6d4ba077e331(v919b4ea45c44);
            case 41: return v6d4ba077e331(v72757675e767);
            case 42: return v6d4ba077e331(vf2488e8ca66c);
            case 43: return v6d4ba077e331(v4a225ed97766);
            case 44: return v6d4ba077e331(v07b516673609);
            case 45: return v6d4ba077e331(vc8a16cf9441a);
            case 46: return v6d4ba077e331(v42f17036dab5);
            case 47: return v6d4ba077e331(vae5ecfaca689);
            case 48: return v6d4ba077e331(v1e0d27e36c1f);
            case 49: return v6d4ba077e331(v407cdce147e1);
            case 50: return v6d4ba077e331(va1d7f8f2e478);
            case 51: return v6d4ba077e331(v3991dd43280d);
            case 52: return v6d4ba077e331(v7e5963319b8d);
            case 53: return v6d4ba077e331(v5ba3808edc14);
            case 54: return v6d4ba077e331(v285c0caee48a);
            case 55: return v6d4ba077e331(vba20669b16fb);
            case 56: return v6d4ba077e331(v5b114177ad04);
            case 57: return v6d4ba077e331(vb94b6ee6adf3);
            case 58: return v6d4ba077e331(va1e37863f536);
            case 59: return v6d4ba077e331(v3faf30885e72);
            case 60: return v6d4ba077e331(v889cade9fe37);
            case 61: return v6d4ba077e331(vf64234d30524);
            case 62: return v6d4ba077e331(vc04982c3f651);
            case 63: return v6d4ba077e331(v8d1ed7f1bad8);
            case 64: return v6d4ba077e331(v06f270f79dad);
            case 65: return v6d4ba077e331(v37575bfbd6e5);
            case 66: return v6d4ba077e331(vec0ffa52baf0);
            case 67: return v6d4ba077e331(v311ae5674cc7);
            case 68: return v6d4ba077e331(vcbd9cc8cbc6d);
            case 69: return v6d4ba077e331(v30ad4a4e2ed2);
            case 70: return v6d4ba077e331(vb6628a1bee3f);
            case 71: return v6d4ba077e331(v92bc9a8a219c);
            case 72: return v6d4ba077e331(vc471c4f34b86);
            case 73: return v6d4ba077e331(v6a353e89a764);
            case 74: return v6d4ba077e331(v13bfe1bcc7fd);
            case 75: return v6d4ba077e331(v92ac3fd52d79);
            case 76: return v6d4ba077e331(vc918ec040b27);
            case 77: return v6d4ba077e331(v4a8b960a81a8);
            case 78: return v6d4ba077e331(v7985d67debea);
            case 79: return v6d4ba077e331(v3c9156d62d57);
            case 80: return v6d4ba077e331(v459831e1e44d);
            case 81: return v6d4ba077e331(v2919b6071322);
            case 82: return v6d4ba077e331(v22c27ff0e9aa);
            case 83: return v6d4ba077e331(v485dc4e80e01);
            case 84: return v6d4ba077e331(v5419dc01778f);
            case 85: return v6d4ba077e331(v658f8bc1dffe);
            case 86: return v6d4ba077e331(v00710da519bb);
            case 87: return v6d4ba077e331(v72fa8439fafe);
            case 88: return v6d4ba077e331(vf4326eb95cb7);
            case 89: return v6d4ba077e331(v3d456f2720bc);
            case 90: return v6d4ba077e331(v523025144d56);
            case 91: return v6d4ba077e331(vb463be2c78f1);
            case 92: return v6d4ba077e331(v6967500ceadc);
            case 93: return v6d4ba077e331(v95ce70758234);
            case 94: return v6d4ba077e331(v034e207aa3f1);
            case 95: return v6d4ba077e331(v1ada46e72f30);
            case 96: return v6d4ba077e331(vb3d9ab79358d);
            case 97: return v6d4ba077e331(vaaaff1ce7b11);
            case 98: return v6d4ba077e331(v05031c45de0f);
            case 99: return v6d4ba077e331(v8c43668024c8);
            case 100: return v6d4ba077e331(v698b638a747f);
            case 101: return v6d4ba077e331(vaff12e06599f);
            default: return string.Empty;
        }
    }

    private static string v6d4ba077e331(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)v354566e49003 * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
