using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeMenuHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData _eventData)
    {
        MenuManager.main.SetHoveringState(true);
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        MenuManager.main.SetHoveringState(false);
        gameObject.SetActive(false);
    }

}
