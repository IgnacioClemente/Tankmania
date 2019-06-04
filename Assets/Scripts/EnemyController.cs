using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class EnemyDeathEvent : UnityEvent<EnemyController> { }

public class EnemyController : MonoBehaviour
{
    [SerializeField] float attackDistance = 8;
    [SerializeField] float chaseDistance = 18;
    [SerializeField] float timeToStopChasing = 5;
    [SerializeField] LayerMask layersToHit;

    private Movement movement;
    private Attack attack;
    private Health health;
    private EnemyDeathEvent onDeathEvent;

    private Transform playerTransform;
    private float playerDistance;
    private bool tookDamage;
    private RaycastHit hit;
    private List<Transform> patrolPoints;
    private Transform actualPatrolPoint;

    public EnemyDeathEvent OnDeathEvent { get { return onDeathEvent; } }

    private void Awake()
    {
        onDeathEvent = new EnemyDeathEvent();

        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        playerTransform = PlayerController.Instance.transform;    
    }

    void Update()
    {
        if (gameObject.activeSelf) //si esta activo
        {
            playerDistance = Vector3.Distance(playerTransform.position, transform.position);// calculo la distancia del player

            if (tookDamage || playerDistance < chaseDistance)// si tome daño o mi distancia es menor que chase distance
            {
                LookAtPlayer();//miro al player

                if (playerDistance > attackDistance)//si la distancia del player es mayor a mi distancia de ataque lo persigo
                    ChasePlayer();
                else
                    movement.Move(Vector3.zero);
            }
            else
                Patrol();
        }
    }

    public void LookAtPlayer()
    {
        attack.Aim((playerTransform.position - transform.position).normalized);
        
        if (Physics.Raycast(attack.Head.transform.position, attack.Head.transform.forward, out hit, chaseDistance, layersToHit))
        {
            GameObject hitobject = hit.transform.gameObject;
            if (hitobject.GetComponent<PlayerController>())
                attack.Shoot();
        }
    }

    public void ChasePlayer()
    {
        movement.Move((playerTransform.position - transform.position).normalized);
    }

    public void ActivateTookDamage()
    {
        CancelInvoke(nameof(DeactivateTookDamage));
        tookDamage = true;
        Invoke(nameof(DeactivateTookDamage), timeToStopChasing);
    }

    public void DeactivateTookDamage()
    {
        tookDamage = false;
    }

    public void SetEnemy(List<Transform> patrols, Transform spawnPoint)
    {
        patrolPoints = patrols;
        transform.position = spawnPoint.position;
        actualPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Count - 1)];
    }

    public void KillMe()
    {
        onDeathEvent.Invoke(this);
        DeactivateTookDamage();
        health.RestoreHealth();
        GameManager.Instance.ScoreUp();
        PoolManager.GetInstance().TurnOffByName("Enemy", this.gameObject);
    }

    public void Patrol()
    {
        if(Vector3.Distance(actualPatrolPoint.position, transform.position) < 2)
        {
            actualPatrolPoint = GetNewPatrolPoint();
        }
        movement.Move((actualPatrolPoint.position - transform.position).normalized);
    }

    private Transform GetNewPatrolPoint()
    {
        //Busco un random, si es distinto a acualpatrolpoint, lo retorno, si no vuelvo a llamar esta funcion
        Transform auxPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Count - 1)];
        if(auxPatrolPoint == actualPatrolPoint)
        {
            auxPatrolPoint = GetNewPatrolPoint();
        }

        return auxPatrolPoint;
    }
}