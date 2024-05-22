using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{

    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] Animator anim;

    public static Menu main;

    private bool isHoveringMenu;

    private bool isMenuOpen = true;

    private void Awake()
    {
        main = this;
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    private void OnGUI()
    {
        currencyUI.text = LevelManager.main.currency.ToString();
    }

    public void SetSelected()
    {

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
