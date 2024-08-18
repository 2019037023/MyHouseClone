using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item
{
    public Define.WeaponItem _weaponName;
    ItemCylinder _itemCylinder; // ������ ��ȯ�� ��
    void Start()
    {
        _itemType = Define.Item.Weapon;
        _itemCylinder = transform.parent.parent.GetComponent<ItemCylinder>();
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
        PlayerStatus status = other.GetComponent<PlayerStatus>();
        if (status == null || base._itemType != Define.Item.Weapon) return;

        _itemCylinder.HideSpawnItem();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�������� �����Ÿ� �ȿ� ����");

        PlayerStatus status = other.GetComponent<PlayerStatus>();
        if(status != null)
            status.nearMeleeObject = gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("�������� �����Ÿ� �ȿ� ����");
        PlayerStatus status = other.GetComponent<PlayerStatus>();
        // �÷��̾ �ְ�, ��ó ���� ���� Ž���� �����߰�, ������ �ݱ� ��ư�� ������, ������ ��Ÿ�� �ƴ� ��
        if (status != null && status.nearMeleeObject != null && status._isPickUp && !_itemCylinder._usedItem)
            TakeWeaponItem(other);
        status._isPickUp = false;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("�������� �����Ÿ� ���");
        PlayerStatus status = other.GetComponent<PlayerStatus>();

        if (status == null) return;

        status.nearMeleeObject = null;
    }
}