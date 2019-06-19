using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject speedEffect;

    private PlayerController player;
    private Vector3 myDirection;
    private float actualSpeed;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        actualSpeed = speed;
    }

    private void Update()
    {
        if (myDirection == Vector3.zero) return;

        if (player != null)
        {
            transform.position += transform.forward * actualSpeed * myDirection.z * Time.deltaTime;

            transform.Rotate(0, myDirection.x * rotationSpeed, 0);
        }
        else
        {
            transform.position += transform.forward * actualSpeed * myDirection.magnitude * Time.deltaTime;

            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, myDirection, rotationSpeed * Time.deltaTime, 0.0f));
        }
    }

    //Recibo direction del joystick
    public void Move(Vector3 direction)
    {
        //Se mueve el player?
        if (player != null)
        {
            myDirection.x = direction.x;
            //Uso la y del joystick como mi profundidad
            myDirection.z = direction.y;
        }
        else
        {
            myDirection.x = direction.x;
            myDirection.z = direction.z;
        }
    }

    public void IncreaseSpeed (float speedMultiplier, float duration)
    {
        actualSpeed *= speedMultiplier;
        speedEffect.SetActive(true);
        Invoke(nameof(ResetSpeed), duration);
    }

    public void ResetSpeed()
    {
        actualSpeed = speed;
        speedEffect.SetActive(false);
    }
}
