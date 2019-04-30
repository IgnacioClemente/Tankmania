using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private string myShooterLayer;

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

        
        Destroy(other.gameObject);
        Destroy(gameObject);
         
    }
}
