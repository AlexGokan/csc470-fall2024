using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell_script : MonoBehaviour
{
	
	
	public Renderer cube_renderer;

	public GameObject parent_object;

	public bool alive = false;
	
	public int xi = -1;
	public int yi = -1;

	public float height = 1f;
	
	public Color alive_color;
	public Color dead_color;

	public bool is_edge = true;
	
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
		BoxCollider bc = parent_object.AddComponent<BoxCollider>();
    }

    void Update()
    {
        
		if(Input.GetKeyDown(KeyCode.O)){
			alive = !alive;
		}

		set_color();
    }
	
	void OnMouseDown(){
		if(is_edge){
			alive = !alive;
		}
	}
	
}
