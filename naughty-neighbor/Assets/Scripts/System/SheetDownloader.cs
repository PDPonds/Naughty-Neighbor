using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SheetDownloader : MonoBehaviour
{
    const string sheetId = "12KB8eV75NC_4-saBfeA8Sw4qZHBDGo1wKuXJjTUgj0Q";
    const string url = "https://docs.google.com/spreadsheets/d/" + sheetId + "/export?format=csv";

    private void Start()
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
                GameManager.Instance.gameData = ProcessData(webRequest.downloadHandler.text);
            }
        }
    }

    public GameData ProcessData(string cvsData)
    {
        char lineEnding = '\n';
        string[] rows = cvsData.Split(lineEnding);
        int dataStartRowIndex = 1;
        GameData gameData = new GameData();
        for (int i = dataStartRowIndex; i < rows.Length; i++)
        {
            string[] cells = rows[i].Split(',');
            switch (i)
            {
                case 1:
                    gameData.PlayerHP = ParseInt(cells[3]);
                    break;
                case 2:
                    gameData.EnemyHP_Easy = ParseInt(cells[3]);
                    gameData.EnemyMissedRate_Easy = ParseInt(cells[4]);
                    gameData.EnemyTargetHPToHeal_Easy = ParseInt(cells[5]);
                    gameData.EnemySmallShotRate_Easy = ParseInt(cells[6]);
                    break;
                case 3:
                    gameData.EnemyHP_Normal = ParseInt(cells[3]);
                    gameData.EnemyMissedRate_Normal = ParseInt(cells[4]);
                    gameData.EnemyTargetHPToHeal_Normal = ParseInt(cells[5]);
                    gameData.EnemySmallShotRate_Normal = ParseInt(cells[6]);
                    break;
                case 4:
                    gameData.EnemyHP_Hard = ParseInt(cells[3]);
                    gameData.EnemyMissedRate_Hard = ParseInt(cells[4]);
                    gameData.EnemyTargetHPToHeal_Hard = ParseInt(cells[5]);
                    gameData.EnemySmallShotRate_Hard = ParseInt(cells[6]);
                    break;
                case 5:
                    gameData.NormalAttack = ParseInt(cells[2]);
                    break;
                case 6:
                    gameData.SmallAttack = ParseInt(cells[2]);
                    break;
                case 7:
                    gameData.PowerThrow = ParseInt(cells[2]);
                    break;
                case 8:
                    gameData.DoubleAttackAmount = ParseInt(cells[1]);
                    gameData.DoubleAttack = ParseInt(cells[2]);
                    break;
                case 9:
                    gameData.Heal = ParseInt(cells[3]);
                    break;
                case 10:
                    gameData.TimeToThink = ParseInt(cells[7]);
                    break;
                case 11:
                    gameData.TimeToWarning = ParseInt(cells[7]);
                    break;
                case 12:
                    gameData.MinDistance = ParseFloat(cells[1]);
                    break;
                case 13:
                    gameData.MaxDistance = ParseFloat(cells[1]);
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
    public int TimeToThink;
    public int TimeToWarning;

    [Header("===== Distance =====")]
    public float MinDistance;
    public float MaxDistance;

}

