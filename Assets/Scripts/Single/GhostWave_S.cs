using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GhostWave : MonoBehaviour
{
    [SerializeField]
    private GameObject ghostPrefab;

    private IObjectPool<ModifiedMonster_S> _pool;

    public Transform ghostWavePosition;

    private void Awake()
    {
        Debug.Log("Awake ȣ���");
        _pool = new ObjectPool<ModifiedMonster_S>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 1);
        Debug.Log("_pool �ʱ�ȭ �Ϸ�");
    }

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            _pool.Get();
        }
    }

    private ModifiedMonster_S CreateMonster()
    {
        Debug.Log("CreateMonster ȣ���");
        ModifiedMonster_S monster = Instantiate(ghostPrefab, ghostWavePosition.position, Quaternion.identity).GetComponent<ModifiedMonster_S>();
        if (monster != null)
        {
            monster.SetManagedPool(_pool);
            monster.OnMonsterDied += HandleMonsterDied;
            Debug.Log("monster �ʱ�ȭ �Ϸ�");
        }
        else
        {
            Debug.LogError("ModifiedMonster_S ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // ModifiedMonster_S ��ũ��Ʈ�� ��Ȱ��ȭ�� ��� ������ Ȱ��ȭ
        if (!monster.enabled)
        {
            monster.enabled = true;
            Debug.Log("ModifiedMonster_S ��ũ��Ʈ�� ������ Ȱ��ȭ�߽��ϴ�.");
        }

        return monster;
    }

    private void OnGetMonster(ModifiedMonster_S monster)
    {
        Debug.Log("OnGetMonster ȣ���");
        monster.gameObject.SetActive(true);
    }

    private void OnReleaseMonster(ModifiedMonster_S monster)
    {
        Debug.Log("OnReleaseMonster ȣ���");
        monster.gameObject.SetActive(false);
    }

    private void OnDestroyMonster(ModifiedMonster_S monster)
    {
        Debug.Log("OnDestroyMonster ȣ���");
        Destroy(monster.gameObject);
    }
    private void HandleMonsterDied(ModifiedMonster_S monster)
    {
        Debug.Log("HandleMonsterDied ȣ���");
        _pool.Get(); // ���Ͱ� ���� �� ���ο� ���� ����
    }
}