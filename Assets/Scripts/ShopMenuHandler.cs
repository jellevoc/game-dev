using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopMenuHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData _eventData)
    {
        Menu.main.SetHoveringState(true);
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        Menu.main.SetHoveringState(false);
    }

}
