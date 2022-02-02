using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KouKou : CharacterControl
{
    PlayerControl Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AttackVector = (Player.transform.position - transform.position).normalized;
        rb.velocity = AttackVector * movementSpeed_Final;

        if(rb.velocity.x>0)
        {
            sr.flipX = true;
            transform.GetChild(0).transform.localPosition = new Vector2(0.06f, 0.08f);
        }
        else
        {
            sr.flipX = false;
            transform.GetChild(0).transform.localPosition = new Vector2(-0.06f, 0.08f);
        }

        AnimatorStateInfo Info = anim.GetCurrentAnimatorStateInfo(0);
        if (Info.normalizedTime > 1f)
        {
            if (currentHP <= 0)
            {
                gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                Destroy(gameObject);
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk2"))
            {
                anim.Play("Walk1");
                CommonShoot();
            }
            else
            {
                anim.Play("Walk2");
                CommonShoot();
            }
        }
        if (currentHP <= 0)
        {
            rb.velocity = new Vector3(0, 0, 0);
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            anim.Play("Dead");
        }
    }
}
