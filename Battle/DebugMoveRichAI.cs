using Pathfinding;
using UnityEngine;

// **** Debug script to mouse-control RichAI

public class DebugMoveRichAI : MonoBehaviour
{
    private RichAI richAI;

    private void Start()
    {
        richAI = GetComponent<RichAI>();
    }

    private void Update()
    {
        //Debug.Log("RichAI Speed: " + richAI.velocity.magnitude);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                richAI.destination = hit.point;
            }
        }
    }
}
