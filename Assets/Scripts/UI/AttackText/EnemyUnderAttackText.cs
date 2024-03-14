using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyUnderAttackText : MonoBehaviour
{
    private float moveSpeed = 10f; // 上移速度
    private float fadeOutTime = 1f; // 淡出时间
    private TextMeshProUGUI textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>(); // 获取文本组件
        Destroy(gameObject, fadeOutTime); // 在指定时间后销毁对象
    }

    public void SetText(string text)
    {
        textComponent.text = text; // 设置伤害文本
    }

    private void Update()
    {
        // 上移
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 淡出
        Color color = textComponent.color;
        color.a -= Time.deltaTime / fadeOutTime;
        textComponent.color = color;
    }
}
