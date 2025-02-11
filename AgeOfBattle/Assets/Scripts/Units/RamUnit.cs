using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteringRamUnit : AbstractUnit
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        this.setSpeed(4); // Adjust speed for a battering ram
        this.setDamage(15); // Higher damage for ramming attacks
        this.setHealth(30); // Increased health
        this.setMaxHealth(30);
        this.setUnitWorth(3); // Worth more due to power
        this.setAttackTime(2); // Longer attack time

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
        Debug.Log($"{gameObject.name} (Battering Ram) has been destroyed!");
        if (animator != null)
        {
            animator.Play("BatteringRam_Death"); // Play death animation
        }
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
        StartCoroutine(DestroyAfterDelay(2f)); // Slightly longer destruction delay

        if (audioSource != null && deathSound != null)
        {
            Debug.Log($"{gameObject.name} is playing destruction sound.");
            audioSource.PlayOneShot(deathSound); // Play destruction sound
        }
        else if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on " + gameObject.name);
        }
        else if (deathSound == null)
        {
            Debug.LogError("Destruction sound not assigned in the Inspector.");
        }
    }

    override public void PlayAttackAnimationAndSound()
    {
        if (animator != null)
        {
            Debug.Log($"{gameObject.name} (Battering Ram) is ramming!");
            animator.SetBool("isAttacking", true); // Trigger ramming attack animation
        }
        else
        {
            Debug.LogError("Animator not found on " + gameObject.name);
        }

        if (audioSource != null && attackSound != null)
        {
            Debug.Log($"{gameObject.name} is playing ramming sound.");
            audioSource.PlayOneShot(attackSound); // Play ramming attack sound
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
        yield return new WaitForSeconds(1.5f); // Slightly longer delay for attack animation
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
