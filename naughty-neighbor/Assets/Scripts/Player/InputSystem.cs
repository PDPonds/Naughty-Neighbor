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
        Debug.Log("Start Touch");
        player.isHold = true;

    }

    private void StopHold()
    {
        PlayerManager player = GameManager.Instance.GetCurPlayerManager();
        player.isHold = false;
        Debug.Log(player.curDis);
        player.curDis = 0;
    }


    private void OnDisable()
    {
        input.Disable();
    }

}
