using UnityEngine;
using UnityEngine.InputSystem;

public class HoldStandParameter : MonoBehaviour
{
    public InputActionAsset characterActions;
    private InputAction holdStandAction;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Get the HoldStand action from the Input Action Asset
        holdStandAction = characterActions.FindActionMap("Vessel").FindAction("HoldStand");

        // Enable the action
        holdStandAction.Enable();
    }

    void Update()
    {
        // Get the value of the HoldStand action
        float holdStandValue = holdStandAction.ReadValue<float>();

        // Set the Stand parameter to the value of the HoldStand action
        animator.SetFloat("Stand", Mathf.Clamp01(holdStandValue));
    }

    void OnDestroy()
    {
        // Disable the action when the script is destroyed
        holdStandAction.Disable();
    }
}
