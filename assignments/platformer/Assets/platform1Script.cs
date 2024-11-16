using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform1Script : MonoBehaviour
{
    // Start is called before the first frame update
    public float last_frame_z = 0.0f;
    public float this_frame_z;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        last_frame_z = transform.position.z;

        float z_pos = Mathf.Sin(Time.time * 1.0f);
        this_frame_z = z_pos;
        
        transform.position = new Vector3(-9.1f,0.4f,z_pos);



    }

    
}
