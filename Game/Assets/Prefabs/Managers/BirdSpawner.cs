using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BirdSpawn
{
    public GameObject Bird;

    [Range(0f, 100f)]
    public float Chance;
}

public class BirdSpawner : MonoBehaviour
{
    public BirdSpawn[] Birds;
    public BoxCollider2D[] SpawnAreas;
    public float TimeBetweenSpawns = 1.66f;

    private float _timeSinceLastSpawn = 0;
    private float _totalChance;

    void Start()
    {
        _totalChance = 0f;
        foreach (var bs in Birds)
        {
            _totalChance += bs.Chance;
        }
    }

    void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;

        while (_timeSinceLastSpawn > TimeBetweenSpawns)
        {
            var count = Random.Range(1, 3);
            for (var i = 0; i < count; i++)
            {
                SpawnBird();
            }

            _timeSinceLastSpawn -= TimeBetweenSpawns;
        }
    }

    void SpawnBird()
    {
        var randomBird = GetRandomBird();
        var randomSpawnArea = SpawnAreas[Random.Range(0, SpawnAreas.Length)];

        var spawnOffset = Random.insideUnitCircle * (randomSpawnArea.size / 2);

        var spawnPosition = randomSpawnArea.transform.position + new Vector3(spawnOffset.x, spawnOffset.y, 0f);

        var bird = Instantiate(randomBird.Bird, spawnPosition, Quaternion.identity, transform);
    }

    BirdSpawn GetRandomBird()
    {
        var rand = Random.value * _totalChance;
        foreach (var bs in Birds)
        {
            rand -= bs.Chance;
            if (rand <= 0)
            {
                return bs;
            }
        }

        return null;
    }
}
