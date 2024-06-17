using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory : MonoBehaviour
{
    // four lists are needed to hold all the items by item type.
    [SerializeField] private List<HealingItemEntry> healingItemList;
    [SerializeField] private List<FoodItemEntry> foodItemList;
    [SerializeField] private List<MaterialItemEntry> materialItemList;
    [SerializeField] private List<KeyItemEntry> keyItemList;

    
    public void AddItemToInventory(Item item)
    {
        switch(item.GetFirstItemType())
        {
            case ItemType.Healing:
                if (item is Healing healItem)
                {
                    bool itemInList = false;
                    foreach(var entry in healingItemList)
                    { 
                        if (entry.item == healItem) { itemInList = true; entry.quantity++;} 
                    }

                    if (!itemInList) { healingItemList.Add(new HealingItemEntry{item = healItem, quantity = 1});}
                }
            break;
            case ItemType.Food:
                if (item is Food foodItem)
                {
                    bool itemInList = false;
                    foreach(var entry in foodItemList)
                    { 
                        if (entry.item == foodItem) { itemInList = true; entry.quantity++;} 
                    }

                    if (!itemInList) { foodItemList.Add(new FoodItemEntry{item = foodItem, quantity = 1});}
                }
            break;
            case ItemType.Material:
                if (item is Material materialItem)
                {
                    bool itemInList = false;
                    foreach(var entry in materialItemList)
                    { 
                        if (entry.item == materialItem) { itemInList = true; entry.quantity++;} 
                    }

                    if (!itemInList) { materialItemList.Add(new MaterialItemEntry{item = materialItem, quantity = 1});}
                }
            break;
            case ItemType.Key_Item:
                if (item is KeyItem keyItem)
                {
                    bool itemInList = false;
                    foreach(var entry in keyItemList)
                    { 
                        if (entry.item == keyItem) { itemInList = true; entry.quantity++;} 
                    }

                    if (!itemInList) { keyItemList.Add(new KeyItemEntry{item = keyItem, quantity = 1});}
                }
            break;
        }
        
    }

    // Getters for the different inventory lists
    public List<HealingItemEntry> GetHealingItemList()
    {
        return healingItemList;
    }
    public List<FoodItemEntry> GetFoodItemList()
    {
        return foodItemList;
    }
    public List<MaterialItemEntry> GetMaterialItemList()
    {
        return materialItemList;
    }
    public List<KeyItemEntry> GetKeyItemList()
    {
        return keyItemList;
    }

}

[Serializable]
public class HealingItemEntry
{
    public Healing item;
    public int quantity;
}

[Serializable]
public class FoodItemEntry
{
    public Food item;
    public int quantity;
}

[Serializable]
public class MaterialItemEntry
{
    public Material item;
    public int quantity;
}

[Serializable]
public class KeyItemEntry
{
    public KeyItem item;
    public int quantity;
}