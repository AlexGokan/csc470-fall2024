using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button_script : MonoBehaviour
{

    public int state = 0;
    public List<bool> results = new List<bool>();

    public Renderer rend;

    public int list_len = 0;

    void Start()
    {
        rend = this.GetComponentInChildren<Renderer>();

    }

    void Update()
    {
        
    }

    void OnMouseDown(){
        this.results = new List<bool>();
        
        for(int i=0; i<list_len; i++){
            bool assign = false;
            if(state==0){
                assign = Random.value>0.5;
                rend.material.color = new Color(200,200f,200f);//have to assign color "one step ahead" in the cycle since we assign before reassigning state
            }else if(state==1){
                assign=false;
                rend.material.color = new Color(128f,0f,0f);
            }else if(state==2){
                assign=true;
                rend.material.color = new Color(0f,0f,0);
            }

            this.results.Add(assign);

        }



        state = (state+1)%3;//cycle between 0,1,2   
    }

}
