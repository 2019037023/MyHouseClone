using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] GameObject _player;
    IStatus _status;
    [SerializeField] WeaponManager _robberWeaponManager;
    [SerializeField] WeaponManager _houseownerWeaponManager;
    [SerializeField] WeaponManager _weaponManager;
    [SerializeField] WeaponManager_S _weaponManagerS;

    bool _isDead = false;

    #region UI ����

    [Header("���� UI")]
    // �ð�
    [SerializeField] TextMeshProUGUI _timeSecond;
    float _timer;

    // �������ͽ�
    [SerializeField] Slider _hpBar;
    [SerializeField] Slider _spBar;

    // ����
    public List<Texture2D> _weaponImages = new List<Texture2D>();
    RawImage _weaponIcon;
    TextMeshProUGUI _currentBullet;
    TextMeshProUGUI _totalBullet;

    // ���ؼ�
    [SerializeField] GameObject _crossHair;

    // ���� �޴�
    GameObject _exitMenu;

    [Header("EndUI")]
    int score = 0;
    [SerializeField] float _endTime = 6f;
    [SerializeField] float _fadeDuration = 4.0f;
    [SerializeField] GameObject _gameOverScreen;
    [SerializeField] Image _fadeImageInGameOverScreen;
    [SerializeField] TextMeshProUGUI _quitTimer;
    [SerializeField] TextMeshProUGUI _killGhostText;

    [Header("��庰 UI")]
    [SerializeField] List<Texture2D> _rightUpImages = new List<Texture2D>();
    RawImage _rightUpIcon;
    [SerializeField] TextMeshProUGUI _rightUpText;  // �̱�(���� ���� ��) ��Ƽ(���� ���� �ο�)
    #endregion

    void Start()
    {
        _status = _player.GetComponent<IStatus>();
        InitUI();
    }

    void InitUI()
    {
        // ���� �ð�
        _timeSecond = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

        // Hp, Sp ��
        _hpBar = transform.GetChild(1).GetComponent<Slider>();
        _spBar = transform.GetChild(2).GetComponent<Slider>();

        // ���� ����
        _weaponIcon = transform.GetChild(3).GetChild(0).GetComponent<RawImage>();
        _currentBullet = transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        _totalBullet = transform.GetChild(3).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

        // ���ؼ� UI
        _crossHair = transform.GetChild(4).gameObject;

        // ���� ǥ��
        _rightUpIcon = transform.GetChild(5).GetChild(0).GetComponent<RawImage>();
        _rightUpText = transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>();

        // ���� ���� �޴�
        _exitMenu = transform.GetChild(6).gameObject;

        // ���� ȭ��
        _gameOverScreen = transform.GetChild(7).gameObject;
        _fadeImageInGameOverScreen = _gameOverScreen.GetComponent<Image>();
        _quitTimer = _gameOverScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _killGhostText = _gameOverScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        AliveUI();
        JustNowDeadUI();
        CountQuitGame();
    }


    void AliveUI()
    {
        if (_status.Hp > 0 && !_isDead)
        {
            DisplayRightUp();
            DisplayExitMenu();
            DisplayLivingTime();
            DisplayHp();
            DisplaySp();
            DisplayWeaponInfo();
        }
    }

    void JustNowDeadUI()
    {
        if (_status.Hp <= 0 && !_isDead)
        {
            _isDead = true;
            DisableUI();
            StartCoroutine(ShowDeadScreen());
        }
    }

    void CountQuitGame()
    {
        if (_status.Hp <= 0 && _isDead)
        {
            _endTime -= Time.deltaTime;
            _quitTimer.text = Mathf.FloorToInt(_endTime) + " seconds to quit.";
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
        string weaponTag;
        if (NetworkManager._instance == null) // �̱�
        {
            weaponTag = _weaponManagerS._selectedWeapon.tag;
        }
        else // ��Ƽ
        {
            _weaponManager = (_status.Role == Define.Role.Robber) ? _robberWeaponManager : _houseownerWeaponManager;
            weaponTag = _weaponManager._selectedWeapon.tag;
        }

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

            if (NetworkManager._instance == null) // �̱�
            {
                DisplayWeaponIcon(GetWeaponIconIndex(_weaponManagerS._selectedWeapon.name));
            }
            else // ��Ƽ
            {
                DisplayWeaponIcon(GetWeaponIconIndex(_weaponManager._selectedWeapon.name));
            }

            if (_currentBullet.gameObject.activeSelf) _currentBullet.gameObject.SetActive(false);
            if (_totalBullet.gameObject.activeSelf) _totalBullet.gameObject.SetActive(false);
            if (_crossHair.activeSelf) _crossHair.SetActive(false);
        }
    }

    public void DisplayGunInfo()
    {
        if(NetworkManager._instance == null) // �̱�
        {
            _currentBullet.text = _weaponManagerS._selectedWeapon.GetComponent<Gun_S>().GetCurrentBullet().ToString();    // ���� ������ ź��
            _totalBullet.text = _weaponManagerS._selectedWeapon.GetComponent<Gun_S>().GetTotalBullet().ToString();         // ��ü ź��
        }
        else // ��Ƽ
        {
            _currentBullet.text = _weaponManager._selectedWeapon.GetComponent<Gun>().GetCurrentBullet().ToString();    // ���� ������ ź��
            _totalBullet.text = _weaponManager._selectedWeapon.GetComponent<Gun>().GetTotalBullet().ToString();         // ��ü ź��
        }   
    }

    public void DisplayWeaponIcon(int iconIndex) => _weaponIcon.texture = _weaponImages[iconIndex];

    // ���� �г�
    public void DisplayRightUp() 
    {
        _rightUpText.text = (NetworkManager._instance == null) ? GameManager_S._instance._monsterCount.ToString() : PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        _rightUpIcon.texture = (NetworkManager._instance == null) ? _rightUpImages[0] : _rightUpImages[1];
    }
    // ���� �̸����� ���� ������ ���ϱ�
    public int GetWeaponIconIndex(string weaponName)
    {
        int index = _weaponImages.Select((element, index) => new { element, index })
                        .FirstOrDefault(p => p.element.name == weaponName)
                        ?.index ?? 0;
        return index;
    }

    public void DisplayExitMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _exitMenu.activeSelf)
        {
            _exitMenu.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !_exitMenu.activeSelf)
        {
            _exitMenu.SetActive(true);
        }

        if (_exitMenu.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            ExitToTitle();
        }
    }

    public void DisableUI()
    {
        if (_status.Hp <= 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i == transform.childCount - 1) break;
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    // ���� ���� ��ũ�� �����ֱ�
    IEnumerator ShowDeadScreen()
    {
        _gameOverScreen.SetActive(true);

        if (NetworkManager._instance == null)
        {
            score = GameManager_S._instance._score; // ���� ����
            _killGhostText.text = "Killed Ghost : " + score.ToString();
        }
        float elapsedTime = 1.0f;
        Color color = _fadeImageInGameOverScreen.color;
        color.a = 0.0f; // ����

        // ���İ� 0���� 1�� ����
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0.0f, 1.0f, elapsedTime / _fadeDuration);
            _fadeImageInGameOverScreen.color = color;
            yield return null;
        }

        color.a = 1.0f; // ������
        _fadeImageInGameOverScreen.color = color;
    }

    public void ExitToTitle() => SceneManager.LoadScene("TitleScene");
}