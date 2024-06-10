using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttack1State : IState
{
    private KnightFSM knightFSM;
    private KnightParameters parameters;

    public KnightAttack1State(KnightFSM knightFSM)
    {
        this.knightFSM = knightFSM;
        this.parameters = knightFSM.parameters;
    }

    public void OnEnter()
    {
        knightFSM.StartCoroutine(StartAttack());
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        if (!knightFSM.isLocalPlayer) return;
        
        if(Input.GetButtonDown("Attack2") && !parameters.isAttacking && parameters.playerAttribute.MP >= parameters.playerAttribute.MPConsume)
        {
            knightFSM.ChangeState(KnightStateType.Attack2);
        }
        else if(Input.GetButtonDown("Attack3") && !parameters.isAttacking)
        {
            knightFSM.ChangeState(KnightStateType.Attack3);
        }
    }

    IEnumerator StartAttack()
    {
        parameters.isAttacking = true;
        knightFSM.ShowAnim("Attack");
        knightFSM.transform.Find("KnightAttackTect").GetComponent<Collider2D>().enabled = true;

        yield return new WaitForSeconds(parameters.attackDuration); // 等待攻击动画完成

        parameters.isAttacking = false;
        // knightFSM.ShowAnim("attack");
        knightFSM.transform.Find("KnightAttackTect").GetComponent<Collider2D>().enabled = false;

        knightFSM.ChangeState(KnightStateType.Idle);
    }


}
