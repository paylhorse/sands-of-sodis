using System.Collections;
using UnityEngine;

// **** Apply RPG Maker A-tile animations to Field Quads

public class QuadAnimationController : MonoBehaviour
{
    public GameObject quad1;
    public GameObject quad2;
    public GameObject quad3;
    public float animationSpeed = 0.25f;

    private int currentStep;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= animationSpeed)
        {
            timer = 0;
            currentStep = (currentStep + 1) % 4;
            UpdateQuadVisibility();
        }
    }

    private void UpdateQuadVisibility()
    {
        switch (currentStep)
        {
            case 0:
                SetQuadVisibility(quad1, true);
                SetQuadVisibility(quad2, false);
                SetQuadVisibility(quad3, false);
                break;
            case 1:
                SetQuadVisibility(quad1, false);
                SetQuadVisibility(quad2, true);
                SetQuadVisibility(quad3, false);
                break;
            case 2:
                SetQuadVisibility(quad1, true);
                SetQuadVisibility(quad2, false);
                SetQuadVisibility(quad3, false);
                break;
            case 3:
                SetQuadVisibility(quad1, false);
                SetQuadVisibility(quad2, false);
                SetQuadVisibility(quad3, true);
                break;
        }
    }

    private void SetQuadVisibility(GameObject quad, bool visible)
    {
        if (quad != null)
        {
            quad.SetActive(visible);
        }
    }
}
