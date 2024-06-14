using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MainMenuUIManager : Singleton<MainMenuUIManager>
{
    [SerializeField] GameObject gameName;
    [Header("===== Login Page =====")]
    [SerializeField] GameObject loginPage;
    [SerializeField] Button loginWithGoogleButton;
    [SerializeField] Button loginAsGuestButton;

    [Header("===== Menu Page =====")]
    [SerializeField] GameObject menuPage;
    [Header("- User Profile")]
    [SerializeField] GameObject userParent;
    [SerializeField] Image userProfile;
    [SerializeField] TextMeshProUGUI userName;
    [Header("- Button")]
    [SerializeField] Button howToPlayButton;
    [SerializeField] Button startGameButton;

    [Header("===== How to Play Page =====")]
    [SerializeField] GameObject howToPlayPage;
    [SerializeField] GameObject howToPlayBorder;
    [SerializeField] Button closeHowToPlayerButton;

    [Header("===== Select Mode Page =====")]
    [SerializeField] GameObject selectModePage;
    [SerializeField] GameObject selectModeBorder;
    [SerializeField] Button closeSelectModeButton;
    [SerializeField] Button selectSinglePlayerButton;
    [SerializeField] Button selectMultiPlayerButton;
    [Header("===== Select Difficulty Page =====")]
    [SerializeField] GameObject selectDifficultyPage;
    [SerializeField] GameObject selectDifficultyBorder;
    [SerializeField] Button closeDifficultyButton;
    [SerializeField] Button easyModeButton;
    [SerializeField] Button normalModeButton;
    [SerializeField] Button hardModeButton;

    [Header("===== Sound Setting =====")]
    [SerializeField] GameObject settingBorder;
    [SerializeField] Button settingButton;
    bool isSettingOpen;

    LoginManager loginManager;

    private void Awake()
    {
        loginManager = GetComponent<LoginManager>();
        loginAsGuestButton.onClick.AddListener(LoginAsGuest);
        howToPlayButton.onClick.AddListener(HowToPlay);
        startGameButton.onClick.AddListener(StartGame);
        closeHowToPlayerButton.onClick.AddListener(CloseHowToPlay);
        closeSelectModeButton.onClick.AddListener(CloseSelectMode);
        closeDifficultyButton.onClick.AddListener(CloseSelectDifficulty);
        selectSinglePlayerButton.onClick.AddListener(SelectSinglePlayer);
        selectMultiPlayerButton.onClick.AddListener(SelectMultiPlayer);
        easyModeButton.onClick.AddListener(SelectEasyMode);
        normalModeButton.onClick.AddListener(SelectNormalMode);
        hardModeButton.onClick.AddListener(SelectHardMode);
        settingButton.onClick.AddListener(Setting);
    }

    private void Start()
    {
        Scale(gameName, Vector3.one, 0.5f);
        Scale(loginAsGuestButton.gameObject, Vector3.one, 0.5f);
        Scale(loginWithGoogleButton.gameObject, Vector3.one, 0.5f);

    }

    #region Login Page
    async void LoginAsGuest()
    {
        await loginManager.SinginAnonymous();
        loginPage.SetActive(false);
        menuPage.SetActive(true);
        Scale(howToPlayButton.gameObject, Vector3.one, 0.5f);
        Scale(startGameButton.gameObject, Vector3.one, 0.5f);
        Scale(userParent, Vector3.one, 0.5f);
        SoundManager.Instance.PlayOneShot("Button");

    }
    #endregion

    #region Menu Page

    void HowToPlay()
    {
        howToPlayPage.SetActive(true);
        Scale(howToPlayBorder, Vector3.one, 0.5f);
        SoundManager.Instance.PlayOneShot("Button");

    }

    private void StartGame()
    {
        selectModePage.SetActive(true);
        Scale(selectModeBorder, Vector3.one, 0.5f);
        SoundManager.Instance.PlayOneShot("Button");

    }

    #endregion

    #region How To Play Page

    void CloseHowToPlay()
    {
        SoundManager.Instance.PlayOneShot("Button");

        Scale(howToPlayBorder, Vector3.zero, 0.5f, () =>
        {
            howToPlayPage.SetActive(false);
        });
    }

    #endregion

    #region Select Mode Page

    void CloseSelectMode()
    {
        SoundManager.Instance.PlayOneShot("Button");

        Scale(selectModeBorder, Vector3.zero, 0.5f, () =>
        {
            selectModePage.SetActive(false);
        });
    }

    void SelectSinglePlayer()
    {
        selectDifficultyPage.SetActive(true);
        Scale(selectDifficultyBorder, Vector3.one, 0.5f);
        SoundManager.Instance.PlayOneShot("Button");
    }

    void SelectMultiPlayer()
    {
        GameManager.SwitchGameMode(GameMode.MultiPlayer);
        SoundManager.Instance.PlayOneShot("Button");
        SceneManager.LoadScene(1);
    }

    void SelectEasyMode()
    {
        GameManager.SwitchGameMode(GameMode.SinglePlayer);
        GameManager.SwitchGameDifficulty(GameDifficulty.Easy);
        SoundManager.Instance.PlayOneShot("Button");
        SceneManager.LoadScene(1);
    }

    void SelectNormalMode()
    {
        GameManager.SwitchGameMode(GameMode.SinglePlayer);
        GameManager.SwitchGameDifficulty(GameDifficulty.Normal);
        SoundManager.Instance.PlayOneShot("Button");
        SceneManager.LoadScene(1);
    }

    void SelectHardMode()
    {
        GameManager.SwitchGameMode(GameMode.SinglePlayer);
        GameManager.SwitchGameDifficulty(GameDifficulty.Hard);
        SoundManager.Instance.PlayOneShot("Button");
        SceneManager.LoadScene(1);
    }

    void CloseSelectDifficulty()
    {
        SoundManager.Instance.PlayOneShot("Button");
        Scale(selectDifficultyBorder, Vector3.zero, 0.5f, () =>
        {
            selectDifficultyPage.SetActive(false);
        });
    }

    #endregion

    void Setting()
    {
        if (isSettingOpen)
        {
            Scale(settingBorder, Vector3.zero, 0.25f);
        }
        else
        {
            Scale(settingBorder, Vector3.one, 0.25f);
        }
        isSettingOpen = !isSettingOpen;
        SoundManager.Instance.PlayOneShot("Button");
    }

    void Scale(GameObject go, Vector3 scale, float time)
    {
        LeanTween.scale(go, scale, time).setEaseInOutCubic();
    }

    void Scale(GameObject go, Vector3 scale, float time, System.Action callback)
    {
        LeanTween.scale(go, scale, time).setEaseInOutCubic()
            .setOnComplete(() => callback.Invoke());
    }
}