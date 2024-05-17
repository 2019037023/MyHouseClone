using System.Collections;
using System.Collections.Generic;
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
        playerController = GetComponent<PlayerController>();
        houseownerController = GetComponent<HouseownerController>();
        robberController = GetComponent<RobberController>();

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
                houseownerController.ChangeIsHoldGun(false);
        }
        else
        {
            if (houseownerController != null)
                houseownerController.ChangeIsHoldGun(true);
        }

        // ���ο� ���� Ȱ��ȭ
        weaponList[newIndex].SetActive(true);
    }
}