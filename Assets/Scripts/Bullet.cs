using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timeToExplode = 4;

    private string myShooterLayer;
    private int damage = 5;

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void Spawn(string shooter, int damage)
    {
        CancelInvoke(nameof(Explode));
        myShooterLayer = shooter;
        this.damage = damage;
        Invoke(nameof(Explode), timeToExplode);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) != myShooterLayer)
        {
            if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            {
                Health auxHealth = other.GetComponent<Health>();
                if (auxHealth != null)
                {
                    auxHealth.TakeDamange(damage);
                }
            }
            print(other.tag + other.gameObject);
            Explode();
        }
    }

    private void Explode()
    {
        //TODO: efecto de explosion
        PoolManager.GetInstance().TurnOffByName("Bullet", gameObject);
    }
}
