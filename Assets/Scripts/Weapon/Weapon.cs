using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// �ٰŸ�, ���Ÿ� ������� ��ӹ޴� �Ϲ�ȭ�� ���� Ŭ����
/// </summary>
public class Weapon : NetworkBehaviour
{
    public Define.Type Type { get; protected set; } // ���� Ÿ��

    public Transform Master { get; protected set; } // ����

    public int Attack { get; protected set; }       // ���ݷ�
    public float Rate { get; protected set; } = 0.5f;      // ���ݼӵ�


    void Awake()
    {
        RecordMaster();
        // TODO
        /*
         ���Ⱑ �پ����� �� ���� �̸��̳� Ÿ�Կ� ����
         �������� ���ݼӵ��� �����ϴ� �۾��� ����� ��
         */
    }

    /// <summary>
    /// Use() �����ϸ鼭 �� ���⿡ �´� ���� ȿ�� �ڷ�ƾ�� ���� ����ȴ�.
    /// </summary>
    public virtual void Use()
    {
        // TODO
        // ���⿡ �´� ���� ���
    }

    /// <summary>
    /// ���� ������ �������� Ȯ��
    /// </summary>
    //public virtual void MasterPerception()
    //{
    //    if (Master != null)
    //    {
    //        Debug.Log("Master: " + Master.name);
    //    }
    //    else
    //    {
    //        Debug.Log("No master assigned.");
    //    }
    //}

    /// <summary>
    /// �ֻ��� �θ� �������� ����ϴ� �޼���
    /// </summary>
    public void RecordMaster()
    {
        // �ֻ��� �θ� Master�� ����
        Master = transform.root.GetChild(2);
        Debug.Log("���� ����: " + Master.name);
    }
}