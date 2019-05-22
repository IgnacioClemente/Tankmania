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

    private int waveCounter = 1;
    private int actualWaveAmount;
    private int remainingWaveAmount;
    private float waveTimer;
    private bool waveInProgress;
    private List<EnemyController> _activeEnemies;

    //TODO: Score que cuente todos los enemigos matados y los muestre en pantalla
    //Un valor que cuente los enemigos matados en esta wave y se reinicie al principio de cada wave
    //Y mostrar en pantalla "Mataste x de y enemigos"
    //Y mostrar en pantalla el numero de wave
    //Buscar como formatear el float para que no se vean todos los valores con coma, solo el primer decimal

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
        waveAmountText.text = remainingWaveAmount.ToString() + "/" + initialWaveAmount.ToString();
        waveTimerText.text = "Time to next wave: " + waveTimer.ToString();
        waveInProgress = true;
    }
    
    void Update()
    {
        if (waveInProgress) SpawnEnemies();
        else
        {
            waveTimer -= Time.deltaTime;
            waveTimerText.text = "Time to next wave: " + waveTimer.ToString();
            if (waveTimer <= 0)
            {
                StartNextWave();
            }
        }
    }

    void StartNextWave()
    {
        waveCounter++;
        waveTimer = timeBetweenWaves;
        waveTimerText.text = "Time to next wave: " + waveTimer.ToString();
        waveInProgress = true;
        actualWaveAmount *= waveAmountMultiplier;
        remainingWaveAmount = actualWaveAmount;
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
                waveAmountText.text = remainingWaveAmount.ToString() + "/" + initialWaveAmount.ToString();
            }
        }
    }

    void DespawnEnemy(EnemyController enemy)
    {
        _activeEnemies.Remove(enemy);
    }
}
