using System.Collections;
using UnityEngine;

// **** DEPRECATED
// **** Handle waking up sequence at game start

public class WakeUpController : MonoBehaviour
{
    public Animator playerAnimator;
    public GameObject controllableTrigger;
    private bool isWakingUp = false;

    private void Start()
    {
        playerAnimator.SetBool("IsLyingDown", true);
    }

    private void Update()
    {
        // if player is still lying down and any movement key is pressed
        if (
            playerAnimator.GetBool("IsLyingDown") && Input.GetButtonDown("Horizontal")
            || Input.GetButtonDown("Vertical")
        )
        {
            playerAnimator.SetBool("IsWakingUp", true);
            playerAnimator.SetBool("IsLyingDown", false);
            isWakingUp = true;
        }
        if (isWakingUp && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
        {
            controllableTrigger.SetActive(false);
            isWakingUp = false;
        }
    }
}
