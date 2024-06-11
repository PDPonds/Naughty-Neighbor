using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { SinglePlayer, MultiPlayer }
public enum GameDifficulty { Easy, Normal, Hard }
public enum GameState { RichPick, AuntNextDoor }

public class GameManager : Singleton<GameManager>
{
    public GameMode Mode = GameMode.SinglePlayer;
    public GameDifficulty Difficulty = GameDifficulty.Easy;
    public GameState State = GameState.AuntNextDoor;

    public GameData gameData = new GameData();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    #region Game Mode

    public void SwitchGameMode(GameMode mode)
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

    public bool IsGameMode(GameMode mode)
    {
        return Mode == mode;
    }

    #endregion

    #region Game Difficulty

    public void SwitchGameDifficulty(GameDifficulty difficulty)
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

    public bool IsGameDifficulty(GameDifficulty difficulty)
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
                break;
            case GameState.RichPick:
                break;
        }
    }

    public bool IsGameState(GameState state)
    {
        return State == state;
    }

    #endregion

}
