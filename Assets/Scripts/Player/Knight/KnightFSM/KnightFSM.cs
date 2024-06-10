using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;



public enum KnightStateType
{
    Idle,
    Move,
    Attack1,
    Attack2,
    Attack3,
    UnderAttack,
    Dead
}

[Serializable]
public class KnightParameters
{
    public float moveSpeed = 120f;
    public float dashSpeed = 400f;
    public float dashTime = 0.2f;
    public float attackDuration = 0.3f; // 攻击动画持续时间
    public Material trailMaterial; // 用于残影的材质
    public float trailSpawnInterval = 0.05f; // 残影生成间隔时间
    public GameObject CTcharsPrefab;
    public RectTransform UI;

    public Rigidbody2D rb;
    public Vector2 moveInput;
    public Animator anim;
    public PlayerAttribute playerAttribute;
    public PlayerManager playerManager;

    public bool isDashing;
    public float dashTimeLeft;
    public bool isAttacking;
    public float restoreSpeedMP = 1f;
    [SyncVar] public double lastRestoreMPTime;
    public float underAttackCoolDownTime = 1.0f; // 受击冷却时间为1秒
    public double lastHitTime = 0.0f; // 上次被击中的时间
    public char[] CTchars = { 'W', 'A', 'S', 'D', 'J', 'K', 'L' };


}

public class KnightFSM : PlayerMove
{
    public KnightParameters parameters;
    public IState currentState;
    public Dictionary<KnightStateType, IState> state = new Dictionary<KnightStateType, IState>();

    void Start()
    {
        state.Add(KnightStateType.Idle, new KnightIdleState(this));
        state.Add(KnightStateType.Move, new KnightMoveState(this));
        state.Add(KnightStateType.Attack1, new KnightAttack1State(this));
        state.Add(KnightStateType.Attack2, new KnightAttack2State(this));
        state.Add(KnightStateType.Attack3, new KnightAttack3State(this));
        state.Add(KnightStateType.UnderAttack, new KnightUnderAttackState(this));
        ChangeState(KnightStateType.Idle);

        parameters.rb = GetComponent<Rigidbody2D>();
        parameters.anim = GetComponent<Animator>();
        parameters.playerAttribute = GetComponent<PlayerAttribute>();
        parameters.playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        parameters.lastRestoreMPTime = NetworkTime.time;
        isPlayerInBossRoom = false;
        parameters.UI = GameObject.Find("UI").GetComponent<RectTransform>();

    }


    void Update()
    {
        if (isServer)
        {
            parameters.moveInput.x = Input.GetAxisRaw("Horizontal");
            parameters.moveInput.y = Input.GetAxisRaw("Vertical");
            parameters.moveInput.Normalize();

            if (isLocalPlayer && NetworkTime.time - parameters.lastRestoreMPTime > parameters.restoreSpeedMP)
            {
                UpdateMP(1f);
                parameters.lastRestoreMPTime = NetworkTime.time;
            }

            currentState.OnUpdate();
        }
    }

    void FixedUpdate()
    {
        if (isServer)
        {
            currentState.OnFixedUpdate();
        }
    }

    [Command]
    public void UpdateMP(float value)
    {
        parameters.playerAttribute.ChangeMP(value);
    }

    public void ChangeState(KnightStateType stateType)
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

    [Command]
    public void CommandForSpawnTrail()// 客户端发起请求，里边的操作在服务器执行，然后调用ClientRpc同步到其他客户端
    {
        SpawnTrail();
    }

    [ClientRpc]
    public void SpawnTrail()
    {
        // 创建一个新的Sprite GameObject作为残影
        GameObject trail = new GameObject("Trail");
        SpriteRenderer trailRenderer = trail.AddComponent<SpriteRenderer>();
        trailRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        trailRenderer.material = parameters.trailMaterial;
        trailRenderer.color = new Color(49f / 255f, 110f / 255f, 183f / 255f, 1f);
        //trail.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.01f);
        trail.transform.position = transform.position;
        trail.transform.localScale = transform.localScale;

        // 设置残影的渲染层级确保它在角色下方
        trailRenderer.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
        trailRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;

        Destroy(trail, 1f);
    }

}
