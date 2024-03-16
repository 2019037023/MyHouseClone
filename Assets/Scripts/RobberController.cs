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
    bool swingDown;   // ���콺 ���� Ű ���ȴ���
    bool isSwingReady;  // ���� �غ�
    float swingkDelay; // ���� ������

    protected Weapon weapon;

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
        if (Input.GetMouseButtonDown(0)) // Swing
        {
            swingkDelay += Time.deltaTime;
            // isSwingReady = weapon.rate < swingkDelay; // ���� �����̷� ���� ���� ���� �Ǵ�

            isSwingReady = true;

            //if (swingDown && isSwingReady) // ���� ������ ����
            //{
            //    weapon.Use();
            //    anim.SetTrigger("setTrigger");
            //    swingkDelay = 0;
            //}

            //if (isSwingReady) // ���� ������ ����
            //{
            //    //weapon.Use();
            //    anim.SetTrigger("setTrigger");
            //    swingkDelay = 0;
            //}
            anim.SetTrigger("setSwing");
            swingkDelay = 0;
        }
        else if (Input.GetMouseButtonDown(1)) // Stab
        {
            Debug.Log("Stap!");
            anim.SetTrigger("setStab");
        }
    }
}
