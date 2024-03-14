using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Attribute : EnemyAttribute
{
    private Animator anim;
    private Rigidbody2D rb;
    void Awake()
    {
        HP = 100f;
        ATK = 10f;
    }

    void Start()
    {
        anim = GetComponentInParent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        Die();
    }

    public override void ChangeHP(float value)
    {
        HP += value;
    }

    public override void Die()
    {
        if(HP <= 0)
        {
            anim.SetBool("death", true);
            GetComponent<EnemyMove>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            rb.velocity = Vector2.zero;
            Destroy(gameObject, 1f);
        }
        
    }

}
