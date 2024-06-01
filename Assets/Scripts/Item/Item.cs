using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float _floatHeight = 0.5f; // �������� ���ٴϴ� ����
    public float _floatSpeed = 1.0f;  // �������� ���ٴϴ� �ӵ�
    public float _rotateSpeed = 30f;  // �������� ȸ�� �ӵ�
    public float _floatScale = 0.1f;  // sin �Լ��� ��ȯ ���� ������ �����ϸ� ����, �ϸ��ϰ� �����̰� �Ϸ��� ���

    // �� ������Ʈ�ϱ� �ڽ� ������Ʈ�� �ִ� Mesh �������� ���� ����
    Transform childMesh;

    // ������ Ÿ��
    [SerializeField]
    protected Define.Item itemType;

    // ������ �ʱ� ����
    protected void ItemInit()
    {
        // Mesh�� �����´�.
        childMesh = transform.GetChild(0);

        // SphereCollider ����
        SphereCollider itemCollider = GetComponent<SphereCollider>();
        if(itemCollider == null)
        {
            gameObject.AddComponent<SphereCollider>();
            itemCollider = GetComponent<SphereCollider>();
        }
        itemCollider.isTrigger = true;
        itemCollider.radius = 35f;
    }

    // ������ ���ڸ��� ���ٴϱ�
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
