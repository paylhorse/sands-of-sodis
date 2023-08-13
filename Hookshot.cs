using UnityEngine;

public class Hookshot : MonoBehaviour
{
    [SerializeField] private GameObject hookshotPrefab;
    [SerializeField] private float hookshotSpeed = 10f;
    [SerializeField] private float pullSpeed = 5f;
    [SerializeField] private KeyCode hookshotKey = KeyCode.Mouse1;

    private GameObject hookshotInstance;
    private LineRenderer lineRenderer;
    private bool isAttached = false;

    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(hookshotKey))
        {
            ShootHookshot();
        }
        else if (Input.GetKeyUp(hookshotKey))
        {
            DetachHookshot();
        }

        if (isAttached)
        {
            DrawRope();
            PullCharacter();
        }
    }

    private void ShootHookshot()
    {
        if (hookshotInstance == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider.CompareTag("GrapplePoint"))
                {
                    hookshotInstance = Instantiate(hookshotPrefab, hit.point, Quaternion.identity);
                    isAttached = true;
                    lineRenderer.enabled = true;
                }
            }
        }
    }

    private void DrawRope()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hookshotInstance.transform.position);
    }

    private void PullCharacter()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, hookshotInstance.transform.position, pullSpeed * Time.deltaTime);
        newPosition.y = transform.position.y; // Keep the character at the same height (remove this line if you want vertical movement)
        transform.position = newPosition;
    }

    private void DetachHookshot()
    {
        isAttached = false;
        lineRenderer.enabled = false;

        if (hookshotInstance != null)
        {
            Destroy(hookshotInstance);
        }
    }
}

