using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class KnightAttackTect : NetworkBehaviour
{
    public float knockbackForce = 1f; // 击退力量
    public float knockbackDuration = 0.15f; // 击退时间

    private EnemyAttribute enemyAttribute;
    private float ATK;

    void Start()
    {
        ATK = GetComponentInParent<PlayerAttribute>().ATK;
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

            // StartCoroutine(KnockbackRoutine(other));


            if (other.gameObject.layer == 7)// 7为enemy
            {
                GetComponentInParent<PlayerAttribute>().enemyHPUI.SetActive(true);
                GetComponentInParent<PlayerAttribute>().enemyHPUI.GetComponent<EnemyHPUI>().ActivateEnemyHPUI(other.GetComponent<EnemyAttribute>(), other.GetComponent<SpriteRenderer>());

            }
        }
    }

    //暂停敌人的移动
    IEnumerator KnockbackRoutine(Collider2D other)
    {
        yield return new WaitForSeconds(knockbackDuration);
        Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            // 计算击退方向
            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
            // 应用力
            enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    [Command]
    void ChangeHPCommand(EnemyAttribute ea)
    {
        ea.ChangeHP(-ATK);
    }
}
