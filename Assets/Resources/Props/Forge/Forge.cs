using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemMetaData;

public class Forge : MonoBehaviour, IInteractable
{
    public Transform forgedItemContainer;
    public float timeToSmelt = 10f;
    public ModularInteractText interactText;

    private Toolbar toolbar;
    private SmeltableItems smeltableItems;
    private ItemTypeRepo itemTypeRepo;
    private GameObject objectBeingSmelted;
    private string itemIdBeingSmelted;
    private GameObject smeltedObject;
    private string smeltedItemId;

    private bool containsItem => objectBeingSmelted != null || smeltedObject != null;
    private bool canPlaceItem => smeltableItems.ItemIdIsSmeltable(toolbar.GetEquippedItem()?.type.id) && !containsItem;
    private bool canPickUpItem => smeltedObject != null;

    void Start()
    {
        interactText = GetComponent<ModularInteractText>();
        interactText.SetText("'E' to Smelt");

        toolbar = Toolbar.instance;

        smeltableItems = SmeltableItems.GetInstance();
        itemTypeRepo = ItemTypeRepo.GetInstance();
    }

    public void Interact()
    {
        Item equippedItem = toolbar.GetEquippedItem();
        string equippedItemId = equippedItem?.type.id;
        bool equippedItemIsSmeltable = smeltableItems.ItemIdIsSmeltable(equippedItemId);

        if (canPickUpItem)
        {
            PickUpItem();
        }
        else if (canPlaceItem)
        {
            PlaceItem();
        }
    }

    private void PickUpItem()
    {
        if (!Inventory.instance.IsFull())
        {
            Item smeltedItem = new Item(smeltedItemId);

            Inventory.instance.AddItemToInventory(smeltedItem);

            Destroy(smeltedObject);
            smeltedObject = null;
            itemIdBeingSmelted = null;
            interactText.SetText("'E' to Smelt");
        }
    }

    private void PlaceItem()
    {
        Item itemBeingSmelted = toolbar.GetEquippedItem();
        itemIdBeingSmelted = itemBeingSmelted.type.id;
        GameObject itemBeingSmeltedPrefab = itemBeingSmelted.type.groundItemPrefab;

        objectBeingSmelted = Instantiate(itemBeingSmeltedPrefab, forgedItemContainer);
        Vector3 parentScale = forgedItemContainer.lossyScale;
        objectBeingSmelted.transform.localScale = new Vector3(
            objectBeingSmelted.transform.localScale.x / parentScale.x,
            objectBeingSmelted.transform.localScale.y / parentScale.y,
            objectBeingSmelted.transform.localScale.z / parentScale.z);

        Destroy(objectBeingSmelted.GetComponent<GroundItem>());

        toolbar.DeleteEquippedItem();

        StartCoroutine(Smelt());
    }

    private IEnumerator Smelt()
    {
        yield return new WaitForSeconds(timeToSmelt);

        smeltedItemId = smeltableItems.GetSmeltedItemId(itemIdBeingSmelted);
        Debug.Log($"smeltedItemId: {smeltedItemId}");
        ItemType smeltedItemType = itemTypeRepo.TryFindItemType(smeltedItemId);
        Debug.Log($"smeltedItemType.id: {smeltedItemType.id}");
        GameObject smeltedObjectPrefab = smeltedItemType.groundItemPrefab;

        smeltedObject = Instantiate(smeltedObjectPrefab, forgedItemContainer);
        Vector3 parentScale = forgedItemContainer.lossyScale;
        smeltedObject.transform.localScale = new Vector3(
            smeltedObject.transform.localScale.x / parentScale.x,
            smeltedObject.transform.localScale.y / parentScale.y,
            smeltedObject.transform.localScale.z / parentScale.z);

        Destroy(smeltedObject.GetComponent<GroundItem>());

        Destroy(objectBeingSmelted);
        itemIdBeingSmelted = null;
        objectBeingSmelted = null;
        interactText.SetText("'E' to Take");
    }

    public void ShowIndicator()
    {
        if (canPlaceItem || canPickUpItem)
        {
            if (interactText == null)
            {
                Destroy(gameObject);
            }
            else
            {
                interactText.Enable();
            }
        }
        else
        {
            HideIndicator();
        }
    }

    public void HideIndicator()
    {
        interactText.Disable();
    }

    void OnDestroy()
    {
        Destroy(interactText.textObject);
    } 
}
