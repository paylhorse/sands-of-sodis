using UnityEngine;

// **** Mesh property for card discard visual

public class CardFall : MonoBehaviour
{
    public float upwardForce = 5f;
    public float extraGravity = -5f; // The extra gravity that will be applied to the card. Adjust this in the inspector.
    public float coneAngle = 30f; // The angle of the cone within which the random direction will be chosen.
    public Vector3 maxRandomAngularVelocity = new Vector3(5, 5, 5); // The maximum random angular velocity for each axis.

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (rb == null)
            return;

        // Generate a random direction within an upward-facing cone.
        Vector3 direction = GenerateRandomDirectionInCone(Vector3.up, coneAngle);

        // Apply the initial force in the random direction.
        rb.velocity = direction * upwardForce;

        // Apply a random angular velocity.
        Vector3 randomAngularVelocity = new Vector3(
            Random.Range(-maxRandomAngularVelocity.x, maxRandomAngularVelocity.x),
            Random.Range(-maxRandomAngularVelocity.y, maxRandomAngularVelocity.y),
            Random.Range(-maxRandomAngularVelocity.z, maxRandomAngularVelocity.z));
        rb.angularVelocity = randomAngularVelocity;
    }

    private void FixedUpdate()
    {
        if (rb == null)
            return;

        // Apply the extra gravity
        rb.AddForce(new Vector3(0, extraGravity, 0), ForceMode.Acceleration);
    }

    // This function generates a random direction within a cone.
    // The cone is defined by a central direction and an angle.
    private Vector3 GenerateRandomDirectionInCone(Vector3 direction, float angle)
    {
        // Generate a random point on a circle.
        float radius = Mathf.Tan(angle * Mathf.Deg2Rad);
        Vector2 randomPoint = Random.insideUnitCircle * radius;

        // Create a vector from the random point, and rotate it to align with the desired direction.
        Vector3 randomVector = new Vector3(randomPoint.x, 1, randomPoint.y);
        randomVector = Quaternion.FromToRotation(Vector3.up, direction) * randomVector;

        // Normalize the vector and return it.
        return randomVector.normalized;
    }
}
