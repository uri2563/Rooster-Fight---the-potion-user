using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LogButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public PotionSlot potionSlot;
    private PopUpWindow popUp;

    private void Start()
    {
        popUp = GameUIManager.Instance.popUpWindowPlay;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        popUp.PopUp(potionSlot.GetScriptablePotion());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        popUp.PopDown();
    }
}
