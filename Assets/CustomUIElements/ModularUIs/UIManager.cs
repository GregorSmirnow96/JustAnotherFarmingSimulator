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
    // Alchemy Menu
    public GameObject alchemyMenuPrefab;
    private GameObject alchemyMenuUI;
    private RectTransform alchemyMenuTransform;

    public bool menuIsOpen =>
        inventoryUI != null && inventoryUI.active
        || craftingMenuUI != null && craftingMenuUI.active
        || alchemyMenuUI != null && alchemyMenuUI.active;
        // Add new UIs here!!!

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
                ActivateToolbar();
            }
            else
            {
                DeactivateCraftingMenu();
                DeactivateAlchemyMenu();
                DeactivateToolbar();
                ActivateInventory();
            }
        };
        inputs.RegisterGameplayAction(PlayerInputs.TOGGLE_INVENTORY, toggleInventoryAction);
        inputs.RegisterInventoryAction(PlayerInputs.TOGGLE_INVENTORY, toggleInventoryAction);

        Action closeUIsAction = () => {
            DeactivateCraftingMenu();
            DeactivateInventory();
            DeactivateAlchemyMenu();
            // Close other UIs!
            ActivateToolbar();
        };
        inputs.RegisterInventoryAction(PlayerInputs.FORCE_CLOSE_UIS, closeUIsAction);

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

        alchemyMenuUI = Instantiate(alchemyMenuPrefab, transform);
        alchemyMenuTransform = alchemyMenuUI.GetComponent<RectTransform>();
        alchemyMenuUI.SetActive(false);
    }

    // Inventory
    public void ActivateInventory()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerController.enabled = false;

        inventoryUI.SetActive(true);
    }

    public void DeactivateInventory()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerController.enabled = true;

        inventoryUI.SetActive(false);
    }

    // Toolbar
    public void ActivateToolbar()
    {
        toolbarUI.SetActive(true);
    }

    public void DeactivateToolbar()
    {
        toolbarUI.SetActive(false);
    }

    // Crafting Menu
    public void ActivateCraftingMenu()
    {
        DeactivateToolbar();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerController.enabled = false;

        craftingMenuUI.SetActive(true);
    }

    public void DeactivateCraftingMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerController.enabled = true;

        craftingMenuUI.SetActive(false);
    }

    // Alchemy Menu
    public void ActivateAlchemyMenu()
    {
        DeactivateToolbar();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerController.enabled = false;

        alchemyMenuUI.SetActive(true);
    }

    public void DeactivateAlchemyMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerController.enabled = true;

        alchemyMenuUI.SetActive(false);
    }
}
