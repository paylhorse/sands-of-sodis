using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class VelocityReader : MonoBehaviour
{
    private RichAI richAI;
    private Animator animator;

    private void Start()
    {
        richAI = GetComponent<RichAI>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Convert velocity from global space to local space
        Vector3 localVelocity = transform.InverseTransformDirection(richAI.velocity);

        // Set the animator parameters
        animator.SetFloat("Speed", richAI.velocity.magnitude);
        animator.SetFloat("Speed-X", localVelocity.x);
        animator.SetFloat("Speed-Y", localVelocity.y);
        animator.SetFloat("Speed-Z", localVelocity.z);

        // Uncomment this for debugging
        // Debug.Log("Local Velocity: " + localVelocity);
    }
}
