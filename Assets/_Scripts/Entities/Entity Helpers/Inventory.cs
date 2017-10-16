using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Transform GuiInventory;
    public Transform GuiMissingDoorPieces;
    public Transform GuiCollectableDoorPieces;
    public Sprite[] GuiCollectedPieceImages;

    public Image GuiEquippedItem;
    public Text MoneyText;

    public ToolTip tooltip;

    public bool[] DoorPieces { get { return doorPieceCollected; } }

    public bool IsOpen { get; private set; }

    public int Count { get { return inventory.Count; } }

    public int Money { get; private set; }

    public Item EquippedItem { get; private set; }

    private Text guiUseButton;
    private Text guiNumEquipped;

    private List<Item> inventory = new List<Item>();
    //private Image[] guiInventorySlots;

    private DoorPiece[] collectablePieces;
	private bool[] doorPieceCollected;
    
    private int inventoryLimit;

    private void Awake()
    {
        //guiInventorySlots = GuiInventory.GetChild(1).GetComponentsInChildren<Image>();
        inventoryLimit = 5;// guiInventorySlots.Length;
        collectablePieces = FindObjectsOfType<DoorPiece>();
		ToggleGuiInventory(false);

		doorPieceCollected = new bool[collectablePieces.Length];

        guiUseButton = GuiEquippedItem.transform.GetChild(0).GetComponent<Text>();
        guiNumEquipped = GuiEquippedItem.transform.GetChild(1).GetComponent<Text>();
    }

    /// <summary>Toggles the visibility of the GUI Inventory.</summary>
    public void ToggleGuiInventory()
    {
        IsOpen = !GuiInventory.gameObject.activeInHierarchy;
        GuiInventory.gameObject.SetActive(IsOpen);
        if(!IsOpen)
            HideToolTip();
    }
	
	/// <summary>Sets the visibility of the GUI Inventory.</summary>
	/// <param name="state">controls the state of the inventory, on (true) and off (false)</param>
    public void ToggleGuiInventory(bool state)
    {
        IsOpen = state;
        GuiInventory.gameObject.SetActive(IsOpen);
        if(!IsOpen)
            HideToolTip();
    }

    /// <summary>Sets an item from the player's inventory as the currently equipped item.</summary>
    /// <param name="index">The index of the item in the inventory.</param>
    public void EquipItem(int index)
    {
        if(index >= inventory.Count)
            return;

        EquippedItem = GetItem(index);
        GuiEquippedItem.sprite = EquippedItem.Icon;
        GuiEquippedItem.color = new Color(255, 255, 255, 255);
        guiUseButton.enabled = true;
        guiNumEquipped.enabled = true;
    }

    /// <summary>Adds an item to the player's inventory.</summary>
    /// <param name="item">The item to add to the inventory.</param>
    /// <returns>True if successful, false otherwise.</returns>
    public bool AddItem(Item item)
    {
        if(inventory.Count == inventoryLimit)
            return false;
        
        inventory.Add(item);
        item.gameObject.SetActive(false);
		item.collected = true;

        if(!EquippedItem)
            EquipItem(inventory.Count - 1);

        guiNumEquipped.text = inventory.Where(i => i.TypeId == EquippedItem.TypeId).ToArray().Length.ToString();
        //Sort(item);

        return true;
    }

    /// <summary>Removes an item from the player's inventory.</summary>
    /// <param name="item">The item to remove from the inventory.</param>
    public void RemoveItem(Item item)
    {
        inventory.Remove(item);

        //Sort(item);

        if(item.CompareTag(EquippedItem.tag))
        {
            guiNumEquipped.text = inventory.Where(i => i.TypeId == EquippedItem.TypeId).ToArray().Length.ToString();
            EquippedItem = inventory.Find(e => e.CompareTag(EquippedItem.tag));
            if(EquippedItem == null)
            {
                GuiEquippedItem.sprite = null;
                GuiEquippedItem.color = new Color(255, 255, 255, 0);
                guiUseButton.enabled = false;
                guiNumEquipped.enabled = false;
            }
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
    //private void Sort(Item item)
    //{
    //    inventory = inventory.OrderBy(i => i.TypeId).ToList();

    //    //for(int i = 0; i < guiInventorySlots.Length; ++i)
    //    //{
    //    //    if(i < inventory.Count)
    //    //    {
    //    //        guiInventorySlots[i].sprite = inventory[i].Icon;
    //    //        guiInventorySlots[i].color = new Color(255, 255, 255, 255);
    //    //    }
    //    //    else
    //    //    {
    //    //        guiInventorySlots[i].sprite = null;
    //    //        guiInventorySlots[i].color = new Color(255, 255, 255, 0);
    //    //    }
    //    //}
    //}

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
    public void AddDoorPiece(DoorPiece.PieceType piece)
    {
        switch(piece)
        {
            case DoorPiece.PieceType.Frame:
                GuiMissingDoorPieces.GetChild(0).gameObject.SetActive(false);
                GuiCollectableDoorPieces.GetChild(0).GetComponent<Image>().sprite = GuiCollectedPieceImages[0];
                doorPieceCollected[0] = true;
                break;
            case DoorPiece.PieceType.Panel:
                GuiMissingDoorPieces.GetChild(1).gameObject.SetActive(false);
                GuiCollectableDoorPieces.GetChild(1).GetComponent<Image>().sprite = GuiCollectedPieceImages[1];
                doorPieceCollected[1] = true;
                break;
            case DoorPiece.PieceType.Knob:
                GuiMissingDoorPieces.GetChild(2).gameObject.SetActive(false);
                GuiCollectableDoorPieces.GetChild(2).GetComponent<Image>().sprite = GuiCollectedPieceImages[2];
                doorPieceCollected[2] = true;
                break;
        }
    }

    public void ShowToolTip(int slot)
    {
        if(slot < inventory.Count)
            tooltip.Display(inventory[slot].InventoryTooltip);
        else
            tooltip.Display(null);
    }

    public void HideToolTip()
    {
        tooltip.Display(null);
    }
}
