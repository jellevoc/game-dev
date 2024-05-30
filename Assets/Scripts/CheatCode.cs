using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCode : MonoBehaviour
{
    private KeyCode[] secretCode;
    private int codeIndex;

    void Start()
    {
        //1984a
        secretCode = new KeyCode[] {
            KeyCode.Alpha1,
            KeyCode.Alpha9,
            KeyCode.Alpha8,
            KeyCode.Alpha4,
            KeyCode.A
        };
        codeIndex = 0;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            // If key from code is pressed
            if (Input.GetKeyDown(secretCode[codeIndex]))
            {
                codeIndex++;
                // If right key is pressed.
                if (codeIndex >= secretCode.Length)
                {
                    OnSecretCodeEntered();
                    codeIndex = 0;
                }
            }
            // Reset index of wrong key is pressed.
            else
            {
                codeIndex = 0;
            }
        }
    }

    void OnSecretCodeEntered()
    {
        LevelManager.main.IncreaseCurrency(1984);
    }
}
