using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Transform GuiInventory;
    public Image GuiEquippedItem;
    public Text MoneyText;

	// Boolean array to store the collected pieces, length of three, the index refers to the piece number
	public bool[] doorPieces;

    public bool IsOpen { get; private set; }

    public int Count { get { return inventory.Count; } }

    public int Money { get; private set; }

    public Item EquippedItem { get; private set; }

    private List<Item> inventory = new List<Item>();

    private int inventoryLimit;

    private void Awake()
    {
		//Set the door piece booleans to false, the player hasn't collected them yet.
		doorPieces = new bool[3];

        inventoryLimit = GuiInventory.childCount;
    }

    /// <summary>Toggles the visibility of the GUI Inventory.</summary>
    public void ToggleGuiInventory()
    {
        IsOpen = !GuiInventory.gameObject.activeInHierarchy;
        GuiInventory.gameObject.SetActive(IsOpen);
    }

    /// <summary>Sets an item from the player's inventory as the currently equipped item.</summary>
    /// <param name="index">The index of the item in the inventory.</param>
    public void EquipItem(int index)
    {
        if(index >= inventory.Count)
            return;

        EquippedItem = GetItem(index);
        GuiEquippedItem.sprite = EquippedItem.Icon;
    }

    /// <summary>Adds an item to the player's inventory.</summary>
    /// <param name="item">The item to add to the inventory.</param>
    public void AddItem(Item item)
    {
        if(inventory.Count == inventoryLimit)
            return;

        inventory.Add(item);
        item.gameObject.SetActive(false);

        Sort(item);
    }

    /// <summary>Removes an item from the player's inventory.</summary>
    /// <param name="item">The item to remove from the inventory.</param>
    public void RemoveItem(Item item)
    {
        inventory.Remove(item);

        Sort(item);

        if(item.CompareTag(EquippedItem.tag))
        {
            EquippedItem = inventory.Find(e => e.CompareTag(EquippedItem.tag));
            if(EquippedItem == null)
                GuiEquippedItem.sprite = item.BlankIcon;
        }
    }

    /// <summary>Retrieves an item from the player's inventory.</summary>
    /// <param name="index">The index of the item in the player's inventory.</param>
    /// <returns>The item at the given index in the player's inventory.</returns>
    public Item GetItem(int index)
    {
        if(index < inventory.Count)
            return inventory[index];

        return null;
    }

    /// <summary>Clear's the player's inventory.</summary>
    public void Clear()
    {
        inventory.Clear();
    }

    /// <summary>Sorts the player's inventory.</summary>
    private void Sort(Item item)
    {
        inventory = inventory.OrderBy(i => i.TypeId).ToList();

        for(int i = 0; i < GuiInventory.childCount - 1; ++i)
            GuiInventory.GetChild(i + 1).GetComponentInChildren<Image>().sprite = i < inventory.Count ? inventory[i].Icon : item.BlankIcon;
    }

    /// <summary>Increases the amount of money the player has.</summary>
    /// <param name="value">The amount of money to give to the player.</param>
    public void AddMoney(int value)
    {
        Money += value;
        MoneyText.text = Money.ToString();
    }

    /// <summary>Decreases the amount of money the player has.</summary>
    /// <param name="value">The amount of money to take from the player.</param>
    public void RemoveMoney(int value)
    {
        Money -= value;
        MoneyText.text = Money.ToString();
    }
}
