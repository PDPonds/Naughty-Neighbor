using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : Singleton<MainMenuUIManager>
{

    [Header("===== Login Page =====")]
    [SerializeField] GameObject loginPage;
    [SerializeField] Button loginWithGoogleButton;
    [SerializeField] Button loginAsGuestButton;

    [Header("===== Menu Page =====")]
    [SerializeField] GameObject menuPage;
    [Header("- User Profile")]
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
    }

    #region Login Page
    async void LoginAsGuest()
    {
        await loginManager.SinginAnonymous();
        loginPage.SetActive(false);
        menuPage.SetActive(true);

    }
    #endregion

    #region Menu Page

    void HowToPlay()
    {
        howToPlayPage.SetActive(true);
    }

    private void StartGame()
    {
        selectModePage.SetActive(true);
    }

    #endregion

    #region How To Play Page

    void CloseHowToPlay()
    {
        howToPlayPage.SetActive(false);
    }

    #endregion

    #region Select Mode Page

    void CloseSelectMode()
    {
        selectModePage.SetActive(false);
    }

    void SelectSinglePlayer()
    {
        selectDifficultyPage.SetActive(true);
    }

    void SelectMultiPlayer()
    {
        GameManager.SwitchGameMode(GameMode.MultiPlayer);
        SceneManager.LoadScene(1);
    }

    void SelectEasyMode()
    {
        GameManager.SwitchGameMode(GameMode.SinglePlayer);
        GameManager.SwitchGameDifficulty(GameDifficulty.Easy);
        SceneManager.LoadScene(1);
    }

    void SelectNormalMode()
    {
        GameManager.SwitchGameMode(GameMode.SinglePlayer);
        GameManager.SwitchGameDifficulty(GameDifficulty.Normal);
        SceneManager.LoadScene(1);
    }

    void SelectHardMode()
    {
        GameManager.SwitchGameMode(GameMode.SinglePlayer);
        GameManager.SwitchGameDifficulty(GameDifficulty.Hard);
        SceneManager.LoadScene(1);
    }

    void CloseSelectDifficulty()
    {
        selectDifficultyPage.SetActive(false);
    }

    #endregion

}