﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class Shop : MonoBehaviour
{
    /// <summary>The GUI for the shop.</summary>
    public Transform GuiShop;

    public ToolTip tooltip;

    /// <summary>Is the shop GUI open.</summary>
    public bool IsOpen { get; private set; }

    /// <summary>The number of items left in stock.</summary>
    public int Count { get { return shopInventory.Count; } }

    private List<Item> shopInventory = new List<Item>();

    private Player player;
    private Inventory playerInventory;
    private float shopOpenTime;
	private float shopOpenRate;

    private Shopkeeper shopkeep;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerInventory = FindObjectOfType<Player>().GetComponent<Inventory>();
        shopInventory = GetComponentsInChildren<Item>(true).ToList();
		IsOpen = false;

        shopkeep = FindObjectOfType<Shopkeeper>();

        Image[] guiSlots = GuiShop.GetChild(1).GetComponentsInChildren<Image>();
        for(int i = 0; i < shopInventory.Count; ++i)
        {
            guiSlots[i].sprite = shopInventory[i].Icon;
            guiSlots[i].color = new Color(255, 255, 255, 255);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>())
        {
            player.NearInteractableObject = true;
            shopkeep.PlayGreeting();
        }
    }

    private void OnTriggerStay(Collider other)
    {
		if (other.GetComponent<Player> ()) {
			if (!playerInventory.IsOpen && Input.GetKeyDown (KeyCode.E) && Time.time > shopOpenTime) {
				ToggleGuiShop ();
				shopOpenRate = Time.deltaTime;
				shopOpenTime = Time.time + shopOpenRate;
			}
		}
    }

    private void OnTriggerExit(Collider other)
    {
		if (other.GetComponent<Player> ()) {
			player.NearInteractableObject = false;
			// Sometimes the Shop inventory stays open after an E press, and if the character moves out of the trigger, pressing E won't close it
			ToggleGuiShop (false);
            shopkeep.PlayGoodbye();
        }
    }

    /// <summary>Toggles the visibility of the GUI Shop.</summary>
    public void ToggleGuiShop()
    {
        IsOpen = !IsOpen;
        GuiShop.gameObject.SetActive(IsOpen);
        player.Shopping = !player.Shopping;
        player.StopMovement();
    }
		
	/// <summary>Sets the visibility of the GUI Shop.</summary>
	/// <param name="state"> determines whether the UI is open (true) or closed (false).</param>
	public void ToggleGuiShop(bool state)
	{
		IsOpen = state;
		GuiShop.gameObject.SetActive(IsOpen);
		player.Shopping = state;
		player.StopMovement(state);
	}


    /// <summary>Purchases the selected item, if the player can afford it.</summary>
    /// <param name="index">The index of the item in the shop.</param>
    public void BuyItem(int index)
    {
        Item item = shopInventory[index];
        if(playerInventory.Money < item.ShopCost)
            return;

        if(playerInventory.AddItem(item))
        {
            playerInventory.RemoveMoney(item.ShopCost);
            RemoveItem(item);
            shopkeep.PlayPurchase();
        }
    }

    /// <summary>Removes an item from the shop.</summary>
    /// <param name="item">The item to remove from the shop.</param>
    public void RemoveItem(Item item)
    {
        shopInventory.Remove(item);
        Transform shopSlots = GuiShop.GetChild(1);
        for(int i = 0; i < shopSlots.childCount; ++i)
        {
            Image slot = shopSlots.GetChild(i).GetComponent<Image>();
            if(i < shopInventory.Count)
            {
                slot.sprite = shopInventory[i].Icon;
                slot.color = new Color(255, 255, 255, 255);
            }
            else
            {
                slot.sprite = null;
                slot.color = new Color(255, 255, 255, 0);
            }
        }
    }

    /// <summary>Retrieves an item from the shop.</summary>
    /// <param name="index">The index of the item in the shop.</param>
    /// <returns>The item at the given index in the shop.</returns>
    public Item GetItem(int index)
    {
        if(index < shopInventory.Count)
            return shopInventory[index];

        return null;
    }

    public void ShowToolTip(int slot)
    {
        if(slot < shopInventory.Count)
            tooltip.Display(shopInventory[slot].ShopTooltip);
        else
            tooltip.Display(null);
    }

    public void HideToolTip()
    {
        tooltip.Display(null);
    }
}
