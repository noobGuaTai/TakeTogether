using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Move : EnemyMove
{
    public Transform playerTransform;
    public float detectionRangeA = 10.0f;
    public float stopChaseRangeB = 8.0f;
    public float moveSpeed = 2.0f;

    private SpriteRenderer spriteRenderer; // SpriteRenderer组件引用
    private Rigidbody2D rb; // Rigidbody2D组件引用

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
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                playerTransform = player.transform;
            }
        }
        if(playerTransform !=null)
        {
            Move();
        }

    }

    public override void Move()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
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
            spriteRenderer.flipX = false; // 敌人朝右
        }
        else if (rb.velocity.x < 0)
        {
            spriteRenderer.flipX = true; // 敌人朝左
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRangeA);
        Gizmos.DrawWireSphere(transform.position, stopChaseRangeB);
    }
}
