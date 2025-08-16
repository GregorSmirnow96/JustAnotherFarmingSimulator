using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MaterialCountIcon : MonoBehaviour
{
    public GameObject quantityIconContainer;

    private Inventory inventory;
    private TMP_Text quantityText;
    private string itemId;
    private int quantity;
    private Dictionary<string, int> itemQuantityDict;

    void Start()
    {
        inventory = Inventory.instance;

        quantityText = quantityIconContainer.GetComponent<TMP_Text>();

        quantity = 0;
    }

    void Update()
    {
        if (itemId != null && itemQuantityDict != null)
        {
            bool playerHasRequiredQuantity = inventory.PlayerHasItems(itemQuantityDict);
            quantityText.color =
                playerHasRequiredQuantity
                    ? Color.white
                    : Color.red;
        }
    }

    public void SetItemQuantity(string itemId, int quantity)
    {
        this.itemId = itemId;

        this.quantity = quantity;
        
        if (quantityText == null)
        {
            quantityText = quantityIconContainer.GetComponent<TMP_Text>();
        }
        quantityText.text = quantity.ToString();

        itemQuantityDict = new Dictionary<string, int>()
        {
            { itemId, quantity }
        };
    }
}
