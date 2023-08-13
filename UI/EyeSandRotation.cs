using UnityEngine;

public class EyeSandRotation : MonoBehaviour
{
    public float rotationSpeed = 10f; // The speed of rotation. You can adjust this in the inspector.

    // Update is called once per frame
    void Update()
    {
        // Rotates the object in its local space on all axes by the same amount.
        transform.Rotate(new Vector3(rotationSpeed, rotationSpeed, rotationSpeed) * Time.deltaTime);
    }
}
