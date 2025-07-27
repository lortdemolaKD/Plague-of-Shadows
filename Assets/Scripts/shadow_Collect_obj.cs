using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadow_Collect_obj : MonoBehaviour
{
    public MeshRenderer[] inner_obj;
    public float shadow_REG_delay = 600f;
    public SkinnedMeshRenderer[] inner_obj_skin;
    public GameObject indicator;
    public bool isCollected;
    public bool IsPlayerNear;
    public bool IslampNear;
    public int id;
    public bool isCollected_prev;
    public light_setup lightD;
    // Start is called before the first frame update
    void Start()
    {
        isCollected = false;
        isCollected_prev = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.G) && IsPlayerNear && IslampNear && !isCollected)
        {
            indicator.SetActive(true);
            
        }
        /*else {
            RegShadow();
        }*/
    }
    private void Collect()
    {
        
        indicator.SetActive(false);
        foreach (var obj in inner_obj)
        {
            obj.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            obj.receiveShadows = false;
        }
        foreach (var objS in inner_obj_skin)
        {
            objS.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            objS.receiveShadows = false;
        }
        isCollected = true;
        Invoke(nameof(RegShadow), shadow_REG_delay);

    }
    public int CollectOut()
    {
        if (!isCollected && IsPlayerNear && IslampNear)
        {
            Collect();
            return id;
        }
        else
            return -1;

    }
    public void RegShadow()
    {
        isCollected = false;
        foreach (var obj in inner_obj)
        {
            obj.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
            obj.receiveShadows = true;
        }
        foreach (var objS in inner_obj_skin)
        {
            objS.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
            objS.receiveShadows = true;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.tag == "Player" && !isCollected && lightD.set_time_night)
        {
            IsPlayerNear = true;
            IslampNear = other.gameObject.GetComponentInParent<playermovment>().gotLantern;


        }

    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            IsPlayerNear = false;
            indicator.SetActive(false);
        }
    }
}
