using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //[SerializeField]int health = 20;
    public Transform HealthBar;
    public Slider HealthFill;
    public Text HealthText;
    private EnemyController enemy;
    private PlayerController player;


    private float currentHealth;
    public float MaxHealth;
    public float HealthBarYOffset = 2;

    private void Awake()
    {
        currentHealth = MaxHealth;
        player = GetComponent<PlayerController>();
        enemy = GetComponent<EnemyController>();
        UpdateHealthBar();
    }

    private void Update()
    {
        PositionHealthBar();
    }

    public void TakeDamange(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);

        UpdateHealthBar();

        if (enemy != null) enemy.ActivateTookDamage();

        if (currentHealth <= 0)
        {
            if (player != null) player.KillPlayer();
            else if (enemy != null) enemy.KillMe();
        }
    }

    private void UpdateHealthBar()
    {
        HealthFill.value = currentHealth / MaxHealth;
        if(HealthText != null) HealthText.text = currentHealth + "/" + MaxHealth;
    }

    private void PositionHealthBar()
    {
        if(enemy != null) HealthBar.LookAt(Camera.main.transform);
    }

    public void RestoreHealth()
    {
        currentHealth = MaxHealth;
        HealthFill.value = 1;
    }
}
