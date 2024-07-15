using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    #region ���� �� �ɷ�ġ
    [field: SerializeField] public Define.Role Role = Define.Role.None;
    [field: SerializeField] public float Hp { get; set; } = 100;    // ü��
    [field: SerializeField] public float Sp { get; set; } = 100;    // ���׹̳�
    [field: SerializeField] public float MaxHp { get; private set; } = 100; // �ִ� ü��
    [field: SerializeField] public float MaxSp { get; private set; } = 100; // �ִ� ���׹̳�
    [field: SerializeField] public float Defence { get; private set; } = 1; // ����
    #endregion

    #region �ִϸ��̼� �� ����
    Animator _animator;
    List<Renderer> _renderers;
    #endregion

    [Header("���� ����")]
    [SerializeField] NewWeaponManager _houseOwnerWeaponManager;


    // ���� ��ȯ
    

    void Start()
    {
        InitRole();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Dead();
        TransformIntoHouseowner();
    }

    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    public void InitRole()
    {
        /*
         TODO
        ȣ��Ʈ��, Houseowner���� �ϰ�, Ŭ���̾�Ʈ�� Robber

        �ϴ� ���Ǹ� ���� ������ ����
         */
        Role = Define.Role.Robber;
    }



    /// <summary>
    /// ������ �Ա�
    /// </summary>
    /// <param name="attack"> ���� ���ݷ� </param>
    public void TakedDamage(int attack)
    {
        // ���ذ� ������� ȸ���Ǵ� ������ �Ͼ�Ƿ� ������ ���� 0�̻����� �ǰԲ� ����
        float damage = Mathf.Max(0, attack - Defence);
        Hp -= damage;

        Debug.Log(gameObject.name + "(��)�� " + damage + " ��ŭ ���ظ� �Ծ���!");
        Debug.Log("���� ü��: " + Hp);
    }

    /// <summary>
    /// �ִ� ü���� 0.2��ŭ ȸ��
    /// </summary>
    public void Heal()
    {
        // ���� ü���� �ִ� ü�º��� ���� ���� ȸ�� ����
        if (Hp < MaxHp)
        {
            // ȸ����
            float healAmount = MaxHp * 0.2f;

            // ȸ������ ���� ü�°��� ���� �ִ� ü���� ���� �ʵ��� ����
            float healedAmount = Mathf.Clamp(Hp + healAmount, 0, MaxHp) - Hp;

            Debug.Log("���� ü��" + Hp);
            // ü�� ȸ��
            Hp += healedAmount;
            Debug.Log("ü���� " + healedAmount + "��ŭ ȸ��!");
            Debug.Log("���� ü��: " + Hp);
        }
        else
        {
            Debug.Log("�ִ� ü��. ȸ���� �ʿ� ����.");
        }
    }

    /// <summary>
    /// �ִ� ���׹̳����� ���� ȸ��
    /// </summary>
    public void SpUp()
    {
        // ���� ���׹̳��� �ִ� ���׹̳����� ���� ���� ȸ�� ����
        if (Sp < MaxSp)
        {
            // ȸ������ ���� ���׹̳����� ���� �ִ� ���׹̳��� ���� �ʵ��� ����
            float healedAmount = Mathf.Clamp(Sp + MaxSp, 0, MaxHp) - Sp;

            Debug.Log("���� ���׹̳�" + Sp);
            // ���׹̳� ȸ��
            Sp += healedAmount;
            Debug.Log("���� ȸ��! ���� Sp: " + Sp);
        }
        else
        {
            Debug.Log("�ִ� Sp. ȸ���� �ʿ� ����.");
        }
    }

    /// <summary>
    /// ���׹̳� ��������
    /// </summary>
    public void ChargeSp()
    {
        Sp += Time.deltaTime * 20;
        Sp = Mathf.Clamp(Sp, 0, MaxSp);
    }

    /// <summary>
    /// ���׹̳� ���̱�
    /// </summary>
    public void DischargeSp()
    {
        Sp -= Time.deltaTime * 20;
        Sp = Mathf.Clamp(Sp, 0, MaxSp);
    }

    /// <summary>
    /// ������, ���׹̳� ����
    /// </summary>
    public void JumpSpDown()
    {
        Sp -= 3;
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void DefenceUp()
    {

    }

    /// <summary>
    /// ���������� ����
    /// </summary>
    public void TransformIntoHouseowner()
    {
        if (!Input.GetKeyDown(KeyCode.T)) return;

        transform.GetChild(0).gameObject.SetActive(false); // ���� ��Ȱ��ȭ
        transform.GetChild(1).gameObject.SetActive(true);  // ������ Ȱ��ȭ
        Role = Define.Role.Houseowner;

        Debug.Log("���� ����: " + Role);

        Camera.main.gameObject.GetComponent<CameraController>().SetHouseownerView(); // ������ �������� ����

        Debug.Log("������ ������ ��ȯ");

        // ������ ���� ����
        _houseOwnerWeaponManager.InitRoleWeapon();
        Debug.Log("���������� ���� �Ϸ�");
    }

    
    /// <summary>
    /// ���
    /// </summary>
    public void Dead()
    {
        if (Role != Define.Role.None && Hp <= 0)
        {
            _animator.SetTrigger("setDie");
            Role = Define.Role.None; // ��ü
            StartCoroutine(DeadSinkCoroutine());
        }
    }

    /// <summary>
    /// ��ü �ٴ����� ����ɱ�
    /// </summary>
    /// <returns></returns>
    IEnumerator DeadSinkCoroutine()
    {
        yield return new WaitForSeconds(3f);
        while (transform.position.y > -1.5f)
        {
            transform.Translate(Vector3.down * 0.1f * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// ���� ������ Material �Ӱ� ��ȭ
    /// </summary>
    public void HitChangeMaterials()
    {
        // �±װ� ���� �Ǵ� ����

        for (int i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].material.color = Color.red;
            Debug.Log("�����Ѵ�.");
            //Debug.Log(_renderers[i].material.name);
        }

        StartCoroutine(ResetMaterialAfterDelay(1.7f));

        //Debug.Log($"�÷��̾ {other.transform.root.name}���� ���� ����!");
        Debug.Log("���ݹ��� ���� ü��:" + Hp);
    }

    /// <summary>
    /// ���� �ް� Material ������� ����
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator ResetMaterialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < _renderers.Count; i++)
            _renderers[i].material.color = Color.white;
    }

    void OnTriggerEnter(Collider other)
    {
        //// �ڱ� �ڽſ��� ���� ��� ����
        //if (other.transform.root.name == gameObject.name) return;
        if (other.tag == "Melee" || other.tag == "Gun" || other.tag == "Monster")
            HitChangeMaterials();
    }

    public void SetRoleAnimator(RuntimeAnimatorController animController, Avatar avatar)
    {
        _animator.runtimeAnimatorController = animController;
        _animator.avatar = avatar;

        // �ִϸ����� �Ӽ� ��ü�ϰ� ���ٰ� �Ѿ� ������
        _animator.enabled = false;
        _animator.enabled = true;
    }

    //public PlayerMove _playerMove;
    //public PlayerInputs _input;
    //[Header("���� ����")]
    //bool _isSwingReady;  // ���� �غ�
    //float _swingDelay;   // ���� ������
    //bool _isStabReady;  // ���� �غ�
    //float _stabDelay;   // ���� ������

    //public void ChangeIsHoldGun(bool newIsHoldGun)
    //{
    //    _animator.SetBool("isHoldGun", newIsHoldGun);
    //}

    ///// <summary>
    ///// ���� ����: ��Ŭ��(�ֵθ���), ��Ŭ��(���)
    ///// </summary>
    //public void MeleeAttack()
    //{
    //    // ���� ������Ʈ�� ���ų�, ���Ⱑ ��Ȱ��ȭ �Ǿ� �ְų�, ���Ⱑ ������ ���� ���
    //    if (_weaponManager._melee == null || _weaponManager._melee.activeSelf == false || _weaponManager._meleeWeapon == null)
    //        return;

    //    _swingDelay += Time.deltaTime;
    //    _stabDelay += Time.deltaTime;
    //    _isSwingReady = _weaponManager._meleeWeapon.Rate < _swingDelay; // ���ݼӵ��� ���� �����̺��� ������ �����غ� �Ϸ�
    //    _isStabReady = _weaponManager._meleeWeapon.Rate < _stabDelay;

    //    if (_input.swing && _isSwingReady && _playerMove._grounded) // �ֵθ���
    //    {
    //        Debug.Log("�ֵθ���");
    //        _weaponManager._meleeWeapon.Use();
    //        _animator.SetTrigger("setSwing");
    //        _swingDelay = 0;
    //    }
    //    else if (_input.stap && _isStabReady && _playerMove._grounded) // ���
    //    {
    //        Debug.Log("���");
    //        _weaponManager._meleeWeapon.Use();
    //        _animator.SetTrigger("setStab");
    //        _stabDelay = 0;

    //    }
    //    _input.swing = false;
    //    _input.stap = false;
    //}
}
