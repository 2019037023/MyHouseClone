using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ��Ʈ�ѷ�
/// </summary>
public class RobberController : PlayerController
{

    /// <summary>
    /// ���� ���ݰ� ���õ� ����
    /// </summary>
    bool swingKeyDown;  // ���콺 ���� Ű ���ȴ���
    bool isSwingReady;  // ���� �غ�
    float swingDelay;   // ���� ������

    public Weapon weapon;

    void Start()
    {
    }

    void Update()
    {
        // PlayerConroller�� dir�� �̵��� �Է¹޴´�.
        base.dir.x = Input.GetAxis("Horizontal");
        base.dir.z = Input.GetAxis("Vertical");
        base.dir = dir.normalized;

        base.Jump();
    }

    private void FixedUpdate()
    {
        base.Walk();
        base.Run();
        Attack();
    }


    /// <summary>
    /// ������ ���� ���� |
    /// ��Ŭ��: �ֵθ���, ��Ŭ��: ���
    /// </summary>
    void Attack()
    {
        swingKeyDown = Input.GetMouseButton(0);

        if (weapon == null)
        {
            Debug.Log("���̶� ���Ⱑ ����");
            return;
        }

        swingDelay += Time.deltaTime;

        isSwingReady = weapon.rate < swingDelay; // ���ݼӵ��� ���� �����̺��� ������ �����غ� �Ϸ�

        if(swingKeyDown && isSwingReady && base._isGround)
        {
            Debug.Log("����");
            weapon.Use();
            anim.SetTrigger("setSwing");
            swingDelay = 0;
            swingKeyDown = false;
        }
        //else if (Input.GetMouseButtonDown(1)) // Stab
        //{
        //    Debug.Log("Stap!");
        //    anim.SetTrigger("setStab");
        //}
    }
}
