using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Form : PropBase
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControl Player = collision.GetComponent<PlayerControl>();
        if (Player != null)
        {
            Player.gameObject.layer = 9;
            Destroy(gameObject);
        }
    }
}