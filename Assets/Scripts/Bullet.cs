using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] int damage = 5;
    [SerializeField] private float timeToExplode = 4;

    private string myShooterLayer;

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    public void Spawn(string shooter)
    {
        CancelInvoke(nameof(Explode));
        myShooterLayer = shooter;
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
            Explode();
        }
    }

    private void Explode()
    {
        //TODO: efecto de explosion
        PoolManager.GetInstance().TurnOffByName("Bullet", gameObject);
    }
}
