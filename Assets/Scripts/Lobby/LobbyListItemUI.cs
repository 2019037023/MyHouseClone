using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �κ� ��Ͽ� ������ �κ� ǥ���ϴ� UI Ŭ����
/// </summary>
public class LobbyListItemUI : MonoBehaviour
{
    public TMP_Text lobbyText;      // �κ� �̸�
    public TMP_Text playerCount;    // �÷��̾� ��
    Lobby lobby;

    private void Awake()
    {
        // Ŭ���ϸ� �κ� �� �� �ְ� ��ư�� �̺�Ʈ �߰�
        GetComponent<Button>().onClick.AddListener(() => {
            FindObjectOfType<LobbyBrowseUI>().JoinLobbyById(lobby.Id);
        });
    }
    public void SetLobby(Lobby _lobby)
    {
        lobby = _lobby;                                                 // �Ű������� ���� �κ� ������ �ʵ� ������ lobby�� ����
        lobbyText.text = lobby.Name;                                    // �κ� ��Ͽ� ǥ�õǴ� �κ� �̸�
        playerCount.text = "" + lobby.Players.Count.ToString() + "/4";  // �κ� ��Ͽ� ǥ�õǴ� �ش� �κ� �÷��̾� ��
    }
}
