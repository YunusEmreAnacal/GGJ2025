using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject sheepPrefab;
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;
    public Camera playerCamera;

    private List<Transform> availableSpawnPoints = new List<Transform>();
    private List<GameObject> spawnedSheep = new List<GameObject>();
    private List<GameObject> spawnedZombies = new List<GameObject>();

    public int maxSheepCount = 5;
    public int maxZombieCount = 5;

    void Start()
    {
        UpdateAvailableSpawnPoints();

        for (int i = 0; i < maxSheepCount; i++)
        {
            SpawnSheep();
        }

        for (int i = 0; i < maxZombieCount; i++)
        {
            SpawnZombie();
        }
    }

    void UpdateAvailableSpawnPoints()
    {
        availableSpawnPoints.Clear();

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (!IsPointVisibleFromCamera(spawnPoint.position))
            {
                availableSpawnPoints.Add(spawnPoint);
            }
        }
    }

    bool IsPointVisibleFromCamera(Vector3 point)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);//DIŞARDA
        return GeometryUtility.TestPlanesAABB(planes, new Bounds(point, Vector3.zero));
    }

    void SpawnSheep()
    {
        if (spawnedSheep.Count >= maxSheepCount || availableSpawnPoints.Count == 0) return;

        Transform spawnPoint = GetRandomAvailableSpawnPoint();
        if (spawnPoint == null) return;

        GameObject newSheep = Instantiate(sheepPrefab, spawnPoint.position, spawnPoint.rotation);
        Sheep_Data sheepHealth = newSheep.GetComponent<Sheep_Data>();

        sheepHealth.OnDeathEvent += OnSheepDeath;
        spawnedSheep.Add(newSheep);
    }

    void SpawnZombie()
    {
        if (spawnedZombies.Count >= maxZombieCount || availableSpawnPoints.Count == 0) return;

        Transform spawnPoint = GetRandomAvailableSpawnPoint();
        if (spawnPoint == null) return;

        GameObject newZombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
        Zombie_Data zombieHealth = newZombie.GetComponent<Zombie_Data>();

        zombieHealth.OnDeathEvent += OnZombieDeath;
        spawnedZombies.Add(newZombie);
    }

    Transform GetRandomAvailableSpawnPoint()
    {
        if (availableSpawnPoints.Count == 0) return null;

        int randomIndex = Random.Range(0, availableSpawnPoints.Count);
        Transform selectedPoint = availableSpawnPoints[randomIndex];
        availableSpawnPoints.RemoveAt(randomIndex);
        return selectedPoint;
    }

    void OnSheepDeath(GameObject deadSheep)
    {
        spawnedSheep.Remove(deadSheep);
        UpdateAvailableSpawnPoints(); // Görünür alan kontrolünü yeniden yap
        SpawnSheep(); // Yeni bir koyun spawn et
    }

    void OnZombieDeath(GameObject deadZombie)
    {
        spawnedZombies.Remove(deadZombie);
        UpdateAvailableSpawnPoints(); // Görünür alan kontrolünü yeniden yap
        SpawnZombie(); // Yeni bir zombi spawn et
    }

    
}
