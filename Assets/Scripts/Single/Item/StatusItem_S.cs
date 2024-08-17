using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusItem_S : Item_S
{
    public Define.StatusItem _statusName;
    void Start()
    {
        _itemType = Define.Item.Status;
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
    void TakeStatusItem(Collider other)
    {
        PlayerStatus_S status = other.GetComponent<PlayerStatus_S>();
        if (status == null || base._itemType != Define.Item.Status) return;

        //StartCoroutine(_itemCylinder.FadeOutAndRespawn());
        _itemCylinder.HideSpawnItem();
        if (_statusName == Define.StatusItem.Heart)
        {
            status.Heal();
        }
        else if (_statusName == Define.StatusItem.Energy)
        {
            status.SpUp();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�������� �����Ÿ� �ȿ� ����");

        // ������ ��Ÿ�� �ƴҽÿ��� ����
        if (!_itemCylinder._usedItem)
            TakeStatusItem(other);
    }
}