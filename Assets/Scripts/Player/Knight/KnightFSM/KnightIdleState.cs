using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class KnightIdleState : IState
{
    private KnightFSM knightFSM;
    private KnightParameters parameters;

    public KnightIdleState(KnightFSM knightFSM)
    {
        this.knightFSM = knightFSM;
        this.parameters = knightFSM.parameters;
    }

    public void OnEnter()
    {
        knightFSM.ShowAnim("Idle");
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        if (parameters.moveInput.x != 0 || parameters.moveInput.y != 0)
        {
            knightFSM.ChangeState(KnightStateType.Move);
        }

        if (Input.GetButton("Attack"))
        {
            knightFSM.ChangeState(KnightStateType.Attack1);
        }

        if (Input.GetButton("Attack2") && parameters.playerAttribute.MP >= parameters.playerAttribute.MPConsume)
        {
            knightFSM.ChangeState(KnightStateType.Attack2);
        }

        if (Input.GetButton("Attack") && parameters.playerAttribute.CTP >= 10f)
        {
            knightFSM.ChangeState(KnightStateType.Attack3);
        }
    }


}
