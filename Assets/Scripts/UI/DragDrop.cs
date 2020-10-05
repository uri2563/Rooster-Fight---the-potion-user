using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler,IEndDragHandler, IDragHandler, IDropHandler
{
    public ScriptalbePotion thisPotion;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject slot;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private PopUpWindow popUp;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        popUp = GameUIManager.Instance.popUpWindowShop;
    }

    public void SetSlot(ScriptalbePotion scriptalbePotion)
    {
        thisPotion = scriptalbePotion;
        GetComponent<Image>().sprite = scriptalbePotion.icon;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
        slot.transform.parent.gameObject.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        transform.localPosition = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        popUp.PopUp(thisPotion);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            ScriptalbePotion otherPotion = eventData.pointerDrag.GetComponent<DragDrop>().thisPotion;
            eventData.pointerDrag.GetComponent<DragDrop>().SetSlot(thisPotion);
            SetSlot(otherPotion);
        }
    }

}
