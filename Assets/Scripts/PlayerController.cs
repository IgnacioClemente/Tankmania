using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Joystick movementJoystick;
    [SerializeField] Joystick aimJoystick;

    private Movement movement;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        movement.Move(movementJoystick.Direction);
        Debug.Log(movementJoystick.Direction.magnitude);
    }
}
