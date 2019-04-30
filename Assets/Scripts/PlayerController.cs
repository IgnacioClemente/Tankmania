using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Joystick movementJoystick;
    [SerializeField] Joystick aimJoystick;
    [SerializeField] Button fireButton;
    [SerializeField] Image fireImage;

    private Movement movement;
    private Attack attack;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        fireButton.onClick.AddListener(attack.Shoot);
    }

    private void Update()
    {
        fireImage.fillAmount = attack.RemainingCooldown / attack.AttackSpeed;
        movement.Move(movementJoystick.Direction);
        attack.Aim(aimJoystick.Direction);
    }
}
