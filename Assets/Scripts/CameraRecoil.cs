using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    public float maxRecoilAngleY = 5f; // Y�� �ִ� �ݵ� ����
    public float maxRecoilAngleX = 2f; // X�� �ִ� �ݵ� ����
    public float recoilSpeed = 10f; // �ݵ��� �߻��ϴ� �ӵ�
    public float returnSpeed = 20f; // ���� ��ġ�� ���ƿ��� �ӵ�

    private Vector3 currentRecoil;
    private Vector3 targetRecoil;
    private Vector3 originalRotation;

    private void Start()
    {
        originalRotation = transform.localEulerAngles;
    }

    private void Update()
    {
        // õõ�� ���� ��ġ�� ����
        currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, Time.deltaTime * returnSpeed);
        transform.localEulerAngles = originalRotation + currentRecoil;
    }

    /// <summary>
    /// �� �߻� �� �ݵ��� �����ϴ� �޼���
    /// </summary>
    /// <param name="recoilAmountX">X�� �ݵ�</param>
    /// <param name="recoilAmountY">Y�� �ݵ�</param>
    public void ApplyRecoil(float recoilAmountX, float recoilAmountY)
    {
        recoilAmountX = Mathf.Clamp(recoilAmountX, -maxRecoilAngleX, maxRecoilAngleX);
        recoilAmountY = Mathf.Clamp(recoilAmountY, -maxRecoilAngleY, maxRecoilAngleY);

        targetRecoil += new Vector3(-recoilAmountX, recoilAmountY, 0); // X���� ������ ����
        currentRecoil = Vector3.Lerp(currentRecoil, targetRecoil, Time.deltaTime * recoilSpeed);
    }
}
