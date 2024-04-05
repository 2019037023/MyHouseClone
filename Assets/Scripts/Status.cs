using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public float Hp { get; private set; } = 100;    // ü��
    public float Sp { get; private set; } = 100;    // ���׹̳�
    public float MaxHp { get; private set; } = 100; // �ִ� ü��
    public float MaxSp { get; private set; } = 100; // �ִ� ���׹̳�
    public float Defence { get; private set; } = 1; // ����

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
}
