using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using ItemMetaData;

public class RecipeCard : MonoBehaviour, IPointerDownHandler
{
    public static RecipeCard selectedCard;

    public string craftableItemName;
    public Dictionary<string, int> componentQuantities;

    private ItemTypeRepo itemTypeRepo;

    private Image borderColorImage;
    private Image craftableItemImage;
    private Text craftableItemText;
    private Image componentItemImage1;
    private Text componentItemText1;
    private Image componentItemImage2;
    private Text componentItemText2;
    private Image componentItemImage3;
    private Text componentItemText3;
    private Image componentItemImage4;
    private Text componentItemText4;

    public void InitializeUI(
        string craftableItemName,
        Dictionary<string, int> componentQuantities)
    {
        this.craftableItemName = craftableItemName;
        this.componentQuantities = componentQuantities;

        itemTypeRepo = ItemTypeRepo.GetInstance();

        borderColorImage = GetComponent<Image>();
        DisableBorder();

        craftableItemImage = transform.Find("ItemSprite").GetComponent<Image>();
        craftableItemText = transform.Find("ItemName").GetComponent<Text>();

        componentItemImage1 = transform.Find("ComponentSprite1").GetComponent<Image>();
        componentItemText1 = transform.Find("ComponentQuantity1").GetComponent<Text>();
        componentItemImage2 = transform.Find("ComponentSprite2").GetComponent<Image>();
        componentItemText2 = transform.Find("ComponentQuantity2").GetComponent<Text>();
        componentItemImage3 = transform.Find("ComponentSprite3").GetComponent<Image>();
        componentItemText3 = transform.Find("ComponentQuantity3").GetComponent<Text>();
        componentItemImage4 = transform.Find("ComponentSprite4").GetComponent<Image>();
        componentItemText4 = transform.Find("ComponentQuantity4").GetComponent<Text>();

        craftableItemImage.sprite = null;
        craftableItemText.text = string.Empty;
        componentItemImage1.sprite = null;
        componentItemText1.text = string.Empty;
        componentItemImage2.sprite = null;
        componentItemText2.text = string.Empty;
        componentItemImage3.sprite = null;
        componentItemText3.text = string.Empty;
        componentItemImage4.sprite = null;
        componentItemText4.text = string.Empty;

        componentItemImage1.gameObject.SetActive(false);
        componentItemText1.gameObject.SetActive(false);
        componentItemImage2.gameObject.SetActive(false);
        componentItemText2.gameObject.SetActive(false);
        componentItemImage3.gameObject.SetActive(false);
        componentItemText3.gameObject.SetActive(false);
        componentItemImage4.gameObject.SetActive(false);
        componentItemText4.gameObject.SetActive(false);

        // Assuming a recipe won't have more than 4 components. You might regret this.
        ItemType craftableItemType = itemTypeRepo.TryFindItemType(craftableItemName);
        craftableItemImage.sprite = craftableItemType.inventorySprite;
        craftableItemText.text = craftableItemName;

        int componentIndex = 0;
        foreach (var kvp in componentQuantities)
        {
            string itemName = kvp.Key;
            int quanitity = kvp.Value;
            ItemType itemType = itemTypeRepo.TryFindItemType(itemName);
            switch (componentIndex++)
            {
                case 0:
                    componentItemImage1.sprite = itemType.inventorySprite;
                    componentItemText1.text = $"x{quanitity}";
                    componentItemImage1.gameObject.SetActive(true);
                    componentItemText1.gameObject.SetActive(true);
                    break;
                case 1:
                    componentItemImage2.sprite = itemType.inventorySprite;
                    componentItemText2.text = $"x{quanitity}";
                    componentItemImage2.gameObject.SetActive(true);
                    componentItemText2.gameObject.SetActive(true);
                    break;
                case 2:
                    componentItemImage3.sprite = itemType.inventorySprite;
                    componentItemText3.text = $"x{quanitity}";
                    componentItemImage3.gameObject.SetActive(true);
                    componentItemText3.gameObject.SetActive(true);
                    break;
                case 3:
                    componentItemImage4.sprite = itemType.inventorySprite;
                    componentItemText4.text = $"x{quanitity}";
                    componentItemImage4.gameObject.SetActive(true);
                    componentItemText4.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    private void EnableBorder() => SetBorderAlpha(1);

    private void DisableBorder() => SetBorderAlpha(0);

    private void SetBorderAlpha(float newAlpha)
    {
        Color borderColor = borderColorImage.color;
        borderColor.a = newAlpha;
        borderColorImage.color = borderColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (selectedCard != null)
        {
            selectedCard.DisableBorder();
        }

        EnableBorder();
        selectedCard = this;
    }
}
