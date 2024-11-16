using System.Collections.Generic;
using UnityEngine;
using System;


public class ManagerScript : MonoBehaviour
{

    public bool users_turn = true;

    public GameObject UnitPrefab;

    public GameObject GridSquarePrefab;
    public List<UnitScript> my_units = new List<UnitScript>();

    public List<UnitScript> enemy_units = new List<UnitScript>();

    UnitScript selected_unit;

    GameObject selected_square;

    public static ManagerScript instance;

    GameObject u1;
    GameObject u2;

    UnitScript us1;
    UnitScript us2;

    public Action<UnitScript> unit_clicked;

    public Action<BoardSquareScript> square_clicked;

    GameObject[,] board = new GameObject[6,6];

    public UnitScript[,] units_on_board = new UnitScript[6,6];

    void OnEnable(){
        if (ManagerScript.instance == null){
            ManagerScript.instance = this;
        }else{
            Destroy(this);
        }
    }

    void deselect_all_squares(){
        selected_square = null;
        square_clicked?.Invoke(null);
    }

    void deselect_all_units(){
        selected_unit = null;
        unit_clicked?.Invoke(null);
    }

    public void select_square(BoardSquareScript b){
        if(users_turn){
            deselect_all_squares();

            square_clicked?.Invoke(b);
            

            selected_square = b.gameObject;


            if(selected_unit != null && selected_unit.GetComponent<UnitScript>().move_to(b.xidx,b.yidx)){
                deselect_all_squares();
                deselect_all_units();
                users_turn = !users_turn;
            }
        }

    }

    public void select_unit(UnitScript u){
        if(users_turn){
            deselect_all_squares();

            if(u.friendly){
                unit_clicked?.Invoke(u);
                
                selected_unit = u;
            }
        }

    }

    public void bg_select(BackgroundScript bg){
        deselect_all_squares();
        deselect_all_units();
    }


    void Start()
    {

        for(int i=0; i<6; i++){
            for(int j=0; j<6; j++){
                GameObject sq = Instantiate(GridSquarePrefab);
                board[i,j] = sq;
                sq.GetComponent<BoardSquareScript>().setup(i,j);
            }
        }

        for(int i=0; i<6; i++){
            GameObject u = Instantiate(UnitPrefab);
            UnitScript us = u.GetComponent<UnitScript>();
            us.setup(3,"userTeam",3,i,0,0.35f,true);
            my_units.Add(us);


            GameObject u2 = Instantiate(UnitPrefab);
            UnitScript us2 = u2.GetComponent<UnitScript>();
            us2.setup(3,"userTeam",3,i,1,0.35f,true);
            my_units.Add(us2);

            units_on_board[i,0] = us;
            units_on_board[i,1] = us2;


        }

        for(int i=0; i<6; i++){
            GameObject u = Instantiate(UnitPrefab);
            UnitScript us = u.GetComponent<UnitScript>();
            us.setup(3,"userTeam",2,i,4,0.35f,false);
            my_units.Add(us);


            GameObject u2 = Instantiate(UnitPrefab);
            UnitScript us2 = u2.GetComponent<UnitScript>();
            us2.setup(3,"userTeam",2,i,5,0.35f,false);
            my_units.Add(us2);

            units_on_board[i,4] = us;
            units_on_board[i,5] = us2;

        }


    }

    void Update()
    {
        if(!users_turn){
            //the enemy team should make a move

            users_turn = true;
        }
        
    }
}
