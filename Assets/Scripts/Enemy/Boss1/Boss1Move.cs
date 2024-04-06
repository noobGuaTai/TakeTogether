using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Boss1Move : EnemyMove
{
    public float detectionRangeA = 10.0f;
    public float stopChaseRangeB = 8.0f;
    public float moveSpeed = 2.0f;
    [SyncVar] public int chooseAttack = 0; // 1，2代表不同攻击模式,0为不攻击
    public GameObject barrage2Prefab;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public GameObject barrage1Prefab;
    [SyncVar] private float attackCoolDown = 3f;
    [SyncVar] private double lastAttackTime = 0f;
    private Animator anim;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isAttacking = false;
    }

    void Update()
    {
        if (isServer)
        {
            ServerUpdate();
        }
    }

    [Server]
    void ServerUpdate()
    {
        closedPlayer = FindClosestPlayer();
        allPlayers = FindAllPlayers();

        if (closedPlayer != null)
        {
            Move();

            if (NetworkTime.time - lastAttackTime > attackCoolDown)
            {
                lastAttackTime = NetworkTime.time;
                chooseAttack = UnityEngine.Random.Range(1, 3); // 随机选择攻击模式
                RpcPrepareAttack(); // 客户端播放攻击动画
                ServerPerformAttack(chooseAttack); // 在服务器上执行攻击实例化
                attackCoolDown = UnityEngine.Random.Range(3f, 6f); // 随机冷却时间
            }
        }
    }
 
    [Server]
    public override void Move()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, closedPlayer.transform.position);

        Vector3 directionToPlayer = (closedPlayer.transform.position - transform.position).normalized;
        if (distanceToPlayer <= detectionRangeA)
        {
            isAttacking = true;
            if (distanceToPlayer > stopChaseRangeB)
            {
                // 使用速度来移动
                rb.velocity = directionToPlayer * moveSpeed;
            }
            else
            {
                // 在B范围内停止
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            isAttacking = false;
            // 玩家不在A范围内时停止
            rb.velocity = Vector2.zero;
        }

        // 更新敌人朝向
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(-1 * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // 敌人朝右// 敌人朝右
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // 敌人朝右// 敌人朝右
        }
    }

    [ClientRpc]
    void RpcPrepareAttack()
    {
        StartCoroutine(PerformAttackAnimation());
    }

    IEnumerator PerformAttackAnimation()
    {
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(1.3f); // 等待动画播放
        anim.SetBool("attack", false);
    }

    [Server]
    void ServerPerformAttack(int attackType)
    {
        // 在服务器上根据攻击类型实例化不同的攻击效果
        if (attackType == 1)
        {
            var barrage1Instance = Instantiate(barrage1Prefab, transform.position, Quaternion.identity);
            NetworkServer.Spawn(barrage1Instance);
            Destroy(barrage1Instance, 6f);
        }
        else if (attackType == 2)
        {
            var barrage2Instance = Instantiate(barrage2Prefab, transform.position, Quaternion.identity);
            NetworkServer.Spawn(barrage2Instance);
            Destroy(barrage2Instance, 6f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRangeA);
        Gizmos.DrawWireSphere(transform.position, stopChaseRangeB);
    }
}
