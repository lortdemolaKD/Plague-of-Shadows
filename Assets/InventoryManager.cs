using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<CraftingRecipeClass> craftingRecipes = new List<CraftingRecipeClass>();
    [SerializeField] private List<ItemClass> allitems = new List<ItemClass>();
    [SerializeField] private GameObject itemCursor;

    [SerializeField] private GameObject slotHolder;
    [SerializeField] private GameObject craftingSlotHolder;
    [SerializeField] private GameObject givingSlotHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;

    [SerializeField] private SlotClass[] startingItems;

    public SlotClass[] items;
    public SlotClass[] itemsInCrafting;
    public SlotClass[] itemGiving;
    public GameObject recipelist;
    public GameObject recipe;
    public Canvas canvas;

    private GameObject[] slots;
    private GameObject[] craftingSlots;
    private GameObject[] giveSlot;

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;
    public int selected_recepie;
    bool isMovingItem;

    private void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        craftingSlots = new GameObject[craftingSlotHolder.transform.childCount];
        giveSlot = new GameObject[givingSlotHolder.transform.childCount];
        items = new SlotClass[slots.Length];
        itemsInCrafting = new SlotClass[craftingSlots.Length];
        itemGiving = new SlotClass[giveSlot.Length];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }
        for (int i = 0; i < itemGiving.Length; i++)
        {
            itemGiving[i] = new SlotClass(); 
        }
        for (int i = 0; i < itemsInCrafting.Length; i++)
        {
            itemsInCrafting[i] = new SlotClass();
        }
        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }
        for (int i = 0; i < craftingRecipes.Count; i++)
        {
            GameObject temp = Instantiate(recipe);
            temp.transform.SetParent(recipelist.transform, false);
            temp.transform.GetChild(2).GetComponent<buttonSetup>().CReate(gameObject,i);
            CraftingRecipeClass recipeTemp = craftingRecipes[i];
            SlotClass slottemp = recipeTemp.GetoutputItem();
            ItemClass Itemp = slottemp.GetItem();
            temp.transform.GetChild(2).GetComponent<Button>().image.sprite = Itemp.itemIcon;
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Itemp.itemName;
            String Stemp = "";
            foreach (SlotClass item in recipeTemp.inputItems)
            {
                ItemClass It = item.GetItem();
                Stemp += It.itemName + " ,";
            }
            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Stemp;

            //trigger.onClick.//;

        }

        // set all the slots
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < craftingSlotHolder.transform.childCount; i++)
        {
            craftingSlots[i] = craftingSlotHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < givingSlotHolder.transform.childCount; i++)
        {
            giveSlot[i] = givingSlotHolder.transform.GetChild(i).gameObject;
        }
        RefreshUI();
        //Add(itemToAdd, 1);
       //Remove(itemToRemove);

    }
    public void selectRec(int i)
    {
        selected_recepie = i;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            Craft(craftingRecipes[selected_recepie]);

        itemCursor.SetActive(isMovingItem);
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = canvas.planeDistance;
        Vector3 tempSlot = Camera.main.ScreenToWorldPoint(screenPos);
        itemCursor.transform.position = Input.mousePosition;
        if (isMovingItem)
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;

        if (Input.GetMouseButtonDown(0))
        {
            if (isMovingItem)
            {
                EndItemMove();
            }
            else
            {
                BeginItemMove();
            }
        }
        //left click to split stack in half or similar
    }

    #region Inv Utils
    public void RefreshUI()
    {
        for (int i = 0;i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                //if (items[i].GetItem().isStackable)
                slots[i].transform.GetChild(1).GetComponent<Text>().text = items[i].GetQuantity().ToString() + "";
                //else
                    //slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            try
            {
                craftingSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                craftingSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = itemsInCrafting[i].GetItem().itemIcon;
                //if (items[i].GetItem().isStackable)
                craftingSlots[i].transform.GetChild(1).GetComponent<Text>().text = itemsInCrafting[i].GetQuantity().ToString() + "";
                //else
                //slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            catch
            {
                craftingSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                craftingSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                craftingSlots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }
        for (int i = 0; i < giveSlot.Length; i++)
        {
            try
            {
                giveSlot[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                giveSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = itemGiving[i].GetItem().itemIcon;
                //if (items[i].GetItem().isStackable)
                giveSlot[i].transform.GetChild(1).GetComponent<Text>().text = itemGiving[i].GetQuantity().ToString() + "";
                //else
                //slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            catch
            {
                giveSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                giveSlot[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                giveSlot[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }

}

public bool Add(ItemClass item, int quantity)
    {
        //items.Add(item);
        //chek if there's this item in inventory
        SlotClass slot = Contains(item);
        if (slot != null /*&& slot.GetItem().isStackable*/)
        {
            slot.AddQuantity(quantity);
        }
        else
        {
            for(int i = 0; i < slots.Length; i++)
            {
                if (items[i].GetItem() == null)
                {
                    items[i].AddItem(item, quantity);
                    break;
                }
            }
          /*  if (items.Count < slots.Length)
            {
                items.Add(new SlotClass(item, 1));
            }
            else
                return false;*/
        }

        RefreshUI();
        return true;
    }

    public void Add(ItemClass item)
    {
        //items.Add(item);
        //chek if there's this item in inventory
        SlotClass slot = Contains(item);
        if (slot != null)
        {
            slot.AddQuantity(1);
        }
        else
        {
            items.Append(new SlotClass(item, 1));
        }

        RefreshUI();
    }
    public void AddList(List<Tuple<int, int>> list)
    {
        foreach (var item in list)
        {
            Debug.Log(item.Item1);
           
            if (allitems.Find(eri => eri.itemId == item.Item1) != null)
            {
                Debug.Log(allitems.Find(eri => eri.itemId == item.Item1));
                Add(allitems.Find(eri => eri.itemId == item.Item1),item.Item2);
            }
            
        }

        RefreshUI();
    }


    public bool Remove(ItemClass item)
    {
        //items.Remove(item);
        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
                temp.SubQuantity(1);
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < itemsInCrafting.Length; i++)
                {
                    if (itemsInCrafting[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                itemsInCrafting[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }



        RefreshUI();
        return true;
    }
    public bool RemoveOUT(ItemClass item)
    {
        //items.Remove(item);
        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
                temp.SubQuantity(1);
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < itemGiving.Length; i++)
                {
                    if (itemGiving[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                itemGiving[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }



        RefreshUI();
        return true;
    }
    public bool Remove(ItemClass item, int quantity)
    {
        //items.Remove(item);
        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
                temp.SubQuantity(quantity);
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < itemsInCrafting.Length; i++)
                {
                    if (itemsInCrafting[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                itemsInCrafting[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }



        RefreshUI();
        return true;
    }

    public SlotClass Contains(ItemClass item)
    {
        for (int i = 0; i < itemsInCrafting.Length; i ++)
        {
            if (itemsInCrafting[i].GetItem() == item) 
                return itemsInCrafting[i];
        }

        return null;
    }

    public bool Contains(ItemClass item, int quantity) //for crafting recipes
    {
        for (int i = 0; i < itemsInCrafting.Length; i++)
        {
            if (itemsInCrafting[i].GetItem() == item && itemsInCrafting[i].GetQuantity() >= quantity)
                return true;
        }

        return false;
    }
    /*public SlotClass Contains(ItemClass item)
    {
        foreach (SlotClass slot in items)
        {
            if (slot.GetItem()  == item) 
                return slot;
        }

        return null;
    }*/

    private void Craft(CraftingRecipeClass recipe)
    {
        if (recipe.canCraft(this))
        {
            recipe.Craft(this);
        }
        else
        {
            Debug.Log("Can't craft that item");
        }
    }

    public bool isFull()
    {
        for (int i = 0; i < items.Length; ++i)
        {
            if (items[i].GetItem() == null)
            {
                return false;
            }
        }
        return true;
    }

    #endregion Inv Utils

    #region Moving stuff
    private bool BeginItemMove()
    {
        originalSlot = (GetClosestSlot());
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }

        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();
            return true;
    }

    private bool EndItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            Add(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {
            if (originalSlot.GetItem() != null)
            {
                if (originalSlot.GetItem() == movingSlot.GetItem()) //same item
                {
                    //if (originalSlot.GetItem().isStackable)
                    //{
                    originalSlot.AddQuantity(movingSlot.GetQuantity());
                    movingSlot.Clear();
                    // }
                    /*else
                     {
                         return false;
                     }*/
                }
                else
                {
                    tempSlot = new SlotClass(originalSlot);
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity());

                    RefreshUI();
                    return true;
                }
            }

            else //place as usual
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }

            isMovingItem = false;
            RefreshUI();
            return true;
        
    }

    public ItemClass getGiveItem()
    {
        return itemGiving[0].GetItem();
    }
    private SlotClass GetClosestSlot()
    {
        for (int i = 0; i < giveSlot.Length; i++)
        {
            

            if (Vector2.Distance(giveSlot[i].transform.position, Input.mousePosition) <= 30f)
            {
                // Debug.Log(slots[i].transform.position + "  " + Input.mousePosition / canvas.scaleFactor + "  " + tempSlot);
                return itemGiving[i];
            }
        }
        for (int i = 0; i < craftingSlots.Length; i++)
        {

            Vector3 screenPos = Input.mousePosition;
            screenPos.z = canvas.planeDistance;
            Vector2 tempSlot = Camera.main.ScreenToWorldPoint(screenPos);

            if (Vector2.Distance(craftingSlots[i].transform.position, Input.mousePosition) <= 30f)
            {
                
                return itemsInCrafting[i];
            }
        }
        for (int i = 0; i < slots.Length; i ++ )
        {

            Vector3 screenPos = Input.mousePosition;
            screenPos.z = canvas.planeDistance;
            Vector2 tempSlot  = Camera.main.ScreenToWorldPoint(screenPos);
           
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 30f)
            {
                
                return items[i];
            }
        }

        return null;
    }
    #endregion
}
