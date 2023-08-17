using UnityEngine;
using UnityEngine.UI;
using TMPro;

// **** Class for ANY resource bar on the UI

public class VitBar : MonoBehaviour
{
    [SerializeField]
    private Image healthBarFill;

    [SerializeField]
    private TextMeshProUGUI healthText;

    public void UpdateVitBar(int currentHealth, int maxHealth)
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        healthBarFill.fillAmount = healthPercentage;
        healthText.text = $"{currentHealth}/{maxHealth}";
    }
}
