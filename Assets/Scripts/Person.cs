using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    Status _status;
    Animator _animator;
    public Melee _melee;
    [SerializeField] public Define.Role PlayerRole { get; set; } = Define.Role.None;

    List<Renderer> _renderers;

    void Start()
    {
        PlayerRole = Define.Role.Robber;
        _status = gameObject.GetComponent<Status>();
        _animator = GetComponentInChildren<Animator>();

        // �÷��̾� ������ ��� ���͸��� ���ϱ�
        _renderers = new List<Renderer>();
        Transform[] playerUnderTransforms = GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < playerUnderTransforms.Length; i++)
        {
            Renderer renderer = playerUnderTransforms[i].GetComponent<Renderer>();
            if (renderer != null)
                _renderers.Add(renderer);
        }
    }

    void Update()
    {
        if (PlayerRole == Define.Role.None) return;

        if (Input.GetKeyDown(KeyCode.Y)) 
        {
            _animator.SetTrigger("setAttack");
            _melee.Use();
        }

        Dead();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("��Ҵ�");

        /*
         * ���� ���⿡�� �����
         * ���߿� ���Ÿ� ���⿡ �ǰ� ������ ���� �ϵ� ó���� ��� ��
         * Tag�� �̿��ϱ� ���ٴ� Weapon.cs�� ��� Type���� ������ ���� 
         */
    }

    // ���
    public void Dead()
    {
        if (PlayerRole != Define.Role.None && _status.Hp <= 0)
        {
            _animator.SetTrigger("setDie");
            PlayerRole = Define.Role.None; // ��ü
            StartCoroutine(DeadSinkCoroutine());
        }
    }

    IEnumerator DeadSinkCoroutine()
    {
        yield return new WaitForSeconds(3f);
        while (transform.position.y > -1.5f)
        {
            transform.Translate(Vector3.down * 0.1f * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }

    public void HitChangeMaterials()
    {
        for (int i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].material.color = Color.red;
            Debug.Log("�����Ѵ�.");
            Debug.Log(_renderers[i].material.name);
        }

        StartCoroutine(ResetMaterialAfterDelay(1.7f));
        Debug.Log("���ݹ��� ���� ü��:" + _status.Hp);
    }

    IEnumerator ResetMaterialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < _renderers.Count; i++)
            _renderers[i].material.color = Color.white;
    }
}
