using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class spawner : MonoBehaviour
{
    public light_setup light_setup;
    public int delay;
    public bool spawnAtNight;
    public GameObject enemy;
    public int amount;
    [SerializeField] int currentDelay;
    public bool lasttype;
    public List<GameObject> list;

    // Start is called before the first frame update
    void Start()
    {
        currentDelay = 0;
        lasttype = spawnAtNight;
    }

    // Update is called once per frame
    void Update()
    {

        if (currentDelay >= delay && list.Count < 1)
        {

            if (light_setup.set_time_night == spawnAtNight && light_setup.set_time_night)
            {
                for (int i = 0; i < amount; i++)
                {
                    list.Add(Instantiate(enemy, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity));
                }
                currentDelay = 0;
            }
            if (light_setup.set_time_day == !spawnAtNight && !light_setup.set_time_night)
            {
                for (int i = 0; i < amount; i++)
                {
                    list.Add(Instantiate(enemy, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity));
                }
                currentDelay = 0;
            }
        }
        if (lasttype == light_setup.set_time_night)
        {
            lasttype = !light_setup.set_time_night;
            calcDelay();
        }
        if(list.Count > 0)
        {
            foreach (var item in list)
            {
                if (item == null)
                    list.Remove(item);
            }
        }

    }
    void calcDelay()
    {
       
                currentDelay++;

      
    }
   }
