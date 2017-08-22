using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class Shop : MonoBehaviour
{
    /// <summary>The GUI for the shop.</summary>
    public Transform GuiShop;

    /// <summary>Is the shop GUI open.</summary>
    public bool IsOpen { get; private set; }

    /// <summary>The number of items left in stock.</summary>
    public int Count { get { return shopInventory.Count; } }

    private List<Item> shopInventory = new List<Item>();

    private Inventory playerInventory;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerInventory = FindObjectOfType<Player>().GetComponent<Inventory>();
        shopInventory = GetComponentsInChildren<Item>(true).ToList();

        for(int i = 0; i < shopInventory.Count; ++i)
        {
            Transform child = GuiShop.GetChild(i + 1);
            child.GetComponent<Image>().sprite = shopInventory[i].Icon;
            child.GetComponentInChildren<Text>().text = "COST: " + shopInventory[i].ShopCost;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>())
            player.NearInteractableObject = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.E))
            ToggleGuiShop();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Player>())
            player.NearInteractableObject = false;
    }

    /// <summary>Toggles the visibility of the GUI Shop.</summary>
    public void ToggleGuiShop()
    {
        IsOpen = !GuiShop.gameObject.activeInHierarchy;
        GuiShop.gameObject.SetActive(IsOpen);
        player.Shopping = !player.Shopping;
        player.StopMovement();
    }

    /// <summary>Purchases the selected item, if the player can afford it.</summary>
    /// <param name="index">The index of the item in the shop.</param>
    public void BuyItem(int index)
    {
        Item item = shopInventory[index];
        if(playerInventory.Money < item.ShopCost)
            return;

        playerInventory.AddItem(item);
        playerInventory.RemoveMoney(item.ShopCost);
        RemoveItem(item);
    }

    /// <summary>Removes an item from the shop.</summary>
    /// <param name="item">The item to remove from the shop.</param>
    public void RemoveItem(Item item)
    {
        shopInventory.Remove(item);
        for(int i = 0; i < GuiShop.childCount - 1; ++i)
        {
            Transform child = GuiShop.GetChild(i + 1);
            bool inRange = i < shopInventory.Count;
            child.GetComponent<Image>().sprite = inRange ? shopInventory[i].Icon : item.BlankIcon;
            child.GetComponentInChildren<Text>().text = inRange ? "COST: " + shopInventory[i].ShopCost : "";
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
}
