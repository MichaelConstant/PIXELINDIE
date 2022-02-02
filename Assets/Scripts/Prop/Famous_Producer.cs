using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Famous_Producer : PropBase
{
    public float MeleeEnergyPerHitIncreased;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControl Player = collision.GetComponent<PlayerControl>();
        if (Player != null)
        {
            Player.MeleeEnergyPerHit += MeleeEnergyPerHitIncreased;
            Destroy(gameObject);
        }
    }
}
