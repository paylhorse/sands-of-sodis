using UnityEngine;
using UnityEngine.UI;

// **** Idle animation for Zelda-like heart sprites

public class HeartBeat : MonoBehaviour
{
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    public float speed = 2f;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * speed) + 1) / 2);
        rectTransform.localScale = new Vector3(scale, scale, scale);
    }
}
