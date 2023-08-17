using UnityEngine;
using UnityEngine.UI;

// ---- DEPRECATED
// **** Controller for alternating sunlight overlay

public class GodrayOpacity : MonoBehaviour
{
    [SerializeField]
    private Image godrayImage1;

    [SerializeField]
    private Image godrayImage2;

    [SerializeField]
    private float opacitySpeed = 1.0f;

    [SerializeField]
    private float minOpacity = 0.2f;

    [SerializeField]
    private float maxOpacity = 1.0f;

    private float currentLerpTime1 = 0.0f;
    private float currentLerpTime2 = 0.0f;

    private void Update()
    {
        // Update the lerp times
        currentLerpTime1 += Time.deltaTime * opacitySpeed;
        currentLerpTime2 += Time.deltaTime * opacitySpeed;

        // Calculate the lerp values
        float lerpValue1 = Mathf.PingPong(currentLerpTime1, 1.0f);
        float lerpValue2 = Mathf.PingPong(currentLerpTime2 + 0.5f, 1.0f);

        // Set the opacities
        SetImageOpacity(godrayImage1, lerpValue1);
        SetImageOpacity(godrayImage2, lerpValue2);
    }

    private void SetImageOpacity(Image image, float lerpValue)
    {
        Color currentColor = image.color;
        currentColor.a = Mathf.Lerp(minOpacity, maxOpacity, lerpValue);
        image.color = currentColor;
    }
}
