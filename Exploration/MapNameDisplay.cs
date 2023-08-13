using System.Collections;
using UnityEngine;
using TMPro;

// **** Read Map Area names and display them on the UI

public class MapNameDisplay : MonoBehaviour
{
    public Canvas overlayCanvas;
    public TextMeshProUGUI mapNameText;
    public float displayDuration = 3.0f;

    private Coroutine displayCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MapArea"))
        {
            MapArea area = other.GetComponent<MapArea>();
            if (area != null)
            {
                if (displayCoroutine != null)
                {
                    StopCoroutine(displayCoroutine);
                }
                displayCoroutine = StartCoroutine(DisplayMapName(area.mapName));
            }
        }
    }

    private IEnumerator DisplayMapName(string mapName)
    {
        mapNameText.text = mapName;
        mapNameText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        mapNameText.gameObject.SetActive(false);
    }
}
