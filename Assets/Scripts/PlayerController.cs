using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health),typeof(Movement),typeof(Attack))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] Joystick movementJoystick;
    [SerializeField] Joystick aimJoystick;
    [SerializeField] Button fireButton;
    [SerializeField] Image fireImage;

    private Movement movement;
    private Attack attack;
    private Health health;


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
        fireImage.fillAmount = attack.RemainingCooldown / attack.AttackSpeed;
        movement.Move(movementJoystick.Direction, this);
        attack.Aim(aimJoystick.Direction, this);
    }
}
