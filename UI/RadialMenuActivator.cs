using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

// **** Listener for the mouse-control interface to be activated

public class RadialMenuActivator : MonoBehaviour
{
    public GameObject radialMenu;
    public GameObject psxLabel;
    private CanvasGroup canvasGroup;

    private bool isOver = false;
    private RectTransform rectTransform;

    public Transform eyeMesh;
    private Quaternion originalEyeRotation;

    public float fadeDuration;
    public CanvasRenderer sphereCanvasRenderer;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        originalEyeRotation = eyeMesh.rotation;
    }

    private void Update()
    {
        bool isMouseOver = RectTransformUtility.RectangleContainsScreenPoint(
            rectTransform,
            Input.mousePosition
        );

        if (isMouseOver)
        {
            if (!isOver) // Mouse has just entered the RectTransform
            {
                isOver = true;
                radialMenu.SetActive(true);
                psxLabel.SetActive(false);
                StartCoroutine(FadeOut(fadeDuration));
            }

            // Update Eye Rotation
            UpdateEyeRotation();
        }
        else if (isOver) // Mouse has just exited the RectTransform
        {
            isOver = false;
            radialMenu.gameObject.SetActive(false);
            psxLabel.SetActive(true);
            StartCoroutine(FadeIn(fadeDuration));

            // Reset Eye Rotation
            eyeMesh.rotation = originalEyeRotation;
        }
    }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     isOver = true;
    //     radialMenu.SetActive(true);
    //     psxLabel.SetActive(false);
    //     StartCoroutine(FadeOut(fadeDuration));
    // }

    private Vector2 GetMouseDirectionFromCenter()
    {
        Vector2 screenCenter = RectTransformUtility.WorldToScreenPoint(
            null,
            rectTransform.position
        );
        Vector2 mousePosition = Input.mousePosition;
        return (mousePosition - screenCenter).normalized;
    }

    private IEnumerator FadeIn(float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            sphereCanvasRenderer.SetAlpha(t);
            yield return null;
        }
        sphereCanvasRenderer.SetAlpha(1);
    }

    private IEnumerator FadeOut(float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            sphereCanvasRenderer.SetAlpha(1 - t);
            yield return null;
        }
        sphereCanvasRenderer.SetAlpha(0);
    }

    private void UpdateEyeRotation()
    {
        Vector2 mouseVector = GetMouseVectorFromCenter();
        Vector2 mouseDirection = mouseVector.normalized;
        float distanceFromCenter = mouseVector.magnitude;

        // Convert distance from center to a range of 0 to 1
        float maxDistance = rectTransform.rect.width / 2;
        distanceFromCenter = Mathf.Clamp(distanceFromCenter / maxDistance, 0f, 1f);

        // Convert mouse direction to rotation angles
        // Mouse vertical movement affects X-axis rotation, horizontal movement affects Y-axis rotation
        // Rotation is also scaled by the distance from the center of the RectTransform
        float rotationX = -mouseDirection.y * distanceFromCenter * 30f;
        float rotationY = -mouseDirection.x * distanceFromCenter * 30f;

        // Apply rotation
        eyeMesh.localRotation = originalEyeRotation * Quaternion.Euler(rotationX, rotationY, 0);
    }

    private Vector2 GetMouseVectorFromCenter()
    {
        Vector2 screenCenter = RectTransformUtility.WorldToScreenPoint(
            null,
            rectTransform.position
        );
        Vector2 mousePosition = Input.mousePosition;
        return mousePosition - screenCenter;
    }
}
