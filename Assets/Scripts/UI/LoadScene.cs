using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public float duration = 3f; // 完成移动所需时间

    private RectTransform characterRect; // 使用RectTransform来处理UI元素
    private Image sliderImage;
    private TextMeshProUGUI textPercent;
    private GameObject background1;

    void Start()
    {
        //人物、进度条、数字处理
        characterRect = GameObject.Find("Character").GetComponent<RectTransform>();
        sliderImage = GameObject.Find("Slider").GetComponent<Image>();
        textPercent = GameObject.Find("LoadPercent").GetComponent<TextMeshProUGUI>();

        Vector2 startPosition = characterRect.anchoredPosition;
        Vector2 endPosition = new Vector2(startPosition.x + 1214f, startPosition.y);

        StartCoroutine(MoveAndFillCoroutine(startPosition, endPosition, duration));

        //背景处理
        MoveBackground();

    }

    IEnumerator MoveAndFillCoroutine(Vector2 start, Vector2 end, float time)
    {
        float elapsed = 0;
        sliderImage.fillAmount = 0.1f;

        while (elapsed < time)
        {
            float normalizedTime = elapsed / time;
            characterRect.anchoredPosition = Vector2.Lerp(start, end, normalizedTime);
            sliderImage.fillAmount = normalizedTime;
            textPercent.text = Mathf.RoundToInt(normalizedTime * 100) + "%";
            elapsed += Time.deltaTime;
            yield return null;
        }

        characterRect.anchoredPosition = end;
        sliderImage.fillAmount = 1f;
        textPercent.text = "100%";
        gameObject.SetActive(false);
    }

    void MoveBackground()
    {
        RectTransform image3Rect = GameObject.Find("BackgroundImage-3").GetComponent<RectTransform>();
        RectTransform image4Rect = GameObject.Find("BackgroundImage-4").GetComponent<RectTransform>();

        // 启动协程，移动图片
        StartCoroutine(MoveImageCoroutine(image3Rect, 506f, duration));
        StartCoroutine(MoveImageCoroutine(image4Rect, 1647f, duration));
    }

    IEnumerator MoveImageCoroutine(RectTransform imageRect, float distance, float time)
    {
        Vector2 startPosition = imageRect.anchoredPosition;
        Vector2 endPosition = new Vector2(startPosition.x - distance, startPosition.y);

        float elapsed = 0;

        while (elapsed < time)
        {
            float normalizedTime = elapsed / time;
            imageRect.anchoredPosition = Vector2.Lerp(startPosition, endPosition, normalizedTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        imageRect.anchoredPosition = endPosition;
    }

}
