using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Consumable Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    /*[Header("Consumable")]
    public float villagerHealthAdded;
    public ConsumableType consumableType;
    public enum ConsumableType
    {
        full,
        half,
        shadow
    }*/
    public override ItemClass GetItem() {return this;}
    public override ConsumableClass GetConsumable() { return this; }
    public override IngredientClass GetIngredient() { return null; }
}
