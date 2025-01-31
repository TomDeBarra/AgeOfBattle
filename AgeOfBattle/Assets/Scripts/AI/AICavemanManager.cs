using UnityEngine;
using System.Collections;

public class AICavemanManager : MonoBehaviour
{
    public GameObject enemyGoblinPrefab; // Assign in Inspector or dynamically
    public Transform spawnPoint; // Where the goblin will spawn
    public int unitOneSpawnTime = 4; // Time in seconds between spawns

    void Start()
    {
        StartCoroutine(SpawnEnemyGoblin());
    }

    private IEnumerator SpawnEnemyGoblin()
    {
        while (true) // Continuously spawn goblins
        {
            yield return new WaitForSeconds(unitOneSpawnTime);

            if (enemyGoblinPrefab != null)
            {
                GameObject spawnedGoblin = Instantiate(enemyGoblinPrefab, spawnPoint.position, Quaternion.identity);
                GoblinUnit goblinUnit = spawnedGoblin.GetComponent<GoblinUnit>();
                if (goblinUnit != null)
                {
                    goblinUnit.setPlayerControlled(false); // Set as enemy unit
                    goblinUnit.setDirection(-1); // Make sure enemies move LEFT
                }
                Debug.Log("Spawned an EnemyGoblin at " + spawnPoint.position);
            }
            else
            {
                Debug.LogError("EnemyGoblin prefab is not assigned!");
            }
        }
    }
}
