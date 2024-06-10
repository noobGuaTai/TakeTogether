using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy2AttackState : IState
{
    private Enemy2FSM enemy2FSM;
    private Enemy2Parameters parameters;
    private double lastAttackTime;
    private double attackCoolDown = 2f;
    private bool isCoroutineRunning = false;

    public Enemy2AttackState(Enemy2FSM enemy2FSM)
    {
        this.enemy2FSM = enemy2FSM;
        this.parameters = enemy2FSM.parameters;
    }

    public void OnEnter()
    {
        lastAttackTime = NetworkTime.time - attackCoolDown + 0.5f;// 初次进入状态直接攻击
        parameters.rb.velocity = Vector2.zero;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (enemy2FSM.isServer)
        {
            if (!parameters.isAttacking && parameters.isPlayerDetected && !isCoroutineRunning)
            {
                enemy2FSM.ChangeState(Enemy2StateType.Chase);
            }

            if (NetworkTime.time - lastAttackTime > attackCoolDown)
            {
                ShowAttackAnimRpc();
                lastAttackTime = NetworkTime.time;
                enemy2FSM.StartCoroutine(AttackCoroutine());
            }
        }

    }

    IEnumerator AttackCoroutine()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(1.3f);
        if(parameters.enemy2Attribute.HP <= 0)
        {
            yield break;
        }
        var barrage2Instance = UnityEngine.Object.Instantiate(parameters.enemy2Barrage, enemy2FSM.transform.position, Quaternion.identity);
        NetworkServer.Spawn(barrage2Instance);
        UnityEngine.Object.Destroy(barrage2Instance, 6f);
        attackCoolDown = UnityEngine.Random.Range(1.3f, 3f); // 随机冷却时间
        isCoroutineRunning = false;
    }

    [ClientRpc]
    void ShowAttackAnimRpc()
    {
        enemy2FSM.StartCoroutine(ShowAttackAnimCoroutine());
    }

    IEnumerator ShowAttackAnimCoroutine()
    {
        if (parameters.anim != null)
        {
            parameters.anim.Play("attack");
            yield return new WaitForSeconds(1.6f);
            if(parameters.enemy2Attribute.HP > 0)
                parameters.anim.Play("float");
        }
    }

    bool IsAnimationDone(string stateName)
    {
        AnimatorStateInfo stateInfo = parameters.anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1.0f;
    }

    public void OnFixedUpdate()
    {
        
    }
}
