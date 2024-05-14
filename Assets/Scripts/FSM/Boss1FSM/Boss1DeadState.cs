using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1DeadState : IState
{
    private Boss1FSM boss1FSM;
    private Boss1Parameters parameters;

    public Boss1DeadState(Boss1FSM boss1FSM)
    {
        this.boss1FSM = boss1FSM;
        this.parameters = boss1FSM.parameters;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}
