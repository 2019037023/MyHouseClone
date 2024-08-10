using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float _floatHeight = 0.5f; // �������� ���ٴϴ� ����
    public float _floatSpeed = 1.0f;  // �������� ���ٴϴ� �ӵ�
    public float _rotateSpeed = 30f;  // �������� ȸ�� �ӵ�
    public float _floatScale = 0.1f;  // sin �Լ��� ��ȯ ���� ������ �����ϸ� ����, �ϸ��ϰ� �����̰� �Ϸ��� ���
    public SphereCollider _collider; // ������ ����

    Transform childMesh; // �� ������Ʈ�ϱ� �ڽ� ������Ʈ�� �ִ� Mesh �������� ���� ����

    [SerializeField] protected Define.Item itemType; // ������ Ÿ��

    /// <summary>
    /// ������ �ʱ� ����
    /// </summary>
    protected void ItemInit()
    {
        // Mesh�� �����´�.
        childMesh = transform.GetChild(0);

        // SphereCollider ����
        _collider = GetComponent<SphereCollider>();
        if(_collider == null)
        {
            gameObject.AddComponent<SphereCollider>();
            _collider = GetComponent<SphereCollider>();
        }
        _collider.isTrigger = true;
        _collider.radius = 35f;
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
}
