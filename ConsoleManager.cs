using UnityEngine;

public class ConsoleManager : MonoBehaviour
{
    public GameObject consoleObject; // The object to toggle

    // Update is called once per frame
    void Update()
    {
        // Listen for the tilde key
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            // Toggle the active state of the consoleObject
            consoleObject.SetActive(!consoleObject.activeSelf);
        }
    }
}
