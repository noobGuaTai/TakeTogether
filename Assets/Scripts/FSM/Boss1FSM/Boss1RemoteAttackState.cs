using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Billiards;
using UnityEngine;

public class Boss1RemoteAttackState : IState
{
    private Boss1FSM boss1FSM;
    private Boss1Parameters parameters;
    private double lastAttackTime;
    private int chooseAttack;
    private double attackCoolDown;
    private bool isCoroutineRunning = false;
    private int attackCount = 0;
    private int rushThreshold = 0;// 释放突进攻击的远程攻击次数阈值，远程攻击次数到达阈值后释放突进攻击

    public Boss1RemoteAttackState(Boss1FSM boss1FSM)
    {
        this.boss1FSM = boss1FSM;
        this.parameters = boss1FSM.parameters;
    }

    public void OnEnter()
    {
        if (boss1FSM.isServer)
        {
            parameters.rb.velocity = Vector2.zero;
            rushThreshold = UnityEngine.Random.Range(3, 6);
        }

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (boss1FSM.isServer)
        {
            if (NetworkTime.time - lastAttackTime > attackCoolDown)
            {
                boss1FSM.ChangeXScale();
                lastAttackTime = NetworkTime.time;
                chooseAttack = UnityEngine.Random.Range(1, 3); // 随机选择攻击模式
                RpcPrepareAttack(); // 客户端播放攻击动画
                ServerPerformAttack(chooseAttack); // 在服务器上执行攻击实例化
                attackCoolDown = UnityEngine.Random.Range(2f, 4f); // 随机冷却时间
            }
            if (parameters.isMeleeAttackDetected && !isCoroutineRunning)
            {
                boss1FSM.ChangeState(Boss1StateType.MeleeAttack);
            }
            if (attackCount >= rushThreshold && !isCoroutineRunning)
            {
                RpcPrepareAttack();
                boss1FSM.StartCoroutine(RushAttack());
                attackCount = 0;
            }
        }

    }

    [ClientRpc]
    void RpcPrepareAttack()
    {
        boss1FSM.StartCoroutine(PerformAttackAnimation());
    }

    IEnumerator PerformAttackAnimation()
    {
        boss1FSM.ShowAnim("attack");
        yield return new WaitForSeconds(1.3f);
        boss1FSM.ShowAnim("idle");
    }


    [Server]
    void ServerPerformAttack(int attackType)
    {
        boss1FSM.StartCoroutine(ServerPerformAttackCoroutine(attackType));
    }

    IEnumerator ServerPerformAttackCoroutine(int attackType)
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(1.3f);
        // 在服务器上根据攻击类型实例化不同的攻击效果
        if (attackType == 1)
        {
            var barrage1Instance = UnityEngine.Object.Instantiate(parameters.barrage1Prefab, boss1FSM.transform.position, Quaternion.identity);
            NetworkServer.Spawn(barrage1Instance);
            UnityEngine.Object.Destroy(barrage1Instance, 6f);
        }
        else if (attackType == 2)
        {
            var barrage2Instance = UnityEngine.Object.Instantiate(parameters.barrage2Prefab, boss1FSM.transform.position, Quaternion.identity);
            NetworkServer.Spawn(barrage2Instance);
            UnityEngine.Object.Destroy(barrage2Instance, 6f);
        }
        attackCount++;
        isCoroutineRunning = false;
    }

    IEnumerator RushAttack()
    {
        Vector2 targetPosition = parameters.closedPlayer.transform.position;
        Vector2 startPosition = boss1FSM.transform.position;
        float duration = 1.0f;
        float elapsed = 0;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            boss1FSM.transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        boss1FSM.transform.position = targetPosition;
    }



}
