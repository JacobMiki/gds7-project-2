using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject[] Birds;
    public BoxCollider2D[] SpawnAreas;
    public float TimeBetweenSpawns = 1.66f;
    public float SpeedUpFactor = 1.2f;
    public float TimeBetweenSpeedUps = 10f;

    private float _timeSinceLastSpawn = 0;
    private float _timeSinceLastSpeedUp = 0;
    private float _speedMultiplier = 1;
    private int _speedUpCount = 0;

    void Start()
    {

    }

    void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        _timeSinceLastSpeedUp += Time.deltaTime;

        while (_timeSinceLastSpeedUp > TimeBetweenSpeedUps)
        {
            _speedUpCount++;

            if (_speedUpCount % 2 == 0)
            {
                TimeBetweenSpawns /= SpeedUpFactor;
            }
            else
            {
                _speedMultiplier *= SpeedUpFactor;
            }

            _timeSinceLastSpeedUp -= TimeBetweenSpeedUps;
        }

        while (_timeSinceLastSpawn > TimeBetweenSpawns)
        {
            var randomBird = Birds[Random.Range(0, Birds.Length)];
            var randomSpawnArea = SpawnAreas[Random.Range(0, SpawnAreas.Length)];

            var spawnOffset = Random.insideUnitCircle * (randomSpawnArea.size / 2);

            var spawnPosition = randomSpawnArea.transform.position + new Vector3(spawnOffset.x, spawnOffset.y, 0f);

            var bird = Instantiate(randomBird, spawnPosition, Quaternion.identity, this.transform);
            bird.GetComponent<FlyBirdFly>().Speed *= _speedMultiplier;

            _timeSinceLastSpawn -= TimeBetweenSpawns;
        }
    }
}
