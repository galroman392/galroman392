using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class va834df695281 : MonoBehaviour
{
    public static bool v53d6cf1cf001 { get; private set; }

    private const int v600dcb997d3e = 112;

    private vd14e3cc7c800 v537c3c32abf1;
    private GameObject v562e50f2cb74;
    private RectTransform v74866789eca0;
    private Rect v15b25fd566ea;
    private Vector2 ve58b7587b1e9;

    public void v9e9dffda3544(string vcac21df72523)
    {
        if (string.IsNullOrWhiteSpace(vcac21df72523))
        {
            Debug.LogError(vbd93b4a0467f.v05dd6a7a8cad(0), this);
            return;
        }

        v94a640f30a05();

#if UNITY_ANDROID && !UNITY_EDITOR
        v537c3c32abf1 = new vd14e3cc7c800();
        if (!v537c3c32abf1.Ready)
        {
            v537c3c32abf1.Destroy();
            v537c3c32abf1 = null;
            Application.OpenURL(vcac21df72523);
            return;
        }

        v537c3c32abf1.SetAcceptThirdPartyCookies(true);
        vdbfc91341ac1();
        v537c3c32abf1.Load(vcac21df72523);
        v537c3c32abf1.Show();
        vedb4ecc41b66();
        v53d6cf1cf001 = true;
#else
        Application.OpenURL(vcac21df72523);
#endif
    }

    private void Update()
    {
        if (!v53d6cf1cf001 || v537c3c32abf1 == null)
            return;

        if (v15b25fd566ea != Screen.safeArea ||
            ve58b7587b1e9.x != Screen.width ||
            ve58b7587b1e9.y != Screen.height)
        {
            vdbfc91341ac1();
        }

        if (!v3957dfd83d44())
            return;

        if (v537c3c32abf1.CanGoBack)
            v537c3c32abf1.GoBack();
        else
            v94a640f30a05();
    }

    private void v94a640f30a05()
    {
        v53d6cf1cf001 = false;

        if (v537c3c32abf1 != null)
        {
            v537c3c32abf1.v374b7ca4f1e4();
            v537c3c32abf1.Destroy();
            v537c3c32abf1 = null;
        }

        if (v562e50f2cb74 != null)
        {
            Destroy(v562e50f2cb74);
            v562e50f2cb74 = null;
            v74866789eca0 = null;
        }
    }

    private void vdbfc91341ac1()
    {
        Rect v1eeb22f60420 = Screen.safeArea;
        int left = Mathf.Max(0, Mathf.RoundToInt(v1eeb22f60420.xMin));
        int right = Mathf.Max(0, Mathf.RoundToInt(Screen.width - v1eeb22f60420.xMax));
        int bottom = Mathf.Max(0, Mathf.RoundToInt(v1eeb22f60420.yMin));
        int top = Mathf.Max(0, Mathf.RoundToInt(Screen.height - v1eeb22f60420.yMax)) + v600dcb997d3e;

        if (v537c3c32abf1 != null)
            v537c3c32abf1.SetMargins(left, top, right, bottom, Screen.width, Screen.height);

        if (v74866789eca0 != null)
        {
            v74866789eca0.sizeDelta = new Vector2(0f, top);
            v74866789eca0.anchoredPosition = Vector2.zero;
        }

        v15b25fd566ea = v1eeb22f60420;
        ve58b7587b1e9 = new Vector2(Screen.width, Screen.height);
    }

    private void vedb4ecc41b66()
    {
        v562e50f2cb74 = new GameObject(vbd93b4a0467f.v05dd6a7a8cad(1));
        var canvas = v562e50f2cb74.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 5000;
        v562e50f2cb74.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        v562e50f2cb74.AddComponent<GraphicRaycaster>();
        v0e968eaefc8d();

        int top = Mathf.Max(0, Mathf.RoundToInt(Screen.height - Screen.safeArea.yMax)) + v600dcb997d3e;
        var barObject = new GameObject(vbd93b4a0467f.v05dd6a7a8cad(2), typeof(RectTransform), typeof(Image));
        v74866789eca0 = barObject.GetComponent<RectTransform>();
        v74866789eca0.SetParent(v562e50f2cb74.transform, false);
        v74866789eca0.anchorMin = new Vector2(0f, 1f);
        v74866789eca0.anchorMax = new Vector2(1f, 1f);
        v74866789eca0.pivot = new Vector2(0.5f, 1f);
        v74866789eca0.sizeDelta = new Vector2(0f, top);
        v74866789eca0.anchoredPosition = Vector2.zero;
        barObject.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.12f, 1f);

        Sprite uiSprite = Resources.GetBuiltinResource<Sprite>(vbd93b4a0467f.v05dd6a7a8cad(3));
        var buttonObject = new GameObject(vbd93b4a0467f.v05dd6a7a8cad(4), typeof(RectTransform), typeof(Image), typeof(Button));
        var buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.SetParent(v74866789eca0, false);
        buttonRect.anchorMin = new Vector2(1f, 0f);
        buttonRect.anchorMax = new Vector2(1f, 0f);
        buttonRect.pivot = new Vector2(1f, 0f);
        buttonRect.sizeDelta = new Vector2(180f, 72f);
        buttonRect.anchoredPosition = new Vector2(-24f, 16f);
        Image image = buttonObject.GetComponent<Image>();
        image.sprite = uiSprite;
        image.type = Image.Type.Sliced;
        image.color = new Color(0.95f, 0.96f, 0.98f, 1f);
        buttonObject.GetComponent<Button>().onClick.AddListener(v94a640f30a05);

        var labelObject = new GameObject(vbd93b4a0467f.v05dd6a7a8cad(5), typeof(RectTransform), typeof(Text));
        var labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.SetParent(buttonRect, false);
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = labelRect.offsetMax = Vector2.zero;
        var vc37086856ab0 = labelObject.GetComponent<Text>();
        vc37086856ab0.text = vbd93b4a0467f.v05dd6a7a8cad(6);
        vc37086856ab0.font = Resources.GetBuiltinResource<Font>(vbd93b4a0467f.v05dd6a7a8cad(7));
        vc37086856ab0.fontSize = 28;
        vc37086856ab0.alignment = TextAnchor.MiddleCenter;
        vc37086856ab0.color = new Color(0.12f, 0.12f, 0.15f, 1f);
    }

    private static void v0e968eaefc8d()
    {
        if (EventSystem.current != null)
            return;

        var events = new GameObject(vbd93b4a0467f.v05dd6a7a8cad(8), typeof(EventSystem), typeof(InputSystemUIInputModule));
        events.GetComponent<EventSystem>();
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

    private void OnDestroy()
    {
        v94a640f30a05();
    }
}

internal static class vbd93b4a0467f
{
    private const byte v599bb6c3f444 = 0x92;

    private const string v5c4d87781983 = "A2000000001fa274778b259ada69ebb36bf618602f153e6d7a0aecfc58ad82ae50039ac3d5";
    private const string v5a0608311fb1 = "A200000001dccf63bd6d7aeb1a266bd3beb008ec63b50edc00b6a81396da01b9";
    private const string v2c1a70dbf00c = "A2000000022c9dbfe4a7877a";
    private const string vaf4d397bab62 = "A200000003db9453da72137b190df98b6b99ccefc3428e431c122ad18b";
    private const string v71bfc5f0ec07 = "A2000000040b39502e8c64132c6b";
    private const string v4863680ac36d = "A200000005780e23b3f8c609625c";
    private const string v379d36c7b6b0 = "A20000000625fe20876e78c47958";
    private const string ve65301306933 = "A2000000078cf28b525f88a6b53b4390082dba24e42c995732f6";
    private const string v1266a3d7a677 = "A200000008f13e5b5b79913d10cb33aa391b67b3";

    internal static string v05dd6a7a8cad(int id)
    {
        switch (id)
        {
            case 0: return v4020a44bf0dc(v5c4d87781983);
            case 1: return v4020a44bf0dc(v5a0608311fb1);
            case 2: return v4020a44bf0dc(v2c1a70dbf00c);
            case 3: return v4020a44bf0dc(vaf4d397bab62);
            case 4: return v4020a44bf0dc(v71bfc5f0ec07);
            case 5: return v4020a44bf0dc(v4863680ac36d);
            case 6: return v4020a44bf0dc(v379d36c7b6b0);
            case 7: return v4020a44bf0dc(ve65301306933);
            case 8: return v4020a44bf0dc(v1266a3d7a677);
            default: return string.Empty;
        }
    }

    private static string v4020a44bf0dc(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)v599bb6c3f444 * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
