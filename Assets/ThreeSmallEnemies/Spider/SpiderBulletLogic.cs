using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBulletLogic : EnemyBullet
{
    // Start is called before the first frame update
    void Start()
    {
        SpiderLogic.bulletNumber++;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
