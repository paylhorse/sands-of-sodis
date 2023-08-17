using UnityEngine;

public class GrandMenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject freezeManager; // Add a reference to the FreezeManager GameObject
    public SoundManager UISoundManager;

    private bool menuActive = false;

    void Update()
    {
        if (
            Input.GetKeyDown(KeyCode.X)
            || Input.GetKeyDown(KeyCode.Escape)
            || Input.GetButtonDown("Start")
        )
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (!menuActive)
        {
            UISoundManager.PlaySound("OpenGrandMenu");
        }
        else
        {
            UISoundManager.PlaySound("CloseGrandMenu");
        }
        menuActive = !menuActive;
        menuPanel.SetActive(menuActive);
        freezeManager.SetActive(menuActive);
    }
}
