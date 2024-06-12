using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { SinglePlayer, MultiPlayer }
public enum GameDifficulty { Easy, Normal, Hard }
public enum GameState { RichPig, AuntNextDoor, WaitingForNextTurn, Winner }

public class GameManager : Singleton<GameManager>
{
    public event Action<float> OnSetupWindForce;

    public static GameMode Mode = GameMode.MultiPlayer;
    public static GameDifficulty Difficulty = GameDifficulty.Easy;
    public GameState State = GameState.AuntNextDoor;

    public static GameData gameData = new GameData();

    [Header("===== Player GameObject =====")]
    public GameObject AuntNextDoor;
    public GameObject RichPig;

    [Header("===== Projectile =====")]
    public AnimationCurve trajectoryAnimationCurve;

    [Header("===== Wind =====")]
    public float curWindForce;

    [Header("===== Bullet =====")]
    public GameObject PowerThrowBullet;

    Target lastTarget;
    [HideInInspector] public float gameTime;
    float turnTime;
    float waitingTime;

    private void Awake()
    {
        SetupOnStartGame();
    }

    private void Start()
    {
        SwitchGameState(GameState.AuntNextDoor);
    }

    private void Update()
    {
        UpdateGameState();
    }

    #region Setup On Scene Start
    public void SetupOnStartGame()
    {
        if (IsGameMode(GameMode.SinglePlayer)) EnableEnemyAI(true);
        else EnableEnemyAI(false);

    }

    void EnableEnemyAI(bool boolean)
    {
        EnemyManager enemy = RichPig.GetComponent<EnemyManager>();
        enemy.enabled = boolean;
    }
    #endregion

    #region Game Mode

    public static void SwitchGameMode(GameMode mode)
    {
        Mode = mode;
        switch (mode)
        {
            case GameMode.SinglePlayer:
                break;
            case GameMode.MultiPlayer:
                break;
        }
    }

    public static bool IsGameMode(GameMode mode)
    {
        return Mode == mode;
    }

    #endregion

    #region Game Difficulty

    public static void SwitchGameDifficulty(GameDifficulty difficulty)
    {
        Difficulty = difficulty;
        switch (difficulty)
        {
            case GameDifficulty.Easy:
                break;
            case GameDifficulty.Normal:
                break;
            case GameDifficulty.Hard:
                break;
        }
    }

    public static bool IsGameDifficulty(GameDifficulty difficulty)
    {
        return Difficulty == difficulty;
    }

    #endregion

    #region Game State

    public void SwitchGameState(GameState state)
    {
        State = state;
        switch (State)
        {
            case GameState.AuntNextDoor:

                SetupWindForce();
                if (IsGameMode(GameMode.MultiPlayer))
                {
                    EnablePlayerManagerOnGameObject(RichPig, false);
                    EnablePlayerManagerOnGameObject(AuntNextDoor, true);
                }

                PlayerManager auntPlayer = AuntNextDoor.GetComponent<PlayerManager>();
                auntPlayer.SetupOnPhaseStart();

                DestroyAllBulletInScene();

                break;
            case GameState.RichPig:

                SetupWindForce();
                if (IsGameMode(GameMode.MultiPlayer))
                {
                    EnablePlayerManagerOnGameObject(RichPig, true);
                    EnablePlayerManagerOnGameObject(AuntNextDoor, false);

                    PlayerManager pigPlayer = RichPig.GetComponent<PlayerManager>();
                    pigPlayer.SetupOnPhaseStart();
                }
                else
                {
                    EnemyManager pigEnemy = RichPig.GetComponent<EnemyManager>();
                    pigEnemy.SetupNormalBullet();
                }

                DestroyAllBulletInScene();

                break;
            case GameState.WaitingForNextTurn:
                waitingTime = gameData.WaitingDuration;
                break;
            case GameState.Winner:

                GameUIManager.Instance.ShowEndGamePanel();

                break;
        }
        turnTime = gameData.TimeToThink;
        GameUIManager.Instance.ShowArrow();
    }

    public void SwitchToWaitingForNextTurnState(Target targetType)
    {
        lastTarget = targetType;
        SwitchGameState(GameState.WaitingForNextTurn);
    }

    void UpdateGameState()
    {
        switch (State)
        {
            case GameState.AuntNextDoor:
            case GameState.RichPig:
                DecreaseTurnTime();
                break;
            case GameState.WaitingForNextTurn:
                DecreaseWaitingTime();
                break;
            case GameState.Winner:
                break;
        }
    }

    public bool IsGameState(GameState state)
    {
        return State == state;
    }

    void EnablePlayerManagerOnGameObject(GameObject go, bool enable)
    {
        PlayerManager player2 = go.GetComponent<PlayerManager>();
        player2.enabled = enable;
    }

    public PlayerManager GetCurPlayerManager()
    {
        if (IsGameMode(GameMode.MultiPlayer))
        {
            if (IsGameState(GameState.AuntNextDoor))
            {
                PlayerManager player1 = AuntNextDoor.GetComponent<PlayerManager>();
                return player1 != null ? player1 : null;
            }
            else
            {
                PlayerManager player2 = RichPig.GetComponent<PlayerManager>();
                return player2 != null ? player2 : null;
            }
        }
        else
        {
            PlayerManager player1 = AuntNextDoor.GetComponent<PlayerManager>();
            return player1 != null ? player1 : null;
        }
    }

    GameState GetNextGameState()
    {
        if (IsGameState(GameState.AuntNextDoor)) return GameState.RichPig;
        else if (IsGameState(GameState.RichPig)) return GameState.AuntNextDoor;
        else return GameState.Winner;
    }

    void DecreaseTurnTime()
    {
        gameTime += Time.deltaTime;

        if (IsGameState(GameState.AuntNextDoor))
        {
            PlayerManager player = AuntNextDoor.GetComponent<PlayerManager>();
            if (player.IsPlayerState(PlayerState.BeforeAttack))
            {
                if (!player.isHold)
                {
                    turnTime -= Time.deltaTime;
                }
            }
        }
        else
        {
            if (IsGameMode(GameMode.MultiPlayer))
            {
                PlayerManager player = RichPig.GetComponent<PlayerManager>();
                if (player.IsPlayerState(PlayerState.BeforeAttack))
                {
                    if (!player.isHold)
                    {
                        turnTime -= Time.deltaTime;
                    }
                }
            }
            else
            {
                turnTime -= Time.deltaTime;
            }
        }


        if (turnTime <= gameData.TimeToWarning)
        {
            Debug.Log("Warning");
        }

        if (turnTime <= 0)
        {
            SwitchGameState(GetNextGameState());
        }
    }

    void DecreaseWaitingTime()
    {
        waitingTime -= Time.deltaTime;
        if (waitingTime < 0)
        {
            if (lastTarget == Target.Pig) SwitchGameState(GameState.RichPig);
            else if (lastTarget == Target.Aunt) SwitchGameState(GameState.AuntNextDoor);
        }
    }

    void DestroyAllBulletInScene()
    {
        Bullet[] bullet = FindObjectsOfType<Bullet>();
        foreach (Bullet b in bullet)
        {
            Destroy(b.gameObject);
        }
    }

    #endregion

    #region Wind Force

    void SetupWindForce()
    {
        curWindForce = RandomWindForce();
        OnSetupWindForce?.Invoke(curWindForce);
    }

    float RandomWindForce()
    {
        return UnityEngine.Random.Range(gameData.MinWindForce, gameData.MaxWindForce);
    }

    #endregion

}
