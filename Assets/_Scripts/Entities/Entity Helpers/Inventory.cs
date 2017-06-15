using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Transform GuiInventory;
    public Image GuiEquippedItem;
    public Text MoneyText;

    public bool IsOpen { get; private set; }

    public int Count { get { return inventory.Count; } }

    public int Money { get; private set; }

    public Item EquippedItem { get; private set; }

    private List<Item> inventory = new List<Item>();

    private int inventoryLimit;

    private void Awake()
    {
        inventoryLimit = GuiInventory.childCount;
    }

    /// <summary>
    /// Toggles the visibility of the GUI Inventory.
    /// </summary>
    public void ToggleGuiInventory()
    {
        IsOpen = !GuiInventory.gameObject.activeInHierarchy;
        GuiInventory.gameObject.SetActive(IsOpen);
    }

    public void EquipItem(int index)
    {
        if(index >= inventory.Count)
            return;

        EquippedItem = GetItem(index);
        GuiEquippedItem.sprite = EquippedItem.Icon;
    }

    public void AddItem(Item item)
    {
        if(inventory.Count == inventoryLimit)
            return;

        inventory.Add(item);
        item.gameObject.SetActive(false);

        GuiInventory.GetChild(inventory.Count).GetComponentInChildren<Image>().sprite = item.Icon;
    }

    public void RemoveItem(Item item)
    {
        inventory.Remove(item);
        for(int i = 1; i < GuiInventory.childCount; ++i)
            GuiInventory.GetChild(i).GetComponentInChildren<Image>().sprite = i-1 < inventory.Count ? inventory[i-1].Icon : item.BlankIcon;

        if(item.CompareTag(EquippedItem.tag))
        {
            EquippedItem = inventory.Find(e => e.CompareTag(EquippedItem.tag));
            if(EquippedItem == null)
                GuiEquippedItem.sprite = item.BlankIcon;
        }
    }

    public Item GetItem(int index)
    {
        if(index < inventory.Count)
            return inventory[index];

        return null;
    }

    public void Clear()
    {
        inventory.Clear();
    }

    public void AddMoney(int value)
    {
        Money += value;
        MoneyText.text = Money.ToString();
    }
}
