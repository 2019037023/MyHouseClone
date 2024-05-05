using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� �� ���� ����
/// </summary>
public class Melee : Weapon
{
    BoxCollider meleeArea;       // ���� ���� ����
    TrailRenderer trailEffet;    // �ֵθ� �� ȿ��

    #region ���� ȿ��
    public LayerMask sliceMask; // �ڸ� ����� ���̾� ����ũ
    public float cutForce = 250f; // �ڸ� �� �������� ��

    private Vector3 entryPoint; // ������Ʈ�� �� ����
    private Vector3 exitPoint; // ������Ʈ�� �հ� ���� ����
    private Vector3 cutDirection; // �ڸ��� ����
    private bool hasExited = false; // ������Ʈ�� �հ� �������� ���θ� �����ϴ� ����
    #endregion

    void Awake()
    {
        base.Type = Define.Type.Melee;
        meleeArea = gameObject.GetComponent<BoxCollider>();
        trailEffet = gameObject.GetComponentInChildren<TrailRenderer>();

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
    /// Use() �����ϸ鼭 Attack() �ڷ�ƾ ���� ����ȴ�.
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
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;
        trailEffet.enabled = true;

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffet.enabled = false;
    }

    // Į�� Ʈ���� �ȿ� ���� �� hasExited�� false�� ����
    private void OnTriggerEnter(Collider other)
    {
        hasExited = false;
        entryPoint = other.ClosestPoint(transform.position);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("����");
    }
    private void OnTriggerExit(Collider other)
    {
        // �浹 ������ ������ �ڸ��� �������� ����
        exitPoint = other.ClosestPoint(transform.position);

        Vector3 cutDirection = exitPoint - entryPoint;
        Vector3 cutInPlane = (entryPoint + exitPoint) / 2;

        //Vector3 cutPlaneNormal = Vector3.Cross((entryPoint - exitPoint), (entryPoint - transform.position)).normalized;
        Vector3 cutPlaneNormal = Vector3.Cross((entryPoint - exitPoint), (entryPoint - transform.position)).normalized;
        Debug.Log(cutPlaneNormal.x + ", " + cutPlaneNormal.y + ", " + cutPlaneNormal.z);

        if (cutPlaneNormal.x == 0 && cutPlaneNormal.y == 0 && cutPlaneNormal.z == 0)
        {
            // ���� �ڸ��� ������ normalize �ؼ� �־���� ��
            cutPlaneNormal = (entryPoint - exitPoint).normalized;
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
        if (sliceMask.value == cutableMask)
        {
            Debug.LogWarning("�ڸ� �� �ִ� ������Ʈ");
            // ������Ʈ�� �ڸ���
            Cutter.Cut(other.gameObject, cutInPlane, cutPlaneNormal);

            // �ڸ� �� �������� ���� �����Ͽ� ������Ʈ�� �о
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(-cutPlaneNormal * cutForce); // cutDirection ��ſ� cutPlaneNormal�� ���
            }

            hasExited = true;
        }
        else
        {
            //Debug.Log("sliceMask: " + sliceMask.value);
            //Debug.Log("�ڸ� ���̾�: " + other.gameObject.layer);
            Debug.LogWarning("�� �ȵ�?");
        }
    }
}
