using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2ChaseState : IState
{
    private Enemy2FSM enemy2FSM;
    private Enemy2Parameters parameters;

    public Enemy2ChaseState(Enemy2FSM enemy2FSM)
    {
        this.enemy2FSM = enemy2FSM;
        this.parameters = enemy2FSM.parameters;
    }

    public void OnEnter()
    {
        if (enemy2FSM.isServer)
            enemy2FSM.ShowAnim("float");
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnUpdate()
    {
        if (enemy2FSM.isServer)
        {
            if (!parameters.isPlayerDetected)
            {
                enemy2FSM.ChangeState(Enemy2StateType.Idle);
                return;
            }
            if (parameters.isAttacking)
            {
                parameters.closedPlayer = enemy2FSM.FindClosestPlayer();
                enemy2FSM.ChangeState(Enemy2StateType.Attack);
                return;
            }
            MoveTowards(parameters.closedPlayer.transform.position);
        }

    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 moveDirection = (target - enemy2FSM.transform.position).normalized;
        parameters.rb.velocity = moveDirection * parameters.moveSpeed;

        // 设置一个死区阈值，避免玩家和敌人非常接近时的快速翻转
        float flipDeadZone = 0.1f;

        if (Mathf.Abs(target.x - enemy2FSM.transform.position.x) > flipDeadZone)
        {
            if (moveDirection.x > 0)
            {
                enemy2FSM.transform.localScale = new Vector3(Mathf.Abs(enemy2FSM.transform.localScale.x), enemy2FSM.transform.localScale.y, enemy2FSM.transform.localScale.z);
            }
            else if (moveDirection.x < 0)
            {
                enemy2FSM.transform.localScale = new Vector3(-Mathf.Abs(enemy2FSM.transform.localScale.x), enemy2FSM.transform.localScale.y, enemy2FSM.transform.localScale.z);
            }
        }
    }


}
