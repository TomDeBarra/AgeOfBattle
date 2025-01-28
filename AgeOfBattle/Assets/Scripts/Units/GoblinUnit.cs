using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinUnit : AbstractUnit
{

    // Start is called before the first frame update
    void Start()
    {

        this.setSpeed(6);
        this.setDamage(7);
        this.setHealth(25);
        this.setPlayerControlled(true);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public override void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject); // Destroy the unit
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
}
