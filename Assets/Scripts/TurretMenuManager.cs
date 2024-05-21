using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager main;

    private bool isHoveringMenu;

    private void Awake()
    {
        main = this;
    }

    public void SetHoveringState(bool _state)
    {
        isHoveringMenu = _state;
    }

    public bool IsHoveringMenu()
    {
        return isHoveringMenu;
    }

}
