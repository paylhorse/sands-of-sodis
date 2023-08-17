using UnityEngine;

// **** Foilage animation for the Brush Field Quad

public class QuadSway : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float speed = 1f;
    public float swayAxis = 0.5f;

    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
    }

    void Update()
    {
        for (int i = 0; i < originalVertices.Length; i++)
        {
            float wave = Mathf.Sin(
                Time.time * speed
                    + originalVertices[i].x * swayAxis
                    + originalVertices[i].y * (1 - swayAxis)
            );
            displacedVertices[i] = originalVertices[i] + Vector3.right * wave * amplitude;
        }

        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals();
    }
}
