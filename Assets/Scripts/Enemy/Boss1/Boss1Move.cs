using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Move : EnemyMove
{
    public Transform playerTransform;
    public float detectionRangeA = 10.0f;
    public float stopChaseRangeB = 8.0f;
    public float moveSpeed = 2.0f;
    public int chooseAttack = 0;//1，2代表不同攻击模式,0为不攻击
    public GameObject barrage2;

    private SpriteRenderer spriteRenderer; // SpriteRenderer组件引用
    private Rigidbody2D rb; // Rigidbody2D组件引用
    private GameObject barrage1;
    private float attackCoolDown = 3f;
    private float lastAttackTime = 0f;

    void Awake()
    {
        isAttacking = false;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 获取SpriteRenderer组件
        rb = GetComponent<Rigidbody2D>(); // 获取Rigidbody2D组件 
        barrage1 = GameObject.FindGameObjectWithTag("Boss1Barrage1");
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
        if (playerTransform != null)
        {
            Move();
        }

        if (isAttacking && Time.time - lastAttackTime > attackCoolDown)
        {
            chooseAttack = RandomChooseAttack();
            Attack();
            
            //攻击后记录清0
            chooseAttack = 0;
            lastAttackTime = Time.time;
        }

    }

    int RandomChooseAttack()
    {
        return Random.Range(1, 3); // 随机选取0或1
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
            spriteRenderer.flipX = true; // 敌人朝右
        }
        else if (rb.velocity.x < 0)
        {
            spriteRenderer.flipX = false; // 敌人朝左
        }
    }

    void Attack()
    {
        switch (chooseAttack)
        {
            case 1:
                barrage1.GetComponent<Boss1Barrage1>().canEmission = true;
                break;

            case 2:
                GameObject instance = Instantiate(barrage2, transform.position, Quaternion.identity);
                instance.GetComponent<Boss1Barrage2>().canEmission = true;
                break;

            default:

                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRangeA);
        Gizmos.DrawWireSphere(transform.position, stopChaseRangeB);
    }
}
