using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] PlayerStatus _status;
    [SerializeField] WeaponManager _robberWeaponManager;
    [SerializeField] WeaponManager _houseownerWeaponManager;
    WeaponManager _weaponManager;

    #region UI ����
    // ���� �ο�
    [SerializeField] TextMeshProUGUI _connectedPeople;

    // �ð�
    [SerializeField] TextMeshProUGUI _timeSecond;
    float _timer;

    // �������ͽ�
    [SerializeField] Slider _hpBar;
    [SerializeField] Slider _spBar;

    // ����
    RawImage _weaponIcon;
    public Texture2D[] _weaponImages = new Texture2D[2];
    [SerializeField] TextMeshProUGUI _currentBullet;
    [SerializeField] TextMeshProUGUI _totalBullet;

    // ���ؼ�
    [SerializeField] GameObject _crossHair;
    #endregion

    void Start()
    {
        // ���� �ο�
        _connectedPeople = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

        // �ð� ǥ���� ��
        _timeSecond = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        // Hp, Sp ǥ���� ��
        _hpBar = transform.GetChild(2).GetComponent<Slider>();
        _spBar = transform.GetChild(3).GetComponent<Slider>();

        // ���� ���� ǥ���� ��
        _weaponIcon = transform.GetChild(4).GetChild(0).GetComponent<RawImage>();
        _currentBullet = transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        _totalBullet = transform.GetChild(4).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

        // ���ؼ� UI
        _crossHair = transform.GetChild(5).gameObject;
    }

    void Update()
    {
        if(_status.Hp > 0)
        {
            DisplayConnectedPlayers();
            DisplayLivingTime();
            DisplayHp();
            DisplaySp();
            DisplayWeaponInfo();
        }
        else
        {
            DisplayOut();
        }
    }

    public void DisplayLivingTime()
    {
        // ü���� 0�̸� ���߱�
        if (_status.Hp <= 0) return;

        _timer += Time.deltaTime;
        _timeSecond.text = ((int)_timer).ToString();
    }

    public void DisplayHp() => _hpBar.value = _status.Hp / 100;

    public void DisplaySp() => _spBar.value = _status.Sp / 100;

    public void DisplayWeaponInfo()
    {
        _weaponManager = (_status.Role == Define.Role.Robber) ? _robberWeaponManager : _houseownerWeaponManager;
        string weaponTag = _weaponManager._selectedWeapon.tag;
        Debug.Log("���� ����: " + weaponTag);
        if (weaponTag == "Gun") // ���Ÿ� ������ ���
        {
            if (!_currentBullet.gameObject.activeSelf) _currentBullet.gameObject.SetActive(true);
            if (!_totalBullet.gameObject.activeSelf) _totalBullet.gameObject.SetActive(true);
            if (!_crossHair.activeSelf) _crossHair.SetActive(true);

            DisplayWeaponIcon(1);
            DisplayGunInfo();
        }
        else // ���� ������ ���
        {
            DisplayWeaponIcon(GetWeaponIconIndex(_weaponManager._selectedWeapon.name));
            if (_currentBullet.gameObject.activeSelf) _currentBullet.gameObject.SetActive(false);
            if (_totalBullet.gameObject.activeSelf) _totalBullet.gameObject.SetActive(false);
            if (_crossHair.activeSelf) _crossHair.SetActive(false);
        }
    }

    public void DisplayGunInfo()
    {
        _currentBullet.text =  _weaponManager._selectedWeapon.GetComponent<Gun>().GetCurrentBullet().ToString();    // ���� ������ ź��
        _totalBullet.text = _weaponManager._selectedWeapon.GetComponent<Gun>().GetTotalBullet().ToString();         // ��ü ź��
    }

    public void DisplayWeaponIcon(int iconIndex) => _weaponIcon.texture = _weaponImages[iconIndex];

    public void DisplayConnectedPlayers() => _connectedPeople.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();

    // ���� �̸����� ���� ������ ���ϱ�
    public int GetWeaponIconIndex(string weaponName)
    {
        int index = _weaponImages.Select((element, index) => new { element, index })
                        .FirstOrDefault(p => p.element.name == weaponName)
                        ?.index ?? 0;
        return index;
    }

    public void DisplayOut()
    {
        if (_status.Hp <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
    }
}

