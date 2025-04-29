using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

        LoadAudio("Assets/Prefabs/Units/Unit Sounds/GiantAttack.mp3");
        LoadAudio("Assets/Prefabs/Units/Unit Sounds/GiantDeath.mp3");
        LoadAudio("Assets/Prefabs/Units/Unit Sounds/GiantMoving.mp3");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        checkForFriendlyUnitCollisionAhead();
        PlayMovingSound();
    }

    protected void LoadAudio(string address)
    {
        Addressables.LoadAssetAsync<AudioClip>(address).Completed += (AsyncOperationHandle<AudioClip> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (address.Contains("GiantAttack"))
                    attackSound = handle.Result;
                else if (address.Contains("GiantDeath"))
                    deathSound = handle.Result;
                else if (address.Contains("GiantMoving"))
                    movingSound = handle.Result; // Load moving sound dynamically
            }
            else
            {
                Debug.LogError($"Failed to load audio: {address}");
            }
        };
    }

    public void PlayMovingSound()
    {
        if (movingSound == null || audioSource == null) return;

        if (isMoving && !isAttacking)
        {
            if (!audioSource.isPlaying) // Ensure it only plays if nothing is currently playing
            {
                audioSource.PlayOneShot(movingSound);
            }
        }
    }

    public override void Die()
    {
        PlayDieAnimationAndSound();
        Debug.Log($"{gameObject.name} has died!");
        StartCoroutine(DestroyAfterDelay(1f)); // Destroy after 1 second
        this.isMoving = false;
        this.isDead = true;
    }

    override public void PlayDieAnimationAndSound()
    {
     
        Debug.Log($"{gameObject.name} has died!");
        if (animator != null)
        {
            animator.Play("Giant@Damage01"); // Play death animation
        }
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
        StartCoroutine(DestroyAfterDelay(2.5f)); // Destroy after 2 second

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
