using System.Collections;
using TMPro;
using UnityEngine;

// **** Letter-by-letter appearance behaviour for dialogue text

public class TypingEffect : MonoBehaviour
{
    public float typingSpeed = 0.05f;
    public GameObject completionMarker;

    private TextMeshProUGUI textComponent;
    private string fullText;
    private bool isTypingComplete = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        fullText = textComponent.text;
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textComponent.text = "";

        foreach (char character in fullText.ToCharArray())
        {
            textComponent.text += character;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingComplete = true;
        completionMarker.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            SkipTyping();
        }
    }

    private void SkipTyping()
    {
        if (!isTypingComplete)
        {
            StopCoroutine(typingCoroutine);
            textComponent.text = fullText;
            isTypingComplete = true;
            completionMarker.SetActive(true);
        }
    }
}
