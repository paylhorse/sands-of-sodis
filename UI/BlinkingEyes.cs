using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// **** Blinking behavior for Battle Portrait

public class BlinkingEyes : MonoBehaviour
{
    [SerializeField] private Image upperEyesImage;
    [SerializeField] private float minBlinkInterval = 2f;
    [SerializeField] private float maxBlinkInterval = 5f;
    [SerializeField] private float blinkDuration = 0.1f;
    [SerializeField] private float doubleBlinkDelay = 0.1f;

    private void Start()
    {
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            // Wait for a random interval between minBlinkInterval and maxBlinkInterval
            float blinkInterval = Random.Range(minBlinkInterval, maxBlinkInterval);
            yield return new WaitForSeconds(blinkInterval);

            // Perform the double blink
            for (int i = 0; i < 2; i++)
            {
                // Hide the upper eyes image
                upperEyesImage.enabled = false;
                yield return new WaitForSeconds(blinkDuration);

                // Show the upper eyes image again
                upperEyesImage.enabled = true;

                // Wait for the doubleBlinkDelay between the two blinks
                if (i == 0)
                {
                    yield return new WaitForSeconds(doubleBlinkDelay);
                }
            }
        }
    }
}


