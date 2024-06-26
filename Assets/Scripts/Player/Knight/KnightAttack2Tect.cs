using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class KnightAttack2Tect : NetworkBehaviour
{
    public float knockbackForce = 1f; // 击退力量
    public float knockbackDuration = 0.15f; // 击退时间

    private EnemyAttribute enemyAttribute;
    private float ATK;

    void Start()
    {
        ATK = GetComponentInParent<PlayerAttribute>().ATK * 2;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        enemyAttribute = other.GetComponent<EnemyAttribute>();
        if (enemyAttribute != null)
        {
            ChangeHPCommand(enemyAttribute);
            Debug.Log(enemyAttribute.HP);

            StartCoroutine(KnockbackRoutine(other));

            // 应用击退效果
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                // 计算击退方向
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                // 应用力
                enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }


            GetComponentInParent<PlayerAttribute>().enemyHPUI.SetActive(true);
            GetComponentInParent<PlayerAttribute>().enemyHPUI.GetComponent<EnemyHPUI>().ActivateEnemyHPUI(other.GetComponent<EnemyAttribute>(), other.GetComponent<SpriteRenderer>());


        }
    }

    //暂停敌人的移动
    IEnumerator KnockbackRoutine(Collider2D other)
    {
        other.GetComponent<EnemyMove>().enabled = false;
        yield return new WaitForSeconds(knockbackDuration);
        other.GetComponent<EnemyMove>().enabled = true;
    }

    [Command]
    void ChangeHPCommand(EnemyAttribute ea)
    {
        ea.ChangeHP(-ATK);
    }
}
