using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager_S : MonoBehaviour
{
    PlayerInputs _playerInputs;
    PlayerStatus_S _playerStatus;

    [Tooltip("���� ��ȯ �� ���� �ð��� ����")]
    public float _switchDelay = 1f;

    [Header("���� ����")]
    [SerializeField] public GameObject _leftItemHand;           // �޼տ� �ִ� ������ (�ڽ�: źâ)
    [SerializeField] public GameObject _rightItemHand;          // �����տ� �ִ� ������ (�ڽ�: ����)

    [Header("���� ���� ����")]
    public int _selectedWeaponIdx = 0;
    public GameObject _selectedWeapon;
    public GameObject _recentMelee; // the most recent Melee
    public bool _isHoldGun;

    void Awake()
    {
        _playerInputs = transform.root.GetChild(2).GetComponent<PlayerInputs>();
        _playerStatus = transform.root.GetChild(2).GetComponent<PlayerStatus_S>();
    }

    void Start()
    {
        InitRoleWeapon();

        _recentMelee = transform.Find("Knife").gameObject; // _recentMelee init
    }

    void Update()
    {
        if (!_playerInputs.aim && !_playerInputs.reload) // �������� �ʰ�, �������� ���� �� ���� ��ü ����
            WeaponSwitching(); // ���� ��ü

        if (Input.GetKeyDown(KeyCode.Q) && _selectedWeapon.name != "Rifle" && _selectedWeapon.name != "Knife")
        {
            DropWeapon();
        }
    }

    /// <summary>
    /// ���ҿ� ���� ���� �ʱ�ȭ
    /// </summary>
    public void InitRoleWeapon()
    {
        // ���ҿ� ���� ù ���� ����
        if (_playerStatus.Role == Define.Role.Robber) // ����
        {
            _selectedWeaponIdx = 0;
        }
        else if (_playerStatus.Role == Define.Role.Houseowner) // ������
        {
            _selectedWeaponIdx = 1;
        }
        SelectWeapon();

        Debug.Log("���ҿ� ���� ���� �ʱ�ȭ �Ϸ�");
    }

    /// <summary>
    /// ???? ???
    /// </summary>
    void WeaponSwitching()
    {
        int previousSelectedWeapon = _selectedWeaponIdx;

        if (_selectedWeapon.tag == "Melee") // if now pick weapon tag is Melee
        {
            _recentMelee = _selectedWeapon;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (_selectedWeapon == _recentMelee)
                _selectedWeaponIdx = 1;
            else if (_selectedWeapon.tag == "Gun")
                _selectedWeaponIdx = _recentMelee.transform.GetSiblingIndex();
        }

        if (previousSelectedWeapon != _selectedWeaponIdx) // ???�J ??? ???? ?��??? ???? ???
        {
            if (_playerStatus.Role == Define.Role.Robber) _selectedWeaponIdx = 0;

            SelectWeapon();
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    void SelectWeapon()
    {
        int idx = 0;
        foreach (Transform weapon in transform)
        {
            if (idx == _selectedWeaponIdx)
            {
                weapon.gameObject.SetActive(true);
                _selectedWeapon = weapon.gameObject; // ???? ???? ???? ????
                IsHoldGun();
                _playerStatus.ChangeIsHoldGun(_isHoldGun);
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
        if (_selectedWeapon.tag == "Melee")
        {
            _selectedWeapon.GetComponent<Melee_S>().Use();
        }
        else if (_selectedWeapon.tag == "Gun")
        {
            _selectedWeapon.GetComponent<Gun_S>().Use();
        }
        else
        {
            Debug.Log("This weapon has none tag");
        }
    }

    // Check Hold Gun, To apply for Gun animation
    void IsHoldGun()
    {
        if (_selectedWeapon.tag == "Gun")
            _isHoldGun = true;
        else if (_selectedWeapon.tag == "Melee")
            _isHoldGun = false;
        else
        {
            Debug.Log("This weapon has none tag");
        }
    }


    /// <summary>
    /// ���� �ݱ�
    /// </summary>
    public void PickUpWeapon(string meleeName)
    {
        Transform newMelee = transform.Find(meleeName);
        
        if(_isHoldGun)
        {

        }        
        else
        {
            _selectedWeapon.SetActive(false);
            _selectedWeapon = newMelee.gameObject;
            newMelee.gameObject.SetActive(true);
        }


        WeaponData weapon = GameManager_S._instance.GetWeaponStatusByName(meleeName);
        if (weapon != null)
        {
            Debug.Log($"Picked up {weapon.Name}. Attack: {weapon.Attack}, Rate: {weapon.Rate}");
            Melee_S _currentWeapon = _selectedWeapon.GetComponent<Melee_S>();
            _currentWeapon.Attack = weapon.Attack;
            _currentWeapon.Rate = weapon.Rate;
            _currentWeapon.Range = weapon.Range;
        }
        else
        {
            Debug.LogWarning("Weapon not found!");
        }
    }

    /// <summary>
    /// ���� ������
    /// </summary>
    void DropWeapon()
    {
        GameObject droppedSelectedWeapon = Instantiate(_selectedWeapon, _selectedWeapon.transform.position, _selectedWeapon.transform.rotation); // instatntiation.
        Destroy(droppedSelectedWeapon.GetComponent<Melee_S>()); // Melee_S script delete for error prevention. 

        droppedSelectedWeapon.transform.localScale = droppedSelectedWeapon.transform.localScale * 1.7f; // size up.

        StartCoroutine(DropAndBounce(droppedSelectedWeapon));

        _selectedWeapon.SetActive(false);
        _selectedWeapon = transform.Find("Knife").gameObject;
        _selectedWeapon.SetActive(true);
        WeaponData weapon = GameManager_S._instance.GetWeaponStatusByName("Knife");
        Melee_S _currentWeapon = _selectedWeapon.GetComponent<Melee_S>();
        _currentWeapon.Attack = weapon.Attack;
        _currentWeapon.Rate = weapon.Rate;
        _currentWeapon.Range = weapon.Range;
    }


    IEnumerator DropAndBounce(GameObject droppedSelectedWeapon)
    {
        float floorY = transform.root.GetChild(2).position.y + 0.3f; // floorY is Player object's position.y + 0.3f.

        Vector3 velocity = new Vector3(0, -1f, 0); // first velocity.
        float gravity = -9.8f;
        float bounceDamping = 0.6f;
        float horizontalDamping = 0.98f;

        while (true)
        {
            droppedSelectedWeapon.transform.position += velocity * Time.deltaTime;

            
            if (droppedSelectedWeapon.transform.position.y <= floorY)
            {
                //bouncing.
                droppedSelectedWeapon.transform.position = new Vector3(droppedSelectedWeapon.transform.position.x, floorY, droppedSelectedWeapon.transform.position.z);
                velocity.y = -velocity.y * bounceDamping;

                velocity.x *= horizontalDamping;
                velocity.z *= horizontalDamping;

                if (Mathf.Abs(velocity.y) < 0.1f)
                {
                    velocity.y = 0;
                    break;
                }
            }
            else
            {
                // gravity.
                velocity.y += gravity * Time.deltaTime;
            }

            yield return null; // wait for next frame.
        }

        yield return new WaitForSeconds(1f);
        Destroy(droppedSelectedWeapon);

    }
}