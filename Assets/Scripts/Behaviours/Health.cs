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
    public float MaxHealth;
    public float HealthBarYOffset = 2;
    [Header("PowerUp Effects")]
    [SerializeField] private ParticleSystem healingEffect;
    [SerializeField] private GameObject defenseEffect;

    private EnemyController enemy;
    private PlayerController player;
    private float currentHealth;
    private bool canTakeDamage = true;

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
        if (!canTakeDamage) return;

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

    public void Heal(int extraHealth)
    {
        currentHealth += extraHealth;
        healingEffect.gameObject.SetActive(true);
        Invoke(nameof(TurnOffHealingEffect), healingEffect.main.duration);

        if (currentHealth > MaxHealth) currentHealth = MaxHealth;
        UpdateHealthBar();
    }

    private void TurnOffHealingEffect()
    {
        healingEffect.gameObject.SetActive(false);
    }

    public void Defense(float duration)
    {
        canTakeDamage = false;
        defenseEffect.SetActive(true);
        Invoke(nameof(ResetDefense), duration);
    }

    public void ResetDefense()
    {
        defenseEffect.SetActive(false);
        canTakeDamage = true;
    }
}
