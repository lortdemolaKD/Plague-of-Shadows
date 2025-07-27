using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonSetup : MonoBehaviour
{
    
    public GameObject inv;
    public int id_B;

    private void Start()
    {
        
    }
    public void CReate(GameObject gObj,int id)
    {
        inv = gObj;
        id_B = id;

    }
    public void DoStaf()
    {
        Debug.Log("do");
        inv.GetComponent<InventoryManager>().selectRec(id_B);
    }
}
