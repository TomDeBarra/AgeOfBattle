using UnityEngine;

public class BaseDetector : MonoBehaviour
{
    private BaseHealthManager baseHealthManager;

    private void Start()
    {
        baseHealthManager = GetComponent<BaseHealthManager>(); // Get BaseHealthManager
        if (baseHealthManager == null)
        {
            Debug.LogError("BaseHealthManager not found on " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AbstractUnit unit = other.GetComponent<AbstractUnit>();

        if (unit != null)
        {
            // Only allow enemy units to damage the player base, and vice versa
            if ((baseHealthManager.isPlayer && !unit.getIsPlayerControlled()) || // Enemy attacks player base
                (!baseHealthManager.isPlayer && unit.getIsPlayerControlled()))  // Player attacks enemy base
            {
                int damageToBase = unit.getUnitWorth();
                baseHealthManager.TakeDamage(damageToBase);

                Debug.Log($"{unit.gameObject.name} reached the base! Dealing {damageToBase} damage.");
                Destroy(unit.gameObject); // Remove unit after reaching the base
            }
        }
    }
}

