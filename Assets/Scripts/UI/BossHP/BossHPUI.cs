using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    private Image bossHPImage;
    private Image bossImage;
    private EnemyAttribute enemyAttribute;
    private GameObject boss;

    private void Awake()
    {
        bossHPImage = GameObject.Find("BossHPImage").GetComponent<Image>();
        bossImage = GameObject.Find("BossImage").GetComponent<Image>();

    }

    public void ActivateBossHPUI()
    {
        if (boss != null)
        {
            enemyAttribute = boss.GetComponent<EnemyAttribute>();
            //gameObject.SetActive(true);
            if (bossImage.sprite == null)
            {
                bossImage.sprite = boss.GetComponent<SpriteRenderer>().sprite;
                // 设置scale
                bossImage.rectTransform.localScale = new Vector3(2f, 2f, 1f); // 假设Z轴的scale保持不变
                bossImage.rectTransform.anchoredPosition = new Vector2(bossImage.rectTransform.anchoredPosition.x, 40f);

            }
        }

    }

    void Update()
    {
        if (boss == null)
        {
            boss = GameObject.FindGameObjectWithTag("Boss");
        }
        if (enemyAttribute != null)
        {
            bossHPImage.fillAmount = enemyAttribute.HP / enemyAttribute.MAXHP;
            Debug.Log(12333);
            if (enemyAttribute.HP <= 0) // 当HP为0时
            {
                HideEnemyHPUI();
            }
        }
    }

    private void HideEnemyHPUI()
    {
        gameObject.SetActive(false);
        bossImage.sprite = null; // 释放敌人图像资源
    }
}
