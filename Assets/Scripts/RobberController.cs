using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ���� ��Ʈ�ѷ�
/// </summary>
public class RobberController : PlayerController
{
    private void Update()
    {
        _hasAnimator = TryGetComponent(out base._animator);

        base.JumpAndGravity();
        base.GroundedCheck();
        base.Move();
        base.MeleeAttack();
    }

    private void LateUpdate()
    {
        //CameraRotation();
    }
}