using UnityEngine;

public class GoblinDeath : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator not found on " + gameObject.name);
            return;
        }

        // Play the Goblin Death animation
        PlayDeathAnimation();
    }

    private void PlayDeathAnimation()
    {
        if (animator.HasState(0, Animator.StringToHash("Rig Gob 1_Death")))
        {
            animator.Play("Rig Gob 1_Death");
            Debug.Log("Goblin death animation played on " + gameObject.name);
        }
        else
        {
            Debug.LogError("Rig Gob 1_Death animation state not found in Animator on " + gameObject.name);
        }
    }
}
