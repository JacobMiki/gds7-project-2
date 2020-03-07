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
    public BirdSpawn[] DayBirds;
    public BirdSpawn[] NightBirds;

    public BoxCollider2D[] SpawnAreas;


    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameManager.gameManager;
    }

    public BirdMovement SpawnBird()
    {
        var randomBird = GetRandomBird();
        var randomSpawnArea = SpawnAreas[Random.Range(0, SpawnAreas.Length)];

        var spawnOffset = Random.insideUnitCircle * (randomSpawnArea.size / 2);

        var spawnPosition = randomSpawnArea.transform.position + new Vector3(spawnOffset.x, spawnOffset.y, 0f);

        return Instantiate(randomBird.Bird, spawnPosition, Quaternion.identity, transform).GetComponent<BirdMovement>();
    }

    BirdSpawn GetRandomBird()
    {
        var birdList = _gameManager.IsNight ? NightBirds : DayBirds;

        var totalChance = 0f;
        foreach (var bs in birdList)
        {
            totalChance += bs.Chance;
        }

        var rand = Random.value * totalChance;

        foreach (var bs in birdList)
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
