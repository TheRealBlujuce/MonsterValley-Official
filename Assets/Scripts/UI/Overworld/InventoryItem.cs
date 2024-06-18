using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemAmount;
    [SerializeField] private Image itemSprite;

    public void SetItemData(string name, int amount, Sprite sprite)
    {
        itemName.text = name;
        itemAmount.text = "x " + amount;
        itemSprite.sprite = sprite;
    }

    public TextMeshProUGUI GetItemNameText()
    {
        return itemName;
    }
    public TextMeshProUGUI GetItemAmountText()
    {
        return itemAmount;
    }

    private void Update()
    {
        if (transform.parent == null) { Destroy(this.gameObject); }
    }
}
