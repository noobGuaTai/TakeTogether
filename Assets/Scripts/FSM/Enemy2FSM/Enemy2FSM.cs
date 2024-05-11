using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;



public enum Enemy2StateType
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Dead
}

[Serializable]
public class Enemy2Parameters
{
    public float detectionRadius = 100f;
    public float moveSpeed = 40f;
    public float attackDetectionRadius = 75f;
    public LayerMask playerLayer;
    public GameObject enemy2AttackTect;

    public bool isPlayerDetected = false;
    public bool isAttacking = false;
    public bool canMove = false;
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject closedPlayer;
    public GameObject enemy2Barrage;


}

public class Enemy2FSM : EnemyMove
{
    public Enemy2Parameters parameters;
    public IState currentState;
    public Dictionary<Enemy2StateType, IState> state = new Dictionary<Enemy2StateType, IState>();

    private double time;
    private Enemy2Attribute enemy2Attribute;


    void Start()
    {
        state.Add(Enemy2StateType.Idle, new Enemy2IdleState(this));
        state.Add(Enemy2StateType.Chase, new Enemy2ChaseState(this));
        state.Add(Enemy2StateType.Attack, new Enemy2AttackState(this));
        state.Add(Enemy2StateType.Dead, new Enemy2DeadState(this));
        // currentState = state[Enemy1StateType.Idle];
        ChangeState(Enemy2StateType.Idle);

        time = NetworkTime.time;
        enemy2Attribute = GetComponent<Enemy2Attribute>();

    }


    void Update()
    {
        if (isServer)
        {
            if (NetworkTime.time - time > 2)// 播放完出生动画
            {
                parameters.isPlayerDetected = Physics2D.OverlapCircle(transform.position, parameters.detectionRadius, parameters.playerLayer);
                parameters.isAttacking = Physics2D.OverlapCircle(transform.position, parameters.attackDetectionRadius, parameters.playerLayer);
                currentState.OnUpdate();
                if (enemy2Attribute.HP <= 0)
                {
                    ChangeState(Enemy2StateType.Dead);
                }
            }
        }

    }

    public void ChangeState(Enemy2StateType stateType)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = state[stateType];
        currentState.OnEnter();
    }

    [ClientRpc]
    public void ShowAnim(string name)
    {
        if (parameters.anim != null)
        {
            parameters.anim.Play(name);
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, parameters.detectionRadius);
        Gizmos.DrawWireSphere(transform.position, parameters.attackDetectionRadius);
    }

}
