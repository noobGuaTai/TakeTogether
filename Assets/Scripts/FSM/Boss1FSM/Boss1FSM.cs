using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;



public enum Boss1StateType
{
    Idle,
    MeleeAttack,
    RemoteAttack,
    ChangePeriod,
    Dead
}

[Serializable]
public class Boss1Parameters
{
    public float meleeAttackDetRadius = 50f;
    public float remoteAttackDetRadius = 200f;
    public LayerMask playerLayer;

    public bool isMeleeAttackDetected = false;
    public bool isRemoteAttackDetected = false;
    public Vector3 randomDestination;
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject closedPlayer;
    public GameObject barrage1Prefab;
    public GameObject barrage2Prefab;
    public GameObject meleeAttackTect;
    public Boss1Attribute boss1Attribute;

}

public class Boss1FSM : EnemyMove
{
    public Boss1Parameters parameters;
    public IState currentState;
    public Dictionary<Boss1StateType, IState> state = new Dictionary<Boss1StateType, IState>();

    private double time;


    void Start()
    {
        state.Add(Boss1StateType.Idle, new Boss1IdleState(this));
        state.Add(Boss1StateType.MeleeAttack, new Boss1MeleeAttackState(this));
        state.Add(Boss1StateType.RemoteAttack, new Boss1RemoteAttackState(this));
        state.Add(Boss1StateType.ChangePeriod, new Boss1ChangePeriodState(this));
        state.Add(Boss1StateType.Dead, new Boss1DeadState(this));
        ChangeState(Boss1StateType.Idle);

        time = NetworkTime.time;
        parameters.boss1Attribute = GetComponent<Boss1Attribute>();
    }


    void Update()
    {
        if (isServer)
        {
            if (NetworkTime.time - time > 2)// 播放完出生动画
            {
                parameters.isMeleeAttackDetected = Physics2D.OverlapCircle(transform.position, parameters.meleeAttackDetRadius, parameters.playerLayer);

                if (Physics2D.OverlapCircle(transform.position, parameters.remoteAttackDetRadius, parameters.playerLayer))
                {
                    parameters.isRemoteAttackDetected = true;
                    parameters.closedPlayer = FindClosestPlayer();
                }
                else
                {
                    parameters.isRemoteAttackDetected = false;
                    parameters.closedPlayer = null;
                }

                currentState.OnUpdate();
                if (parameters.boss1Attribute.HP <= 0)
                {
                    ChangeState(Boss1StateType.Dead);
                }
            }
        }
    }

    [Server]
    public void ChangeState(Boss1StateType stateType)
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

    public void ChangeXScale()
    {
        if (parameters.closedPlayer != null)
        {
            Vector2 myPosition = transform.position;
            Vector2 playerPosition = parameters.closedPlayer.transform.position;

            if (playerPosition.x < myPosition.x)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, parameters.meleeAttackDetRadius);
        Gizmos.DrawWireSphere(transform.position, parameters.remoteAttackDetRadius);
    }

}
