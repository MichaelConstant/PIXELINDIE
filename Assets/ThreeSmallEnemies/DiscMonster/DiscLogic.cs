using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscLogic : CharacterControl
{
    Vector2 enemyDirection;
    public float m_moveSpeed;
    void Start() 
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = (enemyDirection * m_moveSpeed);
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null)
        {
            rb.velocity*= (-1);
        }
    }
}
