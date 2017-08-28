using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
public GameObject puzzleDoor;

    public Transform GuiInventory;
    public Transform GuiMissingDoorPieces;
    public Transform GuiCollectedDoorPieces;
    public Sprite[] GuiCollectedPieceImages;

    public Image GuiEquippedItem;
    public Text MoneyText;

	public bool IsOpen { get; private set; }

    public int Count { get { return inventory.Count; } }

    public int Money { get; private set; }

    public Item EquippedItem { get; private set; }

    private List<Item> inventory = new List<Item>();
    private Image[] guiInventorySlots;
    private CollectableDoorPiece[] collectablePieces;

    private int inventoryLimit;

    private void Awake()
    {
        inventoryLimit = GuiInventory.childCount;
        guiInventorySlots = GuiInventory.GetChild(1).GetComponentsInChildren<Image>();
        collectablePieces = FindObjectsOfType<CollectableDoorPiece>();
		ToggleGuiInventory(false);
    }

    private void Update()
    {
        foreach(CollectableDoorPiece piece in collectablePieces)
            if(piece.gameObject.activeInHierarchy)
                return;

        puzzleDoor.SetActive(false);
    }

    /// <summary>Toggles the visibility of the GUI Inventory.</summary>
    public void ToggleGuiInventory()
    {
        IsOpen = !GuiInventory.gameObject.activeInHierarchy;
        GuiInventory.gameObject.SetActive(IsOpen);
    }
	
	/// <summary>Sets the visibility of the GUI Inventory.</summary>
	/// <param name="state">controls the state of the inventory, on (true) and off (false)</param>
    public void ToggleGuiInventory(bool state)
    {
        IsOpen = state;
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

        for(int i = 0; i < guiInventorySlots.Length; ++i)
            guiInventorySlots[i].sprite = i < inventory.Count ? inventory[i].Icon : item.BlankIcon;
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

    /// <summary>Sets the door piece as having been collected in the inventory.</summary>
    /// <param name="piece">The piece that has been collected.</param>
    public void AddDoorPiece(CollectableDoorPiece.DoorPiece piece)
    {
        switch(piece)
        {
            case CollectableDoorPiece.DoorPiece.Frame:
                GuiMissingDoorPieces.GetChild(0).gameObject.SetActive(false);
                GuiCollectedDoorPieces.GetChild(0).GetComponent<Image>().sprite = GuiCollectedPieceImages[0];
                break;
            case CollectableDoorPiece.DoorPiece.Panel:
                GuiMissingDoorPieces.GetChild(1).gameObject.SetActive(false);
                GuiCollectedDoorPieces.GetChild(1).GetComponent<Image>().sprite = GuiCollectedPieceImages[1];
                break;
            case CollectableDoorPiece.DoorPiece.Knob:
                GuiMissingDoorPieces.GetChild(2).gameObject.SetActive(false);
                GuiCollectedDoorPieces.GetChild(2).GetComponent<Image>().sprite = GuiCollectedPieceImages[2];
                break;
        }
    }
}
