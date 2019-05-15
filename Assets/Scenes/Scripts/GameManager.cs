using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] PlayerController playerPrefab;
    [SerializeField] Transform playerSpawnPoint;
    [Header("Enemy")]
    [SerializeField] EnemyController enemyPrefab;
    [SerializeField] EnemySpawnPoint[] EnemySpawnPoints;
    [Header("Wave modifiers")]
    [SerializeField] private int initialWaveAmount;
    [SerializeField] private int waveAmountMultiplier;
    [SerializeField] private float timeBetweenWaves;

    private int waveCounter = 1;
    private int actualWaveAmount;
    private int remainingWaveAmount;
    private bool waveInProgress;

                
    // Start is called before the first frame update
    void Start()
    {
        actualWaveAmount = initialWaveAmount;
        remainingWaveAmount = actualWaveAmount;
        Instantiate(playerPrefab, playerSpawnPoint.position, playerPrefab.transform.rotation);

        StartWave();
    }

    // Update is called once per frame
    void Update()
    {


        if (waveInProgress) SpawnEnemies();
    }

    void StartWave()
    {
        waveInProgress = true;
        actualWaveAmount *= waveAmountMultiplier;
        remainingWaveAmount = actualWaveAmount;
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < EnemySpawnPoints.Length; i++)
        {
            if (EnemySpawnPoints[i].CanSpawn)
            {
                EnemyController auxEnemy = Instantiate(enemyPrefab, EnemySpawnPoints[i].transform.position, enemyPrefab.transform.rotation);
            }
        }
    }
}
