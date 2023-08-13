using UnityEngine;

// **** Controls the target-select interface proceeding commands

public class TargetSelectController : MonoBehaviour
{
    public BattleManager battleManager;
    public GameObject swordCursorPrefab;
    private GameObject currentCursor;
    private int currentEnemyIndex = 0;

    public SoundManager UISoundManager;
    public LayerMask enemyLayer;  // Set this in the inspector to match your enemy's layer

    public CustomCursor customCursor;

    private void Start()
    {
        // Initialize the cursor at the first enemy's position
        SetCursorToEnemy(currentEnemyIndex);

        customCursor = GameObject.FindObjectOfType<CustomCursor>();
    }

    private void OnEnable()
    {
        // Initialize the cursor at the first enemy's position
        SetCursorToEnemy(currentEnemyIndex);
    }

    private void Update()
    {
        // Check for user input
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCursor(-1); // Move the cursor to the previous enemy
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCursor(1); // Move the cursor to the next enemy
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            SelectCurrentEnemy();
        }

        HandleMouseOverEnemy();
    }

    private void SetCursorToEnemy(int enemyIndex)
    {
        // Destroy any previous cursor
        if (currentCursor != null)
        {
            Destroy(currentCursor);
        }

        // Instantiate a new cursor at the enemy's position
        Vector3 cursorPosition = battleManager.Enemies[enemyIndex].transform.position + Vector3.up; // Add Vector3.up to position it above the enemy
        currentCursor = Instantiate(swordCursorPrefab, cursorPosition, swordCursorPrefab.transform.rotation);
    }

    private void MoveCursor(int direction)
    {
        // Get the new enemy index, ensuring it wraps around the list correctly
        currentEnemyIndex = (currentEnemyIndex + direction + battleManager.Enemies.Count) % battleManager.Enemies.Count;

        UISoundManager.PlaySound("Pon");

        // Set the cursor to the new enemy
        SetCursorToEnemy(currentEnemyIndex);
    }

    private void SelectCurrentEnemy()
    {
        // Set the currently selected enemy in the Player Character
        PlayerState player = battleManager.Combatants[0] as PlayerState; // Assuming the first combatant is always the player
        player.selectedEnemy = battleManager.Enemies[currentEnemyIndex].transform; // Assuming the remaining combatants are enemies
        battleManager.ContinueTime();
        Destroy(currentCursor);
        this.enabled = false;
    }

    private void HandleMouseOverEnemy()
    {
        // Raycast to detect if mouse is over an enemy
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Draw the ray for visual debugging in the Scene view
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 2f); // Draw a red line

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayer))
        {
            // Debugging output
            // Debug.Log("Raycast hit: " + hit.transform.name);

            EnemyState enemy = hit.transform.GetComponent<EnemyState>();

            if (enemy != null)
            {
                // Debugging output
                // Debug.Log("Enemy found: " + enemy.name);

                // Find index of the enemy in the list
                int index = battleManager.Enemies.IndexOf(enemy);

                if (index != -1 && index != currentEnemyIndex) // Only if the enemy is different from the current one
                {
                    // Move cursor to this enemy
                    currentEnemyIndex = index;
                    SetCursorToEnemy(currentEnemyIndex);

                    UISoundManager.PlaySound("Pon");

                    customCursor.SetCursorOverEnemy();
                }
            }
        }
        else
        {
            customCursor.SetCustomCursor(customCursor.defaultCursorTexture);
        }
    }

}
