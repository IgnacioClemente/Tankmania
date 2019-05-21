using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] PlayerController playerPrefab;
    [SerializeField] Transform playerSpawnPoint;
    [Header("Enemy")]
    [SerializeField] EnemySpawnPoint[] EnemySpawnPoints;
    [Header("Wave modifiers")]
    [SerializeField] private int initialWaveAmount;
    [SerializeField] private int waveAmountMultiplier;
    [SerializeField] private float timeBetweenWaves;

    private int waveCounter = 1;
    private int actualWaveAmount;
    private int remainingWaveAmount;
    private bool waveInProgress;

    //TODO: añadir enemigos a esta lista al spawnearlos y removerlos al matarlos
    //Si esta lista está en cero y no quedan enemigos a spawnear, comienza el conteo a la proxima wave
    private List<EnemyController> _activeEnemies;

                
    // Start is called before the first frame update
    void Start()
    {
        actualWaveAmount = initialWaveAmount;
        remainingWaveAmount = actualWaveAmount;
        //Instantiate(playerPrefab, playerSpawnPoint.position, playerPrefab.transform.rotation);

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
                Debug.Log("a spawnear");
                remainingWaveAmount--;
                PoolManager.GetInstance().CallByName("Enemy").transform.position = EnemySpawnPoints[i].transform.position;
                if (remainingWaveAmount <= 0)
                {
                    waveInProgress = false;
                    return;
                }
            }
        }
    }
}
