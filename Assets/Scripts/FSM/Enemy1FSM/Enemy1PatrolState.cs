using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Enemy1PatrolState : IState
{
    private Enemy1FSM enemy1FSM;
    private Enemy1Parameters parameters;
    private double timer;
    private double walkCoolDown = 3f;// 巡逻时间间隔

    public Enemy1PatrolState(Enemy1FSM enemy1FSM)
    {
        this.enemy1FSM = enemy1FSM;
        this.parameters = enemy1FSM.parameters;
    }

    public void OnEnter()
    {
        timer = NetworkTime.time;
        if (enemy1FSM.isServer)
            enemy1FSM.ShowAnim("run");
        walkCoolDown = 0f;// 第一次巡逻无需等待
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (enemy1FSM.isServer)
        {
            if (parameters.isPlayerDetected)
            {
                parameters.closedPlayer = enemy1FSM.FindClosestPlayer();
                enemy1FSM.ChangeState(Enemy1StateType.Chase);
                return;
            }
            else if (NetworkTime.time - timer > walkCoolDown)// 巡逻
            {
                walkCoolDown = 3f;// 恢复
                parameters.randomDestination = enemy1FSM.transform.position + Random.insideUnitSphere * parameters.randomMoveDistance;
                parameters.randomDestination.z = 0;
                MoveTowards(parameters.randomDestination);
                if (NetworkTime.time - timer > walkCoolDown)
                {
                    enemy1FSM.ChangeState(Enemy1StateType.Idle);
                    return;
                }
            }
        }

    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 moveDirection = (target - enemy1FSM.transform.position).normalized;
        parameters.rb.velocity = moveDirection * parameters.moveSpeed;

        // 设置一个死区阈值，避免玩家和敌人非常接近时的快速翻转
        float flipDeadZone = 0.1f;

        if (Mathf.Abs(target.x - enemy1FSM.transform.position.x) > flipDeadZone)
        {
            if (moveDirection.x > 0)
            {
                enemy1FSM.transform.localScale = new Vector3(-Mathf.Abs(enemy1FSM.transform.localScale.x), enemy1FSM.transform.localScale.y, enemy1FSM.transform.localScale.z);
            }
            else if (moveDirection.x < 0)
            {
                enemy1FSM.transform.localScale = new Vector3(Mathf.Abs(enemy1FSM.transform.localScale.x), enemy1FSM.transform.localScale.y, enemy1FSM.transform.localScale.z);
            }
        }
    }

}
