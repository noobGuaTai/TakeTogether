using System.Collections;
using TMPro; // 确保导入了TextMeshPro的命名空间
using UnityEngine;
using UnityEngine.UI; // 用于操作UI元素

public class LoadScene : MonoBehaviour
{
    public float duration = 3f;

    private Transform characterTrans;
    private Image sliderImage;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private TextMeshProUGUI textPercent;

    void Start()
    {
        characterTrans = GameObject.Find("Character").transform;
        sliderImage = GameObject.Find("Slider").GetComponent<Image>();
        textPercent = GameObject.Find("LoadPercent").GetComponent<TextMeshProUGUI>();

        startPosition = characterTrans.position;
        endPosition = startPosition + new Vector3(280f, 0, 0);

        StartCoroutine(MoveAndFillCoroutine(startPosition, endPosition, duration));
    }

    IEnumerator MoveAndFillCoroutine(Vector3 start, Vector3 end, float time)
    {
        float elapsed = 0; // 已经过去的时间
        sliderImage.fillAmount = 0.1f;

        while (elapsed < time)
        {
            // 计算已经过去时间的比例
            float normalizedTime = elapsed / time;

            // 在起始和结束位置之间插值角色的位置
            characterTrans.position = Vector3.Lerp(start, end, normalizedTime);

            // 同时根据时间比例设置Image的fillAmount
            sliderImage.fillAmount = Mathf.Lerp(0.1f, 1f, normalizedTime);

            // 更新文本显示的加载百分比
            float percent = Mathf.Lerp(0.1f, 1f, normalizedTime) * 100;
            textPercent.text = percent.ToString("F0") + "%";

            // 更新已经过去的时间
            elapsed += Time.deltaTime;

            // 等待下一帧继续执行
            yield return null;
        }

        characterTrans.position = end;
        sliderImage.fillAmount = 1f;
        textPercent.text = "100%";
        gameObject.SetActive(false);
    }
}
