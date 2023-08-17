using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform

    private Vector3 offset; // Offset distance between the player and the canvas

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - playerTransform.position; // Calculate the initial offset
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + offset; // Update the position
    }
}
