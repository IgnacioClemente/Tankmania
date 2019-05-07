using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField ]int health = 20;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = health;
    }

    public void TakeDamange(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
