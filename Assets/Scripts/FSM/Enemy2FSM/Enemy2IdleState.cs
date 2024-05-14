using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2IdleState : IState
{
    private Enemy2FSM enemy2FSM;
    private Enemy2Parameters parameters;

    public Enemy2IdleState(Enemy2FSM enemy2FSM)
    {
        this.enemy2FSM = enemy2FSM;
        this.parameters = enemy2FSM.parameters;
    }

    public void OnEnter()
    {
        if (enemy2FSM.isServer)
            enemy2FSM.ShowAnim("float");
        parameters.rb.velocity = Vector2.zero;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (parameters.isAttacking)
        {
            parameters.closedPlayer = enemy2FSM.FindClosestPlayer();
            enemy2FSM.ChangeState(Enemy2StateType.Attack);
            return;
        }
    }


}
