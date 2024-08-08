using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using SlimUI.ModernMenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �� ��Ͽ� ������ ���� ǥ���ϴ� UI Ŭ����
/// </summary>
public class RoomListItemUI : MonoBehaviour
{
    public string _roomName;         // �� �̸�
    public TMP_Text _roomTitle;      // �� ����
    public TMP_Text _playerCount;    // �÷��̾� ��

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            FindObjectOfType<UIMenuManager>().LobbyToRoomCamPos();
        });
    }

    public void SetRoom(RoomInfo room) // �� ����
    {
        if (room.Name == null)
            Debug.Log("�̸��� ����");

        _roomName = room.Name;
        _roomTitle.text = _roomName;                                  // �� ��Ͽ� ǥ�õǴ� �� �̸�
        _playerCount.text = "" + room.PlayerCount.ToString() + "/4";  // �� ��Ͽ��� ǥ�õǴ� �ش� ���� �÷��̾� ��
    }

    public void OnJoinPressed()
    {
        NetworkManager._instance._roomNameToJoin = _roomName;   // ������ �� �̸� ����
        NetworkManager._instance._roomName.text = _roomName;    // �� �̸� ����
        Debug.Log(_roomName + " �� ��ư�� ������ �濡 �����մϴ�.");
        NetworkManager._instance.JoinRoom();
    }
}
