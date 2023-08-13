using UnityEngine;
using UnityEngine.Events;
using System.Collections;

// **** Class for collectibles on the Field

public class PickupNode : MonoBehaviour
{
    public UnityEvent onPickup; // Define a custom event that will be triggered on pickup. You can set this in the inspector.
    public GameObject UIElement; // Reference to the UI element that will be activated when player enters the trigger.
    private Animator UIAnimator;

    private bool canPickup = false; // A flag to indicate whether player is in pickup range

    // Add a static counter to keep track of overlapping pickup nodes
    private static int overlappingPickupNodes = 0;

    private void Start()
    {
        UIAnimator = UIElement.GetComponent<Animator>();
    }

    private void Update()
    {
        // Listen for the action button (F, Z, or X on the gamepad)
        if (canPickup && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Z)))
        {
            // Execute the defined event, deactivate the UI element, stop listening for input, and destroy the game object.
            onPickup?.Invoke();
            overlappingPickupNodes--;
            if (overlappingPickupNodes <= 0)
            {
                StartCoroutine(ShowDisappearance());
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the player enters the trigger, activate the UI element and start listening for input.
        if (other.CompareTag("Player"))
        {
            overlappingPickupNodes++;
            StartCoroutine(ShowAppearance());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When the player leaves the trigger, deactivate the UI element and stop listening for input.
        if (other.CompareTag("Player"))
        {
            overlappingPickupNodes--;
            if (overlappingPickupNodes <= 0)
            {
                StartCoroutine(ShowDisappearance());
            }
        }
    }

    private IEnumerator ShowAppearance()
    {
        UIAnimator.SetBool("isVisible", true);
        yield return new WaitForSeconds(UIAnimator.GetCurrentAnimatorStateInfo(0).length);
        canPickup = true;
    }

    private IEnumerator ShowDisappearance()
    {
        UIAnimator.SetBool("isVisible", false);
        yield return new WaitForSeconds(UIAnimator.GetCurrentAnimatorStateInfo(0).length);
        UIElement.SetActive(false);
        canPickup = false;
    }
}
