using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SphereCollider))]
public abstract class Item : MonoBehaviour
{
    public float _floatHeight = 0.5f; // �������� ���ٴϴ� ����
    public float _floatSpeed = 1.0f;  // �������� ���ٴϴ� �ӵ�
    public float _rotateSpeed = 30f;  // �������� ȸ�� �ӵ�
    public float _floatScale = 0.1f;  // sin �Լ��� ��ȯ ���� ������ �����ϸ� ����, �ϸ��ϰ� �����̰� �Ϸ��� ���
    public SphereCollider _collider; // ������ ����
    public Define.Item _itemType = Define.Item.None;

    public float _pickupRange;
    protected Renderer _renderer;
    Transform childMesh; // �� ������Ʈ�ϱ� �ڽ� ������Ʈ�� �ִ� Mesh �������� ���� ����

    /// <summary>
    /// ������ �ʱ�ȭ
    /// </summary>
    protected void InitItem()
    {
        // Mesh�� �����´�.
        childMesh = transform.GetChild(0);

        // SphereCollider ����
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;

        _renderer = transform.GetChild(0).GetComponent<Renderer>();
    }

    /// <summary>
    /// ������ ���ڸ��� ���ٴϱ�
    /// </summary>
    protected void Floating()
    {
        // �������� ȸ��
        // ���� ��ǥ ����(Vector3.up) ȸ��
        childMesh.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime, Space.World);

        // �������� ���Ʒ��� ���ٴ� ����
        float newY = Mathf.Sin(Time.time * _floatSpeed) * _floatScale + _floatHeight;
        childMesh.localPosition = new Vector3(childMesh.localPosition.x, newY, childMesh.localPosition.z);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _pickupRange);
    }
}
