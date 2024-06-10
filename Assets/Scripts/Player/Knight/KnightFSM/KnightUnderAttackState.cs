using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightUnderAttackState : IState
{
    private KnightFSM knightFSM;
    private KnightParameters parameters;

    public KnightUnderAttackState(KnightFSM knightFSM)
    {
        this.knightFSM = knightFSM;
        this.parameters = knightFSM.parameters;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnUpdate()
    {
        
    }


}
