using UnityEngine;

public class UIManager : MonoBehaviour
{
    public CanvasGroup basePanel;
    public CanvasGroup firstPanel;

    public void ActivateFirstLayer()
    {
        // Disable first panel interaction
        basePanel.interactable = false;
        basePanel.blocksRaycasts = false;

        // Enable second panel interaction
        firstPanel.interactable = true;
        firstPanel.blocksRaycasts = true;
    }

    public void DeactivateFirstLayer()
    {
        // Enable first panel interaction
        basePanel.interactable = true;
        basePanel.blocksRaycasts = true;

        // Disable second panel interaction
        firstPanel.interactable = false;
        firstPanel.blocksRaycasts = false;
    }
}
