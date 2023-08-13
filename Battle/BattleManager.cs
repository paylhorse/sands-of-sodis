using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;

//      +--------------+
//     /|             /|
//    *--+-----------* |
//    | | BATTLE     | |
//    | | MANAGER    | |
//    | +------------|-+
//    |/             |/
//    *--------------*
//
// Backbone of the Battle Scene

public class BattleManager : MonoBehaviour
{
    // The Sphere Menu and CSC handle player input
    public SphereMenu sphereMenu;
    public CardSelectController cardSelect;

    // IP management:
    // 
    public float ipMax = 100f;
    public float ipGainPerSecond = 10f;
    public float actGainPerSecond = 40f;

    public float ipBarLength = 407.0f;
    public float actBarLength = 158.0f;

    private bool timeFrozen = false;


    private PlayerState player;
    [SerializeField] private List<EnemyState> enemies;
    [SerializeField] private List<CharacterState> combatants;

    public List<EnemyState> Enemies
    {
        get { return enemies; }
    }

    public List<CharacterState> Combatants
    {
        get { return combatants; }
    }

    public SoundManager UISoundManager;

    private void Start()
    {
        // Find the Player and all Enemies in the scene
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = new List<EnemyState>();

        foreach (GameObject enemyObject in enemyObjects)
        {
            enemies.Add(enemyObject.GetComponent<EnemyState>());
        }

        // Combine player and enemies into a single list of combatants
        combatants = new List<CharacterState> { player };
        combatants.AddRange(enemies.Cast<CharacterState>());

        // Set a random starting IP for each combatant
        foreach (CharacterState combatant in combatants)
        {
            combatant.SetIP(Random.Range(0f, 40f));
        }

        sphereMenu.Deactivate();
    }

    private void Update()
    {
        if (!timeFrozen)
        {
	    // Create a list to contain any enemies found at VIT = 0
            List<EnemyState> deadEnemies = new List<EnemyState>();

            foreach (CharacterState combatant in combatants)
            {
		// KILL ENEMIES
                // If the combatant is an enemy and its health is less than or equal to 0, add it to the deadEnemies list
                if (combatant is EnemyState enemy && enemy.CurrentHealth <= 0)
                {
                    deadEnemies.Add(enemy);
                    continue;  // Skip the rest of the loop for this combatant
                }

                if(combatant.actGauge > 0)
                {
                    Debug.Log("A Combatant is Acting!");
                    // If the character is in the ACT phase
                    combatant.actGauge += (combatant.ACT * actGainPerSecond * Time.deltaTime) / combatant.GetCommandExecutionTime();

                    if(combatant.actGauge >= ipMax)
                    {
                        // If the ACT gauge is full, execute the command
                        if(combatant is PlayerState)
                        {
                            ExecutePlayerCommand();
                        }
                        else if(combatant is EnemyState)
                        {
                            ExecuteEnemyCommand((EnemyState)combatant);
                        }

                        combatant.actGauge = 0;
                    }
                }
                else
                {
                    // If the character is not in the ACT phase
                    combatant.GainIP(combatant.AGI * ipGainPerSecond * Time.deltaTime);

                    if(combatant.GetCurrentIP() >= ipMax)
                    {
                        if(combatant is PlayerState)
                        {
                            FreezeTime();
                            CommandInput();
                            combatant.actGauge += 1;
                            combatant.ipGauge = 0;
                        }
                        else if(combatant is EnemyState)
                        {
                            combatant.SetIP(0);
                            //EnemyTurn((Enemy)combatant);
                            //combatant.actGauge += 1;
                            //combatant.ipGauge = 0;
                        }
                    }
                }

                UpdateBattleboyPosition(combatant);
            }

            // Remove dead enemies from the lists
            foreach (EnemyState deadEnemy in deadEnemies)
            {
                combatants.Remove(deadEnemy);
                enemies.Remove(deadEnemy);
            }
        }
    }

    private void UpdateBattleboyPosition(CharacterState character)
    {
        float newPosition;

        if(character.actGauge > 0)
        {
            // If the character is in the ACT phase
            newPosition = ( (character.actGauge / ipMax) * actBarLength ) + ipBarLength; 
        }
        else
        {
            // If the character is not in the ACT phase
            newPosition = (character.GetCurrentIP() / ipMax) * ipBarLength;
        }

        character.BattleboyInstance.transform.localPosition = new Vector2(newPosition, 0);
    }

    private void CommandInput()
    {
        player.currentState = CharacterState.CharacterStateMachine.SELECTING;
        UISoundManager.PlaySound("Appear");
        sphereMenu.Activate();
    }

    private void EnemyTurn(EnemyState enemyCombatant)
    {
        // Enemy Command Select logic
        Debug.Log("Enemy Turn");
        //combatant.StartCombo();
    }

    // ---------- TIME CONTROL ----------

    public void FreezeTime()
    {
        Debug.Log("Time Frozen!");
        timeFrozen = true;

        foreach (CharacterState combatant in combatants)
        {
            // If this combatant is not the player, stop animator
            if (combatant != player)
            {
                combatant.animator.speed = 0;
            }
        }
    }

    public void ContinueTime()
    {
        Debug.Log("Time Resuming...");

        timeFrozen = false;

        foreach (CharacterState combatant in combatants)
        {
            combatant.animator.speed = 1;
        }
    }

    // ---------- COMMAND EXECUTION ----------

    public void ExecutePlayerCommand()
    {
        if (player.selectedCommand == "COMBO")
        {
            Debug.Log("Starting Combo...");
            player.StartCombo();
        }

        if (player.selectedCommand == "EVADE")
        {
            //
        }
    }

    public void ExecuteEnemyCommand(EnemyState enemyCombatant)
    {
        //
    }
}
