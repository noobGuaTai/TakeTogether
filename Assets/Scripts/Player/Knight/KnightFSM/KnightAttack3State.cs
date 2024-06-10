using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttack3State : IState
{
    private KnightFSM knightFSM;
    private KnightParameters parameters;

    public KnightAttack3State(KnightFSM knightFSM)
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
