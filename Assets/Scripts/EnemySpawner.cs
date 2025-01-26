using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeToWait;
        public List<GameObject> enemies;
    }
    
    [Header("Wave Configuration")]
    public List<Wave> waves;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private float waveTimer;
    
    [Header("Spawn Area Configuration")]
    [SerializeField] private float innerSquareSize = 10f;    // Size of the inner square boundary
    [SerializeField] private float outerSquareSize = 20f;    // Size of the outer square boundary
    [SerializeField] private float minPlayerDistance = 5f;   // Minimum distance from player
    [SerializeField] private Transform playerTransform;      // Reference to the player
    
    [Header("Debug Visualization")]
    [SerializeField] private bool showSpawnArea = true;      // Toggle for debug visualization
    
    private int currentWave = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (playerTransform == null)
            {
                Debug.LogError("Player not found! Please assign player reference or ensure player has 'Player' tag.");
            }
        }
        
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(waveTimer);
        
        currentWave++;
        SetWaveText();

        if (currentWave <= waves.Count)
        {
            Wave currentWaveData = waves[currentWave - 1];
            
            // Spawn enemies for the current wave
            for (int i = 0; i < currentWaveData.enemyCount; i++)
            {
                // Get a random enemy prefab from the wave's enemy list
                GameObject enemyPrefab = currentWaveData.enemies[UnityEngine.Random.Range(0, currentWaveData.enemies.Count)];
                
                // Try to spawn the enemy in a valid position
                Vector3 spawnPosition = GetValidSpawnPosition();
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                activeEnemies.Add(enemy);
                
                // Add small delay between spawns to prevent enemies from stacking
                yield return new WaitForSeconds(0.2f);
            }
            
            // Wait for specified time before starting next wave
            yield return new WaitForSeconds(currentWaveData.timeToWait);
            
            // Start next wave if there are more waves
            if (currentWave < waves.Count)
            {
                StartCoroutine(StartNextWave());
            }
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPos;
        int maxAttempts = 30;  // Prevent infinite loops
        int attempts = 0;
        
        do
        {
            // Generate random position between inner and outer squares
            float x = UnityEngine.Random.Range(-outerSquareSize, outerSquareSize);
            float z = UnityEngine.Random.Range(-outerSquareSize, outerSquareSize);
            
            // If position is within inner square, modify it to be outside
            if (Mathf.Abs(x) < innerSquareSize)
            {
                x = x > 0 ? innerSquareSize : -innerSquareSize;
            }
            if (Mathf.Abs(z) < innerSquareSize)
            {
                z = z > 0 ? innerSquareSize : -innerSquareSize;
            }
            
            spawnPos = new Vector3(x, 0, z) + transform.position;
            attempts++;
            
            // Check if position is far enough from player
            if (playerTransform != null && 
                Vector3.Distance(spawnPos, playerTransform.position) >= minPlayerDistance)
            {
                // Check if position is on NavMesh
                UnityEngine.AI.NavMeshHit hit;
                if (UnityEngine.AI.NavMesh.SamplePosition(spawnPos, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
        } while (attempts < maxAttempts);
        
        // Fallback position if no valid position found
        Debug.LogWarning("Could not find valid spawn position, using fallback position");
        return transform.position + new Vector3(outerSquareSize, 0, outerSquareSize);
    }

    private void SetWaveText()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
        }
    }
    
    // Visualization for debugging spawn area
    private void OnDrawGizmos()
    {
        if (!showSpawnArea) return;
        
        // Draw outer square
        Gizmos.color = Color.red;
        Vector3 center = transform.position;
        Vector3 size = new Vector3(outerSquareSize * 2, 0.1f, outerSquareSize * 2);
        Gizmos.DrawWireCube(center, size);
        
        // Draw inner square
        Gizmos.color = Color.yellow;
        Vector3 innerSize = new Vector3(innerSquareSize * 2, 0.1f, innerSquareSize * 2);
        Gizmos.DrawWireCube(center, innerSize);
        
        // Draw player minimum distance if player reference exists
        if (playerTransform != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(playerTransform.position, minPlayerDistance);
        }
    }
    
    // Optional: Method to cleanup enemies when needed
    public void CleanupEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        activeEnemies.Clear();
    }
}