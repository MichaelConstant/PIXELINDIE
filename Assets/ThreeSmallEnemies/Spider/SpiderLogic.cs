using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLogic : CharacterControl
{
    public static int bulletNumber;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(canShoot);
        bulletNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(bulletNumber < 10)
        {
            CommonShoot();
        }
    }
}