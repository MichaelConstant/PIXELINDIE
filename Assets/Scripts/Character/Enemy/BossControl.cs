using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossControl : CharacterControl
{
    public int EnemyHP = 10;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CommonShoot();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(EnemyHP<=0)
        {
            Destroy(gameObject);
        }
    }
}