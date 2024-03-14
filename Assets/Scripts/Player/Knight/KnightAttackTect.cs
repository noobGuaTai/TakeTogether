using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttackTect : MonoBehaviour
{
    private EnemyAttribute enemyAttribute;
    private float ATK;
    public float knockbackForce = 5f; // 击退力量
    public float knockbackDuration = 0.2f; // 击退时间

    void Start()
    {
        ATK = GetComponentInParent<PlayerAttribute>().ATK;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        enemyAttribute = other.GetComponent<EnemyAttribute>();
        if(enemyAttribute != null)
        {
            enemyAttribute.ChangeHP(-ATK);
            Debug.Log(enemyAttribute.HP);

            StartCoroutine(KnockbackRoutine(other));

            // 应用击退效果
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            if(enemyRb != null)
            {
                // 计算击退方向
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                // 应用力
                enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    //暂停敌人的移动
    IEnumerator KnockbackRoutine(Collider2D other)
    {
        other.GetComponent<EnemyMove>().enabled = false;
        yield return new WaitForSeconds(knockbackDuration);
        other.GetComponent<EnemyMove>().enabled = true;
    }
}
