using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour
{
    public Sprite[] frames;
    public float framesPerSecond = 1.0f;
    private Image imageComponent;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        int frameIndex = 0;
        while (true)
        {
            imageComponent.sprite = frames[frameIndex];
            frameIndex = (frameIndex + 1) % frames.Length;
            yield return new WaitForSeconds(1.0f / framesPerSecond);
        }
    }
}
