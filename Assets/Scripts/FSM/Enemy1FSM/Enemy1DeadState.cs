using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1DeadState : IState
{
    private Enemy1FSM enemy1FSM;
    private Enemy1Parameters parameters;

    public Enemy1DeadState(Enemy1FSM enemy1FSM)
    {
        this.enemy1FSM = enemy1FSM;
        this.parameters = enemy1FSM.parameters;
    }

    public void OnEnter()
    {
        parameters.rb.velocity = Vector2.zero;
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}
