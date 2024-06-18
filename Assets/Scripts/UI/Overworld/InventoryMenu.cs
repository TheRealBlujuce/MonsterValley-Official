using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] PartyMenu partyMenu;
    [SerializeField] TextMeshProUGUI inventoryTitle;
    [SerializeField] private GameObject inventoryHolder;
    [SerializeField] private GameObject inventoryItem;
    [SerializeField] private Color highlightColor;

    public event Action OnMenuClose;
    public int inventorySpace = 20;
    private bool canCloseMenu = false; // this is to make sure we can actually close the menu after a bit of delay.
    [SerializeField] private int currentItem = 0; // int for item selection
    [SerializeField] private int currentItemList = 0; // int for item selection
    private int currentPemo = 0; // int for pemo selection
    public List<ItemEntry> itemList;
    private List<ItemType> inventoryType = new List<ItemType>() {ItemType.Healing, ItemType.Food, ItemType.Material, ItemType.Key_Item};
    private bool hasChangedInventory = false;
    private bool isClearingMenu;
    private bool isSettingInventory = true;
    [SerializeField] private PlayerController player;
    private void Awake()
    {
        Invoke(nameof(SetCloseMenu), 0.15f);
    }

    private void Start()
    {
        player = GameController.gameControllerInstance.GetPlayerController();
    }

    public void UpdatePartyMenuData(PemoParty party)
    {
        partyMenu.SetPartyMenuData(party.GetPemoParty());
    }

#region Setting Up and Updating Inventory
    
    public void InitInventory()
    {
        
        SetCurrentInventory(player.GetComponent<Inventory>(), inventoryType[currentItemList]);
        // Ensure there are enough item objects in the inventory holder
        if(inventoryHolder.transform.childCount < inventorySpace)
        {
            for (int i = 0; i < inventorySpace; i++)
            {
                Instantiate(inventoryItem, inventoryHolder.transform);
            }
        }

        UpdateInventory();
    }

    private void UpdateInventory()
    {

        int maxVisibleItems = 5; // Number of items to show at a time including the selected item

        // Calculate startIndex and endIndex
        int startIndex = Mathf.Clamp(currentItem - maxVisibleItems / 2, 0, Mathf.Max(0, itemList.Count - maxVisibleItems));
        int endIndex = Mathf.Min(startIndex + maxVisibleItems, itemList.Count);

        // Update the data and visibility of each item object
        if (itemList.Count() > 0)
        {
            // Debug.LogWarning("Setting Item Data!");
            for (int i = 0; i < inventoryHolder.transform.childCount; i++)
            {
                var itemObject = inventoryHolder.transform.GetChild(i).gameObject;
                if (i < endIndex - startIndex)
                {
                    var itemEntry = itemList[startIndex + i];
                    itemObject.GetComponent<InventoryItem>().SetItemData(itemEntry.item.GetItemName(), itemEntry.quantity, itemEntry.item.GetItemSprite());
                    itemObject.SetActive(true);
                }
                else
                {
                    itemObject.SetActive(false);
                }
            }
        }
    }

    private void HideInventory()
    {

        if (itemList.Count() <= 0)
        {
            // Debug.LogWarning("Hiding Item Data!");
            for (int i = 0; i < inventoryHolder.transform.childCount; i++)
            {
                var itemObject = inventoryHolder.transform.GetChild(i).gameObject;
                itemObject.SetActive(false);
            }
        }
    }

    public void ClearInventory()
    {
        isClearingMenu = true;
        itemList.Clear();
        isClearingMenu = false;
    }
    public void SetCurrentInventory(Inventory inventory, ItemType type)
    {
        
        if (isSettingInventory) 
        {
            ClearInventory();

            switch(type)
            {
                case ItemType.Healing:
                    if (!hasChangedInventory)
                    {
                        
                        hasChangedInventory = true;
                        itemList = new List<ItemEntry>(inventory.GetHealingItemList().Count);
                        foreach (var entry in inventory.GetHealingItemList())
                        {
                            itemList.Add(new ItemEntry { item = entry.item, quantity = entry.quantity });
                        }
                        inventoryTitle.text = "< Restoratives >";
                    }
                break;
                case ItemType.Food:
                    if (!hasChangedInventory)
                    {
                        
                        hasChangedInventory = true;
                        itemList = new List<ItemEntry>(inventory.GetFoodItemList().Count);
                        foreach (var entry in inventory.GetFoodItemList())
                        {
                            itemList.Add(new ItemEntry { item = entry.item, quantity = entry.quantity });
                        }
                        inventoryTitle.text = "< Food >";
                    }
                break;
                case ItemType.Material:
                    if (!hasChangedInventory)
                    {
                        
                        hasChangedInventory = true;
                        itemList = new List<ItemEntry>(inventory.GetMaterialItemList().Count);
                        foreach (var entry in inventory.GetMaterialItemList())
                        {
                            itemList.Add(new ItemEntry { item = entry.item, quantity = entry.quantity });
                        }
                        inventoryTitle.text = "< Materials > ";
                    }
                break;
                case ItemType.Key_Item:
                    if (!hasChangedInventory)
                    {

                        hasChangedInventory = true;
                        itemList = new List<ItemEntry>(inventory.GetKeyItemList().Count);
                        foreach (var entry in inventory.GetKeyItemList())
                        {
                            itemList.Add(new ItemEntry { item = entry.item, quantity = entry.quantity });
                        }
                        inventoryTitle.text = "< Key Items >";
                    }
                break;
            }

            isSettingInventory = false;
        }
    }

#endregion
    
#region Handeling Menu State
    private void CloseMenu()
    {
        OnMenuClose();
    }
    
    public void SetCloseMenu()
    {
        canCloseMenu = true;
    }

    private void HandleInventoryMenuSelection()
    {
        // control item selection
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentItem++;
            if (currentItem > itemList.Count - 1) { currentItem = 0; }
        }
        else
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentItem--;
            if (currentItem < 0) { currentItem = itemList.Count - 1; }
        }

        // update the current selection in the menu
        UpdateInventorySelection(currentItem);
        
        // Show or Hide the item data.
        if (itemList.Count() > 0 ) { UpdateInventory(); } else { HideInventory(); }

        // control inventory menu selection
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentItemList++;
            hasChangedInventory = false;
            isSettingInventory = true;
            if (currentItemList > inventoryType.Count - 1) { currentItemList = 0; currentItem = 0; }
        }
        else
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentItemList--;
            hasChangedInventory = false;
            isSettingInventory = true;
            if (currentItemList < 0)  { currentItemList = inventoryType.Count -1; currentItem = 0; }
        }

        // set the next inventory
        SetCurrentInventory(player.GetComponent<Inventory>(), inventoryType[currentItemList]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            // perform the action of the selected item. For healing items, it will switch to the party list, for other items, it will perform their action or 
            // show some dialog.
        }
    }

    public void UpdateInventorySelection(int currentItem)
    {
        var itemCount = inventoryHolder.GetComponentsInChildren<InventoryItem>(true);

        int maxVisibleItems = 5; // Number of items to show at a time including the selected item
        int startIndex = Mathf.Clamp(currentItem - maxVisibleItems / 2, 0, Mathf.Max(0, itemList.Count - maxVisibleItems));

        // update the selection feedback of the selected item
        for (int i = 0; i < itemCount.Length; i++)
        {
            int actualIndex = startIndex + i;

            if (i < maxVisibleItems && actualIndex < itemList.Count)
            {
                if (actualIndex == currentItem)
                {
                    itemCount[i].GetItemNameText().color = highlightColor;
                    itemCount[i].GetItemNameText().fontSize = 6f;
                    itemCount[i].GetItemAmountText().color = highlightColor;
                    itemCount[i].GetItemAmountText().fontSize = 6f;
                }
                else
                {
                    itemCount[i].GetItemNameText().color = Color.black;
                    itemCount[i].GetItemNameText().fontSize = 5f;
                    itemCount[i].GetItemAmountText().color = Color.black;
                    itemCount[i].GetItemAmountText().fontSize = 5f;
                }
            }
        }
    }   

#endregion

    public void HandleMenuUpdate()
    {
        HandleInventoryMenuSelection();
        if (Input.GetKeyDown(KeyCode.C) && canCloseMenu){ CloseMenu(); }
    }

}

[Serializable]
public class ItemEntry
{
    public Item item;
    public int quantity;
}