using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ScreenDialogue : MonoBehaviour
{
    public GameObject content; // Reference to the Content GameObject inside the Scroll View
    public ScrollRect scrollRect; // Reference to the ScrollRect component of the Scroll View
    
    public GameObject dialogueLightPrefab; // Reference to the DialogueLight prefab
    public GameObject dialogueDarkPrefab; // Reference to the DialogueDark prefab
    public GameObject nameplatePrefab; // Reference to the Nameplate prefab

    public void PushDialogueLight(string messageText)
    {
        PushMessage(dialogueLightPrefab, messageText);
    }

    public void PushDialogueDark(string messageText)
    {
        PushMessage(dialogueDarkPrefab, messageText);
    }

    public void PushNameplate(string messageText)
    {
        PushMessage(nameplatePrefab, messageText);
    }

    private void PushMessage(GameObject prefab, string messageText)
    {
        // Instantiate the prefab
        GameObject newPanel = Instantiate(prefab, content.transform);

        // Set the panel's text to the given message
        TMP_Text panelText = newPanel.GetComponentInChildren<TMP_Text>();
        if (panelText != null)
        {
            panelText.text = messageText;
        }

        // Scroll to the bottom
        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom()
    {
        // Wait for end of frame and then next frame to allow UI to update
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        float duration = 0.1f; // Duration of the smooth scrolling effect
        float elapsedTime = 0f;
        float targetNormalizedPosition = -0.002f;
        float startingNormalizedPosition = scrollRect.verticalNormalizedPosition;

        while (elapsedTime < duration)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(startingNormalizedPosition, targetNormalizedPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final value is set to the target position
        scrollRect.verticalNormalizedPosition = targetNormalizedPosition;
    }
}
