using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraPosition : MonoBehaviour
{
    Camera minimapCamera;
    [SerializeField] bool x, y, z;
    [SerializeField] GameObject player;
    Transform target;

    [SerializeField]  int secondFloorLayer;

    private void Start()
    {
        minimapCamera = GetComponent<Camera>();

        // �÷��̾�
        player = transform.root.GetChild(2).gameObject;

        // �̴ϸ� ��ġ ����
        target = transform;
        target.position = new Vector3(0, 0, 0);
        target.SetPositionAndRotation(new Vector3(0,30,0), Quaternion.Euler(90,0,0));

        secondFloorLayer = LayerMask.NameToLayer("Floor");
    }
    private void Update()
    {
        if(!target) return;

        // �̴ϸ� ��ġ ���� (�÷��̾��� �ڽĿ� ī�޶� �����Ƿ�)
        transform.position = new Vector3(
            (transform.position.x),
            (transform.position.y),
            (transform.position.z));

        // 2�� ������ ����
        if (player.transform.position.y > 7)
        {
            minimapCamera.cullingMask |= (1 << secondFloorLayer);
        }
        else
        {
            minimapCamera.cullingMask &= ~(1 << secondFloorLayer);
        }
    }
}
