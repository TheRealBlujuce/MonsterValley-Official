using UnityEngine;

[CreateAssetMenu(fileName ="Item", menuName = "Item/Create New Material Item")]
public class Material : ScriptableObject, Item
{

    [SerializeField] private string itemName;
    [SerializeField] [TextArea] private string itemDescription;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private ItemType firstItemType;
    [SerializeField] private ItemType secondItemType;
    
    public string GetItemName()
    {
        return itemName;
    }

    public Sprite GetItemSprite()
    {
        return itemSprite;
    }
    
    public ItemType GetFirstItemType()
    {
        return firstItemType;
    }

    public ItemType GetSecondItemType()
    {
        return secondItemType;
    }

    public void PerformItemAction()
    {
        // perform the action of this type of item. Foodd items are used for taming and cooking.
    }



}
