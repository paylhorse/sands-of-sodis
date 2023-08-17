using UnityEngine;

public class MeshFloating : MonoBehaviour
{
    public float floatSpeed = 2.0f;
    public float floatRange = 0.1f;
    public float positionLerpSpeed = 5.0f;

    private Vector3 originalPosition;
    private float floatTimer = 0.0f;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        floatTimer += Time.deltaTime * floatSpeed;
        float zOffset = Mathf.Sin(floatTimer) * floatRange;
        Vector3 targetPosition = new Vector3(
            originalPosition.x,
            originalPosition.y,
            originalPosition.z + zOffset
        );
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPosition,
            Time.deltaTime * positionLerpSpeed
        );
    }
}
