using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : Singleton<GameUIManager>
{
    [Header("===== HP =====")]
    [SerializeField] Image AuntNextDoorHpFill;
    [SerializeField] Image RichPigHpFill;


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
            PlayerManager player = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
            float curHp = player.curHP;
            float maxHp = GameManager.gameData.PlayerHP;

            float percent = curHp / maxHp;
            RichPigHpFill.fillAmount = percent;
        }
    }

}
