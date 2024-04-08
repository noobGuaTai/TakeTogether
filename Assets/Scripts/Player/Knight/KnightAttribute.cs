using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class KnightAttribute : PlayerAttribute
{
    public GameObject damageTextPrefab;

    private Transform UI;

    void Awake()
    {
        HP = 100f;
        MAXHP = 100f;
        MP = 10f;
        MAXMP = 10f;
        MPConsume = 3f;
        ATK = 60f;
        isInvincible = false;
        isReady = false;// true为开始游戏
        UI = GameObject.Find("UI").transform;
        // DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        // if(enemyHPUI != null)
        // {
        //     enemyHPUI.SetActive(false);
        // }
    }


    void Update()
    {
        // foreach (GameObject otherEnemyHPUI in GameObject.FindGameObjectsWithTag("EnemyHPUI"))
        // {
        //     if(otherEnemyHPUI != enemyHPUI)
        //     {
        //         otherEnemyHPUI.SetActive(false);
        //     }
        // }
    }

    [Server]
    public override void ChangeHP(float value)
    {
        if (isInvincible && value <= 0)
            value = 0;//无敌不扣血
        HP += value;
        if (HP < 0)
        {
            HP = 0;
        }
        if (HP > MAXHP)
        {
            HP = MAXHP;
        }

        if (!isInvincible && value < 0)
        {
            ShowDamageText(value);
        }

    }

    [Command]
    public void CommandForChangeHP(float value)
    {
        if (isInvincible && value <= 0)
            value = 0;//无敌不扣血
        HP += value;
        if (HP < 0)
        {
            HP = 0;
        }
        if (HP > MAXHP)
        {
            HP = MAXHP;
        }

        if (!isInvincible && value < 0)
        {
            ShowDamageText(value);
        }

    }

    [Server]
    public override void ChangeMP(float value)
    {
        MP += value;
        if (MP < 0)
        {
            MP = 0;
        }
        if (MP > MAXMP)
        {
            MP = MAXMP;
        }
    }

    [ClientRpc]
    void ShowDamageText(float value)
    {
        // 实例化伤害文本预设
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 8f);//生成的文本在玩家头上
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
        GameObject damageTextInstance = Instantiate(damageTextPrefab, Vector3.zero, Quaternion.identity, UI.transform);
        damageTextInstance.transform.SetParent(UI);
        damageTextInstance.transform.position = screenPosition;
        // 设置伤害值
        damageTextInstance.GetComponent<PlayerUnderAttackText>().SetText(Math.Abs(value).ToString());

    }
}
