using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public static string userName;

    private async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async Task SinginAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            string id = GetUserName(AuthenticationService.Instance.PlayerId, 5).ArrayToString();
            userName = $"Guest:{id}";
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }

    }

    char[] GetUserName(string userName, int textCount)
    {
        List<char> result = new List<char>();

        char[] allName = userName.ToCharArray();
        for (int i = 0; i < textCount; i++)
        {
            result.Add(allName[i]);
        }

        char[] chars = result.ToArray();
        return chars;
    }

}
