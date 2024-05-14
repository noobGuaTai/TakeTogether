using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Boss1MeleeAttackState : IState
{
    private Boss1FSM boss1FSM;
    private Boss1Parameters parameters;

    private double lastAttackTime;
    private int chooseAttack;
    private double attackCoolDown;
    private bool isCoroutineRunning = false;
    private int attackCount = 0;
    private int underAttackCount = 0;
    private int underAttackThreshold = 0;// 释放反击的受击阈值，攻击时受击会添加反击值，到达之后会释放反击
    public float knockbackForce = 300f;  // 击退力
    public float lastHP;

    public Boss1MeleeAttackState(Boss1FSM boss1FSM)
    {
        this.boss1FSM = boss1FSM;
        this.parameters = boss1FSM.parameters;
    }

    public void OnEnter()
    {
        if (boss1FSM.isServer)
        {
            parameters.rb.velocity = Vector2.zero;
            underAttackThreshold = UnityEngine.Random.Range(2, 3);
            lastHP = parameters.boss1Attribute.HP;
        }

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (boss1FSM.isServer)
        {
            if (!parameters.isMeleeAttackDetected && parameters.isRemoteAttackDetected && !isCoroutineRunning)
            {
                boss1FSM.ChangeState(Boss1StateType.RemoteAttack);
            }
            if (NetworkTime.time - lastAttackTime > attackCoolDown)
            {
                boss1FSM.ChangeXScale();
                lastAttackTime = NetworkTime.time;
                chooseAttack = UnityEngine.Random.Range(1, 2); // 随机选择攻击模式
                //RpcPrepareAttack(); // 客户端播放攻击动画
                ServerPerformAttack(chooseAttack); // 在服务器上执行攻击实例化
                attackCoolDown = UnityEngine.Random.Range(1.3f, 2f); // 随机冷却时间
            }
            if (underAttackCount >= underAttackThreshold && !isCoroutineRunning)
            {
                RpcPrepareAttack();
                CounterAttack();
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
        if (underAttackCount >= underAttackThreshold)
            yield break;
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
        // 在服务器上根据攻击类型实例化不同的攻击效果
        if (attackType == 1)
        {
            RpcPrepareAttack();

            if (parameters.boss1Attribute.HP < lastHP)
            {
                underAttackCount++;
                lastHP = parameters.boss1Attribute.HP;
            }
            if (underAttackCount >= underAttackThreshold)
            {
                isCoroutineRunning = false;
                yield break;
            }
            yield return new WaitForSeconds(1.3f);
            parameters.meleeAttackTect.GetComponent<Collider2D>().enabled = true;
        }

        isCoroutineRunning = false;
    }

    void CounterAttack()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(boss1FSM.transform.position, parameters.meleeAttackDetRadius, parameters.playerLayer);

        foreach (Collider2D player in hitPlayers)
        {
            Vector2 knockbackDirection = (player.transform.position - boss1FSM.transform.position).normalized;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
        underAttackCount = 0;
    }

}
