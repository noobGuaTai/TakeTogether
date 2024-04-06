using System.Collections;
using Mirror;
using NUnit.Framework;
using UnityEngine;

public class Enemy1Move : EnemyMove
{
    public float detectionRadius = 5f;
    public float moveSpeed = 2f;
    public float randomMoveDistance = 2f;
    public LayerMask playerLayer;

    private bool isPlayerDetected = false;
    private Vector3 randomDestination;
    private float moveTimer = 3f;
    private bool canMove = false; // 控制敌人是否可以开始移动的标志
    private Rigidbody2D rb;

    void Start()
    {
        // 延迟开始搜索和移动
        StartCoroutine(DelayMovement(2f));
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(isServer)
        {
            Move();
        }
        
    }

    [Server]
    public override void Move()
    {
        isPlayerDetected = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (isPlayerDetected)
        {
            closedPlayer = FindClosestPlayer();
            MoveTowards(closedPlayer.transform.position);
        }
        else
        {
            if (moveTimer <= 0)
            {
                moveTimer = 3f;
                randomDestination = transform.position + Random.insideUnitSphere * randomMoveDistance;
                randomDestination.z = 0;
            }
            else
            {
                moveTimer -= Time.deltaTime;
            }
            MoveTowards(randomDestination);
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 moveDirection = (target - transform.position).normalized;
        rb.velocity = moveDirection * moveSpeed;

        // 设置一个死区阈值，避免玩家和敌人非常接近时的快速翻转
        float flipDeadZone = 0.1f;

        if (Mathf.Abs(target.x - transform.position.x) > flipDeadZone)
        {
            if (moveDirection.x > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (moveDirection.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }


    private IEnumerator DelayMovement(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true; // 开始移动
        // 初始化随机目的地
        randomDestination = transform.position + Random.insideUnitSphere * randomMoveDistance;
        randomDestination.z = 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }


}
