using System.Collections;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    [Header("Item")]
    public string itemName;
    public Sprite itemIcon;
    //public bool isStackable = true;
    public int itemId;

    public abstract ItemClass GetItem();
    public abstract ConsumableClass GetConsumable();
    public abstract IngredientClass GetIngredient();

}
