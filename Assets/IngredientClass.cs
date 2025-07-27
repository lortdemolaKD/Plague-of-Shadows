using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Ingredient Class", menuName = "Item/Ingredient")]
public class IngredientClass : ItemClass
{
    /*[Header("Ingredient")]
    public IngredientType consumableType;
    public enum IngredientType
    {
        Lava,
        Pond,
        Creature,
        Soul,
        Moonlight
    }*/
    public override ItemClass GetItem() {return this;}
    public override ConsumableClass GetConsumable() { return null; }
    public override IngredientClass GetIngredient() { return this; }
}
