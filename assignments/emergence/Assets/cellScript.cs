using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEditor.UI;
using UnityEngine;

public class cellScript : MonoBehaviour
{

    public Renderer cube_renderer;

    public bool alive = false;

    public int xi = -1;
    public int yi = -1;

    public Color alive_color;
    public Color dead_color;

    private Vector3 pos;


    void Start()
    {
        set_color();
        pos = new Vector3(-1,-1,0);
    }


    void set_color(){
        if(alive){
            cube_renderer.material.color = alive_color;
        }else{
            cube_renderer.material.color = dead_color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        set_color();
        
    }

    void OnMouseDown(){
        alive = !alive;
    }
}
