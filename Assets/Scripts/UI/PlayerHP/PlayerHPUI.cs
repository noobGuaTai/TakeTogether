using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class PlayerHPUI : MonoBehaviour
{
    private GameObject player;
    private GameObject playerImageBorder;
    private GameObject playerImage;
    private Image playerHPImage;
    private Image playerMPImage;
    private PlayerAttribute playerAttribute;


    void Start()
    {
        Init();
    }

    void Update()
    {
        if(player == null)
        {
            Init();
        }
        if (playerAttribute != null)
        {
            if (playerHPImage != null)
            {
                playerHPImage.fillAmount = playerAttribute.HP / playerAttribute.MAXHP;
            }
            if (playerMPImage != null)
            {
                playerMPImage.fillAmount = playerAttribute.MP / playerAttribute.MAXMP;
            }
        }
    }

    void Init()
    {
        player = GameObject.FindWithTag("Player");
        playerImageBorder = GameObject.Find("PlayerImageBorder");
        playerImage = GameObject.Find("PlayerImage");
        playerHPImage = GameObject.Find("PlayerHPImage").GetComponent<Image>();
        playerMPImage = GameObject.Find("PlayerMPImage").GetComponent<Image>();
        playerAttribute = player.GetComponent<PlayerAttribute>();

        Sprite playerSprite = player.GetComponent<SpriteRenderer>().sprite;

        if (playerImage != null)
        {
            Image playerImageComponent = playerImage.GetComponent<Image>();
            if (playerImageComponent != null)
            {
                playerImageComponent.sprite = playerSprite;
            }
        }

        RectTransform rectTransform = playerImage.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(16, 12);
            rectTransform.localScale = new Vector3(1.3f, 0.9f, 1f);
        }
    }
}
