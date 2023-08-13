using UnityEngine;

// **** Handles visual equipment change, based on Chest equipment

public class WearEquipment : MonoBehaviour
{
    public GameObject nudeCharacter;
    public GameObject clothedCharacter;
    public SimpleCameraController cameraController;

    private void Start()
    {
        // Start with nude character visible and clothed character invisible
        nudeCharacter.SetActive(true);
        clothedCharacter.SetActive(false);
    }

    public void EquipArmor()
    {
        // Set the position and rotation of the clothed character to match the nude character's
        clothedCharacter.transform.position = nudeCharacter.transform.position;
        clothedCharacter.transform.rotation = nudeCharacter.transform.rotation;
        
        // Hide nude character and show clothed character
        nudeCharacter.SetActive(false);
        clothedCharacter.SetActive(true);

        // Change the camera's target to the clothed character
        cameraController.target = clothedCharacter.transform;
    }
}
