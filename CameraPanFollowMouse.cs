using UnityEngine;

public class CameraPanFollowMouse : MonoBehaviour
{
    public float panSpeed = 5f;
    public float maxPanDistance = 1f;

    private Camera mainCamera;
    private Vector3 initialPosition;
    private Vector3 currentVelocity;

    private void Start()
    {
        mainCamera = Camera.main;
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        Vector3 mousePosition = Input.mousePosition;

        Vector3 screenCenterToMouse = mousePosition - screenCenter;

        float panX = Mathf.Clamp(screenCenterToMouse.x / (Screen.width * 0.5f), -1f, 1f) * maxPanDistance;
        float panY = Mathf.Clamp(screenCenterToMouse.y / (Screen.height * 0.5f), -1f, 1f) * maxPanDistance;

        Vector3 targetPan = new Vector3(initialPosition.x + panX, initialPosition.y + panY, initialPosition.z);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPan, ref currentVelocity, 1f / panSpeed);
    }
}
