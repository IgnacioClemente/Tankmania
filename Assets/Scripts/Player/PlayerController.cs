using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health), typeof(Movement), typeof(Attack))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("UI")]
    [SerializeField] Joystick movementJoystick;
    [SerializeField] Joystick aimJoystick;
    [SerializeField] Button fireButton;
    [SerializeField] Image fireImage;
    [Header("Turbo PowerUp")]
    [SerializeField] private float turboSpeedMultiplier;
    [SerializeField] private float turboDuration;
    [Header("Damage PowerUp")]
    [SerializeField] private int damageMultiplier;
    [SerializeField] private float damageDuration;
    [Header("Health PowerUp")]
    [SerializeField] private int healingAmount;
    [Header("Defense PowerUp")]
    [SerializeField] private float defenseDuration;


    private Movement movement;
    private Attack attack;
    private Health health;
    private bool isAlive = true;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);

        Instance = this;

        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        health = GetComponent<Health>();
        fireButton.onClick.AddListener(attack.Shoot);
    }

    private void Update()
    {
        if (!isAlive) return;

        fireImage.fillAmount = attack.RemainingCooldown / attack.AttackSpeed;
        movement.Move(movementJoystick.Direction);
        attack.Aim(aimJoystick.Direction, this);
    }

    public void KillPlayer()
    {
        isAlive = false;
        GameManager.Instance.EndGame();
    }

    public void ActivePower(Gesture gesture)
    {
        switch(gesture)
        {
            case Gesture.swipeUp:
                movement.IncreaseSpeed(turboSpeedMultiplier, turboDuration);
                break;
            case Gesture.swipeDown:
                attack.IncreaseDamage(damageMultiplier, damageDuration);
                break;
            case Gesture.swipeRight:
                health.Heal(healingAmount);
                break;
            case Gesture.swipeLeft:
                health.Defense(defenseDuration);
                break;
        }
    }
}
