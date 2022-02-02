using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default_UltraShooter : UltraShooter_Base
{
    private void OnEnable()
    {
        Player.AttackVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z)) - gameObject.transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.Rotate(0, 0, Player.Angle_360(Player.AttackVector));
        GameObject bullet = Instantiate(Player.bullet_Ultra, Player.bulletSpawn.transform.position, Player.bulletSpawn.transform.rotation);
        bullet.transform.parent = Player.transform;
        GetComponent<UltraShooter_Base>().enabled = false;
    }
}