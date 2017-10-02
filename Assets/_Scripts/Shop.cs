using GameLogging;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class Shop : MonoBehaviour
{
    /// <summary>The GUI for the shop.</summary>
    public Transform GuiShop;

    public ToolTip tooltip;

    public AudioClip[] CoinPurchases;

    /// <summary>Is the shop GUI open.</summary>
    public bool IsOpen { get; private set; }

    /// <summary>The number of items left in stock.</summary>
    public int Count { get { return shopInventory.Count; } }

    private List<Item> shopInventory = new List<Item>();

    private Player player;
    private Inventory playerInventory;
    private Shopkeeper shopkeep;
    private new AudioSource audio;

    private float shopOpenTime;
	private float shopOpenRate;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();

        player = FindObjectOfType<Player>();
        playerInventory = FindObjectOfType<Player>().GetComponent<Inventory>();
        shopInventory = GetComponentsInChildren<Item>(true).ToList();

        BuildDebug.Log("SHOP INVENTORY\n");
        foreach(Item item in shopInventory)
            BuildDebug.Log(item.name);
        BuildDebug.Log("\n");

        IsOpen = false;

        shopkeep = FindObjectOfType<Shopkeeper>();

        Image[] guiSlots = GuiShop.GetChild(1).GetComponentsInChildren<Image>();
        BuildDebug.Log("Shop slot count = " + guiSlots.Length, true);
        for(int i = 0; i < shopInventory.Count; ++i)
        {
            guiSlots[i].sprite = shopInventory[i].Icon;
            guiSlots[i].color = new Color(255, 255, 255, 255);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        BuildDebug.Log("Shop trigger entered by " + other.name);
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
        BuildDebug.Log("Shop trigger exited by " + other.name);
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
        BuildDebug.Log("GUI shop toggled to: " + !IsOpen);
        IsOpen = !IsOpen;
        GuiShop.gameObject.SetActive(IsOpen);
        player.Shopping = !player.Shopping;
        player.StopMovement();

		if(!IsOpen)
			tooltip.DisplayNull();
    }
		
	/// <summary>Sets the visibility of the GUI Shop.</summary>
	/// <param name="state"> determines whether the UI is open (true) or closed (false).</param>
	public void ToggleGuiShop(bool state)
    {
        BuildDebug.Log("GUI shop toggled to: " + state);
        IsOpen = state;
		GuiShop.gameObject.SetActive(IsOpen);
		player.Shopping = state;
		player.StopMovement(state);

		if(!state)
			tooltip.DisplayNull();
	}


    /// <summary>Purchases the selected item, if the player can afford it.</summary>
    /// <param name="index">The index of the item in the shop.</param>
    public void BuyItem(int index)
    {
        Item item = shopInventory[index];
        BuildDebug.Log("Purchase attempt for \"" + item.name + "\" from shop slot " + index);
        if(playerInventory.Money < item.ShopCost)
        {
            BuildDebug.Log("Purchase failed, not enough funds. Player money " + playerInventory.Money + " vs Cost " + item.ShopCost);
            return;
        }

        if(playerInventory.AddItem(item))
        {
            BuildDebug.Log("Purchase successful. Cost: " + item.ShopCost);
            playerInventory.RemoveMoney(item.ShopCost);
            RemoveItem(item);
            if(CoinPurchases.Length > 0)
            {
                audio.clip = CoinPurchases[Random.Range(0, CoinPurchases.Length)];
                audio.Play();
            }
            shopkeep.PlayPurchase();
        }
        else
            BuildDebug.Log("Purchase failed, player inventory full");
    }

    /// <summary>Removes an item from the shop.</summary>
    /// <param name="item">The item to remove from the shop.</param>
    public void RemoveItem(Item item)
    {
        BuildDebug.Log("Removing item from shop: " + item.name);
        shopInventory.Remove(item);
        Transform shopSlots = GuiShop.GetChild(1);
        BuildDebug.Log("UPDATING GUI SLOTS", true);
        for(int i = 0; i < shopSlots.childCount; ++i)
        {
            Image slot = shopSlots.GetChild(i).GetComponent<Image>();
            BuildDebug.Log("Slot before: " + slot.sprite.name + " " + slot.color, true);
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
            BuildDebug.Log("Slot after: " + slot.sprite.name + " " + slot.color, true);
        }
    }

    /// <summary>Retrieves an item from the shop.</summary>
    /// <param name="index">The index of the item in the shop.</param>
    /// <returns>The item at the given index in the shop.</returns>
    public Item GetItem(int index)
    {
        BuildDebug.Log("Retrieving shop item at index " + index);
        if(index < shopInventory.Count)
            return shopInventory[index];

        return null;
    }

    public void ShowToolTip(int slot)
    {
        BuildDebug.Log("Showing tooltip for shop index " + slot, true);
        if(slot < shopInventory.Count)
            tooltip.Display(shopInventory[slot].ShopTooltip);
        else
            tooltip.Display(null);
    }

    public void HideToolTip()
    {
        BuildDebug.Log("Hiding currently visible tooltip", true);
        tooltip.Display(null);
    }
}
