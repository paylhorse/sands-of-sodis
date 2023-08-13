using UnityEngine;

// **** Swaying idle animation for mesh objects

public class SwayingMotion : MonoBehaviour
{
    [SerializeField] private float rotationAngle = 5f;
    [SerializeField] private float rotationSpeed = 1f;

    public bool otherAxis;

    private float initialRotationZ;
    private float initialRotationY;

    private void Start()
    {
        initialRotationZ = transform.localEulerAngles.z;
        initialRotationY = transform.localEulerAngles.y;
    }

    private void Update()
    {
        if (otherAxis)
        {
            float yRotation = initialRotationY + Mathf.Sin(Time.time * rotationSpeed) * rotationAngle;
            transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, yRotation, transform.localEulerAngles.z);
        }
        else
        {
            float zRotation = initialRotationZ + Mathf.Sin(Time.time * rotationSpeed) * rotationAngle;
            transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, zRotation);
        }
    }
}

