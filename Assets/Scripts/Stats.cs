using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public int health;
    public AudioClip damagetake;
    public Slider helth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void doDamage(int num)
    {
        health -= num;
        helth.value = health;
    }
}
