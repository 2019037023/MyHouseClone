using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] public float Hp { get; private set; } = 100;    // ü��
    [SerializeField] public float Sp { get; private set; } = 100;    // ���׹̳�
    [SerializeField] public float MaxHp { get; private set; } = 100; // �ִ� ü��
    [SerializeField] public float MaxSp { get; private set; } = 100; // �ִ� ���׹�
    [SerializeField] public float Defence { get; private set; } = 1; // ����

    /// <summary>
    /// ������ ó�� �Լ�
    /// </summary>
    public void TakedDamage(int attack)
    {
        // ���ذ� ������� ȸ���Ǵ� ������ �Ͼ�Ƿ� ������ ���� 0�̻����� �ǰԲ� ����
        float damage = Mathf.Max(0, attack - Defence);
        Hp -= damage;

        Debug.Log(gameObject.name + "(��)�� " + damage + " ��ŭ ���ظ� �Ծ���!");
    }

    // �ִ� ü���� 0.2��ŭ ȸ��
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

    // �ִ� ���׹̳� ���� ȸ��
    public void SpUp()
    {
        // ���� ���׹̳��� �ִ� ���׹̳����� ���� ���� ȸ�� ����
        if (Sp < MaxSp)
        {
            // ȸ������ ���� ���׹̳����� ���� �ִ� ���׹̳��� ���� �ʵ��� ����
            float healedAmount = Mathf.Clamp(Sp + MaxSp, 0, MaxHp) - Sp;

            Debug.Log("���� ���׹̳�" + Sp);
            // ���׹̳� ȸ��
            Sp += healedAmount;
            Debug.Log("���� ȸ��! ���� Sp: " + Sp);
        }
        else
        {
            Debug.Log("�ִ� Sp. ȸ���� �ʿ� ����.");
        }
    }

    public void SpDown()
    {

    }

    public void DefenceUp()
    {

    }
}
