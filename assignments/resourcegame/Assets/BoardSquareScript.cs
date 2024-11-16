using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquareScript : MonoBehaviour
{

    public int xidx;
    public int yidx;

    public Renderer sq_renderer;

    public Color color_a;
    public Color color_b;
    public Color selected_color;

    public bool selected;

    Color base_color;

    void OnEnable(){
        ManagerScript.instance.square_clicked += manager_says_square_clicked;
    }

    void manager_says_square_clicked(BoardSquareScript b){
        if(b == this){
            select_me();
        }else{
            deselect_me();
        }
    }

    public void setup(int i, int j){
        xidx = i;
        yidx = j;

        if((i+j) % 2 == 0){
            base_color = color_a;
        }else{
            base_color = color_b;
        }
        sq_renderer.material.color = base_color;

        transform.position = new Vector3(i,0,j);

    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnMouseDown(){
        ManagerScript.instance.select_square(this);
    }

    public void select_me(){
        sq_renderer.material.color = selected_color;
        selected = true;
    }
    public void deselect_me(){
        sq_renderer.material.color = base_color;
        selected = false;
    }

}
