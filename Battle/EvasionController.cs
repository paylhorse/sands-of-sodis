using UnityEngine;
using System.Collections.Generic;

// **** Controls the EVADE command

public class EvasionController : MonoBehaviour
{
    public GameObject player;
    public float evasionRadius = 5f;
    public int circleResolution = 50;
    public float yOffset = 0.1f;
    public Material circleMaterial;

    private GameObject circleObject;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;

    // Evasion Boundary
    public GameObject evasionBoundaryPrefab;
    public int boundaryColliderCount = 100;
    private List<GameObject> evasionBoundaryColliders;

    private void Start()
    {
        circleObject = new GameObject("EvasionCircle");
        circleObject.transform.SetParent(transform);
        meshFilter = circleObject.AddComponent<MeshFilter>();
        meshRenderer = circleObject.AddComponent<MeshRenderer>();
        meshRenderer.material = circleMaterial;
        meshRenderer.enabled = false;

        mesh = new Mesh();
        meshFilter.mesh = mesh;

        UpdateEvasionCircle();
    }

    public void ShowEvasionCircle()
    {
        meshRenderer.enabled = true;
        CreateEvasionBoundary();
        Debug.Log("Evasion Circle Shown");
    }

    public void HideEvasionCircle()
    {
        meshRenderer.enabled = false;
        DestroyEvasionBoundary();
    }

    private void UpdateEvasionCircle()
    {
        Vector3[] vertices = new Vector3[circleResolution + 1];
        int[] triangles = new int[circleResolution * 3];

        float sumY = 0f;

        for (int i = 0; i < circleResolution; i++)
        {
            float angle = i * 2 * Mathf.PI / circleResolution;
            Vector3 vertexPosition = player.transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * evasionRadius;
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(vertexPosition, out hit, evasionRadius, UnityEngine.AI.NavMesh.AllAreas))
            {
                vertexPosition = hit.position + Vector3.up * yOffset;
                // Debug.Log("NavMesh hit found: " + hit.position);
            }
            vertices[i + 1] = vertexPosition;
            sumY += vertexPosition.y;

            if (i < circleResolution - 1)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
            }
            else
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = 1;
                triangles[i * 3 + 2] = i + 1;
            }

        }

        float avgY = sumY / circleResolution;
        vertices[0] = player.transform.position + Vector3.up * (avgY + yOffset);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void CreateEvasionBoundary()
    {
        evasionBoundaryColliders = new List<GameObject>();

        for (int i = 0; i < boundaryColliderCount; i++)
        {
            float angle = i * 2 * Mathf.PI / boundaryColliderCount;
            Vector3 boundaryPosition = player.transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * evasionRadius;
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(boundaryPosition, out hit, evasionRadius, UnityEngine.AI.NavMesh.AllAreas))
            {
                boundaryPosition = hit.position;
            }

            GameObject boundaryCollider = Instantiate(evasionBoundaryPrefab, boundaryPosition, Quaternion.identity);
            boundaryCollider.transform.SetParent(transform);
            boundaryCollider.transform.LookAt(player.transform);
            evasionBoundaryColliders.Add(boundaryCollider);
        }
    }

    private void DestroyEvasionBoundary()
    {
        foreach (GameObject boundaryCollider in evasionBoundaryColliders)
        {
            Destroy(boundaryCollider);
        }
        evasionBoundaryColliders.Clear();
    }


}
