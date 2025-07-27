using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newCraftingRecipe", menuName ="Crafting/Recipe")]
public class CraftingRecipeClass : ScriptableObject
{
    [Header("Crafting Recipe")]
    public SlotClass[] inputItems;
    public SlotClass outputItem;

    public bool canCraft(InventoryManager inventory)
    {
        //bool canCraft = true;
        if (inventory.isFull())
            return false;

        for (int i = 0; i < inputItems.Length; i++)
        {
            if (!inventory.Contains(inputItems[i].GetItem(), inputItems[i].GetQuantity()))
            {
                return false;
            }
        }

        //return if inv has input items
        return true;
    }

    public void Craft(InventoryManager inventory)
    {
        //remove input items from inventory
        for (int i = 0; i < inputItems.Length; i++)
        {
            inventory.Remove(inputItems[i].GetItem(), inputItems[i].GetQuantity());
        }
        //add output item to inv

        inventory.Add(outputItem.GetItem(), outputItem.GetQuantity());

        }
    public SlotClass GetoutputItem()
    {
        return outputItem;
    }
    public SlotClass[] GetinputItems()
    {
        return inputItems;
    }
}
