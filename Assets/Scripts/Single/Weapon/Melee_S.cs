using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Melee_S : Weapon
{
    PlayerMove_S _playerMove;
    PlayerInputs _playerInputs;
    WeaponManager _weaponManager;

    BoxCollider _meleeArea;       // ���� ���� ����
    TrailRenderer _trailEffet;    // �ֵθ� �� ȿ��
    Animator _animator;

    [Header("���� ����")]
    bool _isSwingReady;  // ���� �غ�
    float _swingDelay;   // ���� ������
    bool _isStabReady;  // ���� �غ�
    float _stabDelay;   // ���� ������

    #region ���� ȿ�� ����
    public LayerMask _sliceMask; // �ڸ� ����� ���̾� ����ũ
    public float _cutForce = 250f; // �ڸ� �� �������� ��

    Vector3 _entryPoint; // ������Ʈ�� �� ����
    Vector3 _exitPoint; // ������Ʈ�� �հ� ���� ����
    bool _hasExited = false; // ������Ʈ�� �հ� �������� ���θ� �����ϴ� ����
    #endregion

    private void Awake()
    {
        InitWeapon();
    }

    void InitWeapon()
    {
        base.Type = Define.Type.Melee;

        _meleeArea = gameObject.GetComponent<BoxCollider>();
        _trailEffet = gameObject.GetComponentInChildren<TrailRenderer>();

        // ���� ���� �ʱ�ȭ
        WeaponData weapon = GameManager_S._instance.GetWeaponStatusByName(transform.name);
        if (weapon != null)
        {
            Debug.Log($"Weapon Name: {weapon.Name}. Attack: {weapon.Attack}, Rate: {weapon.Rate}");
            Attack = weapon.Attack;
            Rate = weapon.Rate;
            Range = weapon.Range;
        }
        else
        {
            Debug.LogWarning("Weapon not found!");
        }
    }


    void Start()
    {
        _playerMove = transform.root.GetChild(2).GetComponent<PlayerMove_S>();
        _playerInputs = transform.root.GetChild(2).GetComponent<PlayerInputs>();

        if (transform.root.childCount > 2)
        {
            _animator = transform.root.GetChild(2).GetChild(0).GetComponent<Animator>();
        }

        _meleeArea = gameObject.GetComponent<BoxCollider>();
        _trailEffet = gameObject.GetComponentInChildren<TrailRenderer>();

        // TODO
        /*
         * ���� �ɷ�ġ�� �����̳� json�� �̿��� ���� ����
         * ���� �о�ͼ� �� ������ ��������� ��
         * ���� �ӽ÷� �׽�Ʈ�� ���� �ϵ��ڵ� ��
        */

        //weaponData = JsonUtility.FromJson<WeaponData>(jsonData);
        // if (gameObject.tag == "Melee")
        //     base.Attack = 50;

    }

    void Update()
    {
        _swingDelay += Time.deltaTime;
        _stabDelay += Time.deltaTime;
    }

    /// <summary>
    /// ���� ����: ��Ŭ��(�ֵθ���), ��Ŭ��(���)
    /// ���� ȿ�� �ڷ�ƾ ���� ����ȴ�.
    /// </summary>
    public override void Use()
    {
        _swingDelay += Time.deltaTime;
        _stabDelay += Time.deltaTime;
        _isSwingReady = base.Rate < _swingDelay; // ���ݼӵ��� ���� �����̺��� ������ �����غ� �Ϸ�
        _isStabReady = base.Rate < _stabDelay;
        int rayCount = 10;
        float angle = 60f;  // ��ä�� ����
        float halfAngle = angle / 2.0f;
        Vector3 startPosition = transform.root.GetChild(2).position;
        startPosition.y += 1.0f;

        if (_playerInputs == null)
            _playerInputs = transform.root.GetChild(2).GetComponent<PlayerInputs>();
        if (_playerMove == null)
            _playerMove = transform.root.GetChild(2).GetComponent<PlayerMove_S>();

        if (_playerInputs.swing &&  _playerMove._grounded || _playerInputs.stab &&  _playerMove._grounded)
        {
            StopCoroutine("MeleeAttackEffect");

            //// ���� ���Ⱑ �ƴϰų� ���Ⱑ Ȱ��ȭ �Ǿ� ���� ������ ����
            //if (_weaponManager._selectedWeapon.tag != "Melee" || !_weaponManager._selectedWeapon.activeSelf) return;

            // ���ݼӵ��� ���� �����̺��� ������ �����غ� �Ϸ�
            //_isSwingReady = _weaponManager._selectedWeapon.GetComponent<Melee>().Rate < _swingDelay;
            //_isStabReady = _weaponManager._selectedWeapon.GetComponent<Melee>().Rate < _stabDelay;

            if (_playerInputs.swing && _playerMove._grounded) // �ֵθ���
            {
                Debug.Log("�ֵθ���");
                // _weaponManager._selectedWeapon.GetComponent<Melee>().Use();
                _animator.SetTrigger("setSwing");
                
                if(_isSwingReady == true && _isStabReady == true)
                {
                    _swingDelay = 0;
                    HashSet<Monster> hitMonsters = new HashSet<Monster>();
                    for (int i = 0; i < rayCount; i++)
                    {
                        float currentAngle = -halfAngle + (i * (angle / (rayCount - 1)));
                        Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.root.GetChild(2).forward;

                        RaycastHit hit;
                        if (Physics.Raycast(startPosition, direction, out hit, base.Range))
                        {
                            if (hit.collider.CompareTag("Monster"))
                            {
                                Monster monster = hit.collider.GetComponent<Monster>();
                                if (monster != null && !hitMonsters.Contains(monster))
                                {
                                    hitMonsters.Add(monster);
                                    StartCoroutine(DelayedDamage(monster));
                                }
                            }
                        }

                        Debug.DrawRay(transform.root.GetChild(2).position, direction * base.Range, Color.red, 1.0f);
                    }
                }
            }
            else if (_playerInputs.stab && _playerMove._grounded) // ���
            {
                
                Debug.Log("���");
                // _weaponManager._selectedWeapon.GetComponent<Melee>().Use();
                _animator.SetTrigger("setStab");
                _stabDelay = 0;

                if(_isSwingReady == true && _isStabReady == true)
                {
                    _stabDelay = 0;
                    HashSet<Monster> hitMonsters = new HashSet<Monster>();
                    for (int i = 0; i < rayCount; i++)
                    {
                        float currentAngle = -halfAngle + (i * (angle / (rayCount - 1)));
                        Vector3 direction = Quaternion.Euler(0, currentAngle / 2, 0) * transform.root.GetChild(2).forward;

                        RaycastHit hit;
                        if (Physics.Raycast(startPosition, direction, out hit, base.Range + 0.5f))
                        {
                            if (hit.collider.CompareTag("Monster"))
                            {
                                Monster monster = hit.collider.GetComponent<Monster>();
                                if (monster != null && !hitMonsters.Contains(monster))
                                {
                                    hitMonsters.Add(monster);
                                    StartCoroutine(DelayedDamage(monster));
                                }
                            }
                        }

                        Debug.DrawRay(transform.root.GetChild(2).position, direction * base.Range, Color.red, 1.0f);
                    }
                }

            }
            _playerInputs.swing = false;
            _playerInputs.stab = false;

            StartCoroutine("MeleeAttackEffect");
        }
        else
        {
            // �������ڸ��� �ֵθ��� ���� ����(����Ƽ Play ���� �� Ŭ�� ������ �׷� �� �ϴ�)
            _playerInputs.swing = false;
            _playerInputs.stab = false;
        }
    }

    /// <summary>
    /// �ڷ�ƾ���� Collider, TrailRenderer Ư�� �ð� ���ȸ� Ȱ��ȭ
    /// </summary>
    IEnumerator MeleeAttackEffect()
    {
        yield return new WaitForSeconds(0.5f);
        _meleeArea.enabled = true;
        _trailEffet.enabled = true;

        yield return new WaitForSeconds(0.5f);
        _meleeArea.enabled = false;

        yield return new WaitForSeconds(0.5f);
        _trailEffet.enabled = false;
    }

    #region ���� ���
    //Į�� Ʈ���� �ȿ� ���� ��
    // _hasExited�� false�� ����
    // void OnTriggerEnter(Collider other)
    //  {
    //      _hasExited = false;
    //      _entryPoint = other.ClosestPoint(transform.position);

    //      // ������ ����

    //      // �ڱ� �ڽſ��� ���� ��� ����
    //      if (other.tag == "Player") return;

    //      if (other.GetComponent<Monster>() != null)
    //      {
    //          other.GetComponent<Monster>().TakedDamage(Attack);
    //      }
    //  }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("����");
    }

    // ���� �� �Ǹ� ���̾ ���� ����
    void OnTriggerExit(Collider other)
    {
        // �浹 ������ ������ �ڸ��� �������� ����
        _exitPoint = other.ClosestPoint(transform.position);

        Vector3 cutDirection = _exitPoint - _entryPoint;
        Vector3 cutInPlane = (_entryPoint + _exitPoint) / 2;

        //Vector3 cutPlaneNormal = Vector3.Cross((entryPoint - exitPoint), (entryPoint - transform.position)).normalized;
        Vector3 cutPlaneNormal = Vector3.Cross((_entryPoint - _exitPoint), (_entryPoint - transform.position)).normalized;
        Debug.Log(cutPlaneNormal.x + ", " + cutPlaneNormal.y + ", " + cutPlaneNormal.z);

        if (cutPlaneNormal.x == 0 && cutPlaneNormal.y == 0 && cutPlaneNormal.z == 0)
        {
            // ���� �ڸ��� ������ normalize �ؼ� �־���� ��
            cutPlaneNormal = (_entryPoint - _exitPoint).normalized;
            Debug.Log("��ü: " + cutPlaneNormal.x + " " + cutPlaneNormal.y + " " + cutPlaneNormal.z);

            bool isHorizontalCut = Mathf.Abs(cutDirection.x) > Mathf.Abs(cutDirection.y);

            // ���η� �ڸ��� ���
            if (isHorizontalCut)
            {
                // x �� �������� �ڸ��� ������ cutPlaneNormal�� x �� ���� ���ͷ� ����
                cutPlaneNormal = Vector3.up;
            }
            else // ���η� �ڸ��� ���
            {
                // y �� �������� �ڸ��� ������ cutPlaneNormal�� y �� ���� ���ͷ� ����
                cutPlaneNormal = Vector3.right;
            }
        }

        LayerMask cutableMask = LayerMask.GetMask(LayerMask.LayerToName(other.gameObject.layer));
        //Debug.Log("�߸� ���̾�: " + LayerMask.LayerToName(other.gameObject.layer));
        if (_sliceMask.value == cutableMask)
        {
            Debug.LogWarning("�ڸ� �� �ִ� ������Ʈ");
            // ������Ʈ�� �ڸ���
            Cutter.Cut(other.gameObject, cutInPlane, cutPlaneNormal);

            // �ڸ� �� �������� ���� �����Ͽ� ������Ʈ�� �о
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(-cutPlaneNormal * _cutForce); // cutDirection ��ſ� cutPlaneNormal�� ���
            }

            _hasExited = true;
        }
        else
        {
            //Debug.Log("sliceMask: " + sliceMask.value);
            //Debug.Log("�ڸ� ���̾�: " + other.gameObject.layer);
            Debug.LogWarning("�� �ȵ�?");
        }
    }
    #endregion


    IEnumerator DelayedDamage(Monster monster)
    {
        yield return new WaitForSeconds(0.8f); // 0.5�� ����
        if(monster._isTakingDamage == false)
        {
            monster.TakedDamage(base.Attack);
            Debug.Log("���� ���ݹ޾ҽ��ϴ�: " + monster.name);
            monster.HitStart();
            monster._isTakingDamage = true;
        }
        yield return new WaitForSeconds(0.3f);
        monster._isTakingDamage = false;
    }
}