using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ���� ��Ʈ�ѷ�
/// </summary>
public class RobberController : NetworkBehaviour
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
        if (!IsLocalPlayer) return;

        // ��ü�� ������ �ְ� �ϱ�
        if (_playerStatus.Role == Define.Role.None) return;

        if (Input.GetKeyUp(KeyCode.T)) // 'T' ������ ���������� ����
            _playerStatus.TransformIntoHouseowner();

        _weaponManager.UseSelectedWeapon();
    }

    void RobberInit()
    {
        _playerStatus.Role = Define.Role.Robber;

        Animator robberAnimator = gameObject.GetComponent<Animator>();
        RuntimeAnimatorController robberAnimController = robberAnimator.runtimeAnimatorController;
        Avatar robberAvatar = robberAnimator.avatar;

        // Player ��ü���� ���� �ִϸ����Ͱ� �����ϹǷ� ���̰� �ȴ�. ���� Robber�� �ִϸ����͸� ����ش�.
        robberAnimator.runtimeAnimatorController = null;
        robberAnimator.avatar = null;

        _playerStatus.SetRoleAnimator(robberAnimController, robberAvatar);
    }
}