using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public abstract class AbstractUnit : MonoBehaviour
{
    protected Animator animator;
    protected AudioSource audioSource;
    protected AudioClip attackSound;
    protected AudioClip deathSound;
    protected int health;
    protected int maxHealth;
    protected int damage;
    protected float speed;
    protected int unitWorth; // How much damage the unit does if it reaches the enemy base.
    protected int direction = 1;
    protected bool isPlayerControlled = true;
    protected int attackTime = 2;
    protected bool isAttacking = false;
    protected bool isMoving = true;

    private void Start()
    {
        
    }

    private void Update()
    {   
        
    }

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;
        Debug.Log($"{gameObject.name} took {damageTaken} damage! Remaining health: {health}");

        GoblinHealthBar healthBar = GetComponentInChildren<GoblinHealthBar>();
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar();
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public abstract void Die();
    public abstract void PlayAttackAnimationAndSound();

    protected void checkForFriendlyUnitCollisionAhead()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f); // Adjust radius if needed
        bool friendlyUnitAhead = false;

        foreach (var hitCollider in hitColliders)
        {
            AbstractUnit otherUnit = hitCollider.GetComponent<AbstractUnit>();
            if (otherUnit != null && otherUnit != this && otherUnit.isPlayerControlled == this.isPlayerControlled) // Ensure it's an AbstractUnit, not itself & friendly
            {
                if ((otherUnit.transform.position.x > transform.position.x && isPlayerControlled) ||
                    (otherUnit.transform.position.x < transform.position.x && !isPlayerControlled))
                {
                    friendlyUnitAhead = true;
                    break; // Exit loop if we find a valid friendly unit ahead
                }
            }
        }

        if (!friendlyUnitAhead && !isAttacking)
        {
            isMoving = true;
            if (animator != null)
            {
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            isMoving = false;
            if (animator != null)
            {
                animator.SetBool("isRunning", false);
            }
        }
    }

    public void Move()
    {
        if (isMoving && !isAttacking)
        {
            Vector3 movement = new Vector3(speed * direction * Time.deltaTime, 0, 0);
            transform.position += movement;
            // transform.Translate(movement);
            if (animator != null)
            {
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isRunning", false);
            }
        }
    }

    public void setPlayerControlled(bool boolean)
    {
        this.isPlayerControlled = boolean;
        if (isPlayerControlled)
            this.direction = 1;
        else
            this.direction = -1;
    }

    public void setDirection(int direction)
    {
        this.direction = direction;
    }


    public void setSpeed(int speed) { this.speed = speed; }
    public void setDamage(int damage) { this.damage = damage; }
    public void setHealth(int health) { this.health = health; }
    public void setMaxHealth(int maxHealth) { this.maxHealth = maxHealth; }
    public void setAttackTime(int attackTime) { this.attackTime = attackTime; }
    public void setUnitWorth(int unitWorth) { this.unitWorth = unitWorth; }

    public int getHealth() { return this.health; }
    public int getUnitWorth() { return this.unitWorth; }
    public int getMaxHealth() { return this.maxHealth; }
    public bool getIsPlayerControlled() { return this.isPlayerControlled; }
    private void OnTriggerEnter(Collider other)
    {
        AbstractUnit otherUnit = other.GetComponent<AbstractUnit>();

        if (otherUnit != null)
        {
            float distanceToOther = Mathf.Abs(transform.position.x - other.transform.position.x);
            float minSeparation = 2f; // Adjust spacing if needed

            if (otherUnit.isPlayerControlled == this.isPlayerControlled)
            {
                if (distanceToOther < 1f)
                {
                    Debug.Log($"{gameObject.name} is continuing movement despite friendly unit inside.");
                    isMoving = true; // Continue moving if the friendly unit is INSIDE
                }
                else
                {
                    if (other.transform.position.x < transform.position.x && isPlayerControlled || other.transform.position.x > transform.position.x && !isPlayerControlled) // Friendly unit is behind
                    {
                        Debug.Log($"{gameObject.name} is continuing movement despite friendly unit behind.");
                        isMoving = true; // Continue moving if the friendly unit is BEHIND
                    }
                    else // Friendly unit is ahead
                    {
                        if (distanceToOther < minSeparation)
                        {
                            Debug.Log($"{gameObject.name} is stopping to maintain spacing.");
                            isMoving = false;
                            if (animator != null)
                            {
                                animator.SetBool("isRunning", false);
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log($"{gameObject.name} is attacking enemy: {otherUnit.gameObject.name}");
                StartCoroutine(AttackRoutine(otherUnit));
                isMoving = false;
                if (animator != null)
                {
                    animator.SetBool("isRunning", false);
                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        AbstractUnit otherUnit = other.GetComponent<AbstractUnit>();
        if (otherUnit != null)
        {
            isMoving = true;
            if (animator != null)
            {
                animator.SetBool("isRunning", true);
            }
        }
    }

    private IEnumerator AttackRoutine(AbstractUnit target)
    {
        isAttacking = true;
        isMoving = false;
        while (target != null && target.health > 0)
        {
            target.TakeDamage(damage);
            PlayAttackAnimationAndSound();
            yield return new WaitForSeconds(attackTime);
        }
        isAttacking = false;
        isMoving = true;
    }
}
