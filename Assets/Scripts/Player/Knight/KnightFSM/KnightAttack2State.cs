using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class KnightAttack2State : IState
{
    private KnightFSM knightFSM;
    private KnightParameters parameters;

    public KnightAttack2State(KnightFSM knightFSM)
    {
        this.knightFSM = knightFSM;
        this.parameters = knightFSM.parameters;
    }

    public void OnEnter()
    {
        knightFSM.StartCoroutine(Dash());
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
    }

    IEnumerator Dash()
    {
        parameters.isDashing = true;
        parameters.playerAttribute.isInvincible = true;//冲刺期间无敌
        knightFSM.UpdateMP(-parameters.playerAttribute.MPConsume);
        knightFSM.transform.Find("KnightAttack2Tect").GetComponent<Collider2D>().enabled = true;
        parameters.dashTimeLeft = parameters.dashTime;
        knightFSM.ShowAnim("Dash-Attack");
        if(parameters.moveInput != Vector2.zero)
            parameters.rb.velocity = parameters.moveInput * parameters.dashSpeed;
        else
            parameters.rb.velocity = new Vector2(knightFSM.transform.localScale.x,0) * parameters.dashSpeed;

        knightFSM.CommandForSpawnTrail();
        yield return new WaitForSeconds(parameters.dashTime / 2);
        knightFSM.CommandForSpawnTrail();
        yield return new WaitForSeconds(parameters.dashTime / 2);
        knightFSM.CommandForSpawnTrail();

        // 结束冲刺，重置状态
        parameters.rb.velocity = Vector2.zero;
        parameters.isDashing = false;
        parameters.playerAttribute.isInvincible = false;
        knightFSM.transform.Find("KnightAttack2Tect").GetComponent<Collider2D>().enabled = false;
        // knightFSM.ShowAnim("dash");

        //退出状态
        knightFSM.ChangeState(KnightStateType.Idle);
    }


}
