using System.Collections;
using TMPro;
using UnityEngine;
using Pathfinding;
using MoonSharp.Interpreter;

//      +--------------+
//     /|             /|
//    *--+-----------* |
//    | | B(ATTLE)   | |
//    | | UNIT       | |
//    | +------------|-+
//    |/             |/
//    *--------------*
//
// C# class and interface for BUnits

// Each BUnit is allocated a Lua table for storing and managing stats, other data.
// The BUnit's in-game STATE, however, is managed in this C# class.

public class BUnit : MonoBehaviour
{
    // Get LuaBackbone instance
    LuaBackbone luaBackbone = LuaBackbone.Instance;

    // Store the lua reference of a unit
    protected DynValue luaUnitReference;

    // Name:
    public string name;

    // IP
    public float ipGauge;
    public float actGauge;

    [Header("Initial Stats")]

    public int STR;    
    public int RES;
    public int AGI;
    public int DEX;
    public int VAS;

    // States
    public CharacterStateMachine currentState;

    public enum CharacterStateMachine
    {
        WAITING,
        SELECTING,
        GUARDING,
        CASTING,
        MOVING,
        COMBOING,
        EVADING
    }

    [Header("On-Map")]
    // NavMesh Agent
    private RichAI richAI;

    // Animation
    public Animator animator;
    private static readonly int Speed = Animator.StringToHash("Speed");

    // Weapon Logic
    [SerializeField] private SwordDamage swordDamage;
    [SerializeField] private GameObject swordTrail;

    [Header("Combatant Canvas")]
    [SerializeField] private Transform combatantCanvas;

    [SerializeField] private TextMeshProUGUI damageTextPrefab;
    [SerializeField] private TextMeshProUGUI missTextPrefab;

    public GameObject actionTextHolder;
    public TextMeshProUGUI actionText;

    [Header("Hidden Stats")]

    // Battleboy
    public GameObject BattleboyPrefab;
    public GameObject BattleboyInstance;

    // Stored Command
    public string selectedCommand;

    // Stored Target
    public Transform selectedEnemy;

    public void SetSelectedEnemy(Transform enemyTransform)
    {
        selectedEnemy = enemyTransform;
    }

    // Animator
    public FractionalRootMotion fractionalRootMotion;

    // Selecting State
    private static readonly int IsSelecting = Animator.StringToHash("IsSelecting");
    private static readonly int IsCasting = Animator.StringToHash("IsCasting");

    // Comboing State
    [SerializeField] private float attackRange = 1.0f;
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    // Health Bar and Damage Numbers
    [Header("Health Bar and Battleboy")]
    public VitBar healthBar; // Reference to the HealthBar script
    [SerializeField] private ParticleSystem bloodParticleEffect;

    public Transform BattleboyHolder;

    // Gore
    public GameObject bloodDecalPrefab;

    public Rigidbody carcassPrefab1;
    public Rigidbody carcassPrefab2;

    // Audio
    public SoundManager UISoundManager;

    protected virtual void Awake()
    {
        // Instantiate the Battleboy prefab and store it
        BattleboyInstance = Instantiate(BattleboyPrefab, Vector2.zero, Quaternion.identity, BattleboyHolder);
    }

    // The 

    protected virtual void Start()
    {	
	// Create the BUnit in Lua
	CreateUnitInLua(name, STR, RES, AGI, DEX, VAS);
	
        currentState = CharacterStateMachine.WAITING;
        richAI = GetComponent<RichAI>();

        // Find the first enemy in the scene and set it as the default selected enemy
        //selectedEnemy = FindObjectOfType<Enemy>()?.transform;

        animator = GetComponent<Animator>();

        // Update the health bar for the first time
        if (healthBar != null)
        {
	    int currentVIT = (int)luaUnitReference.Table.Get("VIT").Number;
	    int maxVIT = (int)luaUnitReference.Table.Get("maxVIT").Number;
            healthBar.UpdateVitBar(currentVIT, maxVIT);
        }
    }

    public DynValue CreateUnitInLua(string name, int STR, int RES, int AGI, int DEX, int VAS)
    {
	// The logic for creating a unit in Lua
    	luaUnitReference = luaBackbone.luaData.DoString($"return BUnit.new('{name}', {STR}, {RES}, {AGI}, {DEX}, {VAS})");
    	return luaUnitReference;
    }

    // ---------- STATE MACHINE ----------

    protected virtual void Update()
    {
        // WAITING Block
        if (currentState == CharacterStateMachine.WAITING)
        {
            fractionalRootMotion.DisableRootMotion();
            FaceTarget();
        }
        // SELECTING Block
        else if (currentState == CharacterStateMachine.SELECTING)
        {
            animator.SetBool(IsSelecting, true);
        }
        else if (currentState == CharacterStateMachine.CASTING)
        {
            animator.SetBool(IsCasting, true);
        }
        // MOVING Block
        else if (currentState == CharacterStateMachine.MOVING)
        {
            //
        }
        // COMBO Block
        else if (currentState == CharacterStateMachine.COMBOING)
        {
            fractionalRootMotion.EnableRootMotion();
            animator.SetBool(IsAttacking, true);
        }
    }

    // ---------- GETTERS ------------------------
    public int GetCurrentVIT()
    {
	    return (int)luaUnitReference.Table.Get("VIT").Number;
    }

    public int GetAGI()
    {
	    return (int)luaUnitReference.Table.Get("AGI").Number;
    }

    public int GetACT()
    {
	    return (int)luaUnitReference.Table.Get("ACT").Number;
    }

    // ---------- IP AND ACT MANAGEMENT ----------

    public void SetIP(float value)
    {
        ipGauge = value;
    }

    public void GainIP(float amount)
    {
        ipGauge += amount;
        if (ipGauge > 100) 
        {
            ipGauge = 100;
        }
    }

    public float GetCurrentIP()
    {
        return ipGauge;
    }

    public void UpdateIPGauge(float deltaIP)
    {
        ipGauge += deltaIP;
    }

    // ---------- DAMAGE RESPONSE ----------

    public void TakeDamage(int damage, Transform source)
    {
	// Push damage to Lua 
	luaBackbone.luaData.Call(luaUnitReference.Table.Get("takeDamage"), luaUnitReference, damage);	
	// Pull back for checks
	int currentVIT = (int)luaUnitReference.Table.Get("VIT").Number;
	int maxVIT = (int)luaUnitReference.Table.Get("maxVIT").Number;
        // Update the health bar every time the character takes damage
        healthBar.UpdateVitBar(currentVIT, maxVIT);

	// Death check
        if (currentVIT <= 0)
        {
            Die();
        }

        // Start hit animation
        animator.Play("Damaged", 0, 0f); // Replace "Damaged" with the name of your hit animation
        UISoundManager.PlaySound("Damage");

        // Instantiate damage text
        ShowDamageText(damage);

        // Play blood particle effect
        PlayBloodEffect();

        // Create a blood decal on the ground
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
        Debug.DrawRay(raycastOrigin, Vector3.down * 10, Color.red, 2f);
        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit))
        {
            Quaternion decalRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            CreateBloodDecal(hit.point, decalRotation);
        }

        // Apply knockback
        RichAI victimAI = GetComponent<RichAI>();
        ApplyKnockback(source, victimAI, 2f, 0.2f);
    }

    private void CreateBloodDecal(Vector3 position, Quaternion rotation)
    {
        float radius = 1f; // Set the radius as needed
        float yOffset = 0.1f; // Set the Y offset as needed

        // Generate a random offset
        Vector3 randomOffset = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));

        // Add the random offset to the position
        position += randomOffset;

        // Add the Y offset to the position
        position.y += yOffset;

        GameObject bloodDecal = Instantiate(bloodDecalPrefab, position, Quaternion.identity);
        Destroy(bloodDecal, 10f);
    }

    public void Die()
    {
        // Instantiate carcasses
        Rigidbody carcass1 = Instantiate(carcassPrefab1, transform.position, Quaternion.identity);
        Rigidbody carcass2 = Instantiate(carcassPrefab2, transform.position, Quaternion.identity);

        // Apply force to the carcasses
        float forceAmount = 5f; // Adjust this value as needed
        Vector3 forceDirection1 = new Vector3(-1, 1, 0).normalized; // Adjust this direction as needed
        Vector3 forceDirection2 = new Vector3(1, 1, 0).normalized; // Adjust this direction as needed

        carcass1.AddForce(forceDirection1 * forceAmount, ForceMode.Impulse);
        carcass2.AddForce(forceDirection2 * forceAmount, ForceMode.Impulse);

        // Destroy the character
        Destroy(gameObject);
    }

    private void ShowDamageText(int damageAmount)
    {
        // Add randomness to the damage text position
        Vector3 randomOffset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);

        // Instantiate the damage text prefab on the CombatantCanvas
        TextMeshProUGUI damageText = Instantiate(damageTextPrefab, combatantCanvas);

        // Set the text value to the damage amount
        damageText.text = "-" + damageAmount.ToString();

        // Set the position of the damage text (this assumes that the canvas has a RectTransform)
        damageText.GetComponent<RectTransform>().anchoredPosition += new Vector2(randomOffset.x, randomOffset.y);

        // Start the coroutine to destroy the damage text after 1 second
        StartCoroutine(DestroyDamageText(damageText.gameObject, 1f));
    }

    private IEnumerator DestroyDamageText(GameObject damageText, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(damageText);
    }

    public void ShowMissText()
    {
        Vector3 randomOffset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);

        TextMeshProUGUI missText = Instantiate(missTextPrefab, combatantCanvas);

        // Set the position of the damage text (this assumes that the canvas has a RectTransform)
        missText.GetComponent<RectTransform>().anchoredPosition += new Vector2(randomOffset.x, randomOffset.y);

        StartCoroutine(DestroyDamageText(missText.gameObject, 1f));
    }

    private IEnumerator ActivateBloodEffect()
    {
        bloodParticleEffect.gameObject.SetActive(true);
        bloodParticleEffect.Play();
        yield return new WaitForSeconds(0.5f);
        bloodParticleEffect.Stop();
        bloodParticleEffect.gameObject.SetActive(false);
    }

    private void PlayBloodEffect()
    {
        StartCoroutine(ActivateBloodEffect());
    }   

    public void Heal(int healAmount)
    {
	luaBackbone.luaData.Call(luaUnitReference.Table.Get("gainVIT"), luaUnitReference, healAmount);
    }

    // ---------- AGENT MANAGEMENT ----------

    public void ResetAgent()
    {
        richAI.isStopped = true;
        animator.SetFloat(Speed, 0f);
        animator.SetBool(IsSelecting, false);
        animator.SetBool(IsAttacking, false);

        swordDamage.Damaging = false;
        swordTrail.SetActive(false);

        FaceTarget();

        currentState = CharacterStateMachine.WAITING;
    }

    private void FaceTarget()
    {
        StartCoroutine(RotateToFaceTarget(0.5f));
    }

    private IEnumerator RotateToFaceTarget(float rotationSpeed)
    {
        if (selectedEnemy != null)
        {
            Vector3 direction = (selectedEnemy.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

            float timeElapsed = 0;
            while (timeElapsed < rotationSpeed)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, timeElapsed / rotationSpeed);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = lookRotation;
        }
    }

    // ---------- COMBAT BEHAVIOR ----------

    public float GetCommandExecutionTime()
    {
        if (selectedCommand == "COMBO")
        {
            return 1.0f;
        }
        else
        {
            return 20.0f;
        }
    }
    // COMBO

    public void StartCombo()
    {
        Debug.Log("Starting Combo!");
        actionTextHolder.SetActive(true);
        actionText.text = "Combo";
        StartCoroutine(CheckDistanceAndStartCombo());
    }

    private IEnumerator CheckDistanceAndStartCombo()
    {
        // Continuously check until character is in range of enemy
        while (true)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, selectedEnemy.position);
            if (distanceToEnemy <= attackRange)
            {
                swordTrail.SetActive(true);
                currentState = CharacterStateMachine.COMBOING;
                richAI.isStopped = true;
                
                break; // If within range, break the loop
            }
            else
            {
                MoveToSelectedEnemy();
            }
            
            yield return new WaitForSeconds(0.2f); // Adjust the delay as needed
        }
    }

    public void MoveToSelectedEnemy()
    {
        if (selectedEnemy != null)
        {
            Debug.Log("Moving to Target!");
            richAI.destination = selectedEnemy.position;
            richAI.isStopped = false;
        }
    }

    public void EndCombo()
    {
        // Reset IP and Agent
        SetIP(0);
        ResetAgent();
    }

    // Sword Damage

    public bool hitRegistered;

    public void ActivateSwordDamage()
    {
        swordDamage.Damaging = true;
        hitRegistered = false;
    }

    public void DeactivateSwordDamage()
    {
        swordDamage.Damaging = false;
        if (!hitRegistered)
        {
            Debug.Log("Missed!");
            ShowMissText();
            UISoundManager.PlaySound("Miss");
            // You can do more than just logging, e.g. showing "Miss" text in the UI
        }
    }


    // Combo Knockback

    private void ApplyKnockback(Transform source, RichAI targetAI, float knockbackForce, float knockbackTime)
    {
        StartCoroutine(KnockbackCoroutine(source, targetAI, knockbackForce, knockbackTime));
    }

    private IEnumerator KnockbackCoroutine(Transform source, RichAI targetAI, float knockbackForce, float knockbackTime)
    {
        float timer = 0;
        
        Vector3 direction = (transform.position - source.position).normalized;

        Debug.Log("!RichAI Disabled!");
        targetAI.enabled = false; // Disable pathfinding at the start of knockback

        while (timer <= knockbackTime)
        {
            transform.position = transform.position + direction * knockbackForce * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null; // wait for next frame
        }
        
        Debug.Log("!RichAI Enabled!");
        targetAI.enabled = true; // Re-enable pathfinding after knockback
    }

    // EVADE

    public void MoveInEvasion(Vector3 Ghost)
    {
        Debug.Log("Evading!");
        actionTextHolder.SetActive(true);
        actionText.text = "Evade";
        richAI.destination = Ghost;
    }

}




