using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    void Start()
    {
        PlayerInputs playerInputs = PlayerInputs.GetInstance();

        playerInputs.RegisterGameplayAction(
            PlayerInputs.USE_ITEM,
            UseEquippedItem);
        
        /*
            TODO: Pick up here!
                Remove the EquippedItemBehaviour script + its references in Toolbar/Inventory.
                    Just use this approach to have items be usable in more than 1 way with more than 1 key.
                    Implement a new script just like this one that has the commented out code here / down below.
                    It will be like UseItem.cs but for items that let you cast spells. This will rely on a new interface: ISpellProvider
                    The ISpellProvider interface will be used for items that provide any number of spells. Its functions must make sense for items that have 1, 2, 3, or 4 spells.
            
                The UI widget that indicates which spells can be cast / their cooldowns can listen to the RenderEquippedItem.instance.renderedItem property.
                    When the equipped item doesn't have a CastSpell component (to be implemented as said above), the widget can disappear.
                    When the CastSpell component *is* present, render the widget. The widget render logic can be driven by public properties attached to the CastSpell script.
                        These properties can be:
                            1) spellCount
                            2) spellSprites
                            3) spellCooldowns
                            4) spellCosts
                        etc.

                The spells must have animations which is technically trivial.
                    These animations are just particle effects being triggered (as seen in the EquippedItemBehaviour script that I'm going to remove).
                    This becomes less trivial when we need to tie in the animation of the item that is used to trigger the spell.
                    This doesn't have to be much more complex, though. Handle the chain of animations (item animation -> spell animation) in the item's SpellUse script.
                    The use method for each spell can handle checking the prerequisites to cast the spell (such as cooldown) and just return early / do nothing if they aren't met.
                    
                    *IMPORTANT!!!*
                        I almsot forgot. This is incredibly important and will (maybe) be the hardest part of the spell animations.
                        We must add a hitbox (collider) that animates along side the particle effect, and this hitbox will determine whether the spell hits something.
                        Maybe this hitbox can be one of the sprites spawned by the particle effect? Probably not, though. Look into this.
                        If we can't achieve this the easy way with the preexisting sprites, we can just create our own collider / animate it to line up with the VFX.

                Cooldown info can simply be stored in the script. The cast logic can use this as well as the UI widget. Yay!
        */
        playerInputs.RegisterGameplayAction(
            PlayerInputs.CAST_SPELL_1,
            UseEquippedItemSpell1);
        playerInputs.RegisterGameplayAction(
            PlayerInputs.CAST_SPELL_2,
            UseEquippedItemSpell2);
        //playerInputs.RegisterGameplayAction(
        //    PlayerInputs.CAST_SPELL_3,
        //    UseEquippedItemSpell3);
        //playerInputs.RegisterGameplayAction(
        //    PlayerInputs.CAST_SPELL_4,
        //    UseEquippedItemSpell4);
    }

    private void UseEquippedItem()
    {
        GameObject renderedItem = RenderEquippedItem.instance.renderedItem;
        if (renderedItem == null) return;

        IUsable usableItem = renderedItem.GetComponent<IUsable>();

        if (usableItem == null)
        {
            usableItem = renderedItem.GetComponentInChildren<IUsable>();
        }

        if (usableItem != null)
        {
            usableItem.Use();
        }
    }

    private void UseEquippedItemSpell1()
    {
        GameObject renderedItem = RenderEquippedItem.instance.renderedItem;
        if (renderedItem == null) return;

        ISpellProvider spellProvider = renderedItem.GetComponent<ISpellProvider>();

        if (spellProvider == null)
        {
            Debug.Log("Spell 1 was not cast since there is no ISpellProvider component on the equipped item");
        }
        else
        {
            spellProvider.CastSpell1();
        }
    }

    private void UseEquippedItemSpell2()
    {
        GameObject renderedItem = RenderEquippedItem.instance.renderedItem;
        if (renderedItem == null) return;

        ISpellProvider spellProvider = renderedItem.GetComponent<ISpellProvider>();

        if (spellProvider == null)
        {
            Debug.Log("Spell 2 was not cast since there is no ISpellProvider component on the equipped item");
        }
        else
        {
            spellProvider.CastSpell2();
        }
    }

    private void UseEquippedItemSpell3()
    {
        GameObject renderedItem = RenderEquippedItem.instance.renderedItem;
        if (renderedItem == null) return;

        ISpellProvider spellProvider = renderedItem.GetComponent<ISpellProvider>();

        if (spellProvider == null)
        {
            Debug.Log("Spell 3 was not cast since there is no ISpellProvider component on the equipped item");
        }
        else
        {
            spellProvider.CastSpell3();
        }
    }

    private void UseEquippedItemSpell4()
    {
        GameObject renderedItem = RenderEquippedItem.instance.renderedItem;
        if (renderedItem == null) return;

        ISpellProvider spellProvider = renderedItem.GetComponent<ISpellProvider>();

        if (spellProvider == null)
        {
            Debug.Log("Spell 4 was not cast since there is no ISpellProvider component on the equipped item");
        }
        else
        {
            spellProvider.CastSpell4();
        }
    }
}
