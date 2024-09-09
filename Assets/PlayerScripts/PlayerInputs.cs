using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private static PlayerInputs Instance;
    public static PlayerInputs GetInstance() =>
        Instance == null
            ? Instance = GameObject.Find("Player").GetComponent<PlayerInputs>()
            : Instance;

    public Dictionary<string, KeyCode> actionKeyCodes;
    public Dictionary<string, Action> gameplayListeners = new Dictionary<string, Action>();
    public Dictionary<string, Action> inventoryListeners = new Dictionary<string, Action>();

    private Inventory playerInventory;

    public const string INTERACT = "INTERACT";
    public const string USE_ITEM = "USE_ITEM";
    public const string TOGGLE_INVENTORY = "TOGGLE_INVENTORY";
    public const string FORCE_CLOSE_INVENTORY = "FORCE_CLOSE_INVENTORY";

    void Start()
    {
        actionKeyCodes = new Dictionary<string, KeyCode>()
        {
            { INTERACT, KeyCode.E },
            { USE_ITEM, KeyCode.Mouse0 },
            { TOGGLE_INVENTORY, KeyCode.I },
            { FORCE_CLOSE_INVENTORY, KeyCode.Escape }
        };

        playerInventory = GetComponent<Inventory>();
    }

    void Update()
    {
        foreach (var kvp in actionKeyCodes)
        {
            string action = kvp.Key;
            KeyCode keyCode = kvp.Value;

            Dictionary<string, Action> activeActionSet =
                playerInventory.inventoryOpen
                    ? inventoryListeners
                    : gameplayListeners;

            if (Input.GetKeyDown(keyCode))
            {
                if (activeActionSet.Keys.Contains(action))
                {
                    Action actionToPerform = activeActionSet[action];
                    actionToPerform();
                }
                else
                {
                    // Debug.Log($"No function is mapped to {action}.");
                }
            }
        }
    }

    public void RegisterGameplayAction(string actionName, Action callback)
    {
        gameplayListeners.Add(actionName, callback);
    }

    public void RegisterInventoryAction(string actionName, Action callback) => inventoryListeners.Add(actionName, callback);

    public void UnregisterGameplayAction(string actionName, Action callback)
    {
        if (gameplayListeners[actionName] == callback)
        {
            gameplayListeners.Remove(actionName);
        }
        else
        {
            Debug.Log("Didn't unregister the callback as it's already been overwritten.");
        }
    }

    public void UnregisterInventoryAction(string actionName, Action callback)
    {
        if (inventoryListeners[actionName] == callback)
        {
            inventoryListeners.Remove(actionName);
        }
        else
        {
            Debug.Log("Didn't unregister the callback as it's already been overwritten.");
        }
    }
}
