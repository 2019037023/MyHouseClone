using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class NewWeaponManager : MonoBehaviour
{
    PlayerController _playerController;
    PlayerStatus _playerStatus;

    [Tooltip("���� ��ȯ �� ���� �ð��� ����")]
    public float _switchDelay = 1f;

    [Header("���� ����")]
    [SerializeField] public GameObject _leftItemHand;           // �޼տ� �ִ� ������ (�ڽ�: źâ)
    [SerializeField] public GameObject _rightItemHand;          // �����տ� �ִ� ������ (�ڽ�: ����)
    [SerializeField] public GameObject _melee;                  // ���� ���� ������Ʈ
    [SerializeField] public GameObject _gun;
    public Melee _meleeWeapon; // ���� ����
    public Gun _gunWeapon;

    [Header("���� ���� ����")]
    public int _selectedWeaponIdx = 0;
    public GameObject _selectedWeapon;

    void Start()
    {
        _playerController = transform.root.GetComponent<PlayerController>();
        InitRoleWeapon(); // ���ҿ� ���� ���� �ʱ�ȭ
    }
     
    void Update()
    {
        if(!_playerController._input.aim && !_playerController._input.reload) // �������� �ʰ�, �������� ���� �� ���� ��ü ����
            WeaponSwitching(); // ���� ��ü
    }

    /// <summary>
    /// ���ҿ� ���� ���� �ʱ�ȭ
    /// </summary>
    public void InitRoleWeapon()
    {
        _playerStatus = transform.root.GetComponent<PlayerStatus>();
        // ���ҿ� ���� ù ���� ����
        if (_playerStatus.Role == Define.Role.Robber) // ����
        {
            _selectedWeaponIdx = 0;
        }
        else if (_playerStatus.Role == Define.Role.Houseowner) // ������
        {
            _selectedWeaponIdx = 1;
        }
        Debug.Log("���ҿ� ���� ���� �ʱ�ȭ �Ϸ�");
    }


    /// <summary>
    /// ���� ��ü
    /// </summary>
    void WeaponSwitching()
    {
        int previousSelectedWeapon = _selectedWeaponIdx;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (_selectedWeaponIdx >= transform.childCount - 1)
                _selectedWeaponIdx = 0;
            else
                _selectedWeaponIdx++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (_selectedWeaponIdx <= 0)
                _selectedWeaponIdx = transform.childCount - 1;
            else
                _selectedWeaponIdx--;
        }

        // if(Input.GetKeyDown(KeyCode.Alpha1)) // ���� ����


        if (previousSelectedWeapon != _selectedWeaponIdx) // ���콺 �ٷ� ���� �ε��� �ٱ͸� ��ü
        {
            SelectWeapon();
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    void SelectWeapon()
    {
        int idx = 0;
        foreach(Transform weapon in transform)
        {
            if (idx == _selectedWeaponIdx)
            {
                weapon.gameObject.SetActive(true);
                _selectedWeapon = weapon.gameObject; // ���� �� ���� ����
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            idx++;
        }
    }

    /// <summary>
    /// ���õ� ���� ���
    /// </summary>
    public void UseSelectedWeapon()
    {
        if(_selectedWeapon.tag == "Melee")
        {
            _selectedWeapon.GetComponent<Melee>().Use();
        }
        else if(_selectedWeapon.tag == "Gun")
        {
            _selectedWeapon.GetComponent<Gun>().Use();
        }
        else
        {
            Debug.Log("This weapon has none tag");
        }
    }

    /// <summary>
    /// ���� �ݱ�
    /// </summary>
    void PickUp()
    {

    }

    /// <summary>
    /// ���� ������
    /// </summary>
    void Drop()
    {

    }
}
