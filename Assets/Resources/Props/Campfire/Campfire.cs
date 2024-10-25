using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour, IFueled, IInteractable
{
    public ParticleSystem alphaParticleSystem;
    public ParticleSystem addParticleSystem;
    public ParticleSystem glowParticleSystem;
    public ParticleSystem sparksParticleSystem;
    public Light light;
    public float remainingFuel = 2f;

    private bool on;

    private ModularInteractText interactText;

    void Start()
    {
        interactText = GetComponent<ModularInteractText>();
        TurnOn();
    }

    void Update()
    {
        remainingFuel -= Time.deltaTime;
        if (remainingFuel <= 0)
        {
            remainingFuel = 0;

            TurnOff();
        }
    }
    
    public void AddFuel(float hoursOfFuel)
    {
        remainingFuel += hoursOfFuel;
    }

    public float GetRemainingFuelTime()
    {
        return remainingFuel;
    }

    public void TurnOn()
    {
        on = true;
        light.gameObject.SetActive(true);
        alphaParticleSystem.Play();
        addParticleSystem.Play();
        glowParticleSystem.Play();
        sparksParticleSystem.Play();
    }

    public void TurnOff()
    {
        on = false;
        light.gameObject.SetActive(false);
        alphaParticleSystem.Stop();
        addParticleSystem.Stop();
        glowParticleSystem.Stop();
        sparksParticleSystem.Stop();
    }

    public void Interact()
    {
        Item equippedItem = Toolbar.instance.GetEquippedItem();
        Debug.Log(equippedItem?.type?.id);
        if (equippedItem?.type.id == null)
        {
            Debug.Log("Cannot add fuel since a fuel source isn't equipped.");
            return;
        }

        Fuel fuelInfo = Fuel.GetInstance();
        string itemId = equippedItem.type.id;
        if (fuelInfo.ItemIsFuel(itemId))
        {
            float fuelStrength = fuelInfo.GetItemFuelStrength(itemId);
            remainingFuel += fuelStrength;
            Toolbar.instance.DeleteEquippedItem();
        }
    }

    public void ShowIndicator()
    {
        Item equippedItem = Toolbar.instance.GetEquippedItem();
        bool itemIsFuel = Fuel.GetInstance().ItemIsFuel(equippedItem?.type.id);
        if (itemIsFuel)
        {
            interactText.Enable();
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
}
