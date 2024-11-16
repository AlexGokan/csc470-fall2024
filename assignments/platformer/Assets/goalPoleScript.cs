using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalPoleScript : MonoBehaviour
{
    
    public GameObject character;

    characterScript cs;

    // Start is called before the first frame update
    void Start()
    {
        cs = character.GetComponent<characterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cs.keys_collected < cs.num_keys_needed){
            transform.position = new Vector3(-175,-10f,0f);
        }
        else{
            transform.position = new Vector3(-175f,8f,0f);
        }
    }
}
