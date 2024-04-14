using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// two hand, range weapon
/// </summary>
public class Gun : Weapon
{
    #region �� ���� ����
    [Tooltip("��Ÿ�")]
    [SerializeField] float range;
    [Tooltip("������ �ð�")]
    [SerializeField] float reloadTime;
    [Tooltip("�� �� ���� ��, ������ ź�� ��")]
    [SerializeField] int reloadBulletCount;
    [Tooltip("���� ź�� ��")]
    [SerializeField] int currentBulletCount;
    [Tooltip("�ִ� ź�� ��")]
    [SerializeField] int maxBulletMagazine;
    [Tooltip("�� ź�� ��")]
    [SerializeField] int totalBulletCount;

    [Tooltip("���� �ݵ�")]
    [SerializeField] float reactionForce;
    [Tooltip("������ �ݵ�")]
    [SerializeField] float reactionFineSightForce;

    [SerializeField] AudioSource _audioSource;
    [Tooltip("��� ȿ��")]
    [SerializeField] ParticleSystem muzzleFlash;
    [Tooltip("�� �Ҹ�")]
    [SerializeField] AudioClip fireSound;

    [Tooltip("�߻� �ӵ�")]
    [SerializeField] float _currentFireRate;
    [Tooltip("������ ������ ����")]
    [SerializeField] bool _isReload = false;
    [Tooltip("������ ������ ����")]
    [SerializeField] bool _isFineSightMode = false;

    [SerializeField] Animator anim;
    [Tooltip("���� ī�޶� ��ġ")]
    [SerializeField] Vector3 _originPos;
    [Tooltip("������ ī�޶� ��ġ")]
    [SerializeField] Vector3 fineSightOriginPos;
    #endregion

    Animator houseownerAnim;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public override void Use()
    {
        GunRateCalc();
        Fire();
        FineSight();
    }

    void GunRateCalc()
    {
        if (_currentFireRate > 0)
        { 
            _currentFireRate -= Time.deltaTime;
            return;
        }
    }

    /// <summary>
    /// �ݹ�
    /// </summary>
    public void Fire()
    {
        if (Input.GetButton("Fire1") && _currentFireRate <= 0 && !_isReload)
        {
            if (currentBulletCount > 0)
                Shoot();
            else
            {
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    /// <summary>
    /// ���
    /// </summary>
    void Shoot() // After shoot
    {
        currentBulletCount--;
        _currentFireRate = base.Rate;
        muzzleFlash.Play();
        PlayAudioSource(fireSound);
        // �ѱ� �ݵ� �ڷ�ƾ ����
        StopAllCoroutines();
       

        Debug.Log("Shoot");
    }

    void Realod()
    {
        if(Input.GetKeyDown(KeyCode.R) && !_isReload && currentBulletCount < reloadBulletCount)
        {
            // ������ ����
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        if (totalBulletCount > 0)
        {
            _isReload = true;

            totalBulletCount += currentBulletCount;
            currentBulletCount = 0;

            yield return new WaitForSeconds(reloadTime);

            if (totalBulletCount >= reloadBulletCount)
            {
                currentBulletCount = reloadBulletCount;
                totalBulletCount -= reloadBulletCount;
            }
            else
            {
                currentBulletCount = totalBulletCount;
                totalBulletCount = 0;
            }

            _isReload = false;
        }
    }

    public void FineSight()
    {
        if (Input.GetMouseButton(1))
        {
            _isFineSightMode = !_isFineSightMode;
            
            // ������ �ִϸ��̼� ���� ����
            //anim.SetBool("", _isFineSightMode);

            if (_isFineSightMode)
            {
                StopAllCoroutines();
                StartCoroutine(FineSightActivateCoroutine());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(FineSightDeactivateCoroutine());
            }
        }
    }

    IEnumerator FineSightActivateCoroutine() // Activate
    {
        while (transform.localPosition != fineSightOriginPos)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, fineSightOriginPos, 0.2f);
            yield return null; // Stand by 1 frame at a time.
        }
    }

    IEnumerator FineSightDeactivateCoroutine() // Deactivate
    {
        while (transform.localPosition != _originPos)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _originPos, 0.2f);
            yield return null; // Stand by 1 frame at a time.
        }
    }

    IEnumerator ReactionCoroutine()
    {
        Vector3 reactionNormal = new Vector3(reactionForce, _originPos.y, _originPos.z);     // ������ �� ���� ���� �ִ� �ݵ�
        Vector3 reactionFineSight = new Vector3(reactionFineSightForce, fineSightOriginPos.y, fineSightOriginPos.z);  // ������ ���� ���� �ִ� �ݵ�

        if (!_isFineSightMode)  // �������� �ƴ� ����
        {
            transform.localPosition = _originPos;

            // �ݵ� ����
            while (transform.localPosition.x <= reactionForce - 0.02f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, reactionNormal, 0.4f);
                yield return null;
            }

            // ����ġ
            while (transform.localPosition != _originPos)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, _originPos, 0.1f);
                yield return null;
            }
        }
        else  // ������ ����
        {
            transform.localPosition = fineSightOriginPos;

            // �ݵ� ����
            while (transform.localPosition.x <= reactionFineSightForce - 0.02f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, reactionFineSight, 0.4f);
                yield return null;
            }

            // ����ġ
            while (transform.localPosition != fineSightOriginPos)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    void PlayAudioSource(AudioClip _clip)
    {
        _audioSource.clip = _clip;
        _audioSource.Play();
    }
}