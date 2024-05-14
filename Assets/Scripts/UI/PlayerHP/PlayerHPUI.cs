using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : NetworkBehaviour
{
    private GameObject _player;
    public GameObject player {
        set {
            _player = value;
            playerAttribute = _player.GetComponent<PlayerAttribute>();
            UpdatePlayerImage();
        }
        get {
            return _player;
        }
    }
    public Image playerHPImage;
    public Image playerMPImage;
    public PlayerAttribute playerAttribute;

    void Start()
    {
        // 直接在Start中初始化组件引用
        playerHPImage = transform.Find("PlayerStatus/HP").GetComponent<Image>();
        playerMPImage = transform.Find("PlayerStatus/MP").GetComponent<Image>();

        //// 验证是否已指定player
        //if (player != null)
        //{
        //    playerAttribute = player.GetComponent<PlayerAttribute>();
        //    UpdatePlayerImage();
        //}
        //else
        //{
        //    Debug.LogError("PlayerHPUI: Player object not assigned.");
        //}
    }

    void Update()
    {
        UpdateHPUI();
    }

    void UpdatePlayerImage()
    {
        // 假设玩家图像显示在UI的一部分，这需要在你的UI预设中有一个具体的Image组件
        //Image playerImageComponent = transform.Find("PlayerImageBorder/PlayerImage").GetComponent<Image>();
        //Sprite playerSprite = player.GetComponent<SpriteRenderer>().sprite;
        //if (playerImageComponent != null && playerSprite != null)
        //{
        //    playerImageComponent.sprite = playerSprite;
        //    RectTransform rectTransform = playerImageComponent.GetComponent<RectTransform>();
        //    rectTransform.anchoredPosition = new Vector2(16, 12);
        //    rectTransform.localScale = new Vector3(1.3f, 0.9f, 1f);
        //}
    }

    void UpdateHPUI()
    {
        // 更新HP和MP UI
        if (playerAttribute != null)
        {
            playerHPImage.fillAmount = playerAttribute.HP / playerAttribute.MAXHP;
            playerMPImage.fillAmount = playerAttribute.MP / playerAttribute.MAXMP;
        }
    }
}
