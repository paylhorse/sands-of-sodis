using System.Collections;
using UnityEngine;

// **** Handles Main Hand equipment swapping

public class EquipmentController : MonoBehaviour
{
    public GameObject swordInHand;
    public GameObject swordOnBack;
    public GameObject bowInHand;
    public GameObject bowOnBack;
    public GameObject handaxeInHand;
    public GameObject handaxeOnBack;

    public GameObject swordOverlay;
    public GameObject bowOverlay;
    public GameObject handaxeOverlay;
    public GameObject unequipOverlay;

    public TrailRenderer swordTrail; // Reference to the TrailRenderer component
    public Animator animator; // Reference to the Animator component

    public float swordSlashAnimationLength = 0.2f;

    private enum Equipment { None, Sword, Bow, Handaxe }
    private Equipment currentEquipment = Equipment.None;

    // Controller Support

    private bool showOverlayOnNextFrame;

    private float equipmentSwitchCooldown = 0.3f; // Cooldown time in seconds
    private float lastEquipmentSwitchTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipHandaxe();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipSword();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipBow();
        }

        // Usage

        if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Button X"))
        {
            if (currentEquipment == Equipment.None)
            {
                // Nothing Here
            }

            if (currentEquipment == Equipment.Sword)
            {
                SwordSlash();
            }

            if (currentEquipment == Equipment.Bow)
            {
                // Nothing Here Yet
            }

            if (currentEquipment == Equipment.Handaxe)
            {
                HandaxeSlash();
            }
        }

        HandleControllerInput();
    }

    private void HandleControllerInput()
    {
        // Check if the right bumper is pressed or released
        if (Input.GetButtonDown("Bumper Right") || Input.GetButtonUp("Bumper Right"))
        {
            // Toggle the overlay for the currently equipped item
            ToggleOverlayForCurrentEquipment();
        }

        // Check if the right bumper is held down
        if (Input.GetButton("Bumper Right"))
        {
            // Switch equipment based on D-pad input
            float horizontalDpad = Input.GetAxis("6th Axis");

            if (Time.time >= lastEquipmentSwitchTime + equipmentSwitchCooldown)
            {
                if (horizontalDpad < 0) // D-pad left
                {
                    CycleEquipment(false);
                    lastEquipmentSwitchTime = Time.time;
                }
                else if (horizontalDpad > 0) // D-pad right
                {
                    CycleEquipment(true);
                    lastEquipmentSwitchTime = Time.time;
                }
            }
        }
        // Check if the right bumper is released
        else
        {
            // Hide the overlay for the currently equipped item
            HideOverlayForCurrentEquipment();
        }
    }

    private void ToggleOverlayForCurrentEquipment()
    {
        GameObject overlay;

        switch (currentEquipment)
        {
            case Equipment.Sword:
                overlay = swordOverlay;
                break;
            case Equipment.Bow:
                overlay = bowOverlay;
                break;
            case Equipment.Handaxe:
                overlay = handaxeOverlay;
                break;
            default:
                overlay = unequipOverlay;
                break;
        }

        overlay.SetActive(!overlay.activeSelf);
    }

    private void CycleEquipment(bool forward)
    {
        if (forward)
        {
            if (currentEquipment == Equipment.None)
            {
                EquipHandaxe();
            }
            else if (currentEquipment == Equipment.Handaxe)
            {
                EquipSword();
            }
            else if (currentEquipment == Equipment.Sword)
            {
                EquipBow();
            }
            else if (currentEquipment == Equipment.Bow)
            {
                EquipHandaxe();
            }
        }
        else
        {
            if (currentEquipment == Equipment.None)
            {
                EquipBow();
            }
            else if (currentEquipment == Equipment.Handaxe)
            {
                EquipBow();
            }
            else if (currentEquipment == Equipment.Sword)
            {
                EquipHandaxe();
            }
            else if (currentEquipment == Equipment.Bow)
            {
                EquipSword();
            }
        }
    }

    private GameObject currentOverlay;

    private void ShowOverlayForCurrentEquipment()
    {
        GameObject overlay;

        switch (currentEquipment)
        {
            case Equipment.Sword:
                overlay = swordOverlay;
                break;
            case Equipment.Bow:
                overlay = bowOverlay;
                break;
            case Equipment.Handaxe:
                overlay = handaxeOverlay;
                break;
            default:
                overlay = unequipOverlay;
                break;
        }

        overlay.SetActive(true);
    }

    private void HideOverlayForCurrentEquipment()
    {
        if (currentOverlay != null)
        {
            currentOverlay.SetActive(false);
        }
    }

    void EquipSword()
    {
        if (currentEquipment == Equipment.Sword)
        {
            // Unequip sword
            swordInHand.SetActive(false);
            swordOnBack.SetActive(true);
            currentEquipment = Equipment.None;
        }
        else
        {
            UnequipCurrentEquipment();

            // Equip sword
            swordInHand.SetActive(true);
            swordOnBack.SetActive(false);
            currentEquipment = Equipment.Sword;

            StartCoroutine(ShowOverlay(swordOverlay, 2));
        }
    }

    void EquipBow()
    {
        if (currentEquipment == Equipment.Bow)
        {
            // Unequip bow
            bowInHand.SetActive(false);
            bowOnBack.SetActive(true);
            currentEquipment = Equipment.None;
        }
        else
        {
            UnequipCurrentEquipment();

            // Equip bow
            bowInHand.SetActive(true);
            bowOnBack.SetActive(false);
            currentEquipment = Equipment.Bow;

            StartCoroutine(ShowOverlay(bowOverlay, 2));
        }
    }

    void EquipHandaxe()
    {
        if (currentEquipment == Equipment.Handaxe)
        {
            // Unequip handaxe
            handaxeInHand.SetActive(false);
            handaxeOnBack.SetActive(true);
            currentEquipment = Equipment.None;
        }
        else
        {
            UnequipCurrentEquipment();

            // Equip handaxe
            handaxeInHand.SetActive(true);
            handaxeOnBack.SetActive(false);
            currentEquipment = Equipment.Handaxe;

            StartCoroutine(ShowOverlay(handaxeOverlay, 2));
        }
    }

    private void UnequipCurrentEquipment()
    {
        // Show the unequip overlay when an item is unequipped
        StartCoroutine(ShowOverlay(unequipOverlay, 2));

        if (currentEquipment == Equipment.Sword)
        {
            swordInHand.SetActive(false);
            swordOnBack.SetActive(true);
        }
        else if (currentEquipment == Equipment.Bow)
        {
            bowInHand.SetActive(false);
            bowOnBack.SetActive(true);
        }
        else if (currentEquipment == Equipment.Handaxe)
        {
            handaxeInHand.SetActive(false);
            handaxeOnBack.SetActive(true);
        }
    }

    private IEnumerator ShowOverlay(GameObject overlay, float duration)
    {
        // Disable all overlays before showing the new one
        DisableAllOverlays();

        overlay.SetActive(true);
        yield return new WaitForSeconds(duration);
        overlay.SetActive(false);
    }

    private void DisableAllOverlays()
    {
        swordOverlay.SetActive(false);
        bowOverlay.SetActive(false);
        handaxeOverlay.SetActive(false);
        unequipOverlay.SetActive(false);
    }

    // Sword

    private void SwordSlash()
    {
        Debug.Log("Slashing Sword!");
        swordTrail.enabled = true;
        animator.Play("SwordSlash", 0 ,0f);
        StartCoroutine(DisableSwordTrailAfterAnimation());
    }

    private IEnumerator DisableSwordTrailAfterAnimation()
    {
        yield return new WaitForSeconds(swordSlashAnimationLength);
        swordTrail.enabled = false;
    }

    // Handaxe

    private void HandaxeSlash()
    {
        Debug.Log("Slashing Handaxe!");
        animator.Play("SwordSlash", 0 ,0f);
    }
}
