using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class TestThirdCamera : MonoBehaviour
{
    [SerializeField]
    float _mouseSensitivity = 3.0f;

    private float _rotationY;
    private float _rotationX;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _distanceFromTarget = 3.0f;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.2f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-40, 40);

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _rotationY += mouseX;
        _rotationX += mouseY;

        // �� �Ʒ� ���� ����
        _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        transform.localEulerAngles = _currentRotation;

        // ��ǥ �������� ī�޶��� ��ġ�� ���� ��ǥ������ �Ÿ��� ���ؼ� �׸�ŭ �������� ��ġ��Ų��.
        transform.position = _target.position - transform.forward * _distanceFromTarget;
    }






























    //[Header("Player ����")]
    //GameObject _player;
    //Transform _playerTransform;

    //// ����
    //[SerializeField]
    //Define.View viewMode;

    //// ���ͺ�
    //// 


    //Vector3 offset;


    //[SerializeField]
    //Vector3 _quaterDelta = new Vector3(0, 12, -9);
    //[SerializeField]
    //Vector3 _thirdDelta = new Vector3(0, 2.7f, -5f);

    //// TODO
    ///*
    // * 1. ,ī�޶�� Ÿ��(�÷��̾�)�� �ʿ�
    // * 2. �÷��̾� ���ҿ� ���� ������ �޸� �ؾ� �� ����: ���ͺ�, ������: TPS
    // * 3. �������� ���������� �� �� ��ġ�� ��尡 ��ȯ�Ǿ�� ��
    // */

    //void Start()
    //{
    //    _player = GameObject.Find("Player");
    //    _playerTransform = _player.transform;
        
    //    viewMode = Define.View.Third;

    //    // TODO
    //    /*
    //     * 1. �÷��̾� ã��
    //     * 2. ������ ��� ��ȯ
    //     * 
    //     * */
    //}

    //void Update()
    //{
    //    if(Input.GetKeyUp(KeyCode.Q)) 
    //    {
    //        viewMode = Define.View.Quater;
    //        InitializeCameraTransform();
    //    }
    //    else if(Input.GetKeyUp(KeyCode.E))
    //    {
    //        viewMode = Define.View.Third;
    //        InitializeCameraTransform(); 
    //    }
    //    offset = transform.position - _playerTransform.position;
    //    TakeCamera();
    //}

    //void TakeCamera()
    //{
    //    if (viewMode == Define.View.Quater)
    //        QuaterView();
    //    else if (viewMode == Define.View.Third)
    //        ThirdView();
    //}

    //void QuaterView()
    //{
    //    transform.position = _player.transform.position + _quaterDelta;
    //    transform.rotation = new Quaternion(50f, 0f, 0f, 0f);       // �缱 
    //    transform.LookAt(_player.transform);
    //}

    //void ThirdView()
    //{
    //    LookAround();
    //    CameraMove();
    //    //TestMove();
    //    //Debug.DrawRay(_cameraArm.position, new Vector3(_cameraArm.forward.x, 0f, _cameraArm.forward.z).normalized, Color.red);
    //    Debug.DrawRay(transform.position, new Vector3(transform.forward.x, 0f, transform.forward.z).normalized, Color.red);
    //}

    ///// <summary>
    ///// ���콺�� ���� ȸ��
    ///// </summary>
    //private void LookAround()
    //{
    //    Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    //    Vector3 camAngle = transform.rotation.eulerAngles;
    //    float x = camAngle.x - mouseDelta.y;

    //    // ȸ�� ���� ����
    //    if (x < 180f) // �������� ȸ��
    //        x = Mathf.Clamp(x, -1f, 70f);
    //    else // �Ʒ������� ȸ��
    //        x = Mathf.Clamp(x, 335f, 361f);

    //    transform.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);

    //}

    //private void CameraMove()
    //{
    //    float cameraPosZ = _playerTransform.position.z + _thirdDelta.z; // "_thirdDelta.z" is the initial value of _camerArm's position.z.
    //    Vector3 cameraPos = new Vector3(_playerTransform.position.x, transform.position.y, cameraPosZ);
    //    //_cameraArm.position = cameraPos;
    //    transform.position = cameraPos;
    //}

    //void InitializeCameraTransform()
    //{
    //    if(viewMode == Define.View.Quater)
    //    {
    //        transform.position = _playerTransform.position + _quaterDelta;
    //        transform.rotation = new Quaternion(50f, 0f, 0f, 0f);
    //    }
    //    else if(viewMode == Define.View.Third)
    //    {
    //        transform.position = _playerTransform.position + _thirdDelta;
    //        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    //    }
    //    transform.LookAt(_playerTransform);
    //}



    //// ĳ���Ͱ� �̵��ϴ� ������ �������� ī�޶� �̵�
    //private void TestMove()
    //{
        
    //    Debug.DrawRay(transform.position, new Vector3(transform.forward.x, 0f, transform.forward.z).normalized, Color.red);
    //}
}
