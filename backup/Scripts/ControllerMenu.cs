using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;



public class ControllerMenu : MonoBehaviour
{
    [SerializeField] private PanelsFlow _panelsFlow;
    [SerializeField] private ControllerPrivacyPolicy _controllerPrivacyPolicy;

    //gameplay
    [SerializeField] private int game_score = 0;
    [SerializeField] private string privacyPolicy = "";

    //ui
    [SerializeField] private TextMeshProUGUI text_ui_main_score;
    [SerializeField] private Button  button_ui_main_play;
    [SerializeField] private Button  button_ui_main_privacy_policy;
    [SerializeField] private GameObject panel_ui_main;

  

    private void Awake()
    {
        _panelsFlow = PanelsFlow.instance;
    }

    private void Start()
    {
        InitUI();
    }

    
    private void Update()
    {
        UpdateUI();
    }

    private void InitUI()
    {
        button_ui_main_play.onClick.AddListener(() => _panelsFlow.PanelTransition(_panelsFlow.panel_game, _panelsFlow.panel_menu));

       button_ui_main_privacy_policy.onClick.AddListener(() => _controllerPrivacyPolicy.OpenPrivacyPolicy(privacyPolicy));
    }

    private void UpdateUI()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        text_ui_main_score.text = game_score.ToString();

    }


    
}
