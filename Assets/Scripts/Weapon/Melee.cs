using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� �� ���� ����
/// </summary>
public class Melee : Weapon
{
    BoxCollider _meleeArea;       // ���� ���� ����
    TrailRenderer _trailEffet;    // �ֵθ� �� ȿ��

    #region ���� ȿ��
    public LayerMask _sliceMask; // �ڸ� ����� ���̾� ����ũ
    public float _cutForce = 250f; // �ڸ� �� �������� ��

    Vector3 _entryPoint; // ������Ʈ�� �� ����
    Vector3 _exitPoint; // ������Ʈ�� �հ� ���� ����
    bool _hasExited = false; // ������Ʈ�� �հ� �������� ���θ� �����ϴ� ����
    #endregion

    void Awake()
    {
        base.Type = Define.Type.Melee;
        _meleeArea = gameObject.GetComponent<BoxCollider>();
        _trailEffet = gameObject.GetComponentInChildren<TrailRenderer>();

        // TODO
        /*
         * ���� �ɷ�ġ�� �����̳� json�� �̿��� ���� ����
         * ���� �о�ͼ� �� ������ ��������� ��
         * ���� �ӽ÷� �׽�Ʈ�� ���� �ϵ��ڵ� ��
        */
        if (gameObject.tag == "Melee")
            base.Attack = 50;

    }

    /// <summary>
    /// Use() �����ϸ鼭 MeleeAttackOn() �ڷ�ƾ ���� ����ȴ�.
    /// </summary>
    public override void Use()
    {
        StopCoroutine("MeleeAttackOn");
        StartCoroutine("MeleeAttackOn");
    }

    /// <summary>
    /// �ڷ�ƾ���� Collider, TrailRenderer Ư�� �ð� ���ȸ� Ȱ��ȭ
    /// </summary>
    IEnumerator MeleeAttackOn()
    {
        yield return new WaitForSeconds(0.5f);
        _meleeArea.enabled = true;
        _trailEffet.enabled = true;

        yield return new WaitForSeconds(0.5f);
        _meleeArea.enabled = false;

        yield return new WaitForSeconds(0.5f);
        _trailEffet.enabled = false;
    }

    // Į�� Ʈ���� �ȿ� ���� �� hasExited�� false�� ����
    void OnTriggerEnter(Collider other)
    {
        _hasExited = false;
        _entryPoint = other.ClosestPoint(transform.position);

        // ������ ����

        // �ڱ� �ڽſ��� ���� ��� ����
        if (other.transform.root.name == gameObject.name) return;

        if (other.GetComponent<Status>() != null)
        {
            other.GetComponent<Status>().TakedDamage(Attack);
        }
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("����");
    }
    void OnTriggerExit(Collider other) // ���� �� �Ǹ� ���̾ ���� ����
    {
        // �浹 ������ ������ �ڸ��� �������� ����
        _exitPoint = other.ClosestPoint(transform.position);

        Vector3 cutDirection = _exitPoint - _entryPoint;
        Vector3 cutInPlane = (_entryPoint + _exitPoint) / 2;

        //Vector3 cutPlaneNormal = Vector3.Cross((entryPoint - exitPoint), (entryPoint - transform.position)).normalized;
        Vector3 cutPlaneNormal = Vector3.Cross((_entryPoint - _exitPoint), (_entryPoint - transform.position)).normalized;
        Debug.Log(cutPlaneNormal.x + ", " + cutPlaneNormal.y + ", " + cutPlaneNormal.z);

        if (cutPlaneNormal.x == 0 && cutPlaneNormal.y == 0 && cutPlaneNormal.z == 0)
        {
            // ���� �ڸ��� ������ normalize �ؼ� �־���� ��
            cutPlaneNormal = (_entryPoint - _exitPoint).normalized;
            Debug.Log("��ü: " + cutPlaneNormal.x + " " + cutPlaneNormal.y + " " + cutPlaneNormal.z);

            bool isHorizontalCut = Mathf.Abs(cutDirection.x) > Mathf.Abs(cutDirection.y);

            // ���η� �ڸ��� ���
            if (isHorizontalCut)
            {
                // x �� �������� �ڸ��� ������ cutPlaneNormal�� x �� ���� ���ͷ� ����
                cutPlaneNormal = Vector3.up;
            }
            else // ���η� �ڸ��� ���
            {
                // y �� �������� �ڸ��� ������ cutPlaneNormal�� y �� ���� ���ͷ� ����
                cutPlaneNormal = Vector3.right;
            }
        }

        LayerMask cutableMask = LayerMask.GetMask(LayerMask.LayerToName(other.gameObject.layer));
        //Debug.Log("�߸� ���̾�: " + LayerMask.LayerToName(other.gameObject.layer));
        if (_sliceMask.value == cutableMask)
        {
            Debug.LogWarning("�ڸ� �� �ִ� ������Ʈ");
            // ������Ʈ�� �ڸ���
            Cutter.Cut(other.gameObject, cutInPlane, cutPlaneNormal);

            // �ڸ� �� �������� ���� �����Ͽ� ������Ʈ�� �о
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(-cutPlaneNormal * _cutForce); // cutDirection ��ſ� cutPlaneNormal�� ���
            }

            _hasExited = true;
        }
        else
        {
            //Debug.Log("sliceMask: " + sliceMask.value);
            //Debug.Log("�ڸ� ���̾�: " + other.gameObject.layer);
            Debug.LogWarning("�� �ȵ�?");
        }
    }
}
