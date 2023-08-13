using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// **** Radial Menu used for mouse input

public class ArrangeInCircle : MonoBehaviour
{
    public float radius = 100f;
    public float fadeInDuration = 1f;
    public float delayBetweenButtons = 0.2f;

    void Start()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            float angle = i * Mathf.PI * 2 / childCount;
            Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            Transform child = transform.GetChild(i);
            child.localPosition = position;

            CanvasGroup canvasGroup = child.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0; // Set initial alpha to zero

            // Start the FadeIn coroutine for this button
            StartCoroutine(FadeIn(canvasGroup, fadeInDuration));

            // Wait for the specified delay before fading in the next button
            if (i < childCount - 1)
            {
                StartCoroutine(WaitAndFadeIn(delayBetweenButtons * (i + 1), transform.GetChild(i + 1).GetComponent<CanvasGroup>(), fadeInDuration));
            }
        }
    }


    private IEnumerator WaitAndFadeIn(float delay, CanvasGroup canvasGroup, float duration)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(FadeIn(canvasGroup, duration));
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            canvasGroup.alpha = t;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }
}
