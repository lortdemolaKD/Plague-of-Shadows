using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectal : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            
        }
        else
        {
            if (other.tag == "Player")
            {
                
                other.GetComponent<Stats>().doDamage(damage);
                Destroy(gameObject);

            }
            
            Destroy(gameObject);
        }
        
    }
}
