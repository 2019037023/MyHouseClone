using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �������� ������ ��ƹ��� ���Ŀ� ��ӹ��� �� �ְ� �ڵ� �����ؾ� ��
/// </summary>
public class Weapon : MonoBehaviour
{
    public Define.Type type;            // ���� Ÿ��
    public int damage;                  // ���ݷ�
    public float rate;                  // ���� �ӵ�
    public BoxCollider meleeArea;       // ���� ���� ����
    public TrailRenderer trailEffet;    // �ֵθ� �� ȿ��

    private void Awake()
    {
        rate = 1;
        type = Define.Type.Melee;
    }

    /// <summary>
    /// Use() �����ϸ鼭 Swing() �ڷ�ƾ�̶� ���� ����ȴ�.
    /// </summary>
    public void Use()
    {
        if(type == Define.Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }

        if(type == Define.Type.Range)
        {

        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;   
        trailEffet.enabled = true;

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffet.enabled = false;
    }
}
