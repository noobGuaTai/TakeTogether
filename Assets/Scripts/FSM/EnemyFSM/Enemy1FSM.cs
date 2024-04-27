using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;



public enum Enemy1StateType
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Dead
}

[Serializable]
public class Parameters
{
    public float detectionRadius = 100f;
    public float moveSpeed = 80f;
    public float randomMoveDistance = 100f;
    public float attackDetectionRadius = 30f;
    public LayerMask playerLayer;
    public GameObject enemy1AttackTect;

    public bool isPlayerDetected = false;
    public bool isAttacking = false;
    public Vector3 randomDestination;
    public float moveTimer = 3f;
    public bool canMove = false;
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject closedPlayer;


}

public class Enemy1FSM : EnemyMove
{
    public Parameters parameters;
    public IState currentState;
    public Dictionary<Enemy1StateType, IState> state = new Dictionary<Enemy1StateType, IState>();

    private double time;
    private Enemy1Attribute enemy1Attribute;


    void Start()
    {
        state.Add(Enemy1StateType.Idle, new Enemy1IdleState(this));
        state.Add(Enemy1StateType.Patrol, new Enemy1PatrolState(this));
        state.Add(Enemy1StateType.Chase, new Enemy1ChaseState(this));
        state.Add(Enemy1StateType.Attack, new Enemy1AttackState(this));
        state.Add(Enemy1StateType.Dead, new Enemy1DeadState(this));
        // currentState = state[Enemy1StateType.Idle];
        ChangeState(Enemy1StateType.Idle);

        time = NetworkTime.time;
        enemy1Attribute = GetComponent<Enemy1Attribute>();

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
                if(enemy1Attribute.HP <= 0)
                {
                    ChangeState(Enemy1StateType.Dead);
                }
            }
        }

    }

    public void ChangeState(Enemy1StateType stateType)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = state[stateType];
        currentState.OnEnter();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, parameters.detectionRadius);
        Gizmos.DrawWireSphere(transform.position, parameters.attackDetectionRadius);
    }

}
