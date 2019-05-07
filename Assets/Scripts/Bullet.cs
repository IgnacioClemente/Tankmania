using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    private string myShooterLayer;
    [SerializeField] int damage = 5;

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    public void Spawn(string shooter)
    {
        myShooterLayer = shooter;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == myShooterLayer) return;

        Health auxHealth = other.GetComponent<Health>();
        if(auxHealth != null)
        {
            auxHealth.TakeDamange(damage);
            Destroy(gameObject);
        }
    }
}
