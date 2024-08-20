using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ��Ʈ�ѷ�
/// </summary>
public class RobberController : MonoBehaviour
{
    PlayerStatus _playerStatus;
    [SerializeField] WeaponManager _weaponManager;

    void Awake()
    {
        _playerStatus = transform.parent.GetComponent<PlayerStatus>();
    }

    void Update()
    {
        // ��ü�� ������ �ְ� �ϱ�
        if (_playerStatus.Role == Define.Role.None) return;

        _weaponManager.UseSelectedWeapon();
    }
}