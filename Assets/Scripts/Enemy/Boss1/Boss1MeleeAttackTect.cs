using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Boss1MeleeAttackTect : EnemyAttackTect
{
    public bool attackTrigger = false;

    private EnemyAttribute enemyAttribute;
    private float timeBeforeAttack = 1f;
    private double time;
    private Animator anim;
    private Collider2D collider;


    void Start()
    {
        enemyAttribute = GetComponentInParent<EnemyAttribute>();
        anim = GetComponentInParent<Animator>();
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }


    void Update()
    {

    }

    public override void AttackPlayer(Collider2D other)// 有bug，两个玩家站在攻击范围内，只有一个会受伤，attackTrigger得改
    {
        if (IsAnimationDone(0.75f, "attack") && !attackTrigger)// 如果当前动画中未攻击
        {
            other.GetComponent<PlayerAttribute>().ChangeHP(-enemyAttribute.ATK);
            Debug.Log("Player HP: " + other.GetComponent<PlayerAttribute>().HP);
            attackTrigger = true;
            collider.enabled = false;
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
