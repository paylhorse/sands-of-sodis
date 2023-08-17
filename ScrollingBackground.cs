using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeedX = 0.5f;

    [SerializeField]
    private float scrollSpeedY = 0.5f;

    private Image backgroundImage;

    private void Start()
    {
        backgroundImage = GetComponent<Image>();
        backgroundImage.material = new Material(backgroundImage.material);
    }

    private void Update()
    {
        float offsetX = Time.time * scrollSpeedX % 1;
        float offsetY = Time.time * scrollSpeedY % 1;

        backgroundImage.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}
