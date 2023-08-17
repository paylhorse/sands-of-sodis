using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// **** Visual for EVADE

public class MeshTrail : MonoBehaviour
{
    public float ghostLifetime = 2.0f;

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3.0f;
    public Transform positionToSpawn;

    [Header("Shader Related")]
    public Material mat;

    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    private bool isTrailActive;

    void Start() { }

    void Update()
    {
        if (!isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(ghostLifetime));
        }
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
            {
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            }

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject ghostObject = new GameObject();
                ghostObject.transform.SetPositionAndRotation(
                    positionToSpawn.position,
                    positionToSpawn.rotation
                );

                MeshRenderer ghostRender = ghostObject.AddComponent<MeshRenderer>();
                MeshFilter ghostFilter = ghostObject.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                ghostFilter.mesh = mesh;
                ghostRender.material = mat;

                Destroy(ghostObject, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
    }
}
