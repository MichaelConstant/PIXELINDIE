using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default_Shooter : Shooter_Base
{
    private void OnEnable()
    {
        if (Player.canShoot && !Player.onRage)
        {
            Player.MeleeEnergyDecreaseOfShooting();
            Player.canShoot = false;
            Player.AttackVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z)) - gameObject.transform.position);

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            transform.Rotate(0, 0, Player.Angle_360(Player.AttackVector));
            switch (Player.MeleeLevel)
            {
                case 0:
                    GameObject bullet = Instantiate(Player.bullet_0, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
                    bullet.transform.parent = Player.transform;
                    break;
                case 1:
                    bullet = Instantiate(Player.bullet_1, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
                    bullet.transform.parent = Player.transform;
                    break;
                case 2:
                    bullet = Instantiate(Player.bullet_2, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
                    bullet.transform.parent = Player.transform;
                    break;
                case 3:
                    bullet = Instantiate(Player.bullet_3, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
                    bullet.transform.parent = Player.transform;
                    break;
            }
            StartCoroutine(Player.ShootInterval());
        }
        GetComponent<Shooter_Base>().enabled = false;
    }
}