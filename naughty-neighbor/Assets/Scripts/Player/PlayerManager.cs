using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public int curHP;

    [Header("===== Attack =====")]
    [SerializeField] Transform spawnBulletPoint;
    [SerializeField] GameObject normalBulletPrefab;
    [HideInInspector] public GameObject curBullet;
    [HideInInspector] public float curDis;
    [HideInInspector] public bool isHold;
    [HideInInspector] public bool isDecreaseDis;

    bool isDoubleAttack;

    private void Start()
    {
        SetupHP();
    }

    private void Update()
    {
        UpdateHold();
    }

    void UpdateHold()
    {
        if (isHold)
        {
            if (isDecreaseDis)
            {
                curDis -= GameManager.gameData.HoldMultiply * Time.deltaTime;
                if (curDis <= GameManager.gameData.MinDistance) isDecreaseDis = false;
            }
            else
            {
                curDis += GameManager.gameData.HoldMultiply * Time.deltaTime;
                if (curDis >= GameManager.gameData.MaxDistance) isDecreaseDis = true;
            }
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

    public void SetupNormalBullet()
    {
        curBullet = normalBulletPrefab;
    }

    public void InstantiatBullet(GameObject bulletPrefab)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, spawnBulletPoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        Vector3 target = Vector3.zero;
        if (GameManager.Instance.IsGameState(GameState.RichPig))
            target = transform.position + new Vector3(curDis, 0, 0);
        else if (GameManager.Instance.IsGameState(GameState.AuntNextDoor))
            target = transform.position - new Vector3(curDis, 0, 0);

        bullet.OnSetupBullet(target);
    }

}
