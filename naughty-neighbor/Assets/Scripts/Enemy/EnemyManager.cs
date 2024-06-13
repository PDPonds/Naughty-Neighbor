using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    SetupAction,
    ChargeToAttack,
    AfterAttack
}


public class EnemyManager : Singleton<EnemyManager>
{
    EnemyState enemyState;

    [HideInInspector] public int curHP;
    [HideInInspector] public int curMaxHP;
    [Header("===== Visual =====")]
    [SerializeField] GameObject visual;
    Animator anim;

    [Header("===== Attack =====")]
    [SerializeField] Transform spawnBulletPoint;
    public GameObject normalBulletPrefab;
    [HideInInspector] public GameObject curBullet;
    [HideInInspector] public float targetDis;
    [HideInInspector] public float curTargetDis;

    public bool isDoubleAttack;

    private void Awake()
    {
        anim = visual.GetComponent<Animator>();
    }

    private void Start()
    {
        SetupEnemy();
    }

    private void Update()
    {
        UpdateState();
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

    public bool TakeDamage(int amount)
    {
        curHP -= amount;

        if (curHP < GameManager.gameData.PlayerHP / 2)
        {
            Anim_Play("Sleep UnFriendly");
            Anim_SetBool("isHurt", true);
        }
        else
        {
            Anim_Play("Sleep Friendly");
        }

        if (curHP <= 0)
        {
            Die();
            return true;
        }
        GameUIManager.Instance.UpdateRichPigHP();
        return false;
    }

    void Die()
    {
        PlayerManager aunt = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
        aunt.Anim_Play("Cheer Friendly");
        Debug.Log("Aunt Cheer");
        Anim_Play("Moody UnFriendly");
        GameManager.Instance.SwitchGameState(GameState.Winner);
    }

    public void Heal(int amount)
    {
        curHP += amount;
        if (curHP >= GameManager.gameData.PlayerHP / 2) Anim_SetBool("isHurt", false);
        if (curHP > GameManager.gameData.PlayerHP)
        {
            curHP = GameManager.gameData.PlayerHP;
        }
        GameUIManager.Instance.UpdateRichPigHP();
    }

    public bool isHurt()
    {
        return curHP <= curMaxHP;
    }

    #endregion

    public void SetupNormalBullet()
    {
        curBullet = normalBulletPrefab;
    }

    public void InstantiatBullet(GameObject bulletPrefab, float targetDis)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, spawnBulletPoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        Vector3 target = Vector3.zero;
        if (GameManager.Instance.IsGameState(GameState.RichPig))
            target = transform.position + new Vector3(this.targetDis, 0, 0);

        bullet.OnSetupBullet(target, Target.Aunt, isDoubleAttack);
    }

    #region Item
    void UseHeal()
    {
        Heal(GameManager.gameData.Heal);
        GameUIManager.Instance.HideAttackRate();
        GameUIManager.Instance.HideAlertPage();
        SwitchState(EnemyState.AfterAttack);
        GameManager.Instance.SwitchGameState(GameManager.Instance.GetNextGameState());
        GameUIManager.Instance.DisableInteractiveButtonAfterUseHeal_Pig();
    }

    void UseDoubleAttack()
    {
        isDoubleAttack = true;
        GameUIManager.Instance.DisableInteractiveButtonAfterUseDoubleAttack_Pig();
    }

    void UsePowerThrow()
    {
        curBullet = GameManager.Instance.PowerThrowBullet;
        GameUIManager.Instance.DisableInteractiveButtonAfterUsePowerThrow_Pig();

    }
    #endregion

    #region State

    public void SwitchState(EnemyState state)
    {
        enemyState = state;
        switch (enemyState)
        {
            case EnemyState.SetupAction:

                GetVariableWithGameDifficulty(out int hpToHeal, out int missChance, out int smallShotRate);

                if (curHP <= hpToHeal && GameUIManager.Instance.pigHeal.activeSelf)
                {
                    UseHeal();
                    return;
                }

                float hitIndex = Random.Range(0, 100f);
                bool isHit = hitIndex > missChance;

                switch (GameManager.Difficulty)
                {
                    case GameDifficulty.Normal:
                        if (GameUIManager.Instance.pigDoubleAttack.activeSelf &&
                            !GameManager.Instance.IsWindy())
                        {
                            UseDoubleAttack();
                        }
                        break;
                    case GameDifficulty.Hard:
                        if (GameUIManager.Instance.pigPowerThrow.activeSelf &&
                            GameManager.Instance.IsWindy())
                        {
                            UsePowerThrow();
                        }
                        break;
                }

                if (isHit)
                {
                    float smallIndex = Random.Range(0, 100f);
                    bool isSmallShot = smallIndex < smallShotRate;

                    if (isSmallShot)
                    {
                        targetDis = (GameManager.Instance.AuntNextDoor.transform.localPosition.x * 2f) - 1f;
                    }
                    else
                    {
                        targetDis = (GameManager.Instance.AuntNextDoor.transform.localPosition.x * 2f);
                    }
                }
                else
                {
                    float toFarIndex = Random.Range(0, 100f);
                    bool isFar = toFarIndex > 50f;
                    if (isFar)
                    {
                        float minFarDis = (GameManager.Instance.AuntNextDoor.transform.localPosition.x * 2f) + 1f;
                        targetDis = Random.Range(minFarDis, GameManager.gameData.MaxDistance);
                    }
                    else
                    {
                        float maxCloseDis = (GameManager.Instance.AuntNextDoor.transform.localPosition.x * 2f) - 2f;
                        targetDis = Random.Range(GameManager.gameData.MinDistance, maxCloseDis);
                    }
                }

                SwitchState(EnemyState.ChargeToAttack);

                break;
            case EnemyState.ChargeToAttack:

                GameUIManager.Instance.ShowAttackRate();

                break;
            case EnemyState.AfterAttack:
                break;
        }
    }

    void GetVariableWithGameDifficulty(out int hpToHeal, out int missChance, out int smallShotRate)
    {
        switch (GameManager.Difficulty)
        {
            case GameDifficulty.Easy:
                hpToHeal = GameManager.gameData.EnemyTargetHPToHeal_Easy;
                missChance = GameManager.gameData.EnemyMissedRate_Easy;
                smallShotRate = GameManager.gameData.EnemySmallShotRate_Easy;
                break;
            case GameDifficulty.Normal:
                hpToHeal = GameManager.gameData.EnemyTargetHPToHeal_Normal;
                missChance = GameManager.gameData.EnemyMissedRate_Normal;
                smallShotRate = GameManager.gameData.EnemySmallShotRate_Normal;
                break;
            case GameDifficulty.Hard:
                hpToHeal = GameManager.gameData.EnemyTargetHPToHeal_Hard;
                missChance = GameManager.gameData.EnemyMissedRate_Hard;
                smallShotRate = GameManager.gameData.EnemySmallShotRate_Hard;
                break;
            default:
                hpToHeal = 0;
                missChance = 0;
                smallShotRate = 0;
                break;
        }
    }

    void UpdateState()
    {
        switch (enemyState)
        {
            case EnemyState.SetupAction:
                break;
            case EnemyState.ChargeToAttack:
                curTargetDis += GameManager.gameData.HoldMultiply * Time.deltaTime;

                if (curTargetDis >= targetDis)
                {
                    InstantiatBullet(curBullet, targetDis);
                    GameUIManager.Instance.HideAttackRate();
                    SwitchState(EnemyState.AfterAttack);
                }
                break;
            case EnemyState.AfterAttack:
                break;
        }
    }

    #endregion

    public void Anim_Play(string name)
    {
        anim.Play(name);
    }

    public void Anim_SetBool(string variable, bool value)
    {
        anim.SetBool(variable, value);
    }
}
