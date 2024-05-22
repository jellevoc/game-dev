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
        if (PauseMenu.isPaused) return;
        if (gameObject.tag == "EnemyTile")
        {
            sr.color = occupiedPlotHoverColor;
            return;
        }
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        // If either of these is true, don't make it posible for user to place turrets.
        if (MenuManager.main.IsHoveringMenu() || Menu.main.IsHoveringMenu()
        || PauseMenu.isPaused || gameObject.tag == "EnemyTile") return;


        // If there is a turret, open the upgrade menu
        if (towerObj != null)
        {
            turret.OpenTurretMenu();
            return;
        }


        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            MessageHandler.main.ShowMessage();
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);


        // Fix torret position on plot
        Vector3 position = transform.position;
        position.y += 0.5f;


        towerObj = Instantiate(towerToBuild.prefab, position, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();
        // if (turret == null)
        // {
        //     Debug.Log("here");
        //     turret = towerObj.GetComponent<TurretSlowmo>();
        // }
    }

}
