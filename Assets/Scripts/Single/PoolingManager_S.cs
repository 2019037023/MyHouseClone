using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolingManager_S : MonoBehaviour
{
    public static PoolingManager_S _instance;
    
    [SerializeField]
    private GameObject _monsterPrefab;

    private IObjectPool<ModifiedMonster_S> _pool;

    public List<Transform> _waveSpawnPoints = new List<Transform>();
    float spawnGhostInterval = 60f;  // ���� ���� ����, ó���� 60�� �ִٰ� ����.
    int additionalSpawnGhostCount = 0;  // �߰� ������ ���� ��

    int spawnPointIdx = 0;


    private void Awake()
    {
        _instance = this;

        if (_pool == null)
        {
            Debug.Log("Centralized pool initialization in Awake");
            _pool = new ObjectPool<ModifiedMonster_S>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 100);
        }
    }

    private void Start()
    {
        // ó���� �� GhostWave ������Ʈ���� 2������ ����
        for (int i = 0; i < 2; i++)
        {
            _pool.Get();
        }

        StartCoroutine(SpawnGhostsOverTime());
    }

    private IEnumerator SpawnGhostsOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnGhostInterval);

            additionalSpawnGhostCount++;
            spawnGhostInterval += 20f;

            for (int i = 0; i < additionalSpawnGhostCount; i++)
            {
                CreateMonster();
            }
        }
    }

    private ModifiedMonster_S CreateMonster()
    {
        if (spawnPointIdx == _waveSpawnPoints.Count)
            spawnPointIdx = spawnPointIdx % _waveSpawnPoints.Count;

        Vector3 randomPosition = _waveSpawnPoints[spawnPointIdx].position + Random.insideUnitSphere * 7f;
        randomPosition.y = 0; // Ghost ���� �� position.y ���� 0�̵��� ����

        Debug.Log("CreateMonster called");
        ModifiedMonster_S monster = Instantiate(_monsterPrefab, randomPosition, Quaternion.identity, _waveSpawnPoints[spawnPointIdx]).GetComponent<ModifiedMonster_S>();
        if (monster != null)
        {
            monster.SetManagedPool(_pool);
            Debug.Log("monster initialized");
        }
        else
        {
            Debug.LogError("ModifiedMonster_S component not found");
        }

        spawnPointIdx++;

        return monster;
    }

    private void OnGetMonster(ModifiedMonster_S monster)
    {
        Debug.Log("OnGetMonster called");
        monster.gameObject.SetActive(true);
    }

    private void OnReleaseMonster(ModifiedMonster_S monster)
    {
        Debug.Log("OnReleaseMonster called");
        monster.gameObject.SetActive(false);
    }

    private void OnDestroyMonster(ModifiedMonster_S monster)
    {
        Debug.Log("OnDestroyMonster called");
        Destroy(monster.gameObject);
    }
}