using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// �÷��̾�(������, ����)�� ���������� ��ӹ޴� Ŭ����
/// </summary>
public class PlayerController : MonoBehaviour
{
    protected string _nickname;  // �г���
    protected Define.Role _role; // �÷��̾� ����

    protected Status _status;

    protected Rigidbody rb;
    protected Animator anim;

    protected Vector3 dir = Vector3.zero;
    protected bool _isGround;
    protected float _walkSpeed = 5f;
    protected float _runSpeed = 15f;
    protected float _moveSpeed;
    protected float _jumpHeight = 4f;         // ���� �Ŀ�
    bool isPressedRunKey;                     // �޸��� ���� �Ǻ�

    protected bool _isDead;                    // �׾����� �Ǻ�
    
    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        gameObject.AddComponent<Status>();
        _status = gameObject.GetComponent<Status>();
    }

    /// <summary>
    /// �⺻ �̵�, �ȴ� �ִϸ��̼� ���
    /// </summary>
    protected virtual void Walk()
    {
        _moveSpeed = _walkSpeed;
        if(dir!=Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.2f);
            transform.position += dir * _moveSpeed * Time.deltaTime;
        }
        anim.SetBool("isWalk", dir != Vector3.zero); // �̵��� ������ �Ȱ�
    }

    /// <summary>
    /// �޸���, �̵� �ӵ��� ��ȭ��Ű�� �޸��� �ִϸ��̼� ���
    /// </summary>
    protected virtual void Run() // �޸��� �ӵ��� �����
    {
        isPressedRunKey = Input.GetKey(KeyCode.LeftShift);
        if (isPressedRunKey)
            _moveSpeed = _runSpeed;
        anim.SetBool("isRun", isPressedRunKey && dir!=Vector3.zero);
    }

    /// <summary>
    /// Ground���� �Ǵ�
    /// </summary>
    protected void IsGround()
    {
        Debug.DrawRay(transform.position + (Vector3.up * 0.2f), Vector3.down, Color.red);

        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        if (Physics.Raycast(transform.position + (Vector3.up * 0.2f), Vector3.down, out hit, 0.3f, layerMask))
            _isGround = true;
        else
            _isGround = false;
    }

    /// <summary>
    /// ����
    /// </summary>
    protected void Jump()
    {
        IsGround();
        if (Input.GetKeyDown(KeyCode.Space) && _isGround)
        {
            Vector3 jumpPower = Vector3.up * _jumpHeight;
            rb.AddForce(jumpPower, ForceMode.VelocityChange); // ForceMode.VelocityChange�� ���� �����ϰ� ���������� �ӵ��� ��ȭ�� �ش�.
            anim.SetTrigger("setJump");
        }
    }

    /// <summary>
    /// hp�� 0�̵Ǹ� ���
    /// </summary>
    protected void Dead()
    {
        if(_status.Hp <= 0 || Input.GetKeyDown(KeyCode.P))
        {
            anim.SetTrigger("setDie");
            _isDead = true;
        }
    }
}
