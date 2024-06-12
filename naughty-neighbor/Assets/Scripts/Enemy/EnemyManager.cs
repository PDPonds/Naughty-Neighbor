using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [HideInInspector] public int curHP;
    [HideInInspector] public int curMaxHP;

    [Header("===== Attack =====")]
    [SerializeField] Transform spawnBulletPoint;
    [SerializeField] GameObject normalBulletPrefab;
    [HideInInspector] public GameObject curBullet;

    bool isDoubleAttack;

    private void Start()
    {
        SetupEnemy();
    }

    #region Status
    public void SetupEnemy()
    {
        switch (GameManager.Difficulty)
        {
            case GameDifficulty.Easy:
                curMaxHP = GameManager.gameData.EnemyHP_Easy;
                break;
            case GameDifficulty.Normal:
                curMaxHP = GameManager.gameData.EnemyHP_Normal;
                break;
            case GameDifficulty.Hard:
                curMaxHP = GameManager.gameData.EnemyHP_Hard;
                break;
        }
        curHP = curMaxHP;
        GameUIManager.Instance.UpdateRichPigHP();
    }

    public void TakeDamage(int amount)
    {
        curHP -= amount;
        if (curHP <= 0)
        {
            Die();
        }
        GameUIManager.Instance.UpdateRichPigHP();
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
        GameUIManager.Instance.UpdateRichPigHP();
    }
    #endregion

    public void SetupNormalBullet()
    {
        curBullet = normalBulletPrefab;
    }

    //public void InstantiatBullet(GameObject bulletPrefab)
    //{
    //    GameObject bulletObj = Instantiate(bulletPrefab, spawnBulletPoint.position, Quaternion.identity);
    //    Bullet bullet = bulletObj.GetComponent<Bullet>();

    //    Vector3 target = transform.position + new Vector3(curDis, 0, 0);

    //    bullet.OnSetupBullet(target);
    //}

}
