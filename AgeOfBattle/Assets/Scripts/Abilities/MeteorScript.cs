using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : MonoBehaviour
{
    public float fallSpeed = 10f; // Speed at which meteors fall

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(4f));
    }

    private void Update()
    {
        // Move the meteor downward
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has an AbstractUnit script
        AbstractUnit unit = other.GetComponent<AbstractUnit>();
        if (unit != null && !unit.getIsPlayerControlled()) // Only affect player-controlled units
        {
            unit.Die(); // Trigger death function
        }

        // Destroy meteor upon impact
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}

