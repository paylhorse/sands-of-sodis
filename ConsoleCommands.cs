using QFSW.QC;
using UnityEngine;

public class CustomCommands : MonoBehaviour
{

    public GameObject performanceOverlay;

    [Command("performance", MonoTargetType.All)]
    private void performance()
    {
      if(performanceOverlay.activeInHierarchy)
      {
        performanceOverlay.SetActive(false);
      }

      else
      {
        performanceOverlay.SetActive(true);
      }
    }
}



