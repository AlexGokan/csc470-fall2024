using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spike1script : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 base_position;
    Quaternion base_rot;

    public float wave_amp = 10.0f;
    public float wave_freq = 1.0f;

    public int transform_axis = 2;

    public float phase_deg = 0f;

    void Start()
    {
        base_position = transform.position;
        base_rot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
    float phase_rad = phase_deg * Mathf.PI / 180f;
     if(transform_axis == 2){
        float new_pos = base_position.z + (wave_amp/2.0f) + wave_amp * Mathf.Sin(wave_freq * Time.time + phase_rad);
        transform.position = new Vector3(base_position.x,base_position.y,new_pos);
     }else{

     }

    }
}
