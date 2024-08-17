using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class Item_S : MonoBehaviour
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

    protected ItemCylinder_S _itemCylinder; // ������ ��ȯ�� ��

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
        _itemCylinder = transform.parent.parent.GetComponent<ItemCylinder_S>();
    }

    /// <summary>
    /// ������ ���ڸ��� ���ٴϱ�
    /// </summary>
    protected void Floating()
    {
        // �������� ȸ�� (���� ��ǥ ����)
        // �ڱ� ��ġ�� �������� ���� ��ǥ ����(Vector3.up) ȸ��
        childMesh.RotateAround(childMesh.position, Vector3.up, _rotateSpeed * Time.deltaTime);

        // childMesh.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        // �������� ���Ʒ��� ���ٴ� ����
        float newY = Mathf.Sin(Time.time * _floatSpeed) * _floatScale + _floatHeight;
        childMesh.position = new Vector3(childMesh.position.x, newY, childMesh.position.z);
    }

    // ���� ������ Ȱ��ȭ/��Ȱ��ȭ
    public void EnableItem(bool state)
    {
        _renderer.enabled = state;
        _collider.enabled = state;
    }

    // ������ �� �ٲٱ�
    public void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }

    // ������ �� ���
    public Color GetItemColor()
    {
        return _renderer.material.color;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _pickupRange);
    }
}
