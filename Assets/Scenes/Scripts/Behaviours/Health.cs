﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //[SerializeField]int health = 20;
    public Transform HealthBar;
    public Slider HealthFill;


    private float currentHealth;
    public float MaxHealth;
    public float HealthBarYOffset = 2;

    private void Awake()
    {
        currentHealth = MaxHealth;
    }

    private void Update()
    {
        PositionHealthBar();
    }

    public void TakeDamange(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);

        HealthFill.value = currentHealth / MaxHealth;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void PositionHealthBar()
    {
        //Vector3 currentpos = transform.position;
        //HealthBar.position = new Vector3(currentpos.x, currentpos.y + HealthBarYOffset, currentpos.z);
        HealthBar.LookAt(Camera.main.transform);
    }
}
