using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { SinglePlayer, MultiPlayer }
public enum GameDifficulty { Easy, Normal, Hard }
public enum GameState { RichPig, AuntNextDoor, Winner }

public class GameManager : Singleton<GameManager>
{
    public static GameMode Mode = GameMode.SinglePlayer;
    public static GameDifficulty Difficulty = GameDifficulty.Easy;
    public static GameState State = GameState.AuntNextDoor;

    public static GameData gameData = new GameData();

    public GameObject AuntNextDoor;
    public GameObject RichPig;

    float gameTime;
    float turnTime;

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

                if (IsGameMode(GameMode.MultiPlayer))
                {
                    EnablePlayerManagerOnGameObject(RichPig, false);
                    EnablePlayerManagerOnGameObject(AuntNextDoor, true);
                }

                break;
            case GameState.RichPig:

                if (IsGameMode(GameMode.MultiPlayer))
                {
                    EnablePlayerManagerOnGameObject(RichPig, true);
                    EnablePlayerManagerOnGameObject(AuntNextDoor, false);
                }

                break;
            case GameState.Winner:
                break;
        }
        turnTime = gameData.TimeToThink;
    }

    void UpdateGameState()
    {
        switch (State)
        {
            case GameState.AuntNextDoor:
            case GameState.RichPig:

                DecreaseTime();

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

    void DecreaseTime()
    {
        gameTime += Time.deltaTime;
        turnTime -= Time.deltaTime;

        if (turnTime <= gameData.TimeToWarning)
        {
            Debug.Log("Warning");
        }

        if (turnTime <= 0)
        {
            SwitchGameState(GetNextGameState());
        }
    }

    #endregion

}
