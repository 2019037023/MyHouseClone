using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ���� ��Ʈ�ѷ�
/// </summary>
public class RobberController : MonoBehaviour
{
    PlayerController _playerController;
    PlayerStatus _playerStatus;
    WeaponManager _weaponManager;

    [Tooltip("ī�޶�")]
    GameObject _cameras;
    GameObject _quaterFollowCamera;
    GameObject _thirdFollowCamera;
    GameObject _aimCamera;

    void Start()
    {
        // ���� ����
        RobberInit();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.T)) // 'T' ������ ���������� ����
            _playerStatus.TransformIntoHouseowner();
    }

    void RobberInit()
    {
        _playerController = transform.parent.GetComponent<PlayerController>();
        _playerStatus = transform.parent.GetComponent<PlayerStatus>();
        _weaponManager = transform.parent.GetComponent<WeaponManager>();

        _playerController.PlayerRole = Define.Role.Robber;

        CameraInit();
        RobberWeaponInit();
    }

    void CameraInit() // ī�޶� ����
    {
        // ī�޶� ������Ʈ ����
        _cameras = Camera.main.gameObject.transform.parent.gameObject;
        _quaterFollowCamera = _cameras.transform.GetChild(1).gameObject;
        _thirdFollowCamera = _cameras.transform.GetChild(2).gameObject;
        _aimCamera = _cameras.transform.GetChild(3).gameObject;

        // ������ �´� ī�޶� ����
        _quaterFollowCamera.SetActive(true);
        _thirdFollowCamera.SetActive(false);
        _aimCamera.SetActive(false);
    }

    void RobberWeaponInit() // ���� ���� ����
    {
        //_weaponManager.InitializeWeapon();
    }
}