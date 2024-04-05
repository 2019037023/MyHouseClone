using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float floatHeight = 0.5f; // �������� ���ٴϴ� ����
    public float floatSpeed = 1.0f;  // �������� ���ٴϴ� �ӵ�
    public float rotateSpeed = 30f;  // �������� ȸ�� �ӵ�
    public float floatScale = 0.1f;  // Sin �Լ��� ��ȯ ���� ������ �����ϸ� ����, �ϸ��ϰ� �����̰� �Ϸ��� ���

    Transform childMesh;
    void Start()
    {
        // Mesh�� �����´�.
        childMesh = transform.GetChild(0);
    }

    void Update()
    {
        // �������� ȸ�� (���� ��ǥ ����)
        // �ڱ� ��ġ�� �������� ���� ��ǥ ����(Vector3.up) ȸ��
        childMesh.RotateAround(childMesh.position, Vector3.up, rotateSpeed * Time.deltaTime);
        
        // childMesh.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        // �������� ���Ʒ��� ���ٴ� ����
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatScale + floatHeight; 
        childMesh.position = new Vector3(childMesh.position.x, newY, childMesh.position.z);
    }
}
