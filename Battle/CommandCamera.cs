using UnityEngine;

public class CommandCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 screenPosition; // Screen position to keep the target at (in pixels)

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetScreenPos = cam.WorldToScreenPoint(target.position);
            Vector3 desiredScreenPos = new Vector3(
                Screen.width * screenPosition.x,
                Screen.height * screenPosition.y,
                targetScreenPos.z
            );

            if (targetScreenPos != desiredScreenPos)
            {
                Vector3 diff = targetScreenPos - desiredScreenPos;
                Vector3 moveDir =
                    cam.ScreenToWorldPoint(targetScreenPos - diff)
                    - cam.ScreenToWorldPoint(targetScreenPos);
                transform.position += moveDir;
            }
        }
    }
}
