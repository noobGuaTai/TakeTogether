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

    void Start()
    {
        characterRect = GameObject.Find("Character").GetComponent<RectTransform>();
        sliderImage = GameObject.Find("Slider").GetComponent<Image>();
        textPercent = GameObject.Find("LoadPercent").GetComponent<TextMeshProUGUI>();

        Vector2 startPosition = characterRect.anchoredPosition;
        Vector2 endPosition = new Vector2(startPosition.x + 1214f, startPosition.y);

        StartCoroutine(MoveAndFillCoroutine(startPosition, endPosition, duration));
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
}
