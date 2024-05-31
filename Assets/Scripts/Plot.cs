using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Plot : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor = new Color(255, 255, 255, 180);
    [SerializeField] private Color occupiedPlotHoverColor = new Color(190, 0, 0, 180);

    public GameObject towerObj;
    public TowerBase turret;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        // Don't show color change if either conditions are met.
        if (PauseMenu.isPaused || GameOverHandler.main.isGameOver) return;

        // If player hovers over enemypath make it clear that you can't place turret on the path.
        if (gameObject.tag == "EnemyTile")
        {
            sr.color = occupiedPlotHoverColor;
            return;
        }

        Debug.Log("Hover");
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        // If either of these is true, don't make it posible for user to place turrets.
        if (CanHoverOrPlace()) return;


        // If there is a turret, open the upgrade menu
        if (towerObj != null)
        {
            if (turret != null)
            {
                turret.OpenTurretMenu();
                return;
            }
            return;
        }


        // Get selected tower from the shop menu.
        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        // If player doesn't have enough money
        if (towerToBuild.cost > LevelManager.main.currency)
        {
            MessageHandler.main.ShowMessage();
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);


        // Update position on Y axis so it fit's better.
        Vector3 position = transform.position;
        position.y += 0.5f;


        towerObj = Instantiate(towerToBuild.prefab, position, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();
        PlaySFX();
    }

    protected void PlaySFX()
    {
        SFXHandler sfx = SFXHandler.main;
        sfx.src.clip = sfx.sfxTowerPlace;
        sfx.src.Play();
    }

    // Conditions to check if player can place turret.
    private bool CanHoverOrPlace()
    {
        return !!(MenuManager.main.IsHoveringMenu() || Menu.main.IsHoveringMenu()
        || PauseMenu.isPaused || gameObject.tag == "EnemyTile" || GameOverHandler.main.isGameOver);
    }

}
