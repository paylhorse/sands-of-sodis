using UnityEngine;
using TMPro;

// **** Fadeout animation for floating damage numbers

public class DamageTextFadeOut : MonoBehaviour
{
    public float fadeOutDuration = 1.0f;
    public float upwardsSpeed = 1.0f;

    private TextMeshProUGUI text;
    private float fadeOutStartTime;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        fadeOutStartTime = Time.time;
    }

    private void Update()
    {
        float elapsedTime = Time.time - fadeOutStartTime;
        float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeOutDuration);
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);

        // Move the text slightly upwards
        transform.position += Vector3.up * upwardsSpeed * Time.deltaTime;

        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
