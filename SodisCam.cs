using UnityEngine;

public class SodisCam : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float distanceToTarget = 10.0f;

    [SerializeField]
    private float smoothTime = 0.1f;

    private Vector3 _followVelocity;

    private void Start()
    {
        if (target == null)
            return;

        // Set initial position of the camera
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z - distanceToTarget);
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate the camera's target position on the x-z plane
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z - distanceToTarget);

        // Smoothly move the camera to the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _followVelocity, smoothTime);
    }
}
