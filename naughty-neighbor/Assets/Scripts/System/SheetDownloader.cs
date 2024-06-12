using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SheetDownloader : MonoBehaviour
{
    const string sheetId = "12KB8eV75NC_4-saBfeA8Sw4qZHBDGo1wKuXJjTUgj0Q";
    const string url = "https://docs.google.com/spreadsheets/d/" + sheetId + "/export?format=csv";

    private void Awake()
    {
        StartCoroutine(DownloadData());
    }

    IEnumerator DownloadData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError ||
                webRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log("Download Error : " + webRequest.error);
            }
            else
            {
                GameManager.gameData = ProcessData(webRequest.downloadHandler.text);
            }
        }
    }

    public GameData ProcessData(string cvsData)
    {
        char lineEnding = '\n';
        string[] rows = cvsData.Split(lineEnding);
        int dataStartRowIndex = 1;
        GameData gameData = new GameData();

        int amountIndex = 1;
        int damageIndex = 2;
        int hpIndex = 3;
        int missedChanceIndex = 4;
        int targetHPToHeal = 5;
        int smallShotRate = 6;
        int secIndex = 7;
        int minIndex = 8;
        int maxIndex = 9;
        int textIndex = 10;

        for (int i = dataStartRowIndex; i < rows.Length; i++)
        {
            string[] cells = rows[i].Split(',');
            switch (i)
            {
                case 1:
                    gameData.PlayerHP = ParseInt(cells[hpIndex]);
                    break;
                case 2:
                    gameData.EnemyHP_Easy = ParseInt(cells[hpIndex]);
                    gameData.EnemyMissedRate_Easy = ParseInt(cells[missedChanceIndex]);
                    gameData.EnemyTargetHPToHeal_Easy = ParseInt(cells[targetHPToHeal]);
                    gameData.EnemySmallShotRate_Easy = ParseInt(cells[smallShotRate]);
                    break;
                case 3:
                    gameData.EnemyHP_Normal = ParseInt(cells[hpIndex]);
                    gameData.EnemyMissedRate_Normal = ParseInt(cells[missedChanceIndex]);
                    gameData.EnemyTargetHPToHeal_Normal = ParseInt(cells[targetHPToHeal]);
                    gameData.EnemySmallShotRate_Normal = ParseInt(cells[smallShotRate]);
                    break;
                case 4:
                    gameData.EnemyHP_Hard = ParseInt(cells[hpIndex]);
                    gameData.EnemyMissedRate_Hard = ParseInt(cells[missedChanceIndex]);
                    gameData.EnemyTargetHPToHeal_Hard = ParseInt(cells[targetHPToHeal]);
                    gameData.EnemySmallShotRate_Hard = ParseInt(cells[smallShotRate]);
                    break;
                case 5:
                    gameData.NormalAttack = ParseInt(cells[damageIndex]);
                    break;
                case 6:
                    gameData.SmallAttack = ParseInt(cells[damageIndex]);
                    break;
                case 7:
                    gameData.PowerThrow = ParseInt(cells[damageIndex]);
                    break;
                case 8:
                    gameData.DoubleAttackAmount = ParseInt(cells[amountIndex]);
                    gameData.DoubleAttack = ParseInt(cells[damageIndex]);
                    break;
                case 9:
                    gameData.Heal = ParseInt(cells[hpIndex]);
                    break;
                case 10:
                    gameData.TimeToThink = ParseFloat(cells[secIndex]);
                    break;
                case 11:
                    gameData.TimeToWarning = ParseFloat(cells[secIndex]);
                    break;
                case 12:
                    gameData.MinDistance = ParseFloat(cells[minIndex]);
                    gameData.MaxDistance = ParseFloat(cells[maxIndex]);
                    break;
                case 13:
                    gameData.MinWindForce = ParseFloat(cells[minIndex]);
                    gameData.MaxWindForce = ParseFloat(cells[maxIndex]);
                    break;
                case 14:
                    gameData.HoldMultiply = ParseFloat(cells[amountIndex]);
                    break;
                case 15:
                    gameData.ShootDuration = ParseFloat(cells[amountIndex]);
                    break;
                case 16:
                    gameData.TrajectoryMaxHeight = ParseFloat(cells[amountIndex]);
                    break;
                case 17:
                    gameData.WaitingDuration = ParseFloat(cells[secIndex]);
                    break;
                case 18:
                    gameData.SinglePlayerWin = cells[textIndex];
                    break;
                case 19:
                    gameData.SinglePlayerLose = cells[textIndex];
                    break;
                case 20:
                    gameData.Player1Win = cells[textIndex];
                    break;
                case 21:
                    gameData.Player2Win = cells[textIndex];
                    break;
            }
        }
        return gameData;
    }

    int ParseInt(string s)
    {
        int result = -1;
        if (!int.TryParse(s, System.Globalization.NumberStyles.Integer,
            System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result))
        {
            Debug.Log("Can't parse int , wrong text.");
        }
        return result;
    }

    float ParseFloat(string s)
    {
        float result = -1;
        if (!float.TryParse(s, System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result))
        {
            Debug.Log("Can't parse float , wrong text.");
        }
        return result;
    }

}

[Serializable]
public class GameData
{
    [Header("===== Player =====")]
    public int PlayerHP;

    [Header("===== Enemy =====")]
    public int EnemyHP_Easy;
    public int EnemyMissedRate_Easy;
    public int EnemyTargetHPToHeal_Easy;
    public int EnemySmallShotRate_Easy;

    public int EnemyHP_Normal;
    public int EnemyMissedRate_Normal;
    public int EnemyTargetHPToHeal_Normal;
    public int EnemySmallShotRate_Normal;

    public int EnemyHP_Hard;
    public int EnemyMissedRate_Hard;
    public int EnemyTargetHPToHeal_Hard;
    public int EnemySmallShotRate_Hard;

    [Header("===== Attack =====")]
    public int NormalAttack;
    public int SmallAttack;

    [Header("===== Item =====")]
    public int PowerThrow;
    public int DoubleAttackAmount;
    public int DoubleAttack;
    public int Heal;

    [Header("===== Time =====")]
    public float TimeToThink;
    public float TimeToWarning;
    public float WaitingDuration;

    [Header("===== Distance =====")]
    public float MinDistance;
    public float MaxDistance;
    public float HoldMultiply;

    [Header("===== Wind =====")]
    public float MinWindForce;
    public float MaxWindForce;

    [Header("===== Bullet =====")]
    public float ShootDuration;
    public float TrajectoryMaxHeight;

    [Header("===== End Game =====")]
    public string SinglePlayerWin;
    public string SinglePlayerLose;
    public string Player1Win;
    public string Player2Win;

}

