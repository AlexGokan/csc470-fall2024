using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class game_managment : MonoBehaviour
{

	public GameObject edge_cell_prefab;
	public GameObject main_cell_prefab;

	public GameObject rule_prefab;	

	public cell_script[,] grid;
	
	public float spacing = 1.1f;

	public int grid_height = 70;
	public int grid_width = 60;

	private GameObject[] rule_objects = {};

	public Camera cam;

    void Start()
    {

		cam.transform.position = new Vector3(43f,90f,43f);//starting position


		for(int i=0; i<8; i++){
			//Vector3 rule_pos = new Vector3(-0.07f + i*0.02f,0.045f,0.1f);
			//give X-positions from -0.07 to +0.07	
			
			Vector3 rule_pos = new Vector3(0f,0f,0f);

			GameObject rule = Instantiate(rule_prefab,cam.transform.position,Quaternion.identity);
			rule.transform.Translate(transform.up*-0.5f);
			rule.transform.Translate(transform.right*0.052f*i);
			rule.transform.Translate(transform.forward * 0.21f);
			rule.transform.Rotate(new Vector3(88.2f,39.5f,39.5f));//no idea where these came from, just want to make them horizontal dawg
			//rule.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));

			rule.transform.parent = cam.transform;
			
			rule_script rs = rule.GetComponent<rule_script>();

			rs.responds_to = i;
			//"rule 30" https://mathworld.wolfram.com/ElementaryCellularAutomaton.html
			int[] zero_set = {0,5,6,7};
			
			if(zero_set.Contains<int>(i)){//this is just the default setting at game start, user can change later
				rs.result = false;
			}else{
				rs.result = true;
			}


			rule_objects = rule_objects.Append<GameObject>(rule).ToArray();//there has got to be a better way to append to an array
		}

		
		

		grid = new cell_script[grid_height,grid_width];
		
		for(int x=0; x<grid_width; x++){
			for(int z=0; z<grid_height; z++){
				Vector3 pos = transform.position;
				pos.x += x*spacing;
				pos.z += z*spacing;

				if(x==0||x==grid_width-1||z==grid_height-1){
					//edge cell
					GameObject cell = Instantiate(edge_cell_prefab,pos,Quaternion.identity);
					cell.transform.localScale = new Vector3(1f,10f,1f);
					grid[z,x] = cell.GetComponent<cell_script>();
					grid[z,x].is_edge = true;
					grid[z,x].parent_object = cell;
				}else{
					GameObject cell = Instantiate(main_cell_prefab,pos,Quaternion.identity);
					grid[z,x] = cell.GetComponent<cell_script>();
					grid[z,x].is_edge = false;
					grid[z,x].parent_object = cell;
				}

				grid[z,x].alive = false;
				if(x == grid_width/2){
					grid[z,x].alive = true;
				}

				grid[z,x].yi = z;
				grid[z,x].xi = x;
				
			}
		}		
	 
    }

	int bool_to_int(bool b){
		return b?1:0; 
	}

	void update_board(){//update the body cells based on the CA rules
		for(int y=grid_height-2; y>=0; y--){//-2 to skip the first row
			for(int x=1; x<grid_width-1; x++){
				int v = 0;
				v += 1*bool_to_int(grid[y+1,x+1].alive);
				v += 2*bool_to_int(grid[y+1,x].alive);
				v += 4*bool_to_int(grid[y+1,x-1].alive);

				//get the v-th rule
				bool change_to = rule_objects[v].GetComponent<rule_script>().result;

				grid[y,x].alive = change_to;
				
				if(grid[y,x].alive){
					GameObject p = grid[y,x].parent_object;
					p.transform.localScale = new Vector3(1f,5f,1f);
					p.GetComponent<BoxCollider>().transform.localScale = new Vector3(1f,5f,1f);
				}else{
					GameObject p = grid[y,x].parent_object;
					p.transform.localScale = new Vector3(1f,1f,1f);
					p.GetComponent<BoxCollider>().transform.localScale = new Vector3(1f,1f,1f);
				}
				

			}
		}

	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
			update_board();
		}

		float movespeed = 0.07f;
		if(Input.GetKey(KeyCode.W)){
			cam.transform.Translate(cam.transform.forward*movespeed);
		}
		if(Input.GetKey(KeyCode.S)){
			cam.transform.Translate(cam.transform.forward*movespeed*-1);
		}
		if(Input.GetKey(KeyCode.D)){
			cam.transform.Translate(cam.transform.right*movespeed);
		}
		if(Input.GetKey(KeyCode.A)){
			cam.transform.Translate(cam.transform.right*movespeed*-1);
		}


		if(Input.GetKeyDown(KeyCode.Alpha1)){
			rule_script rs = rule_objects[0].GetComponent<rule_script>();
			rs.swap_output();
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){
			rule_script rs = rule_objects[1].GetComponent<rule_script>();
			rs.swap_output();
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)){
			rule_script rs = rule_objects[2].GetComponent<rule_script>();
			rs.swap_output();
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)){
			rule_script rs = rule_objects[3].GetComponent<rule_script>();
			rs.swap_output();
		}
		if(Input.GetKeyDown(KeyCode.Alpha5)){
			rule_script rs = rule_objects[4].GetComponent<rule_script>();
			rs.swap_output();
		}
		if(Input.GetKeyDown(KeyCode.Alpha6)){
			rule_script rs = rule_objects[5].GetComponent<rule_script>();
			rs.swap_output();
		}
		if(Input.GetKeyDown(KeyCode.Alpha7)){
			rule_script rs = rule_objects[6].GetComponent<rule_script>();
			rs.swap_output();
		}
		if(Input.GetKeyDown(KeyCode.Alpha8)){
			rule_script rs = rule_objects[7].GetComponent<rule_script>();
			rs.swap_output();
		}
    }
}
