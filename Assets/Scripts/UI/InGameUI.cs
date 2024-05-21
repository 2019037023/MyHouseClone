using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    GameObject _player;
    PlayerController _playerController;
    Status _status;
    WeaponManager _weaponManager;

    //UI ������


    TextMeshProUGUI _timeSecond;
    float _timer;

    Slider _hpBar;
    Slider _spBar;


    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _status = _player.GetComponent<Status>();

        // �ð� ǥ���� ��
        _timeSecond = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        _hpBar = transform.GetChild(1).GetComponent<Slider>();
        _spBar = transform.GetChild(2).GetComponent<Slider>();
    }

    void Update()
    {
        DisplayLivingTime();
        DisplayHp();
        DisplaySp();
    }

    public void DisplayLivingTime()
    {
        // ü���� 0�̸� ���߱�

        _timer += Time.deltaTime;
        _timeSecond.text = ((int)_timer).ToString();
    }

    void DisplayHp()
    {
        _hpBar.value = _status.Hp / 100;
    }

    void DisplaySp()
    {
        _spBar.value = _status.Sp / 100;
    }

    public void DisplayWeaponInfo()
    {

    }

    public void DisplayBullet()
    {

    }

    public void DisplayWeaponIcon()
    {

    }

    public void DisplayConnectedPlayers()
    {

    }
}

