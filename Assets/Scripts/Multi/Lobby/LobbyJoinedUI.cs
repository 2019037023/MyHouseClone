using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �κ� ���� �� ������ ���� Ŭ����
/// </summary>
public class LobbyJoinedUI : MonoBehaviour
{
    public GameObject readyButton; // �غ� ��ư
    public GameObject unreadyButton; // �غ����� ��ư

    public TMP_Text lobbyNameText; // �κ� �̸� ǥ�õǴ� ��
    public TMP_Text lobbyCodeText; // �κ� �ڵ� ǥ�õǴ� ��

    public GameObject leaveLobbyButton; // �κ� ������ ��ư

    private void Awake()
    {
        readyButton.SetActive(true); // �غ� ��ư Ȱ��ȭ
    }

    private void Start()
    {
        Lobby lobby = LobbyManager.instance.GetJoinedLobby();   // ���� �������� �κ� ��������
        lobbyNameText.text = lobby.Name;                        // �κ� �̸� ǥ��
        lobbyCodeText.text = "Lobby Code: " + lobby.LobbyCode;  // �κ� �ڵ� ǥ��
    }

    public void ReadyPressed() // �غ� ��ư ������ �� ȣ��Ǵ� �޼���
    {
        readyButton.SetActive(false);
        unreadyButton.SetActive(true);

        leaveLobbyButton.SetActive(false);
    }

    public void UnReadyPressed() // �غ� ���� ��ư ������ �� ȣ��Ǵ� �޼���
    {
        unreadyButton.SetActive(false);
        readyButton.SetActive(true);

        leaveLobbyButton.SetActive(true);

    }

    public void LeaveLobbyPressed() // �κ� ������ ��ư ������ �� ȣ��Ǵ� �޼���
    {
        LobbyManager.instance.LeaveLobby();     // �κ� ������
        // NetworkManager.Singleton.ConnectionApprovalCallback = null; // ȣ��Ʈ�� �� ����� ���� ����, �ٽ� ���� ������� �� ���� �ȳ��� Approval �� �ٽ� null�� ���� (Ȥ�� �𸣴� ������ ����)
        NetworkManager.Singleton.Shutdown();    // ��Ʈ��ũ ���� ����
        DestroyMultiManagers();
        SceneManager.LoadScene("LoadingScene");
    }

    public void DestroyMultiManagers()
    {
        if (NetworkManager.Singleton != null)
            Destroy(NetworkManager.Singleton.gameObject);
        if (NetworkGameManager.instance != null)
            Destroy(NetworkGameManager.instance.gameObject);
        if (LobbyManager.instance != null)
            Destroy(LobbyManager.instance.gameObject);
    }
}
