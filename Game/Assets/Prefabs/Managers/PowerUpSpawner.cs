using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUpDefinition
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private BoxCollider2D[] _spawnAreas;
    [SerializeField] private float _chanceToSpawn;

    public GameObject Prefab { get => prefab; set => prefab = value; }
    public BoxCollider2D[] SpawnAreas { get => _spawnAreas; set => _spawnAreas = value; }
    public float ChanceToSpawn { get => _chanceToSpawn; set => _chanceToSpawn = value; }
    public float Multiplier { get; set; } = 1.0f;
}

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private PowerUpDefinition[] _powerUps;
    [SerializeField] private float _timeBetweenTryingToSpawn;

    private float _timeSinceLastSpawnTry = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceLastSpawnTry += Time.deltaTime;

        while(_timeSinceLastSpawnTry >= _timeBetweenTryingToSpawn)
        {
            _timeSinceLastSpawnTry -= _timeBetweenTryingToSpawn;

            TrySpawnPowerUp();
        }
    }

    private void TrySpawnPowerUp()
    {
        var def = _powerUps[Random.Range(0, _powerUps.Length)];

        if (def.Multiplier * def.ChanceToSpawn >= Random.value)
        {
            var otherPowerUps = GameObject.FindGameObjectsWithTag("PowerUp");
            if (otherPowerUps.Length == 0)
            {
                def.Multiplier = 1.0f;

                var spawnArea = def.SpawnAreas[Random.Range(0, def.SpawnAreas.Length)];

                var spawnPoint = spawnArea.transform.position + (Vector3)(Random.insideUnitCircle * spawnArea.size / 2);

                var powerUp = Instantiate(def.Prefab, transform);
                powerUp.transform.position = spawnPoint;
            }
            else
            {
                def.Multiplier += 0.2f;
            }
        } else
        {
            def.Multiplier += 0.1f;
        }
    }
}
