using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthbarScript : MonoBehaviour
{
 
    public GameObject hb_cylinder;
    public void set_health(float h, float total){
        hb_cylinder.transform.localScale = new Vector3(1f,h/total,1f);
    }
 
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
