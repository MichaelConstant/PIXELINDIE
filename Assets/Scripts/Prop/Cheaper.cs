using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheaper : PropBase
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControl Player = collision.GetComponent<PlayerControl>();
        if (Player != null)
        {
            Player.gameObject.AddComponent<Cheaper_Buff>();
            Destroy(gameObject);
        }
    }
}