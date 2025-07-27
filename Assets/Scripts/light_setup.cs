using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class light_setup : MonoBehaviour
{
    // Start is called before the first frame update
    public bool set_time_day;
    public bool set_time_night;
    private bool set_time_day_prev;
    private bool set_time_night_prev;
    public List<GameObject> eventObjects;
    public List<GameObject> nightObjects;
    public List<GameObject> NEGnightObjects;
    [SerializeField]bool eventOcured;
    
    public Light sun;
    public Transform sunTrans;
    void Start()
    {
        set_time_day = true;
        set_time_day_prev = true;
        set_time_night = false;
        eventOcured = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(set_time_day != set_time_day_prev)
        {
            day();
        }
        if (set_time_night != set_time_night_prev)
        {
            night();
        }
    }
    public void setDay()
    {
        set_time_day = true;
    }
    public void setNight()
    {
        set_time_night = true;
    }
    void day()
    {
        sun.enabled = true;
        sunTrans.rotation = Quaternion.Euler(30f, sunTrans.rotation.y, sunTrans.rotation.z);
        set_time_day_prev = set_time_day;
        set_time_night_prev = false;
        set_time_night = false;
        
        RenderSettings.fogDensity = 0.02f;
        RenderSettings.reflectionIntensity = 1.0f;
        RenderSettings.fogColor = Color.gray;
        
        foreach (var item in nightObjects)
        {
            item.SetActive(false);
        }
        foreach (var item in NEGnightObjects)
        {
            item.SetActive(true);
        }
        if (eventOcured) 
        {
            foreach (var item in eventObjects)
            {
                item.SetActive(false);
            }
            eventOcured = false;
        }
        //114 on

    }
    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.

        RenderSettings.fog = false;
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);

        //After we have waited 5 seconds print the time again.
        RenderSettings.fog = true;

    }
    void night()
    {
        sun.enabled = false;
        sunTrans.rotation = Quaternion.Euler(280f, sunTrans.rotation.y, sunTrans.rotation.z);
        set_time_night_prev = set_time_night;
        set_time_day_prev = false;
        set_time_day = false;
        RenderSettings.fogColor = Color.HSVToRGB(0, 0, 0);
        RenderSettings.reflectionIntensity = 0.0f;
        RenderSettings.fogDensity = 0.04f;
        foreach (var item in nightObjects)
        {
            item.SetActive(true);
        }
        foreach (var item in NEGnightObjects)
        {
            item.SetActive(false);
        }
        if (Random.value > 0.1) //%20 percent chance
        {
            eventOcured = true;
            foreach (var item in eventObjects)
            {
                item.SetActive(true);
            }
        }
        //290 off
    }
}
