using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deerScript : MonoBehaviour
{

    public bool playerInSightRange;
    public float sightRange;
    public LayerMask whatIsPlayer;
    public GameObject body,armature;

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
    
        if(playerInSightRange && !Input.GetKey(KeyCode.LeftControl))
        {
            body.SetActive(false);
            armature.SetActive(false);
        }
        if (!playerInSightRange)
        {
            body.SetActive(true);
            armature.SetActive(true);
        }
    }
}
