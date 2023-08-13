using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// **** Controls the Main Camera of the Battle Scene

public class BattleCamera : MonoBehaviour
{	
    public Camera mainCam;

    private BattleManager battleManager;

    // Reference preset camera positions
    public Transform evadeCam;
    public Transform targetCam;
    public Transform selectCam;
    public float transitionDuration = 1f;

    // Store original camera positions
    private Vector3 mainCamInitialPosition;
    private Quaternion mainCamInitialRotation;
    private float mainCamInitialOrthographicSize;
    private float originalZ;

    // Camera is 'focused' if it assumes a default position
    private bool isFocused = false;

    private void Start()
    {
	// Find BattleManager    
        battleManager = FindObjectOfType<BattleManager>();

        // Save original position
        mainCamInitialPosition = mainCam.transform.position;
        mainCamInitialRotation = mainCam.transform.rotation;
        mainCamInitialOrthographicSize = mainCam.orthographicSize;
        originalZ = mainCam.transform.position.z;
    }

    void Update()
    {
        if (!isFocused)
        KeepCameraCenteredOnCombatants();
    }

    public void MoveToSelectCam()
    {
        isFocused = true;
        Debug.Log("Moving to Selection Camera...");
        StartCoroutine(TransitionToTransform(selectCam.position, selectCam.rotation, 6f));
    }

    public void MoveToEvadeCam()
    {
        isFocused = true;
        Debug.Log("Moving to Evasion Camera...");
        StartCoroutine(TransitionToTransform(evadeCam.position, evadeCam.rotation, 10f));
    }

    public void MoveToTargetCam()
    {
        isFocused = true;
        Debug.Log("Moving to Target Camera...");
        StartCoroutine(TransitionToTransform(targetCam.position, targetCam.rotation, 8f));
    }

    public void ReturnToMain()
    {
        isFocused = false;
        StartCoroutine(TransitionToTransform(mainCamInitialPosition, mainCamInitialRotation, mainCamInitialOrthographicSize));
    }

    private IEnumerator TransitionToTransform(Vector3 targetPosition, Quaternion targetRotation, float targetOrthographicSize)
    {
        Debug.Log("Transitioning Camera...");
        float elapsedTime = 0;

        Vector3 startPosition = mainCam.transform.position;
        Quaternion startRotation = mainCam.transform.rotation;
        float startOrthographicSize = mainCam.orthographicSize;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            float smoothStepT = Mathf.SmoothStep(0, 1, t);

            mainCam.transform.position = Vector3.Lerp(startPosition, targetPosition, smoothStepT);
            mainCam.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, smoothStepT);
            mainCam.orthographicSize = Mathf.Lerp(startOrthographicSize, targetOrthographicSize, smoothStepT);

            yield return null;
        }

        mainCam.transform.position = targetPosition;
        mainCam.transform.rotation = targetRotation;
        mainCam.orthographicSize = targetOrthographicSize;
    }

    private void KeepCameraCenteredOnCombatants()
    {
        List<CharacterState> combatants = battleManager.Combatants;

        if (combatants.Count > 0)
        {
            // Calculate the average position of all combatants
            Vector3 sumPosition = Vector3.zero;

            foreach (CharacterState combatant in combatants)
            {
                sumPosition += new Vector3(combatant.transform.position.x, 0, combatant.transform.position.z);
            }

            Vector3 centroidPosition = sumPosition / combatants.Count;

            // Calculate the maximum distance from the average position to any combatant
            float radius = 0.0f;

            foreach (CharacterState combatant in combatants)
            {
                float distance = Vector3.Distance(centroidPosition, new Vector3(combatant.transform.position.x, 0, combatant.transform.position.z));
                radius = Mathf.Max(radius, distance);
            }

            // Draw the bounding sphere in the editor for debugging
            DrawDebugSphere(centroidPosition, radius, Color.red);

            // Adjust the camera position so the centroid is in view
            Vector3 targetPosition = centroidPosition;

	    // Retain the original camera height
            targetPosition.y = mainCam.transform.position.y; 

            // Define an offset along the XZ plane
            Vector3 offset = new Vector3(-6, 0, -5); 

            // Apply the offset to the target position
            targetPosition += offset;

            mainCam.transform.position = targetPosition;
        }
    }

    void DrawDebugSphere(Vector3 position, float radius, Color color)
    {
        float theta = 0;
        float x = radius * Mathf.Cos(theta);
        float z = radius * Mathf.Sin(theta);
        Vector3 pos = position + new Vector3(x, 0, z);
        Vector3 newPos = pos;
        Vector3 lastPos = pos;
        for(theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = radius * Mathf.Cos(theta);
            z = radius * Mathf.Sin(theta);
            newPos = position + new Vector3(x, 0, z);
            Debug.DrawLine(pos, newPos, color, 0.1f);
            pos = newPos;
        }
        Debug.DrawLine(pos, lastPos, color, 0.1f);
    }
}
