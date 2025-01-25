using UnityEngine;

public abstract class AbstractUnit : MonoBehaviour
{
    // Unit stats
    private int health; // Health of the unit
    private int damage; // Damage dealt by the unit

    // Movement variables
    private float speed; // Speed of the unit
    private int direction = -1; // Direction of movement: 1 for positive x, -1 for negative x

    // Control type
    private bool isPlayerControlled = false; // Specifies whether the unit is player-controlled or enemy-controlled

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
      
    // Abstract method for dying (to be implemented by derived classes)
    public abstract void Die();

    // Method for moving the unit
    public void Move()
    {
        // Calculate movement based on speed and direction
        Vector3 movement = new Vector3(speed * direction * Time.deltaTime, 0, 0);
        transform.Translate(movement);

        Debug.Log($"{gameObject.name} is moving with speed {speed} and direction {direction}");
    }

    public void setPlayerControlled()
    {
        this.isPlayerControlled = true;
        this.direction = 1;
    }

    public void setSpeed(int speed) 
    { 
        this.speed = speed; 
    }

}
