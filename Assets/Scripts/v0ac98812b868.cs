using UnityEngine;

public class v0ac98812b868 : MonoBehaviour
{
    public static v0ac98812b868 va1dff10c587c;

    public GameObject panel_loader;
    public GameObject panel_menu;
    public GameObject panel_game;

    private void Awake()
    {
        va1dff10c587c = this;
    }

    public void vdd33ad2f57a5(GameObject v1c3c809a2546, GameObject v6398b17fbc61)
    {
        v1c3c809a2546.SetActive(true);
        v6398b17fbc61.SetActive(false);
    }

    public void vd19ca100fb4d(GameObject v1c3c809a2546)
    {
        v1c3c809a2546.SetActive(true);
    }

    public void v8765add62ca0(GameObject v6398b17fbc61)
    {
        v6398b17fbc61.SetActive(false);
    }

    
}
