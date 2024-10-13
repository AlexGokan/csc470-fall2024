using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board_manager_script : MonoBehaviour
{

/*
This script will hold the stuff for clickable buttons that will modify the edges of the board

*/

    public GameObject game_manager;

    public GameObject button_prefab;

    void Start()
    {
    
    }

    void set_edge_randomly(){
        
        int height = game_manager.GetComponent<game_managment>().grid_height;
        int width = game_manager.GetComponent<game_managment>().grid_width;

        cell_script[,] grid = game_manager.GetComponent<game_managment>().grid;        
        
        for(int i=0; i<height; i++){
            grid[i,0].alive = Random.value > 0.5;
            grid[i,width-1].alive = Random.value > 0.5;
        }
        for(int j=0; j<width; j++){
            grid[height-1,j].alive = Random.value>0.5;
        }
    }

    void set_edge_blank(){
             int height = game_manager.GetComponent<game_managment>().grid_height;
        int width = game_manager.GetComponent<game_managment>().grid_width;

        cell_script[,] grid = game_manager.GetComponent<game_managment>().grid;        
        
        for(int i=0; i<height; i++){
            grid[i,0].alive = false;
            grid[i,width-1].alive = false;
        }
        for(int j=0; j<width; j++){
            grid[height-1,j].alive = false;
        }   
    }

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.K)){
            set_edge_randomly();
        }
        if(Input.GetKeyDown(KeyCode.L)){
            set_edge_blank();
        }
        
    }
}
