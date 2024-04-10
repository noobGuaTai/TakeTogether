using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    public bool isDie = false;

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
        gameObject.SetActive(true);
        if (boss == null)
        {
            boss = GameObject.FindGameObjectWithTag("Boss1");
        }
        else
        {
            enemyAttribute = boss.GetComponent<EnemyAttribute>();
            if (bossImage.sprite == null)
            {
                bossImage.sprite = boss.GetComponent<SpriteRenderer>().sprite;
                
                bossImage.rectTransform.localScale = new Vector3(2f, 2f, 1f);
                bossImage.rectTransform.anchoredPosition = new Vector2(bossImage.rectTransform.anchoredPosition.x, 40f);

            }
        }

    }

    void Update()
    {
        if (enemyAttribute != null)
        {
            bossHPImage.fillAmount = enemyAttribute.HP / enemyAttribute.MAXHP;
            if (enemyAttribute.HP <= 0) // 当HP为0时
            {
                isDie = true;
                HideBossHPUI();
            }
        }
    }

    public void HideBossHPUI()
    {
        gameObject.SetActive(false);
        bossImage.sprite = null; // 释放敌人图像资源
    }
}
