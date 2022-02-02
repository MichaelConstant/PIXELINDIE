using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ErrorMonsterLogic : CharacterControl
{
    public int bulletNumber;
    public float bulletSpeed;
    public GameObject ErrorBullet;
    public EnemyState m_errorState = EnemyState.Spawning;
    public int rad;
    
    public float staticTime;


    GameObject m_player;
    Rigidbody2D m_rigidbody;
    Animator anim;
    public bool animationFinished = false;
    public enum EnemyState
    {
        Spawning,
        Static,
        Hiding,
        Transforming,
    }
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_rigidbody = GetComponentInParent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        switch (m_errorState)
        {
            case EnemyState.Spawning:
                anim.Play("Spawn");
                if (animationFinished)
                {
                    Shoot();
                    m_errorState = EnemyState.Static;
                }
                break;
            case EnemyState.Static:
                StartCoroutine(Wait(staticTime));
                    m_errorState = EnemyState.Hiding;
                break;
            case EnemyState.Hiding:
                anim.Play("Hide");
                if (animationFinished)
                {
                    m_errorState = EnemyState.Transforming;
                }
                break;
            case EnemyState.Transforming:
                {
                    float randomX = Random.Range(m_player.transform.position.x - rad, m_player.transform.position.x + rad);
                    float randomY = Random.Range(m_player.transform.position.y - rad, m_player.transform.position.y + rad);
                    this.transform.position =new Vector3 (randomX, randomY, m_player.transform.position.z);

                    //m_rigidbody.velocity = getDirection(m_player.transform) * 1f;
                    //StartCoroutine(WaitForSeconds(staticTime-0.01f, () =>
                    //{
                    //    if (this.transform.position.x<= m_player.transform.position.x - rad|| this.transform.position.x >=m_player.transform.position.x + rad||
                    //    this.transform.position.y< m_player.transform.position.y - rad||this.transform.position.y> m_player.transform.position.y + rad)
                    //    {
                    //        m_rigidbody.velocity = Vector2.zero;
                    //    }
                    //}));

                    m_errorState = EnemyState.Spawning;
                }
                break;
            default:
                break;
        }

    }
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }


    void Shoot()
    {
        for (int i = 0; i < bulletNumber; i++)
        {
            GameObject bullet = Instantiate(ErrorBullet);
            bullet.transform.parent = this.transform;
            bullet.transform.position = this.transform.position;
            bullet.transform.eulerAngles = new Vector3(.0f, .0f, 360.0f / bulletNumber * i);
        }
    }
}