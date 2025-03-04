using System.Collections;
using UnityEngine;

public class AICaveman : MonoBehaviour
{
    public GameObject goblinPrefab; // Goblin unit
    public GameObject batteringRamPrefab; // Battering Ram unit
    public GameObject giantPrefab; // Battering Ram unit

    public float goblinSpawnTime = 10f; // Spawn time for goblins
    public float batteringRamSpawnTime = 15f; // Default spawn time for Battering Rams
    public float giantSpawnTime = 25f; // Default spawn time for Battering Rams

    public Transform spawnPoint;

    void Start()
    {
        // Start spawning both unit types on separate timers
        StartCoroutine(spawnGoblin());
        StartCoroutine(spawnBatteringRam());
        StartCoroutine(spawnGiant());
    }

    private IEnumerator spawnGoblin()
    {
        while (true)
        {
            yield return new WaitForSeconds(goblinSpawnTime); // Wait before spawning

            if (goblinPrefab != null)
            {
                GameObject newUnit = Instantiate(goblinPrefab, spawnPoint.position, Quaternion.identity);
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

    private IEnumerator spawnBatteringRam()
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

    private IEnumerator spawnGiant()
    {
        while (true)
        {
            yield return new WaitForSeconds(giantSpawnTime); // Wait before spawning

            if (giantPrefab != null)
            {
                GameObject newUnit = Instantiate(giantPrefab, spawnPoint.position, Quaternion.identity);
                AbstractUnit unitScript = newUnit.GetComponent<AbstractUnit>();
                if (unitScript != null)
                {
                    unitScript.setPlayerControlled(false); // Mark as enemy unit
                    Debug.Log("Spawned AI Giant!");
                }
                else
                {
                    Debug.LogError("Spawned Giant does not have a GiantUnit script!");
                }
            }
            else
            {
                Debug.LogError("Giant prefab is not assigned!");
            }
        }
    }
}


