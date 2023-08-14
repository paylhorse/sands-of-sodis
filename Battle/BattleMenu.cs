using UnityEngine;

// **** Esc-Key behavior during Battle

public class BattleMenu : MonoBehaviour
{
    private BUnit player;
    private SphereMenu sphereMenu;

    private void Start()
    {
        // Find and store reference to the Player in the scene
        player = FindObjectOfType<BUnit>();

        // Find and store reference to the SphereMenu in the scene
        sphereMenu = FindObjectOfType<SphereMenu>();
    }

    private void Update()
    {
        // Listen for the Esc key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Call HideAllMenus() method in the SphereMenu
            sphereMenu.HideAllMenus();
        }
    }
}
