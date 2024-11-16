using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class HealthbarScript : MonoBehaviour
{
   

    public void set_health(int h, int m){
        transform.GetChild(1).localScale = new Vector3((float)h/(float)m,1f,1f);
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
