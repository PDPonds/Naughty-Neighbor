using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

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
    [SerializeField] GameObject auntUseItemInfo;
    [SerializeField] Image auntUseItemImage;
    [Header("- RichPig")]
    public GameObject pigHeal;
    public GameObject pigDoubleAttack;
    public GameObject pigPowerThrow;
    [SerializeField] GameObject pigUseItemInfo;
    [SerializeField] Image pigUseItemImage;

    [Header("===== Setting =====")]
    [SerializeField] GameObject settingBorder;
    [SerializeField] Button settingButton;
    bool isSettingOpen;

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

        settingButton.onClick.AddListener(Setting);

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
                EnemyManager enemy = GameManager.Instance.RichPig.GetComponent<EnemyManager>();
                float curDis = enemy.curTargetDis;
                float maxDis = GameManager.gameData.MaxDistance;
                float percent = curDis / maxDis;
                RichPigAttackFill.fillAmount = percent;
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

        //AuntNextDoorHpFill.fillAmount = percent;
        StartCoroutine(LerpAuntNextDoorHpFill(AuntNextDoorHpFill.fillAmount, percent, 0.25f));
    }

    public void UpdateRichPigHP()
    {
        if (GameManager.IsGameMode(GameMode.SinglePlayer))
        {
            EnemyManager enemy = GameManager.Instance.RichPig.GetComponent<EnemyManager>();
            float curHp = enemy.curHP;
            float maxHp = enemy.curMaxHP;

            float percent = curHp / maxHp;

            //RichPigHpFill.fillAmount = percent;
            StartCoroutine(LerpRichPigHpFill(RichPigHpFill.fillAmount, percent, 0.25f));
        }
        else
        {
            PlayerManager player = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
            float curHp = player.curHP;
            float maxHp = GameManager.gameData.PlayerHP;

            float percent = curHp / maxHp;

            StartCoroutine(LerpRichPigHpFill(RichPigHpFill.fillAmount, percent, 0.25f));
            //RichPigHpFill.fillAmount = percent;
        }
    }

    IEnumerator LerpRichPigHpFill(float startValue, float endValue, float lerpDuration)
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            RichPigHpFill.fillAmount = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        RichPigHpFill.fillAmount = endValue;
    }

    IEnumerator LerpAuntNextDoorHpFill(float startValue, float endValue, float lerpDuration)
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            AuntNextDoorHpFill.fillAmount = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        AuntNextDoorHpFill.fillAmount = endValue;
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
        settingButton.gameObject.SetActive(false);
        settingBorder.gameObject.SetActive(false);
    }

    public void SetupWinLoseText()
    {
        if (GameManager.IsGameMode(GameMode.SinglePlayer))
        {
            PlayerManager player = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
            EnemyManager enemy = GameManager.Instance.RichPig.GetComponent<EnemyManager>();

            if (player.curHP <= 0)
            {
                winText.text = GameManager.gameData.SinglePlayerLose;
                SoundManager.Instance.PlayOneShot("Defeat");
            }

            if (enemy.curHP <= 0)
            {
                winText.text = GameManager.gameData.SinglePlayerWin;
                SoundManager.Instance.PlayOneShot("Victory");
            }

        }
        else if (GameManager.IsGameMode(GameMode.MultiPlayer))
        {
            PlayerManager aunt = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
            PlayerManager pig = GameManager.Instance.RichPig.GetComponent<PlayerManager>();

            if (aunt.curHP <= 0)
            {
                winText.text = GameManager.gameData.Player2Win;
            }

            if (pig.curHP <= 0)
            {
                winText.text = GameManager.gameData.Player1Win;
            }

            SoundManager.Instance.PlayOneShot("Victory");

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
        SoundManager.Instance.PlayOneShot("Button");

        SceneManager.LoadScene(1);
    }

    void Share()
    {
        SoundManager.Instance.PlayOneShot("Button");

    }

    void Home()
    {
        SoundManager.Instance.PlayOneShot("Button");
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
            ShowAuntItemInfo(auntHeal.image.sprite);
            GameManager.Instance.SwitchGameState(GameManager.Instance.GetNextGameState());
            auntHeal.gameObject.SetActive(false);
            auntDoubleAttack.interactable = false;
            auntPowerThrow.interactable = false;
            SoundManager.Instance.PlayOneShot("Button");
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
            ShowPigItemInfo(pigHeal.GetComponent<Button>().image.sprite);
            GameManager.Instance.SwitchGameState(GameManager.Instance.GetNextGameState());
            DisableInteractiveButtonAfterUseHeal_Pig();
            SoundManager.Instance.PlayOneShot("Button");

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
            SoundManager.Instance.PlayOneShot("Button");
            ShowAuntItemInfo(auntDoubleAttack.image.sprite);
        }
    }

    void PigDoubleAttack()
    {
        if (GameManager.Instance.IsGameState(GameState.RichPig))
        {
            PlayerManager player = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
            player.isDoubleAttack = true;
            DisableInteractiveButtonAfterUseDoubleAttack_Pig();
            SoundManager.Instance.PlayOneShot("Button");
            ShowPigItemInfo(pigDoubleAttack.GetComponent<Button>().image.sprite);
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
            SoundManager.Instance.PlayOneShot("Button");
            ShowAuntItemInfo(auntPowerThrow.image.sprite);
        }
    }

    void PigPowerThrow()
    {
        if (GameManager.Instance.IsGameState(GameState.RichPig))
        {
            PlayerManager player = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
            player.curBullet = GameManager.Instance.PowerThrowBullet;
            DisableInteractiveButtonAfterUsePowerThrow_Pig();
            SoundManager.Instance.PlayOneShot("Button");
            ShowPigItemInfo(pigPowerThrow.GetComponent<Button>().image.sprite);
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

    public void ShowAuntItemInfo(Sprite sprite)
    {
        auntUseItemInfo.gameObject.SetActive(true);
        auntUseItemImage.sprite = sprite;
    }

    public void HideAuntItemInfo()
    {
        auntUseItemInfo.gameObject.SetActive(false);
    }

    public void ShowPigItemInfo(Sprite sprite)
    {
        pigUseItemInfo.gameObject.SetActive(true);
        pigUseItemImage.sprite = sprite;
    }

    public void HidePigItemInfo()
    {
        pigUseItemInfo.gameObject.SetActive(false);
    }

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

}
