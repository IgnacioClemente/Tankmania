using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    private List<Transform> patrolPoints;
    private List<GameObject> objectsInRange;
    private bool canSpawn = true;

    public bool CanSpawn { get { return canSpawn; } }
    public List<Transform> PatrolPoints { get { return patrolPoints; } }

    private void Awake()
    {
        objectsInRange = new List<GameObject>();
        patrolPoints = new List<Transform>();
        foreach(Transform child in transform)
        {
            patrolPoints.Add(child);
        }
    }

    private void Update()
    {
        if (objectsInRange.Count > 0)
            canSpawn = false;
        else
        {
            canSpawn = true;
            return;
        }

        for (int i = 0; i < objectsInRange.Count; i++)
        {
            if (!objectsInRange[i].activeSelf)
            {
                objectsInRange.Remove(objectsInRange[i]);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            objectsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            objectsInRange.Remove(other.gameObject);
        }
    }
}
