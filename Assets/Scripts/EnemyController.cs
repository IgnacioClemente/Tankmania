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
        {
            if (gameObject.activeSelf)
            {
                playerDistance = Vector3.Distance(playerTransform.position, transform.position);

                if (tookDamage || playerDistance < chaseDistance)
                {
                    LookAtPlayer();

                    if (playerDistance > attackDistance)
                        ChasePlayer();
                    else
                        movement.Move(Vector3.zero);
                }
                else
                    movement.Move(Vector3.zero);
            }
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

    public void KillMe()
    {
        onDeathEvent.Invoke(this);
        DeactivateTookDamage();
        health.RestoreHealth();
        GameManager.Instance.ScoreUp();
        PoolManager.GetInstance().TurnOffByName("Enemy", this.gameObject);
    }
}