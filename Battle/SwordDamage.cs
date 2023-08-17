using UnityEngine;

// **** Backbone of collision-based damage system

public class SwordDamage : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 10;

    public BUnit wielder; // Wielder of the weapon

    private Collider swordCollider;

    private bool _damaging;
    public bool Damaging
    {
        get => _damaging;
        set
        {
            _damaging = value;
            swordCollider.enabled = value;
        }
    }

    // Velocity Check
    [SerializeField]
    private float minRequiredVelocity = 1.0f;
    private Vector3 _previousPosition;
    private Vector3 _currentPosition;

    private void Start()
    {
        swordCollider = GetComponent<Collider>();
        swordCollider.enabled = false; // Disable the collider initially
    }

    private void Update()
    {
        _previousPosition = _currentPosition;
        _currentPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Damaging)
            return;

        // Ignore collision with the wielder
        if (other.gameObject == wielder.gameObject)
            return;

        // Check if the collided object has the Character script component
        BUnit character = other.GetComponent<BUnit>();

        // Apply damage to the character
        if (character != null)
        {
            Debug.Log("Character takes damage:" + damageAmount);
            // Pass the wielder's transform as the source of the damage
            character.TakeDamage(damageAmount, wielder.transform);
            // Register hit within wielder Character
            wielder.hitRegistered = true;
        }
    }
}
