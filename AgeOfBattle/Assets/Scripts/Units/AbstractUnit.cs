using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public abstract class AbstractUnit : MonoBehaviour
{
    protected Animator animator;
    protected AudioSource audioSource;
    protected AudioClip attackSound;
    protected int health;
    protected int damage;
    protected float speed;
    protected int direction = 1;
    private bool isPlayerControlled = true;
    private int attackTime = 2;
    private bool isAttacking = false;
    private bool isMoving = true;

    private static List<AbstractUnit> unitQueue = new List<AbstractUnit>();

    private void Awake()
    {
        unitQueue.Add(this); // Add unit to queue when spawned
    }

    private void Start()
    {
        InvokeRepeating("CheckSpawnClogging", 1f, 1f); // Check every second
    }

    private void CheckSpawnClogging()
    {
        if (unitQueue.Count > 1)
        {
            for (int i = 0; i < unitQueue.Count; i++)
            {
                if (i == 0)
                {
                    unitQueue[i].isMoving = true; // First spawned unit has priority
                }
                else
                {
                    float distanceToPrevious = Mathf.Abs(unitQueue[i].transform.position.x - unitQueue[i - 1].transform.position.x);
                    if (distanceToPrevious > 1.5f)
                    {
                        unitQueue[i].isMoving = true;
                    }
                }
            }
        }
    }

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;
        Debug.Log($"{gameObject.name} took {damageTaken} damage! Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    public abstract void Die();
    public abstract void PlayAttackAnimationAndSound();

    public void Move()
    {
        if (isMoving && !isAttacking)
        {
            Vector3 movement = new Vector3(speed * direction * Time.deltaTime, 0, 0);
            transform.Translate(movement);
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
    public void setAttackTime(int attackTime) { this.attackTime = attackTime; }

    private void OnTriggerEnter(Collider other)
    {
        AbstractUnit otherUnit = other.GetComponent<AbstractUnit>();

        if (otherUnit != null)
        {
            float distanceToOther = Mathf.Abs(transform.position.x - other.transform.position.x);
            float minSeparation = 2f;

            if (otherUnit.isPlayerControlled == this.isPlayerControlled)
            {
                if (other.transform.position.x < transform.position.x && distanceToOther > minSeparation)
                {
                    isMoving = true;
                    if (animator != null)
                    {
                        animator.SetBool("isRunning", true);
                        animator.SetBool("isIdle", false);
                    }
                }
                else
                {
                    isMoving = false;
                    if (animator != null)
                    {
                        animator.SetBool("isRunning", false);
                        animator.SetBool("isIdle", true);
                    }
                }
            }
            else
            {
                StartCoroutine(AttackRoutine(otherUnit));
                isMoving = false;
                if (animator != null)
                {
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isIdle", true);
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
                animator.SetBool("isIdle", false);
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
