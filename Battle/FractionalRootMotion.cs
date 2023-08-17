using UnityEngine;

// **** Utility script for fractional control of Animator root motion

public class FractionalRootMotion : MonoBehaviour
{
    [SerializeField]
    private float rootMotionMultiplier = 0.5f;
    private Animator animator;

    // Indicates whether to use root motion for movement
    private bool useRootMotion = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        if (animator && useRootMotion)
        {
            // Apply the fractional root motion
            Vector3 newPosition = transform.position;
            newPosition += animator.deltaPosition * rootMotionMultiplier;
            transform.position = newPosition;
        }
    }

    public void EnableRootMotion()
    {
        if (animator)
        {
            animator.applyRootMotion = true;
            useRootMotion = true;
        }
    }

    public void DisableRootMotion()
    {
        if (animator)
        {
            animator.applyRootMotion = false;
            useRootMotion = false;
        }
    }
}
