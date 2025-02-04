using UnityEngine;

public class BaseHealthManager : MonoBehaviour
{
    public int maxBaseHealth = 100; // Set max health in the Inspector
    private int currentBaseHealth;

    [SerializeField] public bool isPlayer = true; // Set in Inspector: true = player base, false = enemy base

    private void Start()
    {
        currentBaseHealth = maxBaseHealth; // Initialize base health
    }

    public void TakeDamage(int damage)
    {
        currentBaseHealth -= damage;

        if (isPlayer)
        {
            Debug.Log($"Player Base took {damage} damage! Remaining health: {currentBaseHealth}");
        }
        else
        {
            Debug.Log($"Enemy Base took {damage} damage! Remaining health: {currentBaseHealth}");
        }

        if (currentBaseHealth <= 0)
        {
            BaseDestroyed();
        }
    }

    private void BaseDestroyed()
    {
        if (isPlayer)
        {
            Debug.Log("PLAYER BASE DESTROYED! GAME OVER.");
            // Trigger game over sequence for player
        }
        else
        {
            Debug.Log("ENEMY BASE DESTROYED! YOU WIN!");
            // Trigger victory sequence for player
        }
    }

    public bool getIsPlayer() { return isPlayer; }
}

