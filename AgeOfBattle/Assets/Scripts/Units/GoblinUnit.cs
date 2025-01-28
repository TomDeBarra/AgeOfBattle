using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinUnit : AbstractUnit
{

    // Start is called before the first frame update
    void Start()
    {
        health = 25;
        speed = 15.0f;
        damage = 7;

        Move();
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

}
