using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waterable : MonoBehaviour, IInteractable
{
    private ModularInteractText interactText;
    private WaterContainers waterContainers;
    private Toolbar playerToolbar;
    private IWaterable growthScript;
    private GameObject waterLevelIcon;
    private WaterLevelIcon waterLevelIndicator;

    void Start()
    {
        interactText = GetComponent<ModularInteractText>();
        interactText.SetText("'E' to Water");
        waterContainers = WaterContainers.GetInstance();
        playerToolbar = Toolbar.instance;
        growthScript = GetComponent<IWaterable>();

        GameObject waterLevelIconPrefab = Resources.Load<GameObject>("Plants/WaterLevelUI/WaterLevelPanel");
        GameObject mainCanvas = GameObject.Find("UICanvas");
        waterLevelIcon = Instantiate(waterLevelIconPrefab, mainCanvas.transform);

        waterLevelIndicator = waterLevelIcon.GetComponent<WaterLevelIcon>();
        waterLevelIndicator.waterableTransform = gameObject.transform;
    }

    void Update()
    {
        waterLevelIndicator.waterLevel =
            growthScript.IsWatered()
            ? 1f
            : 0f;
    }

    public void Interact()
    {
        Debug.Log(playerToolbar == null);
        Item item = playerToolbar.GetEquippedItem();
        string itemId = item.type.id;
        bool isFullContainer = waterContainers.ItemIsFullContainer(itemId);
        if (isFullContainer && !growthScript.IsWatered())
        {
            ContainerMap containerMap = waterContainers.GetContainerMap(itemId);
            string emptyItemId = containerMap.emptyItemId;
            Item emptyItem = new Item(emptyItemId);
            int equippedItemIndex = playerToolbar.equippedItemIndex;
            playerToolbar.SetInventorySlot(equippedItemIndex, emptyItem);

            growthScript.AddWater();
        }
    }

    public void ShowIndicator()
    {
        if (playerToolbar != null)
        {
            Item item = playerToolbar.GetEquippedItem();
            string itemId = item?.type.id;
            bool isFullContainer = waterContainers.ItemIsFullContainer(itemId);
            if (isFullContainer)
            {
                interactText.Enable();
            }
            else
            {
                interactText.Disable();
            }

            WaterLevelIcon waterIconScript = waterLevelIcon.GetComponent<WaterLevelIcon>();
            waterIconScript.ShowIndicator();
        }
    }

    public void HideIndicator()
    {
        interactText.Disable();
        if (waterLevelIcon != null)
        {
            WaterLevelIcon waterIconScript = waterLevelIcon.GetComponent<WaterLevelIcon>();
            waterIconScript.HideIndicator();
        }
    }

    void OnDestroy()
    {
        Destroy(waterLevelIcon);
    }
}
