using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : MonoBehaviour
{
    public GameObject heartPrefab;
    public int healthPerHeart = 20;

    private Health playerHealth;
    private int maxHealth;
    private int previousMaxHealth;

    void Start()
    {
        playerHealth = SceneProperties.playerTransform.GetComponent<Health>();

        maxHealth = playerHealth.maxHealth;
        previousMaxHealth = maxHealth;

        SetHearts();

        playerHealth.RegisterHealthChangeCallback(UpdateGraphics);
        playerHealth.RegisterMaxHealthChangeCallback(UpdateGraphics);
    }

    private void UpdateGraphics()
    {
        maxHealth = playerHealth.maxHealth;

        if (previousMaxHealth != maxHealth)
        {
            SetHearts();
        }

        int currentHealth = Mathf.Max(playerHealth.health, 0);
        int fullHearts = (int) currentHealth / healthPerHeart;

        float healthPerQuarterHeart = (float) ((float) healthPerHeart / 4.0f);
        float overflowHealth = (float) (currentHealth % healthPerHeart);
        int quarters = (int) Math.Ceiling(overflowHealth / healthPerQuarterHeart);

        for (int i = 0; i < fullHearts; i++)
        {
            transform
                .GetChild(i)
                .GetComponent<HeartSprite>()
                .SetFilledQuarters(4);
        }

        if (fullHearts < maxHealth / healthPerHeart)
        {
            transform
                .GetChild(fullHearts)
                .GetComponent<HeartSprite>()
                .SetFilledQuarters(quarters);
        }

        if (quarters > 0)
        {
            int firstEmptyHeartIndex = fullHearts + 1;
            for (int i = fullHearts + 1; i < maxHealth / healthPerHeart; i++)
            {
                transform
                    .GetChild(i)
                    .GetComponent<HeartSprite>()
                    .SetFilledQuarters(0);
            }
        }


        previousMaxHealth = maxHealth;
    }

    private void SetHearts()
    {
        if (maxHealth % healthPerHeart != 0)
        {
            Debug.Log($"The player's health isn't a multiple of {healthPerHeart} (healthPerHeart). This could lead to unexpected behaviour.");
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int heartCount = (int) maxHealth / healthPerHeart;
        for (int i = 0; i < heartCount; i++)
        {
            Instantiate(heartPrefab, transform);
        }
    }
}
