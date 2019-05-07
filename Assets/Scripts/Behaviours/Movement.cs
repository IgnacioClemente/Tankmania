using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    private Vector3 myDirection;

    private void Update()
    {
        if (myDirection == Vector3.zero) return;

        transform.position += transform.forward * speed * myDirection.magnitude * Time.deltaTime;

        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, myDirection, rotationSpeed * Time.deltaTime, 0.0f));

    }

    //Recibo direction del joystick
    public void Move(Vector3 direction, PlayerController player = null)
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
}
