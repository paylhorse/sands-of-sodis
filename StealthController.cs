using UnityEngine;

public class StealthController : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private GameObject crouchManager;

    [SerializeField]
    private float rotationSpeed = 5.0f; // The speed of rotation

    public bool isStealthOn;
    public bool isHiding;
    public bool isTouchingWall; // New variable

    private Quaternion hidingPlaceRotation; // The rotation of the hiding place
    private Collider currentHidingPlace; // The current hiding place collider

    private AvatarIKGoal activeHand = AvatarIKGoal.RightHand; // Which hand to raise

    private void Update()
    {
        // Get the Right Trigger input
        float rightTrigger = Input.GetAxis("3rd Axis");

        // Check if the Right Trigger is held down
        if (rightTrigger > 0.1f)
        {
            // Set the 'Crouch' parameter to true and activate the CrouchManager
            playerAnimator.SetBool("Crouch", true);
            crouchManager.SetActive(true);
            isStealthOn = true;

            // Check if the player is in a hiding place and not already hiding
            if (currentHidingPlace && !isHiding)
            {
                EnterHidingState();
            }
        }
        else
        {
            // Set the 'Crouch' parameter to false and deactivate the CrouchManager
            playerAnimator.SetBool("Crouch", false);
            crouchManager.SetActive(false);
            isStealthOn = false;
            isHiding = false; // Exiting stealth mode will also cancel hiding state
        }

        // If in hiding state, continuously update player's rotation to match hiding place's
        if (isHiding)
        {
            // If player is moving left or right, adjust hiding rotation accordingly
            float moveDirection = Input.GetAxis("Horizontal");
            if (Mathf.Abs(moveDirection) > 0.1f)
            {
                hidingPlaceRotation = Quaternion.LookRotation(
                    currentHidingPlace.transform.right * Mathf.Sign(moveDirection),
                    Vector3.up
                );
            }

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                hidingPlaceRotation,
                Time.deltaTime * rotationSpeed
            );
        }

        // If touching wall and not in hiding, face perpendicular to wall
        if (isTouchingWall && !isHiding)
        {
            hidingPlaceRotation = Quaternion.LookRotation(
                -currentHidingPlace.transform.forward,
                Vector3.up
            );
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                hidingPlaceRotation,
                Time.deltaTime * 20.0f
            ); // increase the speed here
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is a "hiding place"
        if (other.gameObject.tag == "HidingPlace")
        {
            // Store the current hiding place
            currentHidingPlace = other;
            isTouchingWall = true;

            // If player is in stealth mode and not already hiding
            if (isStealthOn && !isHiding)
            {
                EnterHidingState();
            }
            else if (!isHiding)
            {
                // Calculate hiding place rotation for standing state
                hidingPlaceRotation = Quaternion.LookRotation(
                    -currentHidingPlace.transform.forward,
                    Vector3.up
                );
                transform.rotation = hidingPlaceRotation;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collided object is a "HidingPlace" and the player is in hiding or touching wall
        if (other.gameObject.tag == "HidingPlace" && (isHiding || isTouchingWall))
        {
            Debug.Log("Exiting Hiding State or Touching Wall...");
            // Reset hiding state and touching wall state
            isHiding = false;
            isTouchingWall = false;
        }
    }

    private void EnterHidingState()
    {
        // Calculate the initial target rotation based on player's current rotation
        if (Vector3.Dot(transform.forward, currentHidingPlace.transform.right) > 0)
        {
            // Player is closer to facing right, so align to right
            hidingPlaceRotation = Quaternion.LookRotation(
                currentHidingPlace.transform.right,
                Vector3.up
            );
        }
        else
        {
            // Player is closer to facing left, so align to left
            hidingPlaceRotation = Quaternion.LookRotation(
                -currentHidingPlace.transform.right,
                Vector3.up
            );
        }

        Debug.Log("Entering Hiding State...");
        // Set the player's rotation
        transform.rotation = hidingPlaceRotation;

        // Enter hiding state
        isHiding = true;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isHiding)
        {
            if (
                hidingPlaceRotation
                == Quaternion.LookRotation(currentHidingPlace.transform.right, Vector3.up)
            )
            {
                activeHand = AvatarIKGoal.RightHand;
            }
            else
            {
                activeHand = AvatarIKGoal.LeftHand;
            }

            playerAnimator.SetIKPositionWeight(activeHand, 1);
            playerAnimator.SetIKRotationWeight(activeHand, 1);

            Vector3 handPositionOnWall =
                transform.position + Vector3.up * 1.5f + transform.right * 0.5f;
            playerAnimator.SetIKPosition(activeHand, handPositionOnWall);
            playerAnimator.SetIKRotation(activeHand, Quaternion.LookRotation(transform.forward));
        }
        else
        {
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }
    }
}
