using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// **** Fills the QUARRY panel during Battle

public class EnemyListManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyListItemPrefab;
    private List<EnemyUnit> enemies;

    void Start()
    {
        enemies = new List<EnemyUnit>(FindObjectsOfType<EnemyUnit>());
        UpdateEnemyList();
    }

    void UpdateEnemyList()
    {
        // Clear any existing list items
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Add a new list item for each enemy
        foreach (EnemyUnit enemy in enemies)
        {
            GameObject listItem = Instantiate(enemyListItemPrefab, transform);
            TextMeshProUGUI listItemText = listItem.GetComponent<TextMeshProUGUI>(); // Use Text instead of TextMeshProUGUI if you're using Unity's default UI Text component
            listItemText.text = enemy.name;

            // Assign VitBar component to the healthBar field in the Enemy script
            VitBar healthBar = listItem.GetComponentInChildren<VitBar>();
            enemy.SetVitBar(healthBar);
        }
    }
}
