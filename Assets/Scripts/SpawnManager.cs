using System.Threading;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject prefabToSpawn; // The prefab to spawn
    public float spawnDistance = 2f; // Distance from the obstacle where the prefab will spawn
    public LayerMask obstacleLayer;  // Layer mask to specify the "Obstacle" layer
    public int maxSpawn = 1;

    public void SpawnPrefabBehindRandomObstacle(Transform player)
    {
        // Find all obstacles in the scene
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        if (obstacles.Length == 0)
        {
            Debug.LogWarning("No obstacles found in the scene.");
            return;
        }

        // Choose a random obstacle
        GameObject randomObstacle = obstacles[Random.Range(0, obstacles.Length)];

        // Get the position of the obstacle
        Vector3 obstaclePosition = randomObstacle.transform.position;

        // Calculate the direction from the player to the obstacle
        Vector3 directionToObstacle = (obstaclePosition - player.position).normalized;

        // Calculate the far side position by extending the direction beyond the obstacle
        Vector3 farSidePosition = obstaclePosition + directionToObstacle * spawnDistance;

        // Spawn the prefab at the calculated position
        Instantiate(prefabToSpawn, farSidePosition, Quaternion.identity);
    }

    void Update()
    {

        int numPrefabs = 0;
        GameObject[] currentObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach(GameObject obj in currentObjects)
        {

            if (obj.CompareTag(prefabToSpawn.tag))
            {

                numPrefabs++;
                
            }

        }

        if (numPrefabs < maxSpawn) SpawnPrefabBehindRandomObstacle(GameObject.FindWithTag("Player").transform);

    }
}
