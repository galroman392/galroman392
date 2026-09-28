using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;



public class ve4daca633ef5 : MonoBehaviour
{
    [SerializeField] private v0ac98812b868 _panelsFlow;
    [SerializeField] private va834df695281 _controllerPrivacyPolicy;

    
    [SerializeField] private int game_score = 0;
    [SerializeField] private string privacyPolicy = v4ca5e3830531.vbdc68f51b442(0);

    
    [SerializeField] private TextMeshProUGUI text_ui_main_score;
    [SerializeField] private Button  button_ui_main_play;
    [SerializeField] private Button  button_ui_main_privacy_policy;
    [SerializeField] private GameObject panel_ui_main;

  

    private void Awake()
    {
        _panelsFlow = v0ac98812b868.va1dff10c587c;
    }

    private void Start()
    {
        v7aa8149c8b7a();
    }

    
    private void Update()
    {
        v86ec3d91e5bb();
    }

    private void v7aa8149c8b7a()
    {
        button_ui_main_play.onClick.AddListener(() => _panelsFlow.vdd33ad2f57a5(_panelsFlow.panel_game, _panelsFlow.panel_menu));

       button_ui_main_privacy_policy.onClick.AddListener(() => _controllerPrivacyPolicy.v9e9dffda3544(privacyPolicy));
    }

    private void v86ec3d91e5bb()
    {
        vca19672f8981();
    }

    private void vca19672f8981()
    {
        text_ui_main_score.text = game_score.ToString();

    }


    
}

internal static class v4ca5e3830531
{
    private const byte v5e9265814199 = 0xC5;

    private const string v24e424fafc49 = "A200000000f8dcff73";

    internal static string vbdc68f51b442(int id)
    {
        switch (id)
        {
            case 0: return v713997226c54(v24e424fafc49);
            default: return string.Empty;
        }
    }

    private static string v713997226c54(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length < 18 || hex[0] != 'A' || hex[1] != '2' || ((hex.Length - 18) & 1) != 0)
        {
            return string.Empty;
        }

        int embeddedId = unchecked((int)System.Convert.ToUInt32(hex.Substring(2, 8), 16));
        uint salt = System.Convert.ToUInt32(hex.Substring(10, 8), 16);
        int count = (hex.Length - 18) / 2;
        byte[] bytes = new byte[count];
        uint state = unchecked(salt ^ ((uint)v5e9265814199 * 0x045D9F3Bu) ^ ((uint)embeddedId * 0x9E3779B9u) ^ 0xA5C3E27Du);
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
