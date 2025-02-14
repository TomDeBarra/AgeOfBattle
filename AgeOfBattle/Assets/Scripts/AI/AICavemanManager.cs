using System.Collections;
using UnityEngine;

public class AICaveman : MonoBehaviour
{
    public GameObject unitOnePrefab; // Goblin unit
    public GameObject batteringRamPrefab; // Battering Ram unit

    public float unitOneSpawnTime = 10f; // Spawn time for goblins
    public float batteringRamSpawnTime = 15f; // Default spawn time for Battering Rams

    public Transform spawnPoint;

    void Start()
    {
        // Start spawning both unit types on separate timers
        StartCoroutine(SpawnUnitOne());
        StartCoroutine(SpawnBatteringRam());
    }

    private IEnumerator SpawnUnitOne()
    {
        while (true)
        {
            yield return new WaitForSeconds(unitOneSpawnTime); // Wait before spawning

            if (unitOnePrefab != null)
            {
                GameObject newUnit = Instantiate(unitOnePrefab, spawnPoint.position, Quaternion.identity);
                AbstractUnit unitScript = newUnit.GetComponent<AbstractUnit>();
                if (unitScript != null)
                {
                    unitScript.setPlayerControlled(false); // Mark as enemy unit
                    Debug.Log("Spawned AI Goblin unit!");
                }
                else
                {
                    Debug.LogError("Spawned Goblin unit does not have an AbstractUnit script!");
                }
            }
            else
            {
                Debug.LogError("Unit One (Goblin) prefab is not assigned!");
            }
        }
    }

    private IEnumerator SpawnBatteringRam()
    {
        while (true)
        {
            yield return new WaitForSeconds(batteringRamSpawnTime); // Wait before spawning

            if (batteringRamPrefab != null)
            {
                GameObject newUnit = Instantiate(batteringRamPrefab, spawnPoint.position, Quaternion.identity);
                AbstractUnit unitScript = newUnit.GetComponent<AbstractUnit>();
                if (unitScript != null)
                {
                    unitScript.setPlayerControlled(false); // Mark as enemy unit
                    Debug.Log("Spawned AI Battering Ram!");
                }
                else
                {
                    Debug.LogError("Spawned Battering Ram does not have an AbstractUnit script!");
                }
            }
            else
            {
                Debug.LogError("Battering Ram prefab is not assigned!");
            }
        }
    }
}


