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



    void Start()
    {

		for(int i=0; i<8; i++){
			Vector3 rule_pos = new Vector3(i*6f,-5f,0f);
			GameObject rule = Instantiate(rule_prefab,rule_pos,Quaternion.identity);
			rule_script rs = rule.GetComponent<rule_script>();

			rs.responds_to = i;
			//"rule 30" https://mathworld.wolfram.com/ElementaryCellularAutomaton.html
			int[] zero_set = {0,5,6,7};
			
			if(zero_set.Contains<int>(i)){
				rs.result = false;
			}else{
				rs.result = true;
			}


			rule_objects = rule_objects.Append<GameObject>(rule).ToArray();//there has got to be a better way to append to an array
		}
		

		grid = new cell_script[grid_height,grid_width];
		
		for(int x=0; x<grid_width; x++){
			for(int y=0; y<grid_height; y++){
				Vector3 pos = transform.position;
				pos.x += x*spacing;
				pos.y += y*spacing;

				if(x==0||x==grid_width-1||y==grid_height-1){
					//edge cell
					GameObject cell = Instantiate(edge_cell_prefab,pos,Quaternion.identity);
					grid[y,x] = cell.GetComponent<cell_script>();
					grid[y,x].is_edge = true;
				}else{
					GameObject cell = Instantiate(main_cell_prefab,pos,Quaternion.identity);
					grid[y,x] = cell.GetComponent<cell_script>();
					grid[y,x].is_edge = false;
				}

				grid[y,x].alive = false;
				if(x == grid_width/2){
					grid[y,x].alive = true;
				}

				grid[y,x].yi = y;
				grid[y,x].xi = x;
				
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


			}
		}

	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
			update_board();
		}
    }
}
