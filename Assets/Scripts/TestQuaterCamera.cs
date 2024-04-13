using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuaterCamera : MonoBehaviour
{
    [SerializeField]
    Vector3 _delta;

    [SerializeField]
    GameObject _player;

    private void LateUpdate()
    {
        transform.position = _player.transform.position + _delta;   // ī�޶� ��ġ
        transform.rotation = new Quaternion(50f, 0f, 0f, 0f);       // �缱 
        transform.LookAt(_player.transform);
    }
}
