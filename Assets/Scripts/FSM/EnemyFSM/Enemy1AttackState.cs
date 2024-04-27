using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Enemy1AttackState : IState
{
    private Enemy1FSM enemy1FSM;
    private Parameters parameters;
    private double timer;

    public Enemy1AttackState(Enemy1FSM enemy1FSM)
    {
        this.enemy1FSM = enemy1FSM;
        this.parameters = enemy1FSM.parameters;
    }

    public void OnEnter()
    {
        parameters.anim.Play("attack");
        timer = NetworkTime.time;
        parameters.rb.velocity = Vector2.zero;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        parameters.rb.velocity = Vector2.zero;
        if (!parameters.isAttacking && IsAnimationDone("attack"))
        {
            enemy1FSM.ChangeState(Enemy1StateType.Chase);
        }
    }

    bool IsAnimationDone(string stateName)
    {
        AnimatorStateInfo stateInfo = parameters.anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1.0f;
    }
}
