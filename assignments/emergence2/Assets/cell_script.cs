using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell_script : MonoBehaviour
{
	
	
	public Renderer cube_renderer;
	public bool alive = false;
	
	public int xi = -1;
	public int yi = -1;
	
	public Color alive_color;
	public Color dead_color;
	
	private Vector3 pos;
	

	void set_color(){
		if(alive){
			cube_renderer.material.color = alive_color;
		}else{
			cube_renderer.material.color = dead_color;
		}
	}

    void Start()
    {
        set_color();
		pos = new Vector3(-1,-1,0);
    }

    void Update()
    {
        
		if(Input.GetKeyDown("space")){
			alive = !alive;
		}
		
		set_color();
    }
	
	void OnMouseDown(){
		alive = !alive;
	}
	
}
