using Mirror;
using UnityEngine;

public class UIManager : NetworkBehaviour
{
    public GameObject playerHPPrefab; // 血条UI的预制体
    public GameObject enemyHPPrefab; // 血条UI的预制体
    public GameObject bossHPPrefab;
    public Transform canvas; // UI元素的父对象，通常是Canvas
    public float spacing = 30f; // UI元素之间的垂直间距

    private Vector2 initialPosition = new Vector2(-600f, 400f); // 第一个UI元素的起始位置
    private float nextYPosition; // 下一个UI元素的Y位置

    void Start()
    {
        if (canvas == null)
        {
            Debug.LogError("UIManager: UI Parent (Canvas) is not assigned!");
            return;
        }
        nextYPosition = initialPosition.y; // 初始化下一个UI元素的位置
    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PlayerMove playerMove = player.GetComponent<PlayerMove>();
            if (playerMove != null && playerMove.HPUI == null)
            {
                GameObject playerHPUI = Instantiate(playerHPPrefab, canvas);//本地生成UI
                playerMove.HPUI = playerHPUI;

                RectTransform uiRectTransform = playerHPUI.GetComponent<RectTransform>();
                if (uiRectTransform != null)
                {
                    Vector2 newPosition = new Vector2(initialPosition.x, nextYPosition);
                    uiRectTransform.anchoredPosition = newPosition;

                    // 更新下一个UI元素的位置
                    nextYPosition -= uiRectTransform.rect.height + spacing;
                }

                playerHPUI.GetComponent<PlayerHPUI>().player = player;
            }

            if (player.GetComponent<PlayerAttribute>().enemyHPUI == null)
            {
                //每个玩家都有一个enemyHPUI
                GameObject enemyHPUI = Instantiate(enemyHPPrefab, canvas);
                player.GetComponent<PlayerAttribute>().enemyHPUI = enemyHPUI;
                enemyHPUI.SetActive(false);

                //每个玩家都有一个bossHPUI
                GameObject bossHPUI = Instantiate(bossHPPrefab, canvas);
                player.GetComponent<PlayerAttribute>().bossHPUI = bossHPUI;
                bossHPUI.SetActive(false);
            }

        }
    }

}
