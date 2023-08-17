using UnityEngine;
using UnityEngine.EventSystems;

public class PowerButton : MonoBehaviour, IPointerClickHandler
{
    public AudioClip clickSound;
    public AudioSource audioSource;
    public Transform buttonTransform;
    public float buttonPressDepth = 0.02f;
    public float buttonPressSpeed = 10f;
    public Renderer tvRenderer;
    public Material tvOffMaterial;
    public Material tvOnMaterial;

    private bool tvOn = false;
    private bool buttonPressed = false;
    private Vector3 initialButtonPosition;
    private Vector3 pressedButtonPosition;

    private void Start()
    {
        initialButtonPosition = buttonTransform.localPosition;
        pressedButtonPosition = initialButtonPosition - buttonTransform.forward * buttonPressDepth;
    }

    private void Update()
    {
        if (buttonPressed)
        {
            buttonTransform.localPosition = Vector3.Lerp(
                buttonTransform.localPosition,
                pressedButtonPosition,
                buttonPressSpeed * Time.deltaTime
            );
        }
        else
        {
            buttonTransform.localPosition = Vector3.Lerp(
                buttonTransform.localPosition,
                initialButtonPosition,
                buttonPressSpeed * Time.deltaTime
            );
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonPressed = true;
        Invoke("ReleaseButton", 0.1f);
        audioSource.PlayOneShot(clickSound);
        ToggleTV();
    }

    private void ReleaseButton()
    {
        buttonPressed = false;
    }

    private void ToggleTV()
    {
        tvOn = !tvOn;
        tvRenderer.material = tvOn ? tvOnMaterial : tvOffMaterial;
    }
}
