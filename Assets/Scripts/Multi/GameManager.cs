using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class GameManager : MonoBehaviour
{
    static public GameManager _instance;

    [Tooltip("The prefab to use for representing the player")]
    [SerializeField]
    private GameObject _playerPrefab;
    public Transform[] _spawnPoints;
    public GameObject _localPlayer;
    public TextMeshProUGUI _playerCount;
    // Start is called before the first frame update

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    }

    void Update()
    {
        // ��� Ȯ���ϴ� ���� ���ϰ� ���ϴϱ� NetworkManager�� �ִ� ���� Ȯ���ϴ� ������ ����.
        _playerCount.text = PhotonNetwork.CountOfPlayers.ToString();
    }

    private void OnDestroy()
    {
    }

    public void SapwnPlayer()
    {
        Debug.Log("��ȯ");

        Transform spawnPoint = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)];

        GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, spawnPoint.position, Quaternion.identity);
        _localPlayer = player;

        PlayerStatus status = player.transform.GetChild(2).GetComponent<PlayerStatus>();
        PhotonView photonView = player.transform.GetChild(2).gameObject.GetComponent<PhotonView>();

        status.IsLocalPlayer();  // ���� �÷��̾ �°� ����
        photonView.RPC("SetNickname", RpcTarget.AllBuffered, NetworkManager._instance._nickName); // �̸� ����

        // --- ���� ����(���� ����) --
        Define.Role role = (PhotonNetwork.IsMasterClient) ? Define.Role.Houseowner : Define.Role.Robber;
        photonView.RPC("SetRole", RpcTarget.AllBuffered, role);
        // ----------------------

        if (status.Role == Define.Role.Robber)
            photonView.RPC("TransformIntoRobber", RpcTarget.AllBuffered);
        else if (status.Role == Define.Role.Houseowner) 
            photonView.RPC("TransformIntoHouseowner", RpcTarget.AllBuffered);
    }

    // ������ Ŭ���̾�Ʈ�� ������ ó���ϴ� �Լ�
    public void OnMasterClientKilled(Player killer)
    {
        if (PhotonNetwork.IsMasterClient) // �������� �ڽ��� ���� �÷��̾ ���������� ����
        {
            // ���ο� ������ Ŭ���̾�Ʈ ����
            PhotonNetwork.SetMasterClient(killer);
            Debug.Log("New Master Client is: " + killer.NickName);
        }
    }

    // �÷��̾��� ������ ó���ϴ� �Լ�
    public void OnPlayerKilled(Player killedPlayer, Player killer)
    {
        if (killedPlayer == PhotonNetwork.MasterClient) // ���ش��� �÷��̾ �������̸�
        {
            OnMasterClientKilled(killer);
        }
    }
}