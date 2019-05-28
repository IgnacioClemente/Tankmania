using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player")]
    [SerializeField] PlayerController playerPrefab;
    [SerializeField] Transform playerSpawnPoint;
    [Header("Enemy")]
    [SerializeField] EnemySpawnPoint[] EnemySpawnPoints;
    [Header("Wave modifiers")]
    [SerializeField] private int initialWaveAmount;
    [SerializeField] private int waveAmountMultiplier;
    [SerializeField] private float timeBetweenWaves;
    [Header("Wave UI")]
    [SerializeField] Text waveAmountText;
    [SerializeField] Text waveTimerText;
    [SerializeField] Text killAmount;
    [SerializeField] Text WaveNumber;

    private int waveCounter = 1;
    private int actualWaveAmount;
    private int remainingWaveAmount;
    private float waveTimer;
    private bool waveInProgress;
    private int enemiesKilled;
    private int enemiesKilledPerWave;

    private List<EnemyController> _activeEnemies;

    //TODO: mover tiempo para proxima wave al centro superior de la pantalla (ver bien anchors)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _activeEnemies = new List<EnemyController>();
    }

    void Start()
    {
        waveTimer = timeBetweenWaves;
        actualWaveAmount = initialWaveAmount;
        remainingWaveAmount = actualWaveAmount;
        waveAmountText.text = "Enemies: " + enemiesKilledPerWave.ToString() + "/" + actualWaveAmount.ToString();
        waveTimerText.text = "";
        waveInProgress = true;
        WaveNumber.text = "Wave Number " + waveCounter.ToString();
    }

    void Update()
    {
        if (waveInProgress) SpawnEnemies();
        else
        {
            waveTimer -= Time.deltaTime;
            waveTimerText.text = "Time to next wave: " + ((int)waveTimer).ToString();
            if (waveTimer <= 0)
            {
                StartNextWave();
            }
        }
    }

    void StartNextWave()
    {
        waveCounter++;
        WaveNumber.text = "Wave Number " + waveCounter.ToString();
        enemiesKilledPerWave = 0;
        waveTimer = timeBetweenWaves;
        waveInProgress = true;
        actualWaveAmount *= waveAmountMultiplier;
        remainingWaveAmount = actualWaveAmount;
        waveTimerText.text = "";
        waveAmountText.text = "Enemies: " + enemiesKilledPerWave.ToString() + "/" + actualWaveAmount.ToString();
    }

    void SpawnEnemies()
    {
        if (remainingWaveAmount <= 0)
        {
            if(_activeEnemies.Count <= 0)
                waveInProgress = false;

            return;
        }

        for (int i = 0; i < EnemySpawnPoints.Length; i++)
        {
            if (remainingWaveAmount <= 0) return;

            if (EnemySpawnPoints[i].CanSpawn)
            {
                var auxEnemy = PoolManager.GetInstance().CallByName("Enemy").GetComponent<EnemyController>();

                if (auxEnemy == null) return;

                auxEnemy.OnDeathEvent.AddListener(DespawnEnemy);

                auxEnemy.transform.position = EnemySpawnPoints[i].transform.position;
                _activeEnemies.Add(auxEnemy);
                remainingWaveAmount--;
            }
        }
    }

    void DespawnEnemy(EnemyController enemy)
    {
        _activeEnemies.Remove(enemy);
    }

    public void ScoreUp()
    {
        enemiesKilledPerWave++;
        enemiesKilled++;
        killAmount.text = "Kills: " + enemiesKilled.ToString();
        waveAmountText.text = "Enemies: " + enemiesKilledPerWave.ToString() + "/" + actualWaveAmount.ToString();
    }
}
