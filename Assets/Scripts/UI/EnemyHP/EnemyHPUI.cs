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
    private bool isEnemyHit = false; // 敌人是否被攻击的标志

    private void Awake()
    {
        enemyHPImage = GameObject.Find("EnemyHPImage").GetComponent<Image>();
        enemyImage = GameObject.Find("EnemyImage").GetComponent<Image>();
        // RectTransform rectTransform = enemyImage.GetComponent<RectTransform>();
        // if (rectTransform != null)
        // {
        //     rectTransform.anchoredPosition = new Vector2(16, 12);
        //     rectTransform.localScale = new Vector3(1.3f, 0.9f, 1f);
        // }
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
        isEnemyHit = true; // 设置敌人被攻击的标志
    }

    void Update()
    {
        if (enemyAttribute != null)
        {
            if (enemyAttribute.HP >= 0)
            {
                // 更新HP UI的fillAmount
                enemyHPImage.fillAmount = enemyAttribute.HP / enemyAttribute.MAXHP;

                // 如果敌人在过去3秒内未被攻击，则隐藏UI
                if (Time.time - lastHitTime > 3f && isEnemyHit)
                {
                    HideEnemyHPUI();
                }
            }
            else if (!isEnemyHit || (isEnemyHit && Time.time - lastHitTime > 1f)) // 如果敌人HP为0且1秒后隐藏UI
            {
                HideEnemyHPUI();
            }
        }
    }

    private void HideEnemyHPUI()
    {
        gameObject.SetActive(false);
        isEnemyHit = false; // 重置敌人被攻击的标志
        enemyImage.sprite = null; // 释放敌人图像资源
    }

    // 提供一个方法来更新最后一次被攻击的时间，从外部调用
    public void UpdateLastHitTime()
    {
        lastHitTime = Time.time;
    }
}
