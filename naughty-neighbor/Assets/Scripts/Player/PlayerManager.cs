using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("===== HP =====")]
    int curHP;

    [Header("===== Attack =====")]
    [HideInInspector] public float curDis;
    [HideInInspector] public bool isHold;
    [SerializeField] GameObject normalBulletPrefab;

    [Header("===== Item =====")]
    [SerializeField] bool isDoubleAttack;

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
    }

    public void TakeDamage(int amount)
    {
        curHP -= amount;
        if (curHP <= 0)
        {
            Die();
        }
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
    }

    #endregion


}
