using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    public GameObject towerObj;
    public Turret turret;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        // If menu is open, return and don't make it posible for user to place towers.
        if (MenuManager.main.IsHoveringMenu()) return;

        // If there is a turret, open the upgrade menu
        if (towerObj != null)
        {
            turret.OpenUpgradeMenu();
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("Cant afford");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        // TODO: Fix sprites
        // Manually update new position because I messed up the sprites.
        Vector3 position = transform.position;
        position.y += 0.15f;
        position.x -= 0.06f;

        towerObj = Instantiate(towerToBuild.prefab, position, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();
    }

}
