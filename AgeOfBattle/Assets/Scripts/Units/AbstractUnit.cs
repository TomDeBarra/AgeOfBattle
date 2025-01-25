using UnityEngine;

public abstract class AbstractUnit : MonoBehaviour
{
    // Unit stats
    protected int health; // Health of the unit
    protected int damage; // Damage dealt by the unit

    // Movement variables
    protected float speed; // Speed of the unit
    protected int direction = 1; // Direction of movement: 1 for positive x, -1 for negative x

    // Control type
    protected bool isPlayerControlled = false; // Specifies whether the unit is player-controlled or enemy-controlled

    // Movement state
    private bool isMoving = true; // Determines if the unit is moving

    // Method for taking damage
    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;
        Debug.Log($"{gameObject.name} took {damageTaken} damage! Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }
      
    // Abstract methods for dying and attacking (to be implemented by derived classes)
    public abstract void Die();
    public abstract void PlayAttackAnimationAndSound();

    // Method for moving the unit
    public void Move()
    {
        // Calculate movement based on speed and direction
        Vector3 movement = new Vector3(speed * direction * Time.deltaTime, 0, 0);
        transform.Translate(movement);

        Debug.Log($"{gameObject.name} is moving with speed {speed} and direction {direction}");
    }

    public void setPlayerControlled()
    {
        this.isPlayerControlled = true;
        this.direction = -1;
    }

    public void setSpeed(int speed) 
    { 
        this.speed = speed; 
    }

    public void setHealth(int health) {
        this.health = health;
    }

    public void setSpeed(float speed) {
        this.speed = health;
    }

    // Collision detection
    // Collision detection
    private void OnTriggerEnter(Collider other)
    {
        AbstractUnit otherUnit = other.GetComponent<AbstractUnit>();

        if (otherUnit != null)
        {
            if (otherUnit.isPlayerControlled == this.isPlayerControlled)
            {
                Debug.Log($"{gameObject.name} is waiting due to collision with ally: {otherUnit.gameObject.name}");
                isMoving = false; // Stop movement
            }
            else
            {
                Debug.Log($"{gameObject.name} is attacking enemy: {otherUnit.gameObject.name}");
                Attack(otherUnit);
                isMoving = false; // Stop movement during attack
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AbstractUnit otherUnit = other.GetComponent<AbstractUnit>();

        if (otherUnit != null)
        {
            Debug.Log($"{gameObject.name} has resumed movement after collision with: {otherUnit.gameObject.name}");
            isMoving = true; // Resume movement
        }
    }

    // Attack method
    private void Attack(AbstractUnit target)
    {
        if (target != null)
        {
            target.TakeDamage(damage); // Inflict damage on the target

            // Play attack animation and sound
            PlayAttackAnimationAndSound();

            // Example (commented out):
            // Animator animator = GetComponent<Animator>();
            // animator.Play("AttackAnimation");
            // AudioSource audioSource = GetComponent<AudioSource>();
            // audioSource.PlayOneShot(attackSound);
            // animator.Play("IdleAnimation");
        }
    }
}

