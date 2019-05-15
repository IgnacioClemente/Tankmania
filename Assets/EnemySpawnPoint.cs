using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    private bool canSpawn;

    public bool CanSpawn { get { return canSpawn; } }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player")) canSpawn = false;
        else canSpawn = true;
    }
}
