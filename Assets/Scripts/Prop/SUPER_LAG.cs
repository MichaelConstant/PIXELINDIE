using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Super_Lag : PropBase
{
    public float RangeAttackSpeedUpPercent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject Player = collision.gameObject;
        GameObject PlayerShooter = collision.transform.GetChild(1).gameObject;

        if (Player.GetComponent<PlayerControl>() != null)
        {
            Player.GetComponent<PlayerControl>().ShootInterval_Ratio += RangeAttackSpeedUpPercent;
            Player.GetComponent<PlayerControl>().ShootInterval_Final = Player.GetComponent<PlayerControl>().ShootInterval_Basic / Player.GetComponent<PlayerControl>().ShootInterval_Ratio;

            Destroy(PlayerShooter.GetComponent<Shooter_Base>());
            Destroy(PlayerShooter.GetComponent<RageShooter_Base>());

            PlayerShooter.AddComponent<Super_Lag_Shooter>();
            PlayerShooter.AddComponent<Super_Lag_RageShooter>();

            Player.GetComponent<PlayerControl>().Shooter = Player.GetComponentInChildren<Super_Lag_Shooter>();
            Player.GetComponent<PlayerControl>().RageShooter = Player.GetComponentInChildren<Super_Lag_RageShooter>();

            Destroy(gameObject);
        }
    }
}