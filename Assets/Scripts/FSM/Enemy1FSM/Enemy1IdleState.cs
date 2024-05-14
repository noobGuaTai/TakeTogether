using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Enemy1IdleState : IState
{
    private Enemy1Parameters parameters;
    private Enemy1FSM enemy1FSM;
    private double time;
    private bool flag = false;// 记录第一次运行update的时间

    public Enemy1IdleState(Enemy1FSM enemy1FSM)
    {
        this.enemy1FSM = enemy1FSM;
        this.parameters = enemy1FSM.parameters;
    }

    public void OnEnter()
    {
        if (enemy1FSM.isServer)
            enemy1FSM.ShowAnim("idle");
        time = NetworkTime.time;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (enemy1FSM.isServer)
        {
            parameters.rb.velocity = Vector2.zero;
            if (NetworkTime.time - time > 2)
            {
                enemy1FSM.ChangeState(Enemy1StateType.Patrol);
            }

            if (parameters.isPlayerDetected)
            {
                parameters.closedPlayer = enemy1FSM.FindClosestPlayer();
                enemy1FSM.ChangeState(Enemy1StateType.Chase);
                return;
            }
        }

    }
}
