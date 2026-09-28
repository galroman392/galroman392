using UnityEngine;

public class PanelsFlow : MonoBehaviour
{
    public static PanelsFlow instance;

    public GameObject panel_loader;
    public GameObject panel_menu;
    public GameObject panel_game;

    private void Awake()
    {
        instance = this;
    }

    public void PanelTransition(GameObject _panelActivate, GameObject _panelDeactivate)
    {
        _panelActivate.SetActive(true);
        _panelDeactivate.SetActive(false);
    }

    public void PanelTransitionShow(GameObject _panelActivate)
    {
        _panelActivate.SetActive(true);
    }

    public void PanelTransitionHide(GameObject _panelDeactivate)
    {
        _panelDeactivate.SetActive(false);
    }

    
}
