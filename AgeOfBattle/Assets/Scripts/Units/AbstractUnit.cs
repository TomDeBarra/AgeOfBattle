using System.Collections;
using UnityEngine;

public abstract class AbstractUnit : MonoBehaviour
{
    protected Animator animator;
    protected AudioSource audioSource;
    protected AudioClip attackSound;
    // Unit stats
    protected int health; // Health of the unit
    protected int damage; // Damage dealt by the unit

    // Movement variables
    protected float speed; // Speed of the unit
    protected int direction = 1; // Direction of movement: 1 for positive x, -1 for negative x

    // Control type

    private bool isPlayerControlled = false; // Specifies whether the unit is player-controlled or enemy-controlled
    private int attackTime = 2; // Time delay between attacks
    private bool isAttacking = false; // Prevents multiple attacks at once
    private bool isMoving = true;


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
        if (isMoving && !isAttacking)
        {
            Vector3 movement = new Vector3(speed * direction * Time.deltaTime, 0, 0);
            transform.Translate(movement);
            //   Debug.Log($"{gameObject.name} is moving with speed {speed} and direction {direction}");
            if (animator != null)
            {
                animator.SetBool("isRunning", true); // Play run animation
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isRunning", false); // Stop run animation
            }
            Debug.Log($"{gameObject.name} is stopped and waiting.");
        }

    }

    public void setPlayerControlled(bool boolean)
    {

        this.isPlayerControlled = boolean;
     //   if (isPlayerControlled)
     //       this.direction = 1;
     //   else
     //       this.direction = -1;

    }

    public void setSpeed(int speed) 
    { 
        this.speed = speed; 
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
    }

    public void setHealth (int health)
    {
        this.health = health;
    }

    public void setAttackTime(int attackTime)
    {
        this.attackTime = attackTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        AbstractUnit otherUnit = other.GetComponent<AbstractUnit>();
        
        if (otherUnit != null)
        {
            print("Ouch");

            if (otherUnit.isPlayerControlled == this.isPlayerControlled)
            {
                Debug.Log($"{gameObject.name} is waiting due to collision with ally: {otherUnit.gameObject.name}");
                isMoving = false; // Stop movement
            }
            else
            {
                Debug.Log($"{gameObject.name} is attacking enemy: {otherUnit.gameObject.name}");

                StartCoroutine(AttackRoutine(otherUnit));

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

    // Attack method with delay
    private IEnumerator AttackRoutine(AbstractUnit target)
    {
        isAttacking = true;
        isMoving = false; // Stop movement during attack
        
        while (target != null && target.health > 0)
        {
            Debug.Log($"{gameObject.name} is attacking {target.gameObject.name}!");
            target.TakeDamage(damage); // Inflict damage on the target
            PlayAttackAnimationAndSound(); // Play attack animation and sound

            // Example of returning to idle animation if not attacking
            // Animator animator = GetComponent<Animator>();
            // if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            // {
            //     animator.Play("Idle");
            // }

            yield return new WaitForSeconds(attackTime); // Wait before next attack
        }
        
        isAttacking = false;
        isMoving = true; // Resume movement after combat
    }


}

