using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1ChaseState : IState
{
    private Enemy1FSM enemy1FSM;
    private Enemy1Parameters parameters;

    public Enemy1ChaseState(Enemy1FSM enemy1FSM)
    {
        this.enemy1FSM = enemy1FSM;
        this.parameters = enemy1FSM.parameters;
    }

    public void OnEnter()
    {
        if (enemy1FSM.isServer)
            enemy1FSM.ShowAnim("run");
    }

    public void OnExit()
    {
        parameters.rb.velocity = Vector2.zero;
    }

    public void OnUpdate()
    {
        if (enemy1FSM.isServer)
        {
            if (!parameters.isPlayerDetected)
            {
                enemy1FSM.ChangeState(Enemy1StateType.Idle);
                return;
            }

            if (parameters.isAttacking)
            {
                enemy1FSM.ChangeState(Enemy1StateType.Attack);
            }
            MoveTowards(parameters.closedPlayer.transform.position);
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
