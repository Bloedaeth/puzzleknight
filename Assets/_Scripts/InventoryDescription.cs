using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class InventoryDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
    public Image GuiEquippedItem;

    public bool isOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse enter");
        isOver = true;
        GuiEquippedItem.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exit");
        isOver = false;
        GuiEquippedItem.enabled = false;
    }
}
