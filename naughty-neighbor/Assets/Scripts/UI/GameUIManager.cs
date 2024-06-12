using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : Singleton<GameUIManager>
{
    [Header("===== HP =====")]
    [SerializeField] Image AuntNextDoorHpFill;
    [SerializeField] Image RichPigHpFill;

    [Header("===== Turn =====")]
    [SerializeField] GameObject AuntNextDoorArrow;
    [SerializeField] GameObject RichPigArrow;

    [Header("===== Attack Rate =====")]
    [Header("- Aunt Next Door")]
    [SerializeField] GameObject AuntNextDoorAttackBorder;
    [SerializeField] Image AuntNextDoorAttackFill;
    [Header("- RichPig")]
    [SerializeField] GameObject RichPigAttackBorder;
    [SerializeField] Image RichPigAttackFill;

    [Header("===== Wind =====")]
    [SerializeField] GameObject leftWindArrow;
    [SerializeField] GameObject rightWindArrow;

    [Header("===== EndGame =====")]
    [SerializeField] GameObject endGamePage;
    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Button replayButton;
    [SerializeField] Button shareButton;
    [SerializeField] Button homeButton;

    private void OnEnable()
    {
        GameManager.Instance.OnSetupWindForce += UpdateWindArrow;
    }

    private void Start()
    {
        replayButton.onClick.AddListener(Replay);
        shareButton.onClick.AddListener(Share);
        homeButton.onClick.AddListener(Home);
    }

    private void Update()
    {
        UpdateAttackRateInfo();
    }

    void UpdateAttackRateInfo()
    {
        if (AuntNextDoorAttackBorder.activeSelf)
        {
            PlayerManager player = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
            float curDis = player.curDis;
            float maxDis = GameManager.gameData.MaxDistance;
            float percent = curDis / maxDis;
            AuntNextDoorAttackFill.fillAmount = percent;
        }

        if (RichPigAttackBorder.activeSelf)
        {
            if (GameManager.IsGameMode(GameMode.SinglePlayer))
            {

            }
            else
            {
                PlayerManager player = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
                float curDis = player.curDis;
                float maxDis = GameManager.gameData.MaxDistance;
                float percent = curDis / maxDis;
                RichPigAttackFill.fillAmount = percent;
            }
        }
    }

    public void ShowAttackRate()
    {
        HideAttackRate();

        if (GameManager.Instance.IsGameState(GameState.AuntNextDoor))
        {
            AuntNextDoorAttackBorder.SetActive(true);
        }
        else if (GameManager.Instance.IsGameState(GameState.RichPig))
        {
            RichPigAttackBorder.SetActive(true);
        }

    }

    public void HideAttackRate()
    {
        AuntNextDoorAttackBorder.SetActive(false);
        RichPigAttackBorder.SetActive(false);
    }

    public void ShowArrow()
    {
        AuntNextDoorArrow.SetActive(false);
        RichPigArrow.SetActive(false);
        if (GameManager.Instance.IsGameState(GameState.RichPig))
        {
            RichPigArrow.SetActive(true);
        }
        else if (GameManager.Instance.IsGameState(GameState.AuntNextDoor))
        {
            AuntNextDoorArrow.SetActive(true);
        }
    }

    public void UpdateAuntNextDoorHP()
    {
        PlayerManager player = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
        float curHp = player.curHP;
        float maxHp = GameManager.gameData.PlayerHP;

        float percent = curHp / maxHp;
        AuntNextDoorHpFill.fillAmount = percent;
    }

    public void UpdateRichPigHP()
    {
        if (GameManager.IsGameMode(GameMode.SinglePlayer))
        {
            EnemyManager enemy = GameManager.Instance.RichPig.GetComponent<EnemyManager>();
            float curHp = enemy.curHP;
            float maxHp = enemy.curMaxHP;

            float percent = curHp / maxHp;
            RichPigHpFill.fillAmount = percent;
        }
        else
        {
            PlayerManager player = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
            float curHp = player.curHP;
            float maxHp = GameManager.gameData.PlayerHP;

            float percent = curHp / maxHp;
            RichPigHpFill.fillAmount = percent;
        }
    }

    public void UpdateWindArrow(float windForce)
    {
        leftWindArrow.SetActive(false);
        rightWindArrow.SetActive(false);

        if (windForce > 0)
        {
            rightWindArrow.SetActive(true);
        }
        else if (windForce < 0)
        {
            leftWindArrow.SetActive(true);
        }
    }

    public void ShowEndGamePanel()
    {
        endGamePage.SetActive(true);
        SetupWinLoseText();
        SetupGameTime();
    }

    public void SetupWinLoseText()
    {
        if (GameManager.IsGameMode(GameMode.SinglePlayer))
        {
            PlayerManager player = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
            if (player.curHP <= 0)
            {
                winText.text = GameManager.gameData.SinglePlayerLose;
            }

            EnemyManager enemy = GameManager.Instance.RichPig.GetComponent<EnemyManager>();
            if (enemy.curHP <= 0)
            {
                winText.text = GameManager.gameData.SinglePlayerWin;
            }

        }
        else if (GameManager.IsGameMode(GameMode.MultiPlayer))
        {
            PlayerManager aunt = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
            if (aunt.curHP <= 0)
            {
                winText.text = GameManager.gameData.Player2Win;
            }

            PlayerManager pig = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
            if (pig.curHP <= 0)
            {
                winText.text = GameManager.gameData.Player1Win;
            }

        }
    }

    public void SetupGameTime()
    {
        int min = (int)GameManager.Instance.gameTime / 60;
        int sec = (int)GameManager.Instance.gameTime % 60;
        timeText.text = $"{min} : {sec} m";
    }

    #region Button
    void Replay()
    {
        SceneManager.LoadScene(1);
    }

    void Share()
    {

    }

    void Home()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

}
