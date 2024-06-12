using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void OnEnable()
    {
        GameManager.Instance.OnSetupWindForce += UpdateWindArrow;
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
    }

}
