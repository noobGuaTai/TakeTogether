using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1DeadState : IState
{
    private Enemy1FSM enemy1FSM;

    public Enemy1DeadState(Enemy1FSM enemy1FSM)
    {
        this.enemy1FSM = enemy1FSM;
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}
