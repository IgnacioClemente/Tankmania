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
    private PlayerController player;

    private Vector3 myDirection;
    private float remainingCooldown;
    private bool canShoot = true;

    public float AttackSpeed { get { return attackSpeed; } }
    public float RemainingCooldown { get { return remainingCooldown; } }
    public GameObject Head { get { return head; } }

    private void Awake()
    {
        remainingCooldown = attackSpeed;
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (player != null)
        {
            head.transform.Rotate(0, myDirection.x * headRotation, 0);
            //head.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(head.transform.forward, myDirection, headRotation * Time.deltaTime, 0.0f));
        }
        else
        {
            head.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(head.transform.forward, myDirection, headRotation * Time.deltaTime, 0.0f));
        }

        if (canShoot) return;

        remainingCooldown += Time.deltaTime;

        if (remainingCooldown >= attackSpeed) canShoot = true;

    }

    public void Aim(Vector3 direction, PlayerController player = null)
    {
        if(player != null)
        {
            myDirection.x = direction.x;
            myDirection.z = direction.y;
        }
        else
        {
            myDirection.x = direction.x;
            myDirection.z = direction.z;
        }

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
