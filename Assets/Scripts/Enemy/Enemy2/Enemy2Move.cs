using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Enemy2Move : EnemyMove
{
    public float detectionRangeA = 10.0f;
    public float stopChaseRangeB = 8.0f;
    public float moveSpeed = 2.0f;
    public GameObject enemy2Barrage;

    private SpriteRenderer spriteRenderer; // SpriteRenderer组件引用
    private Rigidbody2D rb; // Rigidbody2D组件引用
    [SyncVar] private float attackCoolDown = 3f;
    [SyncVar] private double lastAttackTime = 0f;

    void Awake()
    {
        isAttacking = false;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 获取SpriteRenderer组件
        rb = GetComponent<Rigidbody2D>(); // 获取Rigidbody2D组件
    }

    void Update()
    {
        if(isServer)
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

            if (NetworkTime.time - lastAttackTime > attackCoolDown && isAttacking)
            {
                lastAttackTime = NetworkTime.time;
                var barrage2Instance = Instantiate(enemy2Barrage, transform.position, Quaternion.identity);
                NetworkServer.Spawn(barrage2Instance);
                Destroy(barrage2Instance, 6f);
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
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // 敌人朝右// 敌人朝右
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1 * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // 敌人朝右// 敌人朝右
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRangeA);
        Gizmos.DrawWireSphere(transform.position, stopChaseRangeB);
    }
}
