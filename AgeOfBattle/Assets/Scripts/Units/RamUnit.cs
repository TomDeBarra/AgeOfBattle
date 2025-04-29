using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
        this.setAttackTime(1); // Longer attack time

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        LoadAudio("Assets/Prefabs/Units/Unit Sounds/RamAttack.mp3");
        LoadAudio("Assets/Prefabs/Units/Unit Sounds/RamDeath.mp3");
        LoadAudio("Assets/Prefabs/Units/Unit Sounds/RamMoving.mp3");
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
                if (address.Contains("RamAttack"))
                    attackSound = handle.Result;
                else if (address.Contains("RamDeath"))
                    deathSound = handle.Result;
                else if (address.Contains("RamMoving"))
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
        Debug.Log($"{gameObject.name} (Battering Ram) has been destroyed!");   
        StartCoroutine(DestroyAfterDelay(2f)); // Slightly longer destruction delay
        this.isMoving = false;
        this.isDead = true;
    }

    override public void PlayDieAnimationAndSound()
    {
        if (animator != null)
        {
            animator.Play("BatteringRam_Death"); // Play death animation
        }
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }

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
