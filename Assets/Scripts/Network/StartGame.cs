using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Collections.Generic;
using Mirror.Discovery;
using Org.BouncyCastle.Pqc.Crypto.Saber;
using TMPro;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public bool load = false;
    public NetworkDiscovery networkDiscovery;
    public GameObject serverAddressContent;
    public GameObject serverAddressPrefab;
    private Dictionary<long, GameObject> instantiatedPrefabs = new Dictionary<long, GameObject>();// 已初始化的服务器
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

    public GameObject StartHostButton;
    public GameObject FindServerButton;
    public GameObject ExitRoomButton;
    public GameObject StartGameButton;
    public GameObject ServerList;

    void Start()
    {
        //ServerList.SetActive(false);
    }

    void Update()
    {
    }

    public void ChangeScene()
    {
        NetworkManager.singleton.ServerChangeScene("Level");
    }

    public void CreateHost()
    {
        discoveredServers.Clear();
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();

        StartHostButton.SetActive(false);
        FindServerButton.SetActive(false);
        ExitRoomButton.SetActive(true);
        StartGameButton.SetActive(true);

        if (ServerList != null)
        {
            ServerList.SetActive(false);
        }
    }

    public void FindServer()
    {
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
        ServerList.SetActive(true);
        StartCoroutine(AddServers());
    }

    IEnumerator AddServers()
    {
        while (true)
        {
            foreach (ServerResponse info in discoveredServers.Values)
            {
                if (!instantiatedPrefabs.ContainsKey(info.serverId))// 已被实例化则不再实例化
                {
                    var prefabInstance = CreateServerAddressPrefab(info);
                    instantiatedPrefabs[info.serverId] = prefabInstance;
                }
            }
            yield return null;
        }
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        discoveredServers[info.serverId] = info;

    }

    GameObject CreateServerAddressPrefab(ServerResponse info)
    {
        var sa = Instantiate(serverAddressPrefab);
        sa.transform.SetParent(serverAddressContent.transform, false);
        sa.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = info.EndPoint.Address.ToString();

        // 为按钮添加点击事件监听器
        Button button = sa.GetComponent<Button>();
        if (button != null)
        {
            // 当按钮被点击时调用Connect方法
            button.onClick.AddListener(() => Connect(info));
        }
        else
        {
            Debug.LogError("Server address prefab does not contain a Button component.");
        }
        return sa;
    }

    public void ExitRoom()
    {
        // 如果玩家是主机
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        // 如果玩家是客户端
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        // 如果玩家仅为服务器
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }

        StartHostButton.SetActive(true);
        FindServerButton.SetActive(true);
        ExitRoomButton.SetActive(false);
    }

    void Connect(ServerResponse info)
    {
        networkDiscovery.StopDiscovery();
        NetworkManager.singleton.StartClient(info.uri);
        ServerList.SetActive(false);
        StartHostButton.SetActive(false);
        FindServerButton.SetActive(false);
        ExitRoomButton.SetActive(true);
    }
}
