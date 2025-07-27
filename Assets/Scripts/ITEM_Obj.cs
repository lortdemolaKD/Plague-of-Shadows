using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITEM_Obj : MonoBehaviour
{
    public int id;
    // Start is called before the first frame update
    public int CollectOut()
    {
        Invoke(nameof(DeleteOBJ), 0.1f);
        return id;
        

    }
    void DeleteOBJ()
    {
        Destroy(gameObject);
    }
}
