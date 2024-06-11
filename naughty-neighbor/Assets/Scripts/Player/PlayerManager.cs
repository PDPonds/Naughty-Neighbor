using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public int curHP;

    [Header("===== Attack =====")]
    [SerializeField] GameObject normalBulletPrefab;
    [HideInInspector] public GameObject curBullet;
    [HideInInspector] public float curDis;
    [HideInInspector] public bool isHold;

    [Header("===== Item =====")]
    [SerializeField] bool isDoubleAttack;
    [SerializeField] GameObject powerThrowBullet;

    private void Start()
    {
        SetupHP();
    }

    private void Update()
    {
        if (isHold)
        {
            curDis += GameManager.gameData.HoldMultiply * Time.deltaTime;
            curDis = Mathf.Clamp(curDis, GameManager.gameData.MinDistance, GameManager.gameData.MaxDistance);
        }
    }

    #region Status
    void SetupHP()
    {
        curHP = GameManager.gameData.PlayerHP;
        UpdateHPInfo();
    }

    public void TakeDamage(int amount)
    {
        curHP -= amount;
        if (curHP <= 0)
        {
            Die();
        }
        UpdateHPInfo();
    }

    void Die()
    {
        GameManager.Instance.SwitchGameState(GameState.Winner);
    }

    public void Heal(int amount)
    {
        curHP += amount;
        if (curHP > GameManager.gameData.PlayerHP)
        {
            curHP = GameManager.gameData.PlayerHP;
        }
        UpdateHPInfo();
    }

    void UpdateHPInfo()
    {
        GameUIManager.Instance.UpdateAuntNextDoorHP();
        if (GameManager.IsGameMode(GameMode.MultiPlayer)) GameUIManager.Instance.UpdateRichPigHP();
    }

    #endregion


}
