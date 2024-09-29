using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class gameManager : MonoBehaviour
{

    public GameObject domino_prefab;

    GameObject first_domino;
    void Start()
    {
        
        Vector3 start_pos = new Vector3(0f,0f,0f);
        for(int i=0;i<200;i++){
            Vector3 dom_pos = start_pos + transform.forward*i;
            dom_pos += 4*Mathf.Sin((float)i/6)*transform.right;
            GameObject domino = Instantiate(domino_prefab,dom_pos,Quaternion.identity);
            if(i==0){
                first_domino = domino;
            }


            Renderer rend = domino.GetComponentInChildren<Renderer>();
            rend.material.color = new Color(UnityEngine.Random.value,UnityEngine.Random.value,UnityEngine.Random.value);
            //rend.material.color = Color.HSVToRGB(i/200,.4f,1f);//doesnt seem to work?

        }   
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Rigidbody rb = first_domino.GetComponent<Rigidbody>();
            rb.AddForce(first_domino.transform.forward*350);
        }
    }
}
