using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCylinder : MonoBehaviour
{   
    [SerializeField] float _fadeDuration = 0f; // ������� �ð�
    [Tooltip("����� �ð�")][SerializeField] float _respawnTimeSetValue = 30.0f; // ����� �ð�
    public bool _usedItem; // ������ ���Ǿ�����(��Ÿ��)
    
    // ������ ����� UI
    public GameObject _timerHolder; 
    public TextMeshPro _itemTimer;  // ������ ����� �ð� ǥ���� UI
    float _respawnTime = 30;             // ������ �Ǹ����� ǥ�õ� �ð�

    // ������ �Ǹ������� ������ ������
    public Define.Item _spawnItemType = Define.Item.None;
    public Item _spawnItem;

    void Awake()
    {
        // ������ ������
        GameObject spawmItemObject = transform.GetChild((int)_spawnItemType).gameObject;
        // ��ũ��Ʈ �߰�
        spawmItemObject.AddComponent<StatusItem>();
        _spawnItem = spawmItemObject.GetComponent<Item>();

        spawmItemObject.SetActive(true);

        // ����� �ð� UI ��Ȱ��ȭ
        _timerHolder.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) StartCoroutine(FadeOutAndRespawn()); // �׽�Ʈ �ڵ�

        CountRespawnTime();
    }

    // ����� �ð� ����
    void CountRespawnTime()
    {
        if (_usedItem)
        {
            _respawnTime -= Time.deltaTime;
            _itemTimer.text = Mathf.FloorToInt(_respawnTime).ToString();
        }
    }

    /// <summary>
    /// ������ ����ȭ
    /// </summary>
    /// <param name="other"></param>
    public IEnumerator FadeOutAndRespawn()
    {
        float currentTime = 0f;
        StatusItem statusItem = _spawnItem.GetComponent<StatusItem>();

        Color initialColor = statusItem.GetItemColor();

        // ������ �Ⱥ��̰� �ϱ�
        while (currentTime < _fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, currentTime / _fadeDuration);
            Color newColor = initialColor;
            newColor.a = alpha;
            statusItem.ChangeColor(newColor);
            yield return null;
        }

        // ������ ���ð� ���� �Ⱥ��̰� �ϱ�
        _usedItem = true;
        statusItem.EnableItem(false);
        _timerHolder.SetActive(true);
        yield return new WaitForSeconds(_respawnTimeSetValue);

        // ������ ������
        _usedItem = false;
        ResetRespawnTime();
        statusItem.EnableItem(true);
        statusItem.ChangeColor(initialColor);
        _timerHolder.SetActive(false);
    }

    public void ResetRespawnTime()
    {
        _respawnTime = _respawnTimeSetValue;
    }
}