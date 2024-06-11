using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;

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
            userName = $"Player:{AuthenticationService.Instance.PlayerId}";
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }

    }

}
