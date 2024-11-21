using UnityEngine;

public class GroundScript : MonoBehaviour
{
    
    public ManagerScript game_manager_script;

    void Start()
    {
        game_manager_script = GameObject.Find("GameManager").GetComponent<ManagerScript>();
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            OnSpaceDown();
        }
        if(Input.GetKeyDown(KeyCode.P)){
            OnPDown();
        }
        
    }

    void OnMouseDown(){//if we click on the ground, move to "waiting for piece selection"
        //its enter function will deselect the piece for us
        game_manager_script.game_state_machine.change_state(game_manager_script.waitingForUnitSelection_State);

    }

    void OnSpaceDown(){
        if(game_manager_script.selected_unit != null){
            game_manager_script.selected_unit.unit_state_machine.change_state(game_manager_script.selected_unit.movingToAttack_State);
        }
    }

    void OnPDown(){
        if(game_manager_script.selected_unit != null){
            game_manager_script.selected_unit.unit_state_machine.change_state(game_manager_script.selected_unit.dying_State);
        }
    }

    

}
