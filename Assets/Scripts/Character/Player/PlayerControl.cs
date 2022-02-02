using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : CharacterControl
{
    #region 变量

    public int KeyCounts;
    public Text keyText;

    public int CoinCounts;
    public Text coinText;

    public Image HPUI;

    public Image MeleeEnergyUI;
    public Text MeleeLevelUI;
    public GameObject MeleeLevelHint;

    public Image RangeEnergyUI;
    public GameObject RangeUltraText;

    #region 能量系统相关

    [HideInInspector]
    public bool onRage = false;

    [Header("远程/近战初始能量，默认为0")]
    public float RangeEnergy = 0;
    public float MeleeEnergy = 0;

    [Header("每一击远程/近战获得能量，近战获得能量，远程减少能量")]
    public float RangeEnergyPerHit;
    public float MeleeEnergyPerHit;
    //这里的远程每击减少能量指每次远程射击减少近战能量槽的量
    //远程射击每次命中增长远程能量槽固定为1，通过调整远程最大能量上限调整

    [Header("每0.1秒远程/近战能量衰减")]
    public float RangeEnergyDecreaseAmount;
    public float MeleeEnergyDecreaseAmount;

    private bool canDecrease = true;

    [Header("远程/近战初始等级，默认为0")]
    public int RangeLevel = 0;
    public int MeleeLevel = 0;

    [Header("远程/近战能量上限")]
    public int RangeEnergyMax;
    public int MeleeEnergyMax;

    [Header("远程/近战等级上限")]
    public int MeleeLevelMax;
    public int RangeLevelMax;

    [Header("能量等级升级时的保底百分比，数值为0-1（0%-100%）")]
    public float RangeEnergyProtectPercent;
    public float MeleeEnergyProtectPercent;

    #endregion

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        keyText.text = "Key: " + KeyCounts;
        coinText.text = "Coin: " + CoinCounts;
    }
    // Update is called once per frame
    void Update()
    {
        MeleeEnergyUI.fillAmount = (float)MeleeEnergy / MeleeEnergyMax;
        RangeEnergyUI.fillAmount = (float)RangeEnergy / RangeEnergyMax;
        HPUI.fillAmount = (float)currentHP / maxHP;

        if (isAlive)
        {
            xInput = (int)Input.GetAxisRaw("Horizontal");
            yInput = (int)Input.GetAxisRaw("Vertical");

            #region Character Animation
            if (xInput != 0 || yInput != 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
            anim.SetBool("isMoving", isMoving);
            AttackVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z)) - gameObject.transform.position);
            float Angle = Angle_360(AttackVector);
            if ((Angle <= 0 && Angle >= -45) || (Angle >= 0 && Angle <= 45))
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
            #endregion

            #region Character Input

            if ((Input.GetMouseButton(0)) && (!Input.GetMouseButtonDown(1)))
            {
                Shooter.enabled = false;
                RageShooter.enabled = false;
                Shooter.enabled = true;
                RageShooter.enabled = true;
            }
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(Attack());
            }
            if (Input.GetKeyDown(KeyCode.E) && RangeLevel >= 1)
            {
                UltraShooter.enabled = false;
                UltraShooter.enabled = true;
                RangeLevel -= 1;
                RangeEnergy = 0;
                RangeUltraText.SetActive(false);
                }
            if (Input.GetKeyDown(KeyCode.Q) && MeleeLevel >= 1)
            {
                onRage = true;
                MeleeEnergyDecreaseAmount = 0.1f;
            }
            #endregion
        }
    }
    void FixedUpdate()
    {
        if (isAlive && canMove)
        {
            Move(xInput, yInput);
        }
        if (canDecrease)
        {
            StartCoroutine(MeleeEnergyDecrease());
        }
        if (currentHP <= 0)
        {
            if(gameObject.GetComponent<Sakiro_Buff>() != null)
            {
                currentHP = maxHP / 2;
                Destroy(gameObject.GetComponent<Sakiro_Buff>());
            }
            else
            {
                isAlive = false;
                isMoving = false;
                canMove = false;
                rb.velocity = new Vector3(0, 0, 0);
                gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                anim.SetInteger("State", 0);
                anim.Play("Dead");
                StartCoroutine(PlayerDead());
            }
        }
    }
    public IEnumerator Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            AttackVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z)) - gameObject.transform.position);
            GetComponentInChildren<Shooter_Base>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            GetComponentInChildren<Shooter_Base>().transform.Rotate(0, 0, Angle_360(AttackVector));
            GetComponentInChildren<Shooter_Base>().GetComponentInChildren<Animator>().Play("Attack");
            yield return new WaitForSeconds(AttackInterval_Final);
            canAttack = true;
        }
    }
    public IEnumerator MeleeEnergyDecrease()
    {
        canDecrease = false;
        MeleeLevelUI.text = "LV: " + (MeleeLevel + 1);
        MeleeLevelHint.GetComponent<Text>().text = "Pree Q to RAGE: LV." + (MeleeLevel + 1);
        if (MeleeLevel > 0)
        {
            MeleeLevelHint.SetActive(true);
        }
        else
        {
            MeleeLevelHint.SetActive(false);
        }
        if (MeleeEnergy > 0)
        {
            MeleeEnergy -= MeleeEnergyDecreaseAmount;
        }
        if (MeleeEnergy <= 0 && MeleeLevel > 0)
        {
            MeleeLevel -= 1;
            MeleeEnergy += MeleeEnergyMax;
        }
        if (MeleeEnergy <= 0 && MeleeLevel == 0)
        {
            onRage = false;
            MeleeEnergyDecreaseAmount = 0.01f;
        }
        yield return new WaitForSeconds(0.1f);
        canDecrease = true;
    }
    public void MeleeEnergyDecreaseOfShooting()
    {
        if (canShoot && !onRage)
        {
            if (MeleeEnergy > 0)
            {
                MeleeEnergy -= RangeEnergyPerHit;
            }
            if (MeleeEnergy <= 0 && MeleeLevel > 0)
            {
                MeleeLevel -= 1;
                MeleeLevelUI.text = "LV: " + MeleeLevel;
                MeleeEnergy = MeleeEnergyMax;
            }
        }
    }
    public IEnumerator PlayerDead()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Studio");
    }
}