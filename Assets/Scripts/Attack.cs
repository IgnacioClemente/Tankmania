using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    [SerializeField] Bullet bullet;
    [SerializeField] Transform shotSpawn;
    [SerializeField] GameObject head;
    [SerializeField] float headRotation;
    [SerializeField] float attackSpeed;

    private Vector3 myDirection;
    private float remainingCooldown;
    private bool canShoot = true;

    public float AttackSpeed { get { return attackSpeed; } }
    public float RemainingCooldown { get { return remainingCooldown; } }

    private void Awake()
    {
        remainingCooldown = attackSpeed;
    }

    private void Update()
    {
        head.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(head.transform.forward, myDirection, headRotation * Time.deltaTime, 0.0f));

        if (canShoot) return;

        remainingCooldown += Time.deltaTime;

        if (remainingCooldown >= attackSpeed) canShoot = true;

    }

    public void Aim(Vector2 direction)
    {
        myDirection.x = direction.x;
        //Uso la y del joystick como mi profundidad
        myDirection.z = direction.y;
    }

    public void Shoot()
    {
        if (!canShoot) return;

        var auxBullet = Instantiate(bullet, shotSpawn.position, bullet.transform.rotation);
        auxBullet.transform.up = head.transform.forward;
        auxBullet.Spawn(LayerMask.LayerToName(gameObject.layer));

        canShoot = false;
        remainingCooldown = 0;
    }
}
