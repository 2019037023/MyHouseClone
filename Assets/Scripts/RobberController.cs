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
    PlayerStatus _playerStatus;
    [SerializeField] NewWeaponManager _weaponManager;

    void Awake()
    {
        _playerStatus = transform.parent.GetComponent<PlayerStatus>();
    }
    void Start()
    {
        RobberInit(); // ���� ����
    }

    void Update()
    {
        // ��ü�� ������ �ְ� �ϱ�
        if (_playerStatus.Role == Define.Role.None) return;

        if (Input.GetKeyUp(KeyCode.T)) // 'T' ������ ���������� ����
            _playerStatus.TransformIntoHouseowner();

        _weaponManager.UseSelectedWeapon();
    }

    void RobberInit()
    {
        _playerStatus.Role = Define.Role.Robber;
    }
}