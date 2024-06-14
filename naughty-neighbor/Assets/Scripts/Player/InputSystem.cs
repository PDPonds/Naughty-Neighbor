using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    PlayerInput input;
    private void OnEnable()
    {
        if (input == null)
        {
            input = new PlayerInput();

            input.Player.Press.started += i => StartHold();
            input.Player.Press.canceled += i => StopHold();

        }

        input.Enable();
    }

    private void StartHold()
    {
        PlayerManager player = GameManager.Instance.GetCurPlayerManager();
        if (player.IsPlayerState(PlayerState.BeforeAttack) && !player.isHold)
        {
            player.isHold = true;
            player.isDecreaseDis = false;
            GameUIManager.Instance.ShowAttackRate();
            SoundManager.Instance.PlayOneShot("Pull");
        }
    }

    private void StopHold()
    {
        PlayerManager player = GameManager.Instance.GetCurPlayerManager();
        if (player.IsPlayerState(PlayerState.BeforeAttack) && player.isHold)
        {
            player.isHold = false;
            player.InstantiatBullet(player.curBullet);
            GameUIManager.Instance.HideAttackRate();
            player.SwitchState(PlayerState.AfterAttack);
        }
    }


    private void OnDisable()
    {
        input.Disable();
    }

}
