using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantUnit : AbstractUnit
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        this.setSpeed(2);
        this.setDamage(40);
        this.setHealth(100);
        this.setMaxHealth(100);
        this.setUnitWorth(10);
        this.setAttackTime(4);

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        checkForFriendlyUnitCollisionAhead();
    }

    public override void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        if (animator != null)
        {
            animator.Play("RigGob1_Death"); // Play death animation
        }
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
        StartCoroutine(DestroyAfterDelay(1f)); // Destroy after 1 second

        if (audioSource != null && attackSound != null)
        {
            Debug.Log($"{gameObject.name} is playing attack sound.");
            audioSource.PlayOneShot(deathSound); // Play attack sound
        }
        else if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on " + gameObject.name);
        }
        else if (deathSound == null)
        {
            Debug.LogError("Death sound not assigned in the Inspector.");
        }
    }

    override public void PlayAttackAnimationAndSound()
    {
        if (animator != null)
        {
            Debug.Log($"{gameObject.name} is playing attack animation.");
            animator.SetBool("isAttacking", true); // Trigger attack animation
        }
        else
        {
            Debug.LogError("Animator not found on " + gameObject.name);
        }

        if (audioSource != null && attackSound != null)
        {
            Debug.Log($"{gameObject.name} is playing attack sound.");
            audioSource.PlayOneShot(attackSound); // Play attack sound
        }
        else if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on " + gameObject.name);
        }
        else if (attackSound == null)
        {
            Debug.LogError("Attack sound not assigned in the Inspector.");
        }

        // Return to idle animation after attack (using Coroutine for delay)
        StartCoroutine(ReturnToIdle());
    }

    private System.Collections.IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(1.0f); // Adjust delay based on attack animation length
        if (animator != null)
        {
            Debug.Log($"{gameObject.name} returning to idle animation.");
            animator.SetBool("isAttacking", false); // Return to idle animation
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
