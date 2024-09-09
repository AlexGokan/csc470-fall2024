using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demoscript : MonoBehaviour
{

    //public Rigidbody rb2;


    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.position = new Vector3(0.0f,0.0f,0.0f);        
    }

    // Update is called once per frame
    void Update()
    {

        /*
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x+0.1f,//require the f because vector3 needs doubles
                                                        this.gameObject.transform.position.y,
                                                        this.gameObject.transform.position.z);
        */


        
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();

        if(Input.GetKeyDown(KeyCode.Space)){
            rb.useGravity = true;
        }

        

        
        
        

    }
}
