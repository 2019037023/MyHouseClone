using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class ItemCylinder : MonoBehaviour
{
    [Tooltip("����� ����")]
    [SerializeField] float _fadeDuration = 0f; // ������� �ð�
    [SerializeField] float _respawnTimeSetValue; // ����� �ð�
    public bool _usedItem = false; // ������ ���Ǿ�����(��Ÿ��)

    [Tooltip("������ ����� �ð� UI ����")]
    public GameObject _timerHolder;
    public TextMeshPro _itemTimer;  // ������ ����� �ð� ǥ���� UI
    public float _respawnTime;      // ������ �Ǹ����� ǥ�õ� �ð�

    [Tooltip("������ �Ǹ������� ������ ������ ����")]
    public Define.Item _spawnItemType = Define.Item.None;
    public GameObject _spawnItemObject;
    public Item _spawnItem;

    public void InitSpawnItem(int _spawnItemTypeNum, int childIdxNum)
    {
        DisableItemType();
        _usedItem = false;

        // ������ ������
        _spawnItemType = (Define.Item)_spawnItemTypeNum;
        GameObject spawnItemObjectParent = transform.GetChild((int)_spawnItemType).gameObject;
        _spawnItemObject = spawnItemObjectParent.transform.GetChild(childIdxNum).gameObject;

        // ��ũ��Ʈ �����ϰ� ���� ������, ��ũ��Ʈ �߰�
        if (_spawnItemType == Define.Item.Status && _spawnItemObject.GetComponent<StatusItem>() == null)
            _spawnItemObject.AddComponent<StatusItem>();
        else if (_spawnItemType == Define.Item.Weapon && _spawnItemObject.GetComponent<WeaponItem>() == null)
            _spawnItemObject.AddComponent<WeaponItem>();
        _spawnItem = _spawnItemObject.GetComponent<Item>();

        _spawnItemObject.SetActive(true);

        // ����� �ð� UI ��Ȱ��ȭ
        SetRespawnTime();
        _timerHolder.SetActive(false);
    }

    void Update()
    {
        CountRespawnTime();
    }

    // ����� �ð� ����
    void CountRespawnTime()
    {
        if (_usedItem)
        {
            _respawnTime -= Time.deltaTime;
            _itemTimer.text = Mathf.FloorToInt(_respawnTime).ToString();
            if (_respawnTime <= 0)
            {
                // ������ Ŭ���̾�Ʈ���� ������ ��ȯ ��û
                GameManager._instance.gameObject.GetComponent<PhotonView>().RPC("ReceiveRequestToSpawnItemRPC", RpcTarget.MasterClient, int.Parse(gameObject.name));
            }
        }
    }

    public void HideSpawnItem()
    {
        _usedItem = true;
        _spawnItem.gameObject.SetActive(false);
        _timerHolder.SetActive(true);
    }

    public void SetRespawnTime()
    {
        _respawnTime = _respawnTimeSetValue;
    }

    // ������ ��ȯ �� Ȱ��ȭ �Ǿ� �ִ� ������ ��Ȱ��ȭ ó��
    public void DisableItemType()
    {
        Transform statusItem = transform.GetChild(1);
        Transform weaponItem = transform.GetChild(2);
        foreach(Transform item in statusItem)
        {
            item.gameObject.SetActive(false);
        }
        foreach (Transform item in weaponItem)
        {
            item.gameObject.SetActive(false);
        }
    }
}