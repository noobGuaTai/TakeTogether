using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EnemyAttribute : NetworkBehaviour
{
    [SyncVar] public float HP;
    public float MAXHP;
    [SyncVar] public float ATK;

    public GameObject damageTextPrefab;

    private Animator anim;
    private Rigidbody2D rb;
    private Transform UI;
    public bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        UI = GameObject.Find("UI").transform;
    }


    void Update()
    {
        if (HP <= 0 && isServer && !isDead)
        {
            Die();
        }

    }

    [Server]
    public virtual void ChangeHP(float value)
    {
        HP += value;

        // 实例化伤害文本预设
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.4f);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
        GameObject damageTextInstance = Instantiate(damageTextPrefab, screenPosition, Quaternion.identity, UI.transform);
        damageTextInstance.transform.SetParent(UI);
        // 设置伤害值
        damageTextInstance.GetComponent<EnemyUnderAttackText>().SetText(System.Math.Abs(value).ToString());
    }

    [ClientRpc]
    public virtual void Die()
    {
        anim.SetBool("death", true);
        GetComponent<EnemyMove>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EnemyMove>().isAttacking = false;
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 1f);
        gameObject.SetActive(false);
        isDead = true;
    }

}
