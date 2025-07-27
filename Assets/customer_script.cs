using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;


public class customer_script : MonoBehaviour
{
    public GameObject human;
    public GameObject shadow;

    public GameObject[] lantern;
    public GameObject ite;
    public GameObject btn;
    public Sprite man;
    public Sprite woman;

    public Sprite[] manShadows;
    public Sprite[] womanShadows;
    public InventoryManager inventory;
    bool VillagerIN;
    bool Man;
    private int coutWrong;
    public illness current_illness;

    public enum illness
    {
        Blindness = 0,
        Burns = 1,
        Curse = 2,
        Cold = 3,
        Critical_state = 4,
        Death = 5,
        Dehydration = 6,
        Deep_wounds = 7,
        Fatigue = 8,
        Frost_bite = 9,
        Normal = 10,
        Poisoning = 11,
        Trance = 12
    }
    public enum cure
    {
        Antidote = 1,
        CharmingTincture = 2,
        ColdBrew = 3,
        CoolingPotion = 4,
        CurseLiftingBrew = 5,
        DeathWoker = 6,
        DeephealPotion = 7,
        DefrostingPotion = 8,
        HeatingMixture = 9,
        NeutralizingTonic = 10,
        RefreshingTonic = 11,
        SightEnhancementSerum = 12,
        TitansBrew = 13,
        RejuvenationDrink = 14,
        TranceBreaker = 15,
        VisionRestorationDrops = 16,
        VolcanicTea = 17,
        WarmingDrink = 18
    }


    void Start()
    {
        coutWrong = 0;
        Man = false;
        VillagerIN = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            RandomPerson();
    }
    public void RandomPerson()
    {

        int rNum = UnityEngine.Random.Range(0, 2);

        if (rNum == 0)
        {
            Man = true;
            int rNumShadow = UnityEngine.Random.Range(0, manShadows.Length);
            human.gameObject.GetComponent<Image>().sprite = man;
            shadow.gameObject.GetComponent<Image>().sprite = manShadows[rNumShadow];
            Enum.TryParse(rNumShadow.ToString(), out illness value);
            current_illness = value;

        }
        else
        {
            Man = false;
            int rNumShadow = UnityEngine.Random.Range(0, womanShadows.Length);
            human.gameObject.GetComponent<Image>().sprite = woman;
            shadow.gameObject.GetComponent<Image>().sprite = womanShadows[rNumShadow];
            Enum.TryParse(rNumShadow.ToString(), out illness value);
            current_illness = value;
        }
    }
    public void showShadow()
    {
        bool temp = shadow.gameObject.activeSelf;

        shadow.gameObject.SetActive(!temp);
        lantern[0].SetActive(!temp);
        lantern[1].SetActive(temp);
    }
    public void getitem()
    {
        if (VillagerIN && inventory.getGiveItem() != null)
        {
            ItemClass item = inventory.getGiveItem();
            //current_illness;
            if (current_illness == illness.Burns)
            {
                if (item.itemName.Replace(" ", "") == cure.ColdBrew.ToString() || item.itemName.Replace(" ", "") == cure.CoolingPotion.ToString())
                {

                    inventory.RemoveOUT(item);
                    if (Man)
                        shadow.gameObject.GetComponent<Image>().sprite = manShadows[10];
                    else
                        shadow.gameObject.GetComponent<Image>().sprite = womanShadows[10];
                    callOutVillager();


                }
                else
                {
                    coutWrong++;
                    if (coutWrong == 3)
                    {
                        callOutVillager();
                    }
                }
            }
            else if (current_illness == illness.Frost_bite)
            {
                if (item.itemName.Replace(" ", "") == cure.HeatingMixture.ToString() || item.itemName.Replace(" ", "") == cure.DefrostingPotion.ToString())
                {

                    inventory.RemoveOUT(item);
                    if (Man)
                        shadow.gameObject.GetComponent<Image>().sprite = manShadows[10];
                    else
                        shadow.gameObject.GetComponent<Image>().sprite = womanShadows[10];
                    callOutVillager();
                }
                else
                {
                    coutWrong++;
                    if (coutWrong == 3)
                    {
                        callOutVillager();
                    }
                }
            }
            else if (current_illness == illness.Cold)
            {
                if (item.itemName.Replace(" ", "") == cure.VolcanicTea.ToString() || item.itemName.Replace(" ", "") == cure.WarmingDrink.ToString())
                {

                    inventory.RemoveOUT(item);
                    if (Man)
                        shadow.gameObject.GetComponent<Image>().sprite = manShadows[10];
                    else
                        shadow.gameObject.GetComponent<Image>().sprite = womanShadows[10];
                    callOutVillager();
                }
                else
                {
                    coutWrong++;
                    if (coutWrong == 3)
                    {
                        callOutVillager();
                    }
                }
            }
            else if (current_illness == illness.Poisoning)
            {
                if (item.itemName.Replace(" ", "") == cure.Antidote.ToString() || item.itemName.Replace(" ", "") == cure.NeutralizingTonic.ToString())
                {

                    inventory.RemoveOUT(item);
                    if (Man)
                        shadow.gameObject.GetComponent<Image>().sprite = manShadows[10];
                    else
                        shadow.gameObject.GetComponent<Image>().sprite = womanShadows[10];
                    callOutVillager();
                }
                else
                {
                    coutWrong++;
                    if (coutWrong == 3)
                    {
                        callOutVillager();
                    }
                }
            }
            else if (current_illness == illness.Blindness)
            {
                if (item.itemName.Replace(" ", "") == cure.VisionRestorationDrops.ToString() || item.itemName.Replace(" ", "") == cure.SightEnhancementSerum.ToString())
                {

                    inventory.RemoveOUT(item);
                    callOutVillager();
                }
                else
                {
                    coutWrong++;
                    if (coutWrong == 3)
                    {
                        callOutVillager();
                    }
                }
            }
            else if (current_illness == illness.Curse)
            {
                if (item.itemName.Replace(" ", "") == cure.CurseLiftingBrew.ToString() || item.itemName.Replace(" ", "") == cure.CharmingTincture.ToString())
                {

                    inventory.RemoveOUT(item);
                    if (Man)
                        shadow.gameObject.GetComponent<Image>().sprite = manShadows[10];
                    else
                        shadow.gameObject.GetComponent<Image>().sprite = womanShadows[10];
                    callOutVillager();
                }
                else
                {
                    coutWrong++;
                    if (coutWrong == 3)
                    {
                        callOutVillager();
                    }
                }
            }
            else if (current_illness == illness.Death)
            {
                if (item.itemName.Replace(" ", "") == cure.DeathWoker.ToString())
                {

                    inventory.RemoveOUT(item);
                    if (Man)
                        shadow.gameObject.GetComponent<Image>().sprite = manShadows[10];
                    else
                        shadow.gameObject.GetComponent<Image>().sprite = womanShadows[10];
                    callOutVillager();
                }
                else
                {
                    coutWrong++;
                    if (coutWrong == 3)
                    {
                        callOutVillager();
                    }
                }
            }
            else if (current_illness == illness.Trance)
            {
                if (item.itemName.Replace(" ", "") == cure.TranceBreaker.ToString())
                {

                    inventory.RemoveOUT(item);
                    if (Man)
                        shadow.gameObject.GetComponent<Image>().sprite = manShadows[10];
                    else
                        shadow.gameObject.GetComponent<Image>().sprite = womanShadows[10];
                    callOutVillager();
                }
                else
                {
                    coutWrong++;
                    if (coutWrong == 3)
                    {
                        callOutVillager();
                    }
                }
            }
            else if (current_illness == illness.Fatigue)
            {
                if (item.itemName.Replace(" ", "") == cure.TitansBrew.ToString())
                {

                    inventory.RemoveOUT(item);
                    if (Man)
                        shadow.gameObject.GetComponent<Image>().sprite = manShadows[10];
                    else
                        shadow.gameObject.GetComponent<Image>().sprite = womanShadows[10];
                    callOutVillager();
                }
                else
                {
                    coutWrong++;
                    if (coutWrong == 3)
                    {
                        callOutVillager();
                    }
                }
            }
            else if (current_illness == illness.Dehydration)
            {
                if (item.itemName.Replace(" ", "") == cure.RefreshingTonic.ToString())
                {

                    inventory.RemoveOUT(item);
                    if (Man)
                        shadow.gameObject.GetComponent<Image>().sprite = manShadows[10];
                    else
                        shadow.gameObject.GetComponent<Image>().sprite = womanShadows[10];
                    callOutVillager();
                }
                else
                {
                    coutWrong++;
                    if (coutWrong == 3)
                    {
                        callOutVillager();
                    }
                }
            }
            else if (current_illness == illness.Deep_wounds)
            {
                if (item.itemName.Replace(" ", "") == cure.DeephealPotion.ToString())
                {

                    inventory.RemoveOUT(item);
                    if (Man)
                        shadow.gameObject.GetComponent<Image>().sprite = manShadows[10];
                    else
                        shadow.gameObject.GetComponent<Image>().sprite = womanShadows[10];
                    callOutVillager();
                }
                else
                {
                    coutWrong++;
                    if (coutWrong == 3)
                    {
                        callOutVillager();
                    }
                }
            }
            else if (current_illness == illness.Critical_state)
            {
                if (item.itemName.Replace(" ", "") == cure.RejuvenationDrink.ToString() || item.itemName.Replace(" ", "") == cure.SightEnhancementSerum.ToString())
                {

                    inventory.RemoveOUT(item);
                    if (Man)
                        shadow.gameObject.GetComponent<Image>().sprite = manShadows[10];
                    else
                        shadow.gameObject.GetComponent<Image>().sprite = womanShadows[10];
                    callOutVillager();
                }
                else
                {
                    coutWrong++;
                    if (coutWrong == 3)
                    {
                        callOutVillager();
                    }
                }
            }
            else
            {
                coutWrong++;
                if (coutWrong == 3)
                {
                    callOutVillager();
                }
            }
        }

    }
    public void callInVillager()
    {
        RandomPerson();
        ite.SetActive(true);
        btn.SetActive(true);
        VillagerIN = true;
        gameObject.GetComponent<Animator>().SetBool("show", true);
    }
    public void callOutVillager()
    {
        coutWrong = 0;
        ite.SetActive(false);
        btn.SetActive(false);
        VillagerIN = false;
        gameObject.GetComponent<Animator>().SetBool("show", false);
    }
}
