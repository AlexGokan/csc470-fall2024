using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanScript : MonoBehaviour
{
    Vector3 home_base;
    
    public RaceManagerScript race_manager;

    float time_offset;

    IEnumerator jump_up_and_down(){
        for(;;){
            float amp = (race_manager.cheer_amount + 0.2f) * 2;
            float time_scale = 4f * (1 + 2*race_manager.cheer_amount);
            Vector3 offset = Vector3.up * Mathf.Sin(time_scale*Time.time + time_offset) * amp;
            offset.y += amp/2f;

            //Debug.Log(offset.y);

            transform.position = home_base + offset;
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        time_offset = Random.Range(0,6.28f);
        home_base = transform.position;

        StartCoroutine(jump_up_and_down());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
