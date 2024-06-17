using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Item
{
    public string GetItemName();
    public Sprite GetItemSprite();
    public ItemType GetFirstItemType();
    public ItemType GetSecondItemType();
    public void PerformItemAction();
}

public enum ItemType 
{
    None,
    Healing,
    Food,
    Material,
    Key_Item
}