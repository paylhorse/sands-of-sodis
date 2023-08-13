using UnityEngine;
using UnityEngine.EventSystems;

// **** Class for mouse control radial buttons

public class RadialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int buttonGridX;
    public int buttonGridY;
    private SphereMenu sphereMenu;

    // Grow
    public float scaleFactor = 1.04f;
    private Vector3 originalScale;

    // Audio
    public SoundManager UISoundManager;

    private void Start()
    {
        originalScale = transform.localScale;
        sphereMenu = FindObjectOfType<SphereMenu>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * scaleFactor;
        sphereMenu.MoveToButton(buttonGridX, buttonGridY);
        UISoundManager.PlaySound("SphereMove");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
