using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Attribute : EnemyAttribute
{
    public GameObject damageTextPrefab;

    private Animator anim;
    private Rigidbody2D rb;
    private Transform UI;
    void Awake()
    {
        HP = 100f;
        ATK = 10f;
    }

    void Start()
    {
        anim = GetComponentInParent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        UI = GameObject.Find("UI").transform;
    }


    void Update()
    {
        Die();
    }

    public override void ChangeHP(float value)
    {
        HP += value;

        // 实例化伤害文本预设
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.4f);//生成的文本在敌人头上
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
        GameObject damageTextInstance = Instantiate(damageTextPrefab, screenPosition, Quaternion.identity, UI.transform);
        damageTextInstance.transform.SetParent(UI);
        // 设置伤害值
        damageTextInstance.GetComponent<EnemyUnderAttackText>().SetText(Math.Abs(value).ToString());
    }

    public override void Die()
    {
        if (HP <= 0)
        {
            anim.SetBool("death", true);
            GetComponent<EnemyMove>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            rb.velocity = Vector2.zero;
            Destroy(gameObject, 1f);
        }

    }

}
