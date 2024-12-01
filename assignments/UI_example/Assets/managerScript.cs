/*
observor pattern:
    allows a subject to broadcast a message to observors

    game-manager is the subject
    units/objects are observers


*/

using System;//needed for actions






using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class managerScript : MonoBehaviour
{

    public Action SpacebarPressed;

    public Action<unitScript> UnitClicked;

    public static managerScript instance;

    public unitScript selected_unit;

    public List<unitScript> units = new List<unitScript>();

    public string name_text;

    public string bio_text;

    public string stats_text;

    public Camera main_cam;

    void OnEnable(){//happens before start()
        if(managerScript.instance == null){
            managerScript.instance = this;
        }else{
            Destroy(this);//prevents a duplicate gamemanager
        }
    }
    

    public void OpenCharacterSheet(unitScript us){
        name_text = us.name;
        bio_text = us.bio;
        stats_text = us.stats;
    }

    void Start()
    {
        
    }

    public void selectUnit(unitScript unit){

        /*
        foreach(unitScript u in units){
            u.selected = false;
            u.rend.material.color = u.deselected_color;
        }
        */
        UnitClicked?.Invoke(unit);

        /*
        unit.selected = true;
        unit.rend.material.color = unit.selected_color;
        selected_unit = unit;
        */
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space)){
            if(SpacebarPressed != null){//if htere are no subscribers, then the action is null
                SpacebarPressed.Invoke();
            }
            //equivalent to SpacebarPressed?.Invoke();
        }


        RaycastHit hit_info;
        LayerMask ground_mask = LayerMask.GetMask("Ground","Unit");
        //raycast from the camera to the mouse into the world
        Ray ray_into_screen = main_cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray_into_screen,out hit_info, float.PositiveInfinity,ground_mask)){
            //the mouse is over the ground
            Debug.Log("true");
            
            if(hit_info.collider.CompareTag("ground")){
                if(Input.GetMouseButtonDown(0)){
                    if(selected_unit != null){
                        selected_unit.gameObject.transform.position = hit_info.point;
                    }
                }
            }else if(hit_info.collider.CompareTag("Unit")){
                selectUnit(hit_info.collider.gameObject.GetComponent<unitScript>());
            }

        }else{
            Debug.Log("false");
        }
    }
}
