using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatformScript : MonoBehaviour
{
    
    float start_z;
    float start_x;

    float wave_amp = .03f;
    float time_scale = 4.0f;

    public float phase = 0f;

    public Vector3 delta;
    
    // Start is called before the first frame update
    void Start()
    {
        start_z = transform.position.z;
        start_x = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float offset_z = wave_amp * Mathf.Sin((time_scale*Time.time) + phase);
        //float offset_x = wave_amp * Mathf.Cos(time_scale*Time.time);

        delta = new Vector3(0f,0f,offset_z);

        transform.position += delta;
    }
}
