using UnityEngine;
using UnityEngine.EventSystems;

// **** UI-Element property to spawn tooltips

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject tooltipUI;

    private void Start()
    {
        TooltipPanel[] tooltipPanels = FindObjectsOfType<TooltipPanel>();
        foreach (var panel in tooltipPanels)
        {
            if (panel.gameObject.CompareTag("TooltipPanel"))
            {
                tooltipUI = panel.gameObject;
                break;
            }
        }

        if (tooltipUI == null)
        {
            Debug.LogError(
                "Tooltip UI not found. Please ensure there's a GameObject with the 'TooltipPanel' tag and 'TooltipPanel' script in the scene."
            );
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipUI != null)
        {
            tooltipUI.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipUI != null)
        {
            tooltipUI.SetActive(false);
        }
    }
}
