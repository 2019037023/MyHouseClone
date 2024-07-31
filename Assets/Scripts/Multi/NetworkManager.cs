using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager _instance;

    public string _roomNameToJoin = "test"; // ������ �� �̸� 


    [Header("DisconnectPanel")]
    public TMP_InputField _nickNameInput; // �г��� �̸�

    [Header("LobbyPanel")]
    public GameObject _lobbyPanel;
    public TextMeshPro _welcomeText;
    public TextMeshPro _lobbyInfoText;
    public GameObject _roomListUI;
    public GameObject _roomListItemUI;    // �� (�� ��Ͽ� ǥ�õǴ� ���� ��)
    List<RoomInfo> cacheRoomList = new List<RoomInfo>(); // �� ���� ��Ƶ� ����Ʈ


    [Header("RoomPanel")]
    public GameObject _roomPanel;
    public TextMeshPro _roomName; // �� ����
    public TextMeshPro ListText;
    public TextMeshPro _roomInfoText;

    // ä�� ����
    public GameObject _chatListUI;
    public GameObject _chatListItemUI;
    public TMP_InputField _chatInput; // ä�� �Է�â

    [Header("ETC")]
    public TextMeshPro StatusText;
    public PhotonView PV;

    [Header("�˸� �޽��� ����")]
    public GameObject connectionResponseUI;             // '������' ��Ÿ���� UI
    public TMP_Text messsageText;                       // ���� ���� ����
    public GameObject connectionResponseCloseButton;    // ���� ���� ���� �� �ݴ� ��ư

    #region ��������
    void Awake()
    {
        if (_instance == null)
            _instance = this;

        DontDestroyOnLoad(_instance);
    }
    void Update()
    {
        _lobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "Lobby / " + PhotonNetwork.CountOfPlayers + "Connected"; // �� ���� �����ֱ�
        
        // ���� ĥ ��, �濡 �ְ� ȭ��Ʈ �����̽��� �ƴϿ��� ��
        if(PhotonNetwork.InRoom && !string.IsNullOrWhiteSpace(_chatInput.text) && Input.GetKeyDown(KeyCode.Return))
        {
            Send();
        }
        
    }

    // ���� ������ ����
    public void Connect()
    {
        Debug.Log("���� ������ �����մϴ�.");
        PhotonNetwork.ConnectUsingSettings();
    }

    // ������ �����ϰ�, �ݹ����� ȣ��� -> �κ� �����ϰ� ��
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    // �κ� �������� �� �ݹ�
    public override void OnJoinedLobby()
    {
        //_lobbyPanel.SetActive(true);
        //_roomPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = _nickNameInput.text;
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " �� �κ� �����߽��ϴ�.");
        cacheRoomList.Clear();
    }

    // ���� ���� ���� ����
    public void Disconnect()
    {
        Debug.Log("���� ����(�����͵� ������ ����)�մϴ�.");
        PhotonNetwork.Disconnect();
    }

    // ���� ���� ������, �ݹ����� ȣ���
    public override void OnDisconnected(DisconnectCause cause) 
    {
        //_lobbyPanel.SetActive(false);
        //_roomPanel.SetActive(false);
    }
    #endregion

    #region ��

    // �� �����
    public void CreateRoom()
    {
        // �� ����
        PhotonNetwork.CreateRoom(_roomName.text = "Room" + UnityEngine.Random.Range(100, 1000).ToString(), new RoomOptions { // �� �ɼ�
            MaxPlayers = 4,     // �ִ� �ο� ��
            EmptyRoomTtl = 0    // ���� ��� ���� �� ��� ����  
        });
        
        // connectionResponseUI.SetActive(true); // ���Ŀ� ����ó���� �޽��� �޾ƿ� ����
    }

    // ������ �������� ����
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    // �� ������
    public void LeaveRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " ��(��) �����մϴ�.");

        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom() // 1. ���� ����� �� �� 2. �ٷ� ������ �� ��
    {
        Debug.Log( PhotonNetwork.CurrentRoom.Name + " �� �����մϴ�.");
        //RoomPanel.SetActive(true);
        //RoomRenewal();
        //ChatInput.text = "";
        //for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
    }

    //// ���� �÷��̾ ���� ������ ��, ȣ��
    //public override void OnLeftRoom()
    //{
    //    base.OnLeftRoom();

    //    // ���� �޴��� ���ư��� �ϱ�
    //}

    // �� ����� �������� �� �ݹ�
    public override void OnCreateRoomFailed(short returnCode, string message) 
    { 
        _roomName.text = "Room"; 
        CreateRoom(); 
    }

    // ���� ���� �������� �� �ݹ�
    public override void OnJoinRandomFailed(short returnCode, string message) 
    { 
        _roomName.text = ""; 
        CreateRoom(); 
    }

    // �濡 �������� ��, �ȿ� �ִ� ��ο��� ����
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }

    // ���� ���� ��, �ȿ� �ִ� ��ο��� ����
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("������ " + PhotonNetwork.CurrentRoom.Name + "���� �����Ѵٰ� �մϴ�.");
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }

    void RoomRenewal()
    {        
        for(int i=0; i<PhotonNetwork.PlayerList.Length;i++)
        {
            Debug.Log(PhotonNetwork.PlayerList[i].NickName);
        }
        Debug.Log("�̻�");
    }


    /// <summary>
    /// �� ��ư ������ ����
    /// </summary>
    public void JoinRoomButtonPressed() 
    {
        Debug.Log("������...");

        PhotonNetwork.JoinOrCreateRoom(_roomNameToJoin, new RoomOptions
        { // �� �ɼ�
            MaxPlayers = 4,     // �ִ� �ο� ��
            EmptyRoomTtl = 0    // ���� ��� ���� �� ��� ����  
        }, null);

        /*
         �������̶�� ���� Ȱ��ȭ��ų ���� 
        */

        //nameUI.SetActive(false);
        //connectingUI.SetActive(true);
    }
    #endregion

    #region �渮��Ʈ ����

    public override void OnRoomListUpdate(List<RoomInfo> roomList) // �� ����� �ٱ͸� ȣ��Ǵ� �ݹ�
    {
        int roomCount = roomList.Count;

        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!cacheRoomList.Contains(roomList[i])) cacheRoomList.Add(roomList[i]);
                else cacheRoomList[cacheRoomList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (cacheRoomList.IndexOf(roomList[i]) != -1) cacheRoomList.RemoveAt(cacheRoomList.IndexOf(roomList[i]));
        }
        UpdateRoomListUI();
    }


    void UpdateRoomListUI() // �� ��� UI ������Ʈ
    {
        // ���� �� ��� UI �����ϱ�
        foreach (Transform child in _roomListUI.transform)
        {
            Destroy(child.gameObject);  // ���� �κ� ��� UI ��� ����
        }

        // �� UI ����
        foreach (RoomInfo room in cacheRoomList)
        {
            GameObject _room = Instantiate(_roomListItemUI, _roomListUI.transform);   // �� ���ø� �����ؼ� ���� �� UI ����
            _room.SetActive(true);
            _room.GetComponent<RoomListItemUI>().SetRoom(room); // �� �κ� UI�� ���� �κ� ������ ����
        }
    }


    #endregion

    #region ä��
    public void Send()
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + _chatInput.text); // �濡 �ִ� ��ο��� ���
        _chatInput.text = "";
    }

    //[PunRPC] // RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�
    //void ChatRPC(string msg)
    //{
    //    bool isInput = false;
    //    for (int i = 0; i < ChatText.Length; i++)
    //        if (ChatText[i].text == "")
    //        {
    //            isInput = true;
    //            ChatText[i].text = msg;
    //            break;
    //        }
    //    if (!isInput) // ������ ��ĭ�� ���� �ø�
    //    {
    //        for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
    //        ChatText[ChatText.Length - 1].text = msg;
    //    }
    //}

    [PunRPC] // RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�
    void ChatRPC(string msg)
    {
        GameObject chatManager = Instantiate(_chatListItemUI, _chatListUI.transform);
        chatManager.GetComponent<ChatMessage>().SetText(msg);
    }
    #endregion
}
