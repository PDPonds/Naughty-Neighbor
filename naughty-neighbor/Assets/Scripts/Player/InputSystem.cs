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

            input.Player.Press.performed += i => StartHold();
            input.Player.Press.canceled += i => StopHold();

        }

        input.Enable();
    }

    private void StartHold()
    {
        PlayerManager player = GameManager.Instance.GetCurPlayerManager();
        player.isHold = true;
        player.isDecreaseDis = false;
        GameUIManager.Instance.ShowAttackRate();
    }

    private void StopHold()
    {
        PlayerManager player = GameManager.Instance.GetCurPlayerManager();
        player.isHold = false;
        player.InstantiatBullet(player.curBullet);
        GameUIManager.Instance.HideAttackRate();
        player.curDis = 0;
    }


    private void OnDisable()
    {
        input.Disable();
    }

}
