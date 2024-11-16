using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Il2Cpp;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public GameObject healthbar;

    
    public int xcoord;
    public int ycoord;

    int health;
    int max_health;

    public int level;

    string unitname;

    HealthbarScript hbs;

    public bool alive;

    public Color selectedColor;
    public Color deselectedColor;

    GameObject main_cam;

    public bool selected;

    public Renderer rend;

    public bool friendly;

    
    void OnEnable(){
        ManagerScript.instance.unit_clicked += manager_says_unit_clicked;
    }

    void manager_says_unit_clicked(UnitScript unit){
        if(unit == this){
            select_me();
        }else{
            deselect_me();
        }
    }

    public void setup(int mh, string name, int strength, int xidx, int yidx, float scale, bool friendly_unit){
        max_health = mh;
        unitname = name;
        health = max_health;
        transform.position = new Vector3(xidx,0,yidx);
        xcoord = xidx; ycoord = yidx;
        level = strength;
        transform.localScale = new Vector3(scale,scale,scale);
        friendly = friendly_unit;

        if(friendly){
            deselectedColor = new Color(1f,0f,0f);
        }else{
            deselectedColor = new Color(0f,0f,1f);
        }

        rend.material.color = deselectedColor;

    }

    public void change_health(int delta){
        set_health(this.health + delta);
    }
    
    void set_health(int h){
        this.health = h;
        hbs.set_health(h,max_health);
    }
    
    
    bool valid_attack(int xf,int yf){
        int xdiff = xcoord - xf;
        int ydiff = ycoord - yf;

        if(Mathf.Abs(xdiff) >= 2 || Math.Abs(ydiff) >= 2){
            Debug.Log("attacking too far away");
            return false;
        }
        
        if(ManagerScript.instance.units_on_board[xf,yf] != null){
            if(ManagerScript.instance.units_on_board[xf,yf].friendly == false){
                return true;
            }
        }

        return false;
    }

    bool valid_move(int xf, int yf){
        int xdiff = xcoord - xf;
        int ydiff = ycoord - yf;

        if(Mathf.Abs(xdiff) >= 2 || Math.Abs(ydiff) >= 2){
            Debug.Log("moving too far away");
            return false;
        }

        if(ManagerScript.instance.units_on_board[xf,yf] == null){
            return true;
        }



        return false;
    }

    public bool move_to(int i, int j){
        Debug.Log("moving to"+i+" "+j);
        
        if(valid_attack(i,j)){
            attack(ManagerScript.instance.units_on_board[i,j], 1);
            
            Debug.Log("Valid attack");
            return true;
        }

        if(valid_move(i,j)){
            Debug.Log("Valid move");
            ManagerScript.instance.units_on_board[xcoord,ycoord] = null;
            xcoord = i; ycoord = j;
            transform.position = new Vector3(i,0,j);
            ManagerScript.instance.units_on_board[i,j] = this;
            return true;
        }
        //if there is a teammate do nothing

        //if there is an enemy attack it

        //if there is nobody there then move
        return false;
    }

    
    public void attack(UnitScript other, int damage_amount){
        if(this.level == other.level){
            other.change_health(-damage_amount);
            this.change_health(-damage_amount);
            Debug.Log("both hurt");
            return;
        }
        
        if(this.level > other.level){
            other.change_health(-damage_amount);
            Debug.Log("you did damage");
        }else{
            this.change_health(-damage_amount);
            Debug.Log("you took damage");
        }
        
        
    }
    
    
    void Start()
    {
        hbs = healthbar.GetComponent<HealthbarScript>();
        main_cam = GameObject.Find("Main Camera");
    }

    
    void Update()
    {

        transform.LookAt(main_cam.transform);

        if(health <= 0){
            killMe();
        }
    }

    
    public void killMe(){
        this.alive = false;
        ManagerScript.instance.units_on_board[this.xcoord,this.ycoord] = null;
        if(this.friendly){
            ManagerScript.instance.my_units.Remove(this);
        }else{
            ManagerScript.instance.enemy_units.Remove(this);
        }
        Destroy(this.gameObject);
    }
    
    
    void OnDestroy(){
        Debug.Log("Killed");
        ManagerScript.instance.unit_clicked -= manager_says_unit_clicked;
    }

    public void select_me(){
        this.selected = true;
        rend.material.color = selectedColor;

    }

    public void deselect_me(){
        this.selected = false;
        rend.material.color = deselectedColor;
    }


    void OnMouseDown(){
        ManagerScript.instance.select_unit(this);
    }

}
