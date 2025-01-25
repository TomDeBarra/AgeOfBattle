using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinUnit : AbstractUnit
{
    public int health = 25;
    public int damage = 7;
    public float speed = 15; // Speed of the unit

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public override void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject); // Destroy the unit
    }

}
