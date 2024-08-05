using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus_S : MonoBehaviour
{
    #region ���� �� �ɷ�ġ
    //[field: SerializeField] public Define.Role Role = Define.Role.None; // ���͵� ��ü�� �Ǵϱ� Role �ʿ��Ϸ���? -> Yes: �� �ڵ� Ȱ��ȭ, No: �� �ڵ� �����
    [field: SerializeField] public float Hp { get; set; } = 100;    // ü��
    [field: SerializeField] public float MaxHp { get; private set; } = 100; // �ִ� ü��
    // ���� ������
    #endregion

    #region �ִϸ��̼� �� ����
    Animator _animator;
    List<Renderer> _renderers;
    #endregion

    #region ���� ǥ�ÿ�
    #endregion


    void Awake()
    {
        _animator = GetComponent<Animator>();

        // ���� ��������
        _renderers = new List<Renderer>();
        Transform[] underTransforms = GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < underTransforms.Length; i++)
        {
            Renderer renderer = underTransforms[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                _renderers.Add(renderer);
                // if (renderer.material.color == null) Debug.Log("�� ���� ��?");
            }
        }
    }

    void Update()
    {
        Dead();
    }

    /// <summary>
    /// ������ �Ա�
    /// </summary>
    /// <param name="attack"> ���� ���ݷ� </param>
    public void TakedDamage(int attack)
    {
        // ���ذ� ������� ȸ���Ǵ� ������ �Ͼ�Ƿ� ������ ���� 0�̻����� �ǰԲ� ����
        float damage = Mathf.Max(0, attack);
        Hp -= damage;

        Debug.Log(gameObject.name + "(��)�� " + damage + " ��ŭ ���ظ� �Ծ���!");
        Debug.Log("���� ü��: " + Hp);
    }

    /// <summary>
    /// �ִ� ü���� 0.2��ŭ ȸ��
    /// </summary>
    public void Heal()
    {
        // ���� ü���� �ִ� ü�º��� ���� ���� ȸ�� ����
        if (Hp < MaxHp)
        {
            // ȸ����
            float healAmount = MaxHp * 0.2f;

            // ȸ������ ���� ü�°��� ���� �ִ� ü���� ���� �ʵ��� ����
            float healedAmount = Mathf.Clamp(Hp + healAmount, 0, MaxHp) - Hp;

            Debug.Log("���� ü��" + Hp);
            // ü�� ȸ��
            Hp += healedAmount;
            Debug.Log("ü���� " + healedAmount + "��ŭ ȸ��!");
            Debug.Log("���� ü��: " + Hp);
        }
        else
        {
            Debug.Log("�ִ� ü��. ȸ���� �ʿ� ����.");
        }
    }

    /// <summary>
    /// ���
    /// </summary>
    public void Dead()
    {
        if (Hp <= 0)
        {
            _animator.SetTrigger("setDie");
            //Role = Define.Role.None; // ���͵� ��ü�� �Ǵϱ� Role �ʿ��Ϸ���? -> Yes: �� �ڵ� Ȱ��ȭ, No: �� �ڵ� �����
            StartCoroutine(DeadSinkCoroutine());
        }
    }

    /// <summary>
    /// ��ü �ٴ����� ����ɱ�
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// ���� ������ Material �Ӱ� ��ȭ
    /// </summary>
    public void HitChangeMaterials()
    {
        for (int i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].material.color = Color.red;
            Debug.Log("�� ���Ѵ�.");
            //Debug.Log(_renderers[i].material.name);
        }

        StartCoroutine(ResetMaterialAfterDelay(1.7f));
        Debug.Log("���ݹ��� ���� ü��:" + Hp);
    }

    /// <summary>
    /// ���� �ް� Material ������� ����
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator ResetMaterialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < _renderers.Count; i++)
            _renderers[i].material.color = Color.white;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee" || other.tag == "Gun") // Melee�� Gun�� ���̸� ���� �ٲ�� HitChangeMaterials() ȣ��
            HitChangeMaterials();
    }

    public void SetRoleAnimator(RuntimeAnimatorController animController, Avatar avatar)
    {
        _animator.runtimeAnimatorController = animController;
        _animator.avatar = avatar;

        // �ִϸ����� �Ӽ� ��ü�ϰ� ���ٰ� �Ѿ� ������
        _animator.enabled = false;
        _animator.enabled = true;
    }
}
