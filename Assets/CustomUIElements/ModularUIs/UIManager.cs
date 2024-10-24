using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private FPSMovement playerController;

    // Inventory
    public GameObject inventoryPrefab;
    private GameObject inventoryUI;
    private RectTransform inventoryTransform;
    // Toolbar
    public GameObject toolbarPrefab;
    private GameObject toolbarUI;
    private RectTransform toolbarTransform;
    // Crafting Menu
    public GameObject craftingMenuPrefab;
    private GameObject craftingMenuUI;
    private RectTransform craftingMenuTransform;

    public bool menuIsOpen => inventoryUI != null && inventoryUI.active;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnDeactivatedUIs();

        // Register the buttons used to open and close the inventory.
        // TODO: This will need to be done again for the pause menu
        PlayerInputs inputs = PlayerInputs.GetInstance();
        Action toggleInventoryAction = () => {
            if (inventoryUI.active)
            {
                DeactivateInventory();
                DeactivateCraftingMenu();
                ActivateToolbar(new Vector2(0, 0));
            }
            else
            {
                ActivateInventory(new Vector2(-118, -8));
                ActivateCraftingMenu(new Vector2(270, 10));
                DeactivateToolbar();
            }
        };
        inputs.RegisterGameplayAction(PlayerInputs.TOGGLE_INVENTORY, toggleInventoryAction);
        inputs.RegisterInventoryAction(PlayerInputs.TOGGLE_INVENTORY, toggleInventoryAction);

        // Set the player controller reference so that movement can be halted / started depending on the active UI(s)
        playerController = SceneProperties.playerTransform.gameObject.GetComponent<FPSMovement>();

        // Start with the toolbar visible
        toolbarUI.SetActive(true);
    }

    private void SpawnDeactivatedUIs()
    {
        inventoryUI = Instantiate(inventoryPrefab, transform);
        inventoryTransform = inventoryUI.GetComponent<RectTransform>();
        inventoryUI.SetActive(false);

        toolbarUI = Instantiate(toolbarPrefab, transform);
        toolbarTransform = toolbarUI.GetComponent<RectTransform>();
        toolbarUI.SetActive(false);

        craftingMenuUI = Instantiate(craftingMenuPrefab, transform);
        craftingMenuTransform = craftingMenuUI.GetComponent<RectTransform>();
        craftingMenuUI.SetActive(false);
    }

    // Inventory
    public void ActivateInventory(Vector2 center)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerController.enabled = false;

        inventoryUI.SetActive(true);
        inventoryTransform.anchoredPosition = center;
    }

    public void DeactivateInventory()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerController.enabled = true;

        inventoryUI.SetActive(false);
    }

    // Toolbar
    public void ActivateToolbar(Vector2 center)
    {
        toolbarUI.SetActive(true);
        toolbarTransform.anchoredPosition = center;
    }

    public void DeactivateToolbar()
    {
        toolbarUI.SetActive(false);
    }

    // Crafting Menu
    public void ActivateCraftingMenu(Vector2 center)
    {
        craftingMenuUI.SetActive(true);
        craftingMenuTransform.anchoredPosition = center;
    }

    public void DeactivateCraftingMenu()
    {
        craftingMenuUI.SetActive(false);
    }
}
