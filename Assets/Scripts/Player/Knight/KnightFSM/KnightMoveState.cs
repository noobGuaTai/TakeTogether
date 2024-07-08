using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMoveState : IState
{
    private KnightFSM knightFSM;
    private KnightParameters parameters;

    public KnightMoveState(KnightFSM knightFSM)
    {
        this.knightFSM = knightFSM;
        this.parameters = knightFSM.parameters;
    }

    public void OnEnter()
    {
        knightFSM.ShowAnim("Run");
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        if (parameters.moveInput.x == 0 && parameters.moveInput.y == 0)
        {
            knightFSM.ChangeState(KnightStateType.Idle);
        }

        if (Input.GetButton("Attack"))
        {
            knightFSM.ChangeState(KnightStateType.Attack1);
        }

        if (Input.GetButton("Attack2") && parameters.playerAttribute.MP >= parameters.playerAttribute.MPConsume)
        {
            knightFSM.ChangeState(KnightStateType.Attack2);
        }

        if (Input.GetButton("Attack") && parameters.playerAttribute.connectedAttackPoint >= 10f)
        {
            knightFSM.ChangeState(KnightStateType.Attack3);
        }
    }

    public void OnFixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if (!knightFSM.isLocalPlayer) return;
        parameters.rb.MovePosition(parameters.rb.position + parameters.moveInput * parameters.moveSpeed * Time.fixedDeltaTime);
        knightFSM.transform.localScale = new Vector3(Mathf.Sign(parameters.moveInput.x) * Mathf.Abs(knightFSM.transform.localScale.x), knightFSM.transform.localScale.y, knightFSM.transform.localScale.z);
    }


}
