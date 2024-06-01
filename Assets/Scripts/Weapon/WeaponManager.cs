using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Tooltip("���� ��ȯ �� ���� �ð��� ����")]
    public float _switchDelay = 1f;

    [Tooltip("�÷��̾ ����� �� �ִ� ���� ���")]
    public List<GameObject> _weaponList = new List<GameObject>();

    int _index = 0;
    bool _isSwitching = false;

    PlayerController _playerController;
    RobberController _robberController;
    HouseownerController _houseownerController;

    [Header("���� ����")]
    [SerializeField] public GameObject _leftItemHand;           // �޼տ� �ִ� ������ (�ڽ�: źâ)
    [SerializeField] public GameObject _rightItemHand;          // �����տ� �ִ� ������ (�ڽ�: ����)
    [SerializeField] public GameObject _melee;                  // ���� ���� ������Ʈ
    [SerializeField] public GameObject _gun;
    public Melee _meleeWeapon; // ���� ����
    public Gun _gunWeapon;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _robberController = transform.GetChild(0).GetComponent<RobberController>();
        _houseownerController = transform.GetChild(1).GetComponent<HouseownerController>();
    }

    public void PlayerWeaponInit() // ���� ����
    {
        // ���� ����Ʈ ����
        ClearWeaponList();

        // ���� ã��
        // ������ ��� �ڽ� ������Ʈ �� "Hand" �±׸� ���� Ȱ��ȭ�� ������Ʈ�� ã��
        GameObject[] itemHand = GameObject.FindGameObjectsWithTag("Hand");

        // �̸������� ����
        Array.Sort(itemHand, (a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));

        if (itemHand.Length < 2)
        {
            Debug.LogError("Both left and right hand objects are not found.");
            return;
        }

        _leftItemHand = itemHand[0];
        _rightItemHand = itemHand[1];

        // ���� ���� ������ ���� ���� ����
        _melee = _rightItemHand.transform.GetChild(0).gameObject;

        _meleeWeapon = _melee.GetComponent<Melee>();
        AddWeaponInList(_melee);

        // ���Ÿ� ���� ������ ���� ���� ����
        if (_rightItemHand.transform.childCount == 2) // �����տ� ���� �ִ� ���
        {
            _gun = _rightItemHand.transform.GetChild(1).gameObject;
            _gunWeapon = _gun.GetComponent<Gun>();
            AddWeaponInList(_gun);
        }
    }

    // ���� ��ȯ �Է��� ó���ϴ� �޼���
    public void HandleWeaponSwitching()
    {
        if (!_isSwitching)
        {
            // ���콺 �� ���� ��ũ�� �� ���� ����� ��ȯ
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                _index = (_index + 1) % _weaponList.Count;
                StartCoroutine(SwitchDelay(_index));
            }
            // ���콺 �� �Ʒ��� ��ũ�� �� ���� ����� ��ȯ
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                _index = (_index - 1 + _weaponList.Count) % _weaponList.Count;
                StartCoroutine(SwitchDelay(_index));
            }

            // ���� Ű(1~9)�� ���� ��ȯ
            for (int i = 49; i < 58; i++)
            {
                int keyIndex = i - 49;
                if (Input.GetKeyDown((KeyCode)i) && _weaponList.Count > keyIndex && _index != keyIndex)
                {
                    _index = keyIndex;
                    StartCoroutine(SwitchDelay(_index));
                }
            }
        }
    }

    // ���� ��Ͽ� ���ο� ���⸦ �߰�
    public void AddWeaponInList(GameObject weaponObject)
    {
        if (weaponObject != null && !_weaponList.Contains(weaponObject))
            _weaponList.Add(weaponObject);
    }

    // ���� ��Ͽ��� Ư�� ���⸦ ����
    public void RemoveWeaponInList(GameObject weaponObject)
    {
        if (weaponObject != null && _weaponList.Contains(weaponObject))
            _weaponList.Remove(weaponObject);
    }

    public void ClearWeaponList()
    {
        _weaponList.Clear();
    }

    // ���⸦ �ʱ� ���·� ����
    public void InitializeWeapon()
    {
        ClearWeaponList();
        PlayerWeaponInit();

        // ��� ���⸦ ��Ȱ��ȭ
        foreach (GameObject weapon in _weaponList)
        {
            if (weapon != null)
                weapon.SetActive(false);
        }

        // ���ҿ� ���� ù ���� ����
        if(_playerController.PlayerRole == Define.Role.Houseowner) // ������
        {
            //houseownerController.ChangeIsHoldGun(true);
            _index = Mathf.Clamp(1, 0, _weaponList.Count - 1); // �ε����� ����Ʈ ���� ���� �ִ��� Ȯ��
            if (_weaponList.Count > _index)
                _weaponList[_index].SetActive(true);
        }
        else if(_playerController.PlayerRole == Define.Role.Robber) // ����
        {
            _index = Mathf.Clamp(0, 0, _weaponList.Count - 1);
            if (_weaponList.Count > _index)
                _weaponList[_index].SetActive(true);
        }

        Debug.Log("������ ���� ���� �Ϸ�");
    }

    // ���� ��ȯ �� ���� �ð��� �߰��Ͽ� ���� ��ȯ ����
    IEnumerator SwitchDelay(int newIndex)
    {
        _isSwitching = true;
        SwitchWeapons(newIndex);
        yield return new WaitForSeconds(_switchDelay);
        _isSwitching = false;
    }

    void SwitchWeapons(int newIndex)
    {
        // ��� ���⸦ ��Ȱ��ȭ
        for (int i = 0; i < _weaponList.Count; i++)
        {
            if (_weaponList[i] != null)
                _weaponList[i].SetActive(false);
        }

        // ���� �ε����� ��ȿ���� Ȯ��
        if (newIndex < 0 || newIndex >= _weaponList.Count || _weaponList[newIndex] == null)
            return;

        // ���� �������� ���ο� ���� �������� ���� ���¸� ����
        if (_weaponList[newIndex].CompareTag("Melee"))
        {
            if (_houseownerController != null)
                _playerController.ChangeIsHoldGun(false);
        }
        else
        {
            if (_houseownerController != null)
                _playerController.ChangeIsHoldGun(true);
        }

        // ���ο� ���� Ȱ��ȭ
        _weaponList[newIndex].SetActive(true);
    }

    // ���� ��� �ִ� ������ �±� ���ϱ�
    public string GetCurrentWeaponTag()
    {
        return _weaponList[_index].tag;
    }
}