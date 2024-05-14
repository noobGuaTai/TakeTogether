using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2DeadState : IState
{
    private Enemy2FSM enemy2FSM;
    private Enemy2Parameters parameters;

    public Enemy2DeadState(Enemy2FSM enemy2FSM)
    {
        this.enemy2FSM = enemy2FSM;
        this.parameters = enemy2FSM.parameters;
    }

    public void OnEnter()
    {
        if (enemy2FSM.isServer)
            enemy2FSM.ShowAnim("death");
    }

    public void OnExit()
    {
        parameters.rb.velocity = Vector2.zero;
    }

    public void OnUpdate()
    {

    }




}
