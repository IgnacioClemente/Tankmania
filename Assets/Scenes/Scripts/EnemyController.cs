using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float attackDistance = 8;
    [SerializeField] float chaseDistance = 18;
    [SerializeField] LayerMask layersToHit;

    private Movement movement;
    private Attack attack;
    private Health health;

    private Transform playerTransform;
    float playerDistance;
    RaycastHit hit;

    private void Awake()
    {
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

                if (playerDistance < chaseDistance)
                {
                    LookAtPlayer();

                    if (playerDistance > attackDistance)
                        ChasePlayer();
                    else
                        movement.Move(Vector3.zero);
                }
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
}