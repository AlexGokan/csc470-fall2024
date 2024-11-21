using UnityEngine;

public class BoardSqaureScript : MonoBehaviour
{
    
    public ManagerScript game_manager_script;
    bool black_square;
    int xidx;
    int yidx;

    public Renderer rend;

    public void setup_square(int xidx, int yidx){
        this.xidx = xidx;
        this.yidx = yidx;

        this.black_square = (xidx + yidx) % 2 == 0;
    }
   
    void Start()
    {
        game_manager_script = GameObject.Find("GameManager").GetComponent<ManagerScript>();
        
        if(this.black_square){
            this.rend.material.color = Color.black;
        }else{
            this.rend.material.color = (0.75f*Color.white) + (0.25f*Color.red);
        }

        transform.position = new Vector3(this.xidx,0f,this.yidx);


    }

    void Update()
    {
        
    }

    void OnMouseDown(){
        if(game_manager_script.game_state_machine.curr_state == game_manager_script.waitingForDestinationSelection_State){
            game_manager_script.selected_unit.set_home_base(transform.position,false);
            

            game_manager_script.selected_unit.unit_state_machine.change_state(game_manager_script.selected_unit.checkingMoveValidity_State);
        }else{
            Debug.Log("mouse down on square but not in right state to check move");
        }
    }
}
