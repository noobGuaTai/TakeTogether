using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttribute : PlayerAttribute
{
    public GameObject damageTextPrefab;

    private Transform UI;

    void Awake()
    {
        HP = 100f;
        ATK = 10f;
        isInvincible = false;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        UI = GameObject.Find("UI").transform;
    }


    void Update()
    {

    }

    public override void ChangeHP(float value)
    {
        if (isInvincible && value <= 0)
            value = 0;//无敌不扣血
        HP += value;
        if (!isInvincible && value < 0)
        {
            // 实例化伤害文本预设
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.4f);//生成的文本在玩家头上
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
            GameObject damageTextInstance = Instantiate(damageTextPrefab, screenPosition, Quaternion.identity, UI.transform);
            damageTextInstance.transform.SetParent(UI);
            // 设置伤害值
            damageTextInstance.GetComponent<PlayerUnderAttackText>().SetText(Math.Abs(value).ToString());
        }

    }
}
