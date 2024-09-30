using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_managment : MonoBehaviour
{

	public GameObject edge_cell_prefab;
	public GameObject main_cell_prefab;
	
	private cell_script[,] grid;
	
	public float spacing = 1.1f;

    void Start()
    {
		
		grid = new cell_script[100,20];
		
		for(int x=0; x<20; x++){
			for(int y=0; y<100; y++){
				Vector3 pos = transform.position;
				pos.x += x*spacing;
				pos.y += y*spacing;
				
				GameObject cell = Instantiate(edge_cell_prefab,pos,Quaternion.identity);
				grid[y,x] = cell.GetComponent<cell_script>();
				
			}
		}
		
	 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
