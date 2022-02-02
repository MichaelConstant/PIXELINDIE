using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Super_Lag_RageShooter : RageShooter_Base
{
    private void OnEnable()
    {
        if (Player.canShoot && Player.onRage)
        {
            Player.MeleeEnergyDecreaseOfShooting();
            Player.canShoot = false;Player.AttackVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z)) - gameObject.transform.position);
            switch (Player.MeleeLevel)
            {
                case 0:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    transform.Rotate(0, 0, Random.Range(-180, 180));
                    GameObject bullet = Instantiate(Player.bullet_0, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
                    bullet.transform.parent = Player.transform;

                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    transform.Rotate(0, 0, Random.Range(-180, 180));
                    bullet = Instantiate(Player.bullet_0, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
                    bullet.transform.parent = Player.transform;
                    break;
                case 1:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    transform.Rotate(0, 0, Random.Range(-180, 180));
                    bullet = Instantiate(Player.bullet_0, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
                    bullet.transform.parent = Player.transform;

                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    transform.Rotate(0, 0, Random.Range(-180, 180));
                    bullet = Instantiate(Player.bullet_0, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
                    bullet.transform.parent = Player.transform;

                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    transform.Rotate(0, 0, Random.Range(-180, 180));
                    bullet = Instantiate(Player.bullet_0, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
                    bullet.transform.parent = Player.transform;
                    break;
                case 2:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    transform.Rotate(0, 0, Random.Range(-180, 180));
                    bullet = Instantiate(Player.bullet_0, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
                    bullet.transform.parent = Player.transform;

                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    transform.Rotate(0, 0, Random.Range(-180, 180));
                    bullet = Instantiate(Player.bullet_0, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
                    bullet.transform.parent = Player.transform;

                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    transform.Rotate(0, 0, Random.Range(-180, 180));
                    bullet = Instantiate(Player.bullet_0, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
                    bullet.transform.parent = Player.transform;

                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    transform.Rotate(0, 0, Random.Range(-180, 180));
                    bullet = Instantiate(Player.bullet_0, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
                    bullet.transform.parent = Player.transform;
                    break;
            }
            StartCoroutine(Player.ShootInterval());
        }
        GetComponent<RageShooter_Base>().enabled = false;
    }
}
