using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1AttackTect : EnemyAttackTect
{
    public float attackCooldown = 1f;

    private float lastAttackTime;
    private EnemyAttribute enemyAttribute;
    

    void Start()
    {
        enemyAttribute = GetComponentInParent<EnemyAttribute>();
    }


    void Update()
    {

    }

    public override void AttackPlayer(Collider2D other)
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            other.GetComponent<PlayerAttribute>().ChangeHP(-enemyAttribute.ATK);
            Debug.Log("Player HP: " + other.GetComponent<PlayerAttribute>().HP);
            lastAttackTime = Time.time;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AttackPlayer(other);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AttackPlayer(other);
        }
    }

}
