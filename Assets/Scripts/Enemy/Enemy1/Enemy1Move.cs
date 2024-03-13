using System.Collections;
using UnityEngine;

public class Enemy1Move : EnemyMove
{
    public float detectionRadius = 5f;
    public float moveSpeed = 2f;
    public float randomMoveDistance = 2f;
    public LayerMask playerLayer;

    private Transform playerTransform;
    private bool isPlayerDetected = false;
    private Vector3 randomDestination;
    private float moveTimer = 3f;
    private bool canMove = false; // 控制敌人是否可以开始移动的标志

    void Start()
    {
        // 延迟开始搜索和移动
        StartCoroutine(DelayMovement(2f));
    }

    void Update()
    {
        // 如果canMove标志为false，敌人不会开始搜索玩家
        if (!canMove) return;

        // 搜索玩家
        isPlayerDetected = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (isPlayerDetected)
        {
            // 获取玩家的位置并移动向玩家
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            MoveTowards(playerTransform.position);
        }
        else
        {
            // 随机移动
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
        // 计算移动步长
        float step = moveSpeed * Time.deltaTime;

        // 确定移动方向
        Vector3 moveDirection = (target - transform.position).normalized;

        // 移动敌人
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // 检查移动方向并翻转Sprite
        if (moveDirection.x > 0)
        {
            // 向右移动，确保localScale的x为正值
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveDirection.x < 0)
        {
            // 向左移动，确保localScale的x为负值
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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
