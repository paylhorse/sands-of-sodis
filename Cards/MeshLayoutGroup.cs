using UnityEngine;

// **** Forced horizontal layout for card mesh interface

public class MeshLayoutGroup : MonoBehaviour
{
    public float spacing = 2.0f; // Distance between each GameObject
    private Transform[] children;

    void Start()
    {
        // Get all the child GameObjects
        children = GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            // Skip the parent GameObject
            if (children[i] == transform)
                continue;

            // Position each GameObject
            Vector3 position = new Vector3(i * spacing, 0, 0);
            children[i].localPosition = position;
        }
    }
}
