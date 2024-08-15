using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ٰŸ�, ���Ÿ� ������� ��ӹ޴� �Ϲ�ȭ�� ���� Ŭ����
/// </summary>
public class Weapon : MonoBehaviour
{
    public Define.Type Type { get; set; } // ���� Ÿ��

    public Transform Master { get; set; } // ����

    public int Attack { get; set; }       // ���ݷ�
    public float Rate { get; set; } = 0.5f;      // ���ݼӵ�
    public float Range { get; set; } = 5f;


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