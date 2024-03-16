using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour
{
    private Image enemyHPImage;
    private Image enemyImage;
    private EnemyAttribute enemyAttribute;
    private float lastHitTime; // 记录最后一次被攻击的时间
    private bool isEnemyUnderAttack = false; // 敌人是否被攻击的标志

    private void Awake()
    {
        enemyHPImage = GameObject.Find("EnemyHPImage").GetComponent<Image>();
        enemyImage = GameObject.Find("EnemyImage").GetComponent<Image>();
    }

    public void ActivateEnemyHPUI(EnemyAttribute ea, SpriteRenderer sr)
    {
        enemyAttribute = ea;
        gameObject.SetActive(true);
        if (enemyImage.sprite == null)
        {
            enemyImage.sprite = sr.sprite;
        }
        lastHitTime = Time.time; // 更新最后一次被攻击的时间
        isEnemyUnderAttack = true; // 设置敌人被攻击的标志
    }

    void Update()
    {
        if (enemyAttribute != null)
        {
            enemyHPImage.fillAmount = enemyAttribute.HP / enemyAttribute.MAXHP;

            // 检查敌人是否被击中且是否超过3秒未被再次攻击
            if (isEnemyUnderAttack && Time.time - lastHitTime > 3f)
            {
                HideEnemyHPUI();
            }
            else if (enemyAttribute.HP <= 0) // 当HP为0时
            {
                if (Time.time - lastHitTime > 1f) // 如果HP为0超过1秒
                {
                    HideEnemyHPUI();
                }
            }
        }
    }

    private void HideEnemyHPUI()
    {
        gameObject.SetActive(false);
        isEnemyUnderAttack = false; // 重置敌人被攻击的标志
        enemyImage.sprite = null; // 释放敌人图像资源
    }

    // 提供一个方法来更新最后一次被攻击的时间，从外部调用
    public void UpdateLastHitTime()
    {
        lastHitTime = Time.time;
        isEnemyUnderAttack = true; // 一旦被攻击，更新标志
    }
}
