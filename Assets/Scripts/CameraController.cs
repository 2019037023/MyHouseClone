using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ī�޶�, ����� ���ͺ丸 �����ϰ� �Ǿ�����
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    Vector3 _delta;

    [SerializeField]
    GameObject _player;

    private void Awake()
    {
        
    }

    void LateUpdate()
    {
        QuterView();
    }


    void QuterView()
    {
        transform.position = _player.transform.position + _delta;   // ī�޶� ��ġ
        transform.rotation = new Quaternion(50f, 0f, 0f, 0f);       // �缱 
        transform.LookAt(_player.transform);
    }

    void TPSView()
    {

    }
}
