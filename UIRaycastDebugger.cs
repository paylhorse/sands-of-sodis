using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIRaycastDebugger : MonoBehaviour
{
    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;
    private PointerEventData pointerEventData;

    private void Start()
    {
        // Get the GraphicRaycaster and EventSystem components
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();

        // Check if GraphicRaycaster and EventSystem are available
        if (graphicRaycaster == null || eventSystem == null)
        {
            Debug.LogError("UIRaycastDebugger requires a GraphicRaycaster and EventSystem component on the same GameObject.");
            return;
        }
    }

    private void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            // Perform the raycast and store the results in a list
            var raycastResults = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerEventData, raycastResults);

            // Loop through the results and log the name of the hit UI element
            foreach (var result in raycastResults)
            {
                Debug.Log("UI Element Clicked: " + result.gameObject.name);
            }
        }
    }
}
