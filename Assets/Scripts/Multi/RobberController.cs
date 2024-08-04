using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    void Start()
    {
        RobberInit(); // ���� ����
        //_weaponManager.InitRoleWeapon();
    }

    void Update()
    {

        // ��ü�� ������ �ְ� �ϱ�
        if (_playerStatus.Role == Define.Role.None) return;

        _weaponManager.UseSelectedWeapon();
    }

    void RobberInit()
    {
        Animator robberAnimator = gameObject.GetComponent<Animator>();
        RuntimeAnimatorController robberAnimController = robberAnimator.runtimeAnimatorController;
        Avatar robberAvatar = robberAnimator.avatar;

        //// Player ��ü���� ���� �ִϸ����Ͱ� �����ϹǷ� ���̰� �ȴ�. ���� Robber�� �ִϸ����͸� ����ش�.
        //robberAnimator.runtimeAnimatorController = null;
        //robberAnimator.avatar = null;

        //_playerStatus.SetRoleAnimator(robberAnimController, robberAvatar);
    }
}