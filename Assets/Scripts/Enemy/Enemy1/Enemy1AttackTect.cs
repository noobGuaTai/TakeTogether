using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Enemy1AttackTect : EnemyAttackTect
{
    public float attackCooldown = 1f;
    public bool attackTrigger = false;

    private EnemyAttribute enemyAttribute;
    private float timeBeforeAttack = 1f;
    private double time;
    private Animator anim;


    void Start()
    {
        enemyAttribute = GetComponentInParent<EnemyAttribute>();
        anim = GetComponentInParent<Animator>();
    }


    void Update()
    {

    }

    public override void AttackPlayer(Collider2D other)
    {
        if (IsAnimationDone(0.75f, "attack") && !attackTrigger)// 如果当前动画中未攻击
        {
            other.GetComponent<PlayerAttribute>().ChangeHP(-enemyAttribute.ATK);
            Debug.Log("Player HP: " + other.GetComponent<PlayerAttribute>().HP);
            attackTrigger = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AttackPlayer(other);
        }
    }

    bool IsAnimationDone(float time, string stateName)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName(stateName) && stateInfo.normalizedTime % 1.0 <= time)
        {
            attackTrigger = false;// 攻击前摇，处于可以攻击状态
        }
        return stateInfo.IsName(stateName) && stateInfo.normalizedTime % 1.0 >= time;
    }

}
