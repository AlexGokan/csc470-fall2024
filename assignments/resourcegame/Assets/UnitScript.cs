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
    
    
    bool valid_move(int xf, int yf){
        int xdiff = xcoord - xf;
        int ydiff = ycoord - yf;

        if(Mathf.Abs(xdiff) >= 2 || Math.Abs(ydiff) >= 2){
            return false;
        }


        return true;
    }

    public bool move_to(int i, int j){
        Debug.Log("moving to"+i+" "+j);

        
        if(valid_move(i,j)){
            xcoord = i; ycoord = j;
            transform.position = new Vector3(i,0,j);
            return true;
        }
        //if there is a teammate do nothing

        //if there is an enemy attack it

        //if there is nobody there then move
        return false;
    }

    
    public void attack(UnitScript other, int damage_amount){
        other.change_health(-damage_amount);
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
        Destroy(this.gameObject);
    }
    
    
    void OnDestroy(){
        Debug.Log("Killed");
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
