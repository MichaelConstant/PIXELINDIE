﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : CharacterControl
{
    #region 变量

    Vector3 AimVector;

    bool alive = true;
    bool isMoving = false;

    public int KeyCounts = 1;
    public Text keyText;
    public int CoinCounts = 1;
    public Text coinText;

    public Image HPUI;

    public Image MeleeEnergyUI;
    public Text MeleeLevelUI;
    public Text MeleeLevelHint;

    public Image RangeEnergyUI;
    public GameObject RangeUltraText;

    #region 能量系统相关
    public static float RangeEnergyStatic;
    public static float MeleeEnergyStatic;

    public static bool onRage = false;

    [Header("远程/近战初始能量，默认为0")]
    private readonly float RangeEnergy = 0;
    private readonly float MeleeEnergy = 0;

    public static float RangeEnergyPerHitStatic;
    public static float MeleeEnergyPerHitStatic;

    [Header("每一击远程/近战获得能量，近战获得能量，远程减少能量")]
    public float RangeEnergyPerHit;
    public float MeleeEnergyPerHit;
    //这里的远程每击减少能量指每次远程射击减少近战能量槽的量，远程射击每次命中增长远程能量槽固定为1，通过调整远程最大能量上限调整

    [Header("每0.1秒远程/近战能量衰减")]
    public float RangeEnergyDecreaseAmount;
    public float MeleeEnergyDecreaseAmount;

    private bool canDecrease = true;

    [Header("远程/近战初始等级，默认为0")]
    private readonly int RangeLevel = 0;
    private readonly int MeleeLevel = 0;

    public static int RangeLevelStatic;
    public static int MeleeLevelStatic;

    [Header("远程/近战能量上限")]
    public int RangeEnergyMax;
    public int MeleeEnergyMax;

    public static int RangeEnergyMaxStatic;
    public static int MeleeEnergyMaxStatic;

    [Header("远程/近战等级上限")]
    public int MeleeLevelMax;
    public int RangeLevelMax;

    public static int RangeLevelMaxStatic;
    public static int MeleeLevelMaxStatic;

    [Header("能量等级升级时的保底百分比，数值为0-1（0%-100%）")]
    public float RangeEnergyProtectPercent;
    public float MeleeEnergyProtectPercent;

    public static float RangeEnergyProtectPercentStatic;
    public static float MeleeEnergyProtectPercentStatic;
    #endregion

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        alive = true;

        keyText.text = "Key: " + KeyCounts;
        coinText.text = "Coin: " + CoinCounts;

        RangeEnergyStatic = RangeEnergy;
        MeleeEnergyStatic = MeleeEnergy;

        RangeEnergyPerHitStatic = RangeEnergyPerHit;
        MeleeEnergyPerHitStatic = MeleeEnergyPerHit;

        RangeEnergyPerHitStatic = RangeEnergyPerHit;
        MeleeEnergyPerHitStatic = MeleeEnergyPerHit;

        RangeLevelStatic = RangeLevel;
        MeleeLevelStatic = MeleeLevel;

        RangeEnergyMaxStatic = RangeEnergyMax;
        MeleeEnergyMaxStatic = MeleeEnergyMax;

        RangeLevelMaxStatic = RangeLevelMax;
        MeleeLevelMaxStatic = MeleeLevelMax;

        RangeEnergyProtectPercentStatic = RangeEnergyProtectPercent;
        MeleeEnergyProtectPercentStatic = MeleeEnergyProtectPercent;
    }
    // Update is called once per frame
    void Update()
    {
        MeleeEnergyUI.fillAmount = (float)MeleeEnergyStatic / MeleeEnergyMaxStatic;
        RangeEnergyUI.fillAmount = (float)RangeEnergyStatic / RangeEnergyMaxStatic;
        HPUI.fillAmount = (float)currentHP / maxHP;

        xInput = (int)Input.GetAxisRaw("Horizontal");
        yInput = (int)Input.GetAxisRaw("Vertical");

        if(xInput !=0 || yInput != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        anim.SetBool("isMoving", isMoving);

        AimVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z)) - gameObject.transform.position);
        float Angle = Angle_360(AimVector);
        Debug.Log(Angle);

        if((Angle <= 0 && Angle >= -45) || (Angle >= 0 && Angle <= 45))
        {
            anim.SetInteger("State", 1);
        }
        else if (Angle > 45 && Angle <= 90)
        {
            anim.SetInteger("State", 2);
        }
        else if (Angle >= -90 && Angle < -45)
        {
            anim.SetInteger("State", 3);
        }
        
        else if (Angle <= -135 || Angle >= 135)
        {
            anim.SetInteger("State", 4);
        }
        else if (Angle > 90 && Angle < 135)
        {
            anim.SetInteger("State", 5);
        }
        else if (Angle > -135 && Angle < -90)
        {
            anim.SetInteger("State", 6);
        }

        if (Input.GetMouseButton(0))
            {
                PlayerShoot();
            }
        if (Input.GetMouseButton(1))
            {
                Attack();
            }
        if (Input.GetKeyDown(KeyCode.E) && RangeLevelStatic == 1)
            {
                PlayerUltraShoot();
                RangeLevelStatic = 0;
                RangeEnergyStatic = 0;
                RangeUltraText.SetActive(false);
            }
        if (Input.GetKeyDown(KeyCode.Q) && MeleeLevelStatic >= 1)
            {
                onRage = true;
                MeleeEnergyDecreaseAmount = 0.1f;
            }
    }

    void FixedUpdate()
    {
        if (alive)
        {
            Move(xInput, yInput);
        }
        if (canDecrease == true)
        {
            MeleeEnergyDecrease();
        }
        if (currentHP <= 0)
        {
            alive = false;
            xInput = yInput = 0;
            anim.SetInteger("State", 0);
            anim.Play("Dead");
            Invoke("Dead", 2);
        }
    }
    void PlayerShoot()
    {
        if (canShoot)
        {
            if (onRage)
            {
                MeleeEnergyDecreaseOfShooting();
                canShoot = false;
                AttackVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z)) - gameObject.transform.position);

                switch (PlayerControl.MeleeLevelStatic)
                {
                    case 0:
                        GetComponentInChildren<BulletSpawn>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        GetComponentInChildren<BulletSpawn>().transform.Rotate(0, 0, Angle_360(AttackVector) - 5);
                        GameObject bullet = Instantiate(bullet0, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                        bullet.transform.parent = transform;

                        GetComponentInChildren<BulletSpawn>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        GetComponentInChildren<BulletSpawn>().transform.Rotate(0, 0, Angle_360(AttackVector) + 5);
                        bullet = Instantiate(bullet0, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                        bullet.transform.parent = transform;
                        break;
                    case 1:
                        GetComponentInChildren<BulletSpawn>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        GetComponentInChildren<BulletSpawn>().transform.Rotate(0, 0, Angle_360(AttackVector) - 15);
                        bullet = Instantiate(bullet0, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                        bullet.transform.parent = transform;

                        GetComponentInChildren<BulletSpawn>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        GetComponentInChildren<BulletSpawn>().transform.Rotate(0, 0, Angle_360(AttackVector));
                        bullet = Instantiate(bullet0, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                        bullet.transform.parent = transform;

                        GetComponentInChildren<BulletSpawn>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        GetComponentInChildren<BulletSpawn>().transform.Rotate(0, 0, Angle_360(AttackVector) + 15);
                        bullet = Instantiate(bullet0, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                        bullet.transform.parent = transform;
                        break;
                    case 2:
                        GetComponentInChildren<BulletSpawn>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        GetComponentInChildren<BulletSpawn>().transform.Rotate(0, 0, Angle_360(AttackVector) - 5);
                        bullet = Instantiate(bullet0, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                        bullet.transform.parent = transform;

                        GetComponentInChildren<BulletSpawn>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        GetComponentInChildren<BulletSpawn>().transform.Rotate(0, 0, Angle_360(AttackVector) + 5);
                        bullet = Instantiate(bullet0, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                        bullet.transform.parent = transform;

                        GetComponentInChildren<BulletSpawn>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        GetComponentInChildren<BulletSpawn>().transform.Rotate(0, 0, Angle_360(AttackVector) - 15);
                        bullet = Instantiate(bullet0, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                        bullet.transform.parent = transform;

                        GetComponentInChildren<BulletSpawn>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        GetComponentInChildren<BulletSpawn>().transform.Rotate(0, 0, Angle_360(AttackVector) + 15);
                        bullet = Instantiate(bullet0, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                        bullet.transform.parent = transform;
                        break;
                }
            }
            else
            {
                MeleeEnergyDecreaseOfShooting();
                canShoot = false;
                AttackVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z)) - gameObject.transform.position);

                GetComponentInChildren<BulletSpawn>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                GetComponentInChildren<BulletSpawn>().transform.Rotate(0, 0, Angle_360(AttackVector));

                switch (PlayerControl.MeleeLevelStatic)
                {
                    case 0:
                        GameObject bullet = Instantiate(bullet0, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                        bullet.transform.parent = transform;
                        break;
                    case 1:
                        bullet = Instantiate(bullet1, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                        bullet.transform.parent = transform;
                        break;
                    case 2:
                        bullet = Instantiate(bullet2, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                        bullet.transform.parent = transform;
                        break;
                    case 3:
                        bullet = Instantiate(bullet3, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                        bullet.transform.parent = transform;
                        break;
                }
            }
            StartCoroutine(ShootInterval());
        }
    }
    void Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            AttackVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z)) - gameObject.transform.position);
            GetComponentInChildren<BulletSpawn>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            GetComponentInChildren<BulletSpawn>().transform.Rotate(0, 0, Angle_360(AttackVector));
            GetComponentInChildren<BulletSpawn>().GetComponentInChildren<Animator>().Play("Attack");
            StartCoroutine(AttackInterval());
        }
    }
    void PlayerUltraShoot()
    {
        AttackVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z)) - gameObject.transform.position);
        GetComponentInChildren<BulletSpawn>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        GetComponentInChildren<BulletSpawn>().transform.Rotate(0, 0, Angle_360(AttackVector));
        GameObject bullet = Instantiate(bulletUltra, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.transform.parent = transform;
    }
    private IEnumerator MeleeEnergyDecreaseInterval()
    {
        yield return new WaitForSeconds(0.1f);
        canDecrease = true;
    }
    private void MeleeEnergyDecrease()
    {
        canDecrease = false;
        if (MeleeEnergyStatic > 0)
        {
            MeleeEnergyStatic -= MeleeEnergyDecreaseAmount;
        }
        if(MeleeEnergyStatic <= 0 && MeleeLevelStatic > 0)
        {
            MeleeLevelStatic -= 1;
            MeleeLevelUI.text = "LV: " + (MeleeLevelStatic + 1);
            MeleeLevelHint.text = "Pree Q to RAGE LV." + (MeleeLevelStatic + 1);
            MeleeEnergyStatic += MeleeEnergyMaxStatic;
        }
        if (MeleeEnergyStatic <= 0 && MeleeLevelStatic == 0)
        {
            onRage = false;
            MeleeEnergyDecreaseAmount = 0.01f;
        }
        StartCoroutine(MeleeEnergyDecreaseInterval());
    }
    private void MeleeEnergyDecreaseOfShooting()
    {
        if (canShoot&&!onRage)
        {
            if (MeleeEnergyStatic > 0)
            {
                MeleeEnergyStatic -= RangeEnergyPerHit;
            }
            if (MeleeEnergyStatic <= 0 && MeleeLevelStatic > 0)
            {
                MeleeLevelStatic -= 1;
                MeleeLevelUI.text = "LV: " + MeleeLevel;
                MeleeEnergyStatic = MeleeEnergyMaxStatic;
            }
        }
    }
    public void Dead()
    {
        SceneManager.LoadScene("Studio");
    }
}