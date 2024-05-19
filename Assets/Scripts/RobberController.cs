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
    PlayerController playerController;
    WeaponManager weaponManager;

    [Tooltip("���������� ���� �� ���")]
    RuntimeAnimatorController houseownerAnimController;
    Avatar houseownerAvatar;

    [Tooltip("ī�޶�")]
    GameObject cameras;
    GameObject quaterFollowCamera;
    GameObject thirdFollowCamera;
    GameObject aimCamera;

    void Awake()
    {
    }

    private void Start()
    {
        // ���� ����
        RobberInit();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.T)) // 'T' ������ ���������� ����
            TransformationHouseowner();
    }

    void RobberInit()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
        weaponManager = transform.parent.GetComponent<WeaponManager>();

        playerController.PlayerRole = Define.Role.Robber;

        PrepareToBeHouseowner();
        CameraInit();
        RobberWeaponInit();
    }

    void PrepareToBeHouseowner()
    {
        // ����->������ �ÿ� ����� ������ �ִϸ��̼� ���õ� �͵� �غ�
        houseownerAnimController = Resources.Load<RuntimeAnimatorController>("Animations/HouseownerAnimations/HouseownerAnimationController");
        houseownerAvatar = Resources.Load<Avatar>("Animations/HouseownerAnimations/HouseownerAvatar");
    }

    void CameraInit() // ī�޶� ����
    {
        // ī�޶� ������Ʈ ����
        cameras = Camera.main.gameObject.transform.parent.gameObject;
        quaterFollowCamera = cameras.transform.GetChild(1).gameObject;
        thirdFollowCamera = cameras.transform.GetChild(2).gameObject;
        aimCamera = cameras.transform.GetChild(3).gameObject;

        // ������ �´� ī�޶� ����
        quaterFollowCamera.SetActive(true);
        thirdFollowCamera.SetActive(false);
        aimCamera.SetActive(false);
    }

    void RobberWeaponInit() // ���� ���� ����
    {
        weaponManager.InitializeWeapon();
    }

    void TransformationHouseowner()
    {
        transform.parent.GetChild(0).gameObject.SetActive(false); // ���� ��Ȱ��ȭ
        transform.parent.GetChild(1).gameObject.SetActive(true);  // ������ Ȱ��ȭ
        playerController.PlayerRole = Define.Role.Houseowner;

        Debug.Log(playerController.PlayerRole);

        weaponManager.InitializeWeapon();                         // ������ ���� ����
        Debug.Log("���������� ���� �Ϸ�");
    }
}