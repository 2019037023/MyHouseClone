using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Tooltip("���� ��ȯ �� ���� �ð��� ����")]
    public float switchDelay = 1f;

    [Tooltip("�÷��̾ ����� �� �ִ� ���� ���")]
    public List<GameObject> weaponList = new List<GameObject>();

    private int index = 0;
    private bool isSwitching = false;

    private PlayerController playerController;
    private RobberController robberController;
    private HouseownerController houseownerController;


    [Header("���� ����")]
    [SerializeField] public GameObject _leftItemHand;           // �޼տ� �ִ� ������ (�ڽ�: źâ)
    [SerializeField] public GameObject _rightItemHand;          // �����տ� �ִ� ������ (�ڽ�: ����)
    [SerializeField] public GameObject _melee;                  // ���� ���� ������Ʈ
    [SerializeField] public GameObject _gun;
    public Melee meleeWeapon; // ���� ����
    public Gun gunWeapon;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        robberController = transform.GetChild(0).GetComponent<RobberController>();
        houseownerController = transform.GetChild(1).GetComponent<HouseownerController>();
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

        meleeWeapon = _melee.GetComponent<Melee>();
        AddWeaponInList(_melee);

        // ���Ÿ� ���� ������ ���� ���� ����
        if (_rightItemHand.transform.childCount == 2) // �����տ� ���� �ִ� ���
        {
            _gun = _rightItemHand.transform.GetChild(1).gameObject;
            gunWeapon = _gun.GetComponent<Gun>();
            AddWeaponInList(_gun);
        }
    }



    // ���� ��ȯ �Է��� ó���ϴ� �޼���
    public void HandleWeaponSwitching()
    {
        if (!isSwitching)
        {
            // ���콺 �� ���� ��ũ�� �� ���� ����� ��ȯ
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                index = (index + 1) % weaponList.Count;
                StartCoroutine(SwitchDelay(index));
            }
            // ���콺 �� �Ʒ��� ��ũ�� �� ���� ����� ��ȯ
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                index = (index - 1 + weaponList.Count) % weaponList.Count;
                StartCoroutine(SwitchDelay(index));
            }

            // ���� Ű(1~9)�� ���� ��ȯ
            for (int i = 49; i < 58; i++)
            {
                int keyIndex = i - 49;
                if (Input.GetKeyDown((KeyCode)i) && weaponList.Count > keyIndex && index != keyIndex)
                {
                    index = keyIndex;
                    StartCoroutine(SwitchDelay(index));
                }
            }
        }
    }

    // ���� ��Ͽ� ���ο� ���⸦ �߰�
    public void AddWeaponInList(GameObject weaponObject)
    {
        if (weaponObject != null && !weaponList.Contains(weaponObject))
            weaponList.Add(weaponObject);
    }

    // ���� ��Ͽ��� Ư�� ���⸦ ����
    public void RemoveWeaponInList(GameObject weaponObject)
    {
        if (weaponObject != null && weaponList.Contains(weaponObject))
            weaponList.Remove(weaponObject);
    }

    public void ClearWeaponList()
    {
        weaponList.Clear();
    }

    // ���⸦ �ʱ� ���·� ����
    public void InitializeWeapon()
    {
        ClearWeaponList();
        PlayerWeaponInit();

        // ��� ���⸦ ��Ȱ��ȭ
        foreach (GameObject weapon in weaponList)
        {
            if (weapon != null)
                weapon.SetActive(false);
        }

        // ���ҿ� ���� ù ���� ����
        if(playerController.PlayerRole == Define.Role.Houseowner) // ������
        {
            //houseownerController.ChangeIsHoldGun(true);
            index = Mathf.Clamp(1, 0, weaponList.Count - 1); // �ε����� ����Ʈ ���� ���� �ִ��� Ȯ��
            if (weaponList.Count > index)
                weaponList[index].SetActive(true);
        }
        else if(playerController.PlayerRole == Define.Role.Robber) // ����
        {
            index = Mathf.Clamp(0, 0, weaponList.Count - 1);
            if (weaponList.Count > index)
                weaponList[index].SetActive(true);
        }

        Debug.Log("������ ���� ���� �Ϸ�");
    }

    // ���� ��ȯ �� ���� �ð��� �߰��Ͽ� ���� ��ȯ ����
    private IEnumerator SwitchDelay(int newIndex)
    {
        isSwitching = true;
        SwitchWeapons(newIndex);
        yield return new WaitForSeconds(switchDelay);
        isSwitching = false;
    }

    private void SwitchWeapons(int newIndex)
    {
        // ��� ���⸦ ��Ȱ��ȭ
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (weaponList[i] != null)
                weaponList[i].SetActive(false);
        }

        // ���� �ε����� ��ȿ���� Ȯ��
        if (newIndex < 0 || newIndex >= weaponList.Count || weaponList[newIndex] == null)
        {
            return;
        }

        // ���� �������� ���ο� ���� �������� ���� ���¸� ����
        if (weaponList[newIndex].CompareTag("Melee"))
        {
            if (houseownerController != null)
                playerController.ChangeIsHoldGun(false);
        }
        else
        {
            if (houseownerController != null)
                playerController.ChangeIsHoldGun(true);
        }

        // ���ο� ���� Ȱ��ȭ
        weaponList[newIndex].SetActive(true);
    }
}