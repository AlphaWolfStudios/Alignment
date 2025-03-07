﻿using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Crafting : MonoBehaviour
{

    public Action<Item> OnCraft;

    Inventory inventory = null;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    public void Craft(Item item)
    {
        bool craftFailed = false;
        if(IsCraftPosible(item))
        {
            foreach (var ingredient in item.recipe)
            {
                inventory.RemoveItem(ingredient.item, ingredient.amount);
            }
            if(inventory.CanItemBeAdded(item.id, 1)) 
            {
                inventory.AddNewItem(item.id, 1);
            }
            else 
            {
                foreach (var ingredient in item.recipe)
                {
                    inventory.AddNewItem(ingredient.item.id, ingredient.amount);
                }
                craftFailed = true;
            }
        }
        else
        {
            craftFailed = true;
        }
        if (craftFailed)
        {
            AkSoundEngine.PostEvent(AK.EVENTS.UICRAFTFAIL, gameObject);
        }
        else 
        {
            OnCraft?.Invoke(item);
            AkSoundEngine.PostEvent(AK.EVENTS.UICRAFTSUCCESSFUL, gameObject);
        }
    }
    public bool IsCraftPosible(Item item)
    {
        int itemsAmount = 0;
        if (item.recipe.Count > 0) 
        {
            foreach (var ingredient in item.recipe)
            {
                if (ingredient.item)
                {
                    if (inventory.CheckForItem(ingredient.item, ingredient.amount))
                    {
                        itemsAmount++;
                    }
                }
                else
                {
                    Debug.LogWarning("Se jodió: " + item.itemName);
                }
            }
        }
        else
        {
            Debug.LogWarning("No se cargó la lista.");
        }
        return itemsAmount == item.recipe.Count;
    }

    public int PosibleCraftAmount(Item item) 
    {
        int posibleCrafts = 0;
        bool nextCraftPosible;
        do
        {
            nextCraftPosible = false;
            int itemsAmount = 0;
            foreach (var ingredient in item.recipe)
            {
                if (ingredient.item)
                {
                    if (inventory.CheckForItem(ingredient.item, ingredient.amount * (posibleCrafts + 1)))
                    {
                        itemsAmount++;
                    }
                }
                else
                {
                    Debug.LogWarning("Se jodió: " + item.itemName);
                }
            }
            if (itemsAmount == item.recipe.Count)
            {
                nextCraftPosible = true;
                posibleCrafts++;
            }
        } while (nextCraftPosible);
        return posibleCrafts;
    }

}