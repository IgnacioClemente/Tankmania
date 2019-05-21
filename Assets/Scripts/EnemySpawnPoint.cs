using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    //TODO: crear una lista de gameobjects en donde guardo todos los enemigos y players que entran en mi trigger.
    //Cuando salen (exit) sacarlos de la lista
    //Si esta lista tiene un count igual a cero, puedo spawnear
    //Si no, no
    //Ademas recorrer la lista en el update y preguntar si el objeto esta activo. Si no lo está, sacarlo de la lista
    private bool canSpawn = true;

    public bool CanSpawn { get { return canSpawn; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player")) canSpawn = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player")) canSpawn = true;
    }
}
