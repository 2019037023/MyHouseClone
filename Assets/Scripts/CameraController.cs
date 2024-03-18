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

    void LateUpdate()
    {
        transform.position = _player.transform.position + _delta;
        transform.LookAt(_player.transform);
    }
}
