using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { SinglePlayer, MultiPlayer }
public enum GameDifficulty { Easy, Normal, Hard }
public enum GameState { RichPig, AuntNextDoor }

public class GameManager : Singleton<GameManager>
{
    public static GameMode Mode = GameMode.SinglePlayer;
    public static GameDifficulty Difficulty = GameDifficulty.Easy;
    public static GameState State = GameState.AuntNextDoor;

    public static GameData gameData = new GameData();

    public GameObject AuntNextDoor;
    public GameObject RichPig;

    private void Awake()
    {
        SetupOnStartGame();
    }

    private void Start()
    {
        SwitchGameState(GameState.AuntNextDoor);
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

    #endregion

}
