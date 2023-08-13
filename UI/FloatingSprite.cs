using UnityEngine;

// **** Idle y-axis floating animation for sprites

public class FloatingSprite : MonoBehaviour
{
    public float floatAmplitude = 0.5f;
    public float floatSpeed = 1f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        Vector3 newPosition = new Vector3(initialPosition.x, initialPosition.y + yOffset, initialPosition.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * floatSpeed);
    }
}
