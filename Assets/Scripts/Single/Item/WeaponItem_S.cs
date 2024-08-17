using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem_S : Item_S
{
    public Define.WeaponItem _weaponName;
    void Start()
    {
        _itemType = Define.Item.Weapon;
        base.InitItem();
    }

    void Update()
    {
        base.Floating();
    }

    /// <summary>
    /// ���� ���� ������ ȹ��
    /// </summary>
    /// <param name="other"></param>
    public void TakeWeaponItem(Collider other)
    {
        PlayerStatus_S status = other.GetComponent<PlayerStatus_S>();
        if (status == null || base._itemType != Define.Item.Weapon) return;

        _itemCylinder.HideSpawnItem();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�������� �����Ÿ� �ȿ� ����");

        PlayerStatus_S status = other.GetComponent<PlayerStatus_S>();
        if(status != null)
            status.nearMeleeObject = gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("�������� �����Ÿ� �ȿ� ����");
        PlayerStatus_S status = other.GetComponent<PlayerStatus_S>();
        // �÷��̾ �ְ�, ��ó ���� ���� Ž���� �����߰�, ������ �ݱ� ��ư�� ������, ������ ��Ÿ�� �ƴ� ��
        if (status != null && status.nearMeleeObject != null && status._isPickUp && !_itemCylinder._usedItem)
            TakeWeaponItem(other);
        status._isPickUp = false;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("�������� �����Ÿ� ���");
        PlayerStatus_S status = other.GetComponent<PlayerStatus_S>();

        if (status == null) return;

        status.nearMeleeObject = null;
    }
}