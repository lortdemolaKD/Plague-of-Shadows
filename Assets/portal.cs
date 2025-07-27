using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityStandardAssets.Effects;

public class portal : MonoBehaviour
{
    public GameObject Player;
    public GameObject inH;
    public GameObject outH;
    public GameObject home;
    public InventoryManager inventoryManager;
    public playermovment playermovment;
    public Button button;
    public bool isIN;
        
    // Start is called before the first frame update
    void Start()
    {
        isIN = false;
        home.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void usePortal()
    {
        if (isIN) 
        {
            home.SetActive(false);
            Player.transform.position = outH.transform.position;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isIN = false;

        }
        else 
        {
            home.SetActive(true);
            button.interactable = true;
            inventoryManager.AddList(playermovment.invItems);
            Player.transform.position = inH.transform.position;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
           
            isIN = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        usePortal();
    }
}
