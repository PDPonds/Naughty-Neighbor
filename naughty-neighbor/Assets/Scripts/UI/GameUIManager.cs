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

    [Header("===== Alert Time =====")]
    [Header("- Aunt Next Door")]
    [SerializeField] GameObject AuntNextDoorAlertPage;
    [SerializeField] Image AuntNextDoorAlertFill;
    [Header("- RichPig")]
    [SerializeField] GameObject RichPigAlertPage;
    [SerializeField] Image RichPigAlertFill;

    [Header("===== Wind =====")]
    [SerializeField] GameObject leftWindArrow;
    [SerializeField] GameObject rightWindArrow;
    [SerializeField] RectTransform windParamotor;

    [Header("===== EndGame =====")]
    [SerializeField] GameObject endGamePage;
    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Button replayButton;
    [SerializeField] Button shareButton;
    [SerializeField] Button homeButton;

    [Header("===== Item =====")]
    [Header("- Aunt Next Door")]
    [SerializeField] Button auntHeal;
    [SerializeField] Button auntDoubleAttack;
    [SerializeField] Button auntPowerThrow;
    [Header("- RichPig")]
    public GameObject pigHeal;
    public GameObject pigDoubleAttack;
    public GameObject pigPowerThrow;

    private void OnEnable()
    {
        GameManager.Instance.OnSetupWindForce += UpdateWindArrow;
    }

    private void Start()
    {
        EnablePigItem();

        replayButton.onClick.AddListener(Replay);
        shareButton.onClick.AddListener(Share);
        homeButton.onClick.AddListener(Home);

        auntHeal.onClick.AddListener(AuntHeal);
        auntDoubleAttack.onClick.AddListener(AuntDoubleAttack);
        auntPowerThrow.onClick.AddListener(AuntPowerThrow);

        Button pHeal = pigHeal.GetComponent<Button>();
        pHeal.onClick.AddListener(PigHeal);
        Button pDoubleAttack = pigDoubleAttack.GetComponent<Button>();
        pDoubleAttack.onClick.AddListener(PigDoubleAttack);
        Button pPowerThrow = pigPowerThrow.GetComponent<Button>();
        pPowerThrow.onClick.AddListener(PigPowerThrow);
    }

    private void Update()
    {
        UpdateAttackRateInfo();
        UpdateAlertInfo();
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

    void UpdateAlertInfo()
    {
        float maxAlertTime = GameManager.gameData.TimeToWarning;
        float curTime = GameManager.Instance.turnTime;
        float percent = curTime / maxAlertTime;

        if (AuntNextDoorAlertPage.activeSelf)
        {
            AuntNextDoorAlertFill.fillAmount = percent;
        }

        if (RichPigAlertPage.activeSelf)
        {
            RichPigAlertFill.fillAmount = percent;
        }
    }

    public void ShowAlertPage()
    {
        HideAlertPage();
        if (GameManager.Instance.IsGameState(GameState.AuntNextDoor))
        {
            AuntNextDoorArrow.SetActive(false);
            AuntNextDoorAlertPage.SetActive(true);
        }
        else if (GameManager.Instance.IsGameState(GameState.RichPig))
        {
            RichPigArrow.SetActive(false);
            RichPigAlertPage.SetActive(true);
        }
    }

    public void HideAlertPage()
    {
        AuntNextDoorAlertPage.SetActive(false);
        RichPigAlertPage.SetActive(false);
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
        UpdateWindParamotor(windForce);
    }

    void UpdateWindParamotor(float windForce)
    {
        float min = Mathf.Abs(GameManager.gameData.MinWindForce);
        float max = Mathf.Abs(GameManager.gameData.MaxWindForce);
        float windRange = min + max;
        float curWindRange = windForce + min;
        float percent = (curWindRange / windRange) * 100f;

        float posX = ((180f * percent) / 100f) - 90f;
        windParamotor.anchoredPosition = new Vector3(posX, 0, 0);
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

    #region Item

    void EnablePigItem()
    {
        if (GameManager.IsGameMode(GameMode.SinglePlayer))
        {
            Button heal = pigHeal.GetComponent<Button>();
            Button dba = pigDoubleAttack.GetComponent<Button>();
            Button pt = pigPowerThrow.GetComponent<Button>();

            heal.enabled = false;
            dba.enabled = false;
            pt.enabled = false;
        }
        else
        {
            Button heal = pigHeal.GetComponent<Button>();
            Button dba = pigDoubleAttack.GetComponent<Button>();
            Button pt = pigPowerThrow.GetComponent<Button>();

            heal.enabled = true;
            dba.enabled = true;
            pt.enabled = true;
        }
    }

    void AuntHeal()
    {
        if (GameManager.Instance.IsGameState(GameState.AuntNextDoor))
        {
            PlayerManager player = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
            player.Heal(GameManager.gameData.Heal);
            HideAttackRate();
            HideAlertPage();
            GameManager.Instance.SwitchGameState(GameManager.Instance.GetNextGameState());
            auntHeal.gameObject.SetActive(false);
            auntDoubleAttack.interactable = false;
            auntPowerThrow.interactable = false;
        }
    }

    void PigHeal()
    {
        if (GameManager.Instance.IsGameState(GameState.RichPig))
        {
            PlayerManager player = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
            player.Heal(GameManager.gameData.Heal);
            HideAttackRate();
            HideAlertPage();
            GameManager.Instance.SwitchGameState(GameManager.Instance.GetNextGameState());
            DisableInteractiveButtonAfterUseHeal_Pig();
        }
    }

    void AuntDoubleAttack()
    {
        if (GameManager.Instance.IsGameState(GameState.AuntNextDoor))
        {
            PlayerManager player = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
            player.isDoubleAttack = true;
            auntDoubleAttack.gameObject.SetActive(false);
            auntHeal.interactable = false;
            auntPowerThrow.interactable = false;
        }
    }

    void PigDoubleAttack()
    {
        if (GameManager.Instance.IsGameState(GameState.RichPig))
        {
            PlayerManager player = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
            player.isDoubleAttack = true;
            DisableInteractiveButtonAfterUseDoubleAttack_Pig();
        }
    }

    void AuntPowerThrow()
    {
        if (GameManager.Instance.IsGameState(GameState.AuntNextDoor))
        {
            PlayerManager player = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
            player.curBullet = GameManager.Instance.PowerThrowBullet;
            auntPowerThrow.gameObject.SetActive(false);
            auntHeal.interactable = false;
            auntDoubleAttack.interactable = false;
        }
    }

    void PigPowerThrow()
    {
        if (GameManager.Instance.IsGameState(GameState.RichPig))
        {
            PlayerManager player = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
            player.curBullet = GameManager.Instance.PowerThrowBullet;
            DisableInteractiveButtonAfterUsePowerThrow_Pig();
        }
    }

    public void DisableInteractiveButtonAfterUseHeal_Pig()
    {
        pigHeal.gameObject.SetActive(false);
        pigDoubleAttack.GetComponent<Button>().interactable = false;
        pigPowerThrow.GetComponent<Button>().interactable = false;
    }

    public void DisableInteractiveButtonAfterUseDoubleAttack_Pig()
    {
        pigDoubleAttack.gameObject.SetActive(false);
        pigHeal.GetComponent<Button>().interactable = false;
        pigPowerThrow.GetComponent<Button>().interactable = false;
    }

    public void DisableInteractiveButtonAfterUsePowerThrow_Pig()
    {
        pigPowerThrow.gameObject.SetActive(false);
        pigHeal.GetComponent<Button>().interactable = false;
        pigDoubleAttack.GetComponent<Button>().interactable = false;
    }

    public void EnableInteractiveAuntItem()
    {
        auntHeal.interactable = true;
        auntDoubleAttack.interactable = true;
        auntPowerThrow.interactable = true;
    }

    public void EnableInteractivePigItem()
    {
        pigHeal.GetComponent<Button>().interactable = true;
        pigDoubleAttack.GetComponent<Button>().interactable = true;
        pigPowerThrow.GetComponent<Button>().interactable = true;
    }

    #endregion

}
