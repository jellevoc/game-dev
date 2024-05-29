using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurrencyHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Attributes")]
    [SerializeField] private GameObject currencyMenu;
    [SerializeField] private GameObject bitcoinAddressMenu;

    public void OnPointerEnter(PointerEventData _eventData)
    {
        currencyMenu.SetActive(false);
        bitcoinAddressMenu.SetActive(true);
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        currencyMenu.SetActive(true);
        bitcoinAddressMenu.SetActive(false);
    }
}
