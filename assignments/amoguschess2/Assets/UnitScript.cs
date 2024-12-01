
using Unity.VisualScripting;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public string curr_state;
    
    public StateMachine unit_state_machine;

    public Vector3 target;

    public Vector3 home_base;

    public Vector3 last_home_base;

    public bool selected;

    bool friendly;

    public Renderer rend;

    public Unit_holdingForTime_state holdingForTime_State;
    public Unit_idling_state idling_State;

    public Unit_movingToAttack_state movingToAttack_State;

    public Unit_movingToHome_state movingToHome_State;

    public Unit_returningHomFromAttack_state returningHomFromAttack_State;

    public Unit_checkingMoveValidity_state checkingMoveValidity_State;

    public Unit_dying_state dying_State;

    public ManagerScript game_manager_script;

    GameObject main_camera;



    void Start()
    {

        unit_state_machine = new StateMachine();
        
        holdingForTime_State = new Unit_holdingForTime_state(this,2f);
        idling_State = new Unit_idling_state(this);
        movingToAttack_State = new Unit_movingToAttack_state(this);
        movingToHome_State = new Unit_movingToHome_state(this);
        returningHomFromAttack_State = new Unit_returningHomFromAttack_state(this);
        dying_State = new Unit_dying_state(this);
        checkingMoveValidity_State = new Unit_checkingMoveValidity_state(this);

        unit_state_machine.set_state_abrupt(idling_State);

        game_manager_script = GameObject.Find("GameManager").GetComponent<ManagerScript>();

        main_camera = GameObject.Find("Main Camera");
    }

    public void deselect_me(){
        this.selected = false;
        if(game_manager_script.selected_unit != null && game_manager_script.selected_unit == this){
            game_manager_script.selected_unit = null;
        }
    }

    public void select_me(){
        if(game_manager_script.selected_unit != null){
            game_manager_script.selected_unit.selected = false;
        }

        this.selected = true;
        game_manager_script.selected_unit = this;
    }

    public void destroy_myself(){
        Destroy(this.transform.GameObject());
    }

    
    public void set_target(Vector3 t){
        target = t;
    }

    public void set_home_base(Vector3 hb, bool override_last){
        if(override_last){
            last_home_base = hb;
        }else{
            last_home_base = home_base;//to use in case a move validity check fails
        }
        home_base = hb;
    }

    public void set_pos_at_start(Vector3 v){
        home_base = v;
        transform.position = v;
    }

    public void set_friendly_status(bool f){
        this.friendly = f;
    }

    void Update()
    {

        IState next_state = unit_state_machine.curr_state.next_state();
        if(next_state != null){//have to catch the null case for when unit dies
            unit_state_machine.change_state(next_state);

            unit_state_machine.update();

            if(this.selected){
                rend.material.color = Color.yellow;
            }else{
                rend.material.color = Color.white;
            }
        }

        transform.LookAt(main_camera.transform.position);
    
    }

    void OnMouseDown(){//once a piece is selected, change to waiting for a destination square
       if(selected){
            deselect_me();
            game_manager_script.game_state_machine.change_state(game_manager_script.waitingForUnitSelection_State);
       }else{
            select_me();
            game_manager_script.game_state_machine.change_state(game_manager_script.waitingForDestinationSelection_State);
       }

       
    }

}

public class Unit_checkingMoveValidity_state : IState{
    UnitScript owner;

    public bool valid;

    public Unit_checkingMoveValidity_state(UnitScript owner){
        this.owner = owner;
    }


    public void enter(){
        Debug.Log("entering move check state");
        valid = false;
    }

    public void execute(){
        owner.curr_state = "validating move with raycast";
    }

    public void exit(){
    }

    public IState next_state(){
        //do a ray cast towards target
        //if it hits nothing, proceed to moving state
        //if it does hit something, return to the select a unit state

        //this does feel like the wrong way to do it
        //I probably should not be manupulating the game state like this
        //but whatever

        RaycastHit hit;
        LayerMask lm = ~LayerMask.GetMask("Checkerboard");//why do I need to invert it??????

        Vector3 rc_origin = owner.transform.position + 1.0f*Vector3.up;//make the raycast origin above the units head
        //then it will cast downward diagonally into the environment, avoiding itself and hopefully only hitting other units

        if(Physics.Raycast(rc_origin,owner.home_base-rc_origin,out hit,Mathf.Infinity, lm)){
            Debug.Log("raycast hit something");
            Debug.Log(owner.transform.position);
            Debug.Log(owner.home_base);
            Debug.Log(hit.point);
            Debug.Log(hit.collider.gameObject.name);
            owner.game_manager_script.game_state_machine.change_state(owner.game_manager_script.waitingForUnitSelection_State);
            owner.set_home_base(owner.last_home_base,true);
            return owner.idling_State;
        }else{
            Debug.Log("raycast all clear");
            Debug.Log(owner.transform.position);
            Debug.Log(owner.home_base);

            owner.game_manager_script.game_state_machine.change_state(owner.game_manager_script.movingFriendlyUnit_State);
            return owner.movingToHome_State;
        }
    }
}

public class Unit_movingToHome_state : IState{
    UnitScript owner;
    public bool finished;
    
    public Unit_movingToHome_state(UnitScript owner){
        this.owner = owner;
    }

    public void enter(){
        owner.deselect_me();
        owner.rend.material.color = Color.white;
        finished = false;

        owner.curr_state = "Moving to home";
    }

    public void execute(){
        owner.deselect_me();
        Vector3 to_target = owner.home_base - owner.transform.position;
        if(to_target.magnitude > 0.1){
            owner.transform.Translate(3f*to_target.normalized*Time.deltaTime);
            
        }else{
            finished = true;
            owner.game_manager_script.movingFriendlyUnit_State.finished = true;//mark the game manager as having finished the move
        }
    }

    public void exit(){
        owner.transform.position = owner.home_base;
        this.owner.deselect_me();
    }

    public IState next_state(){
        if(!this.finished){
            return this;
        }
        else{
            return owner.idling_State;
        }
    }
}

public class Unit_returningHomFromAttack_state : IState{
    UnitScript owner;
    public bool finished;
    
    public Unit_returningHomFromAttack_state(UnitScript owner){
        this.owner = owner;
    }

    public void enter(){
        owner.deselect_me();
        owner.rend.material.color = Color.white;
        finished = false;

        owner.curr_state = "Returning home from attack";
    }

    public void execute(){
        owner.deselect_me();
        Vector3 to_target = owner.home_base - owner.transform.position;
        if(to_target.magnitude > 0.1){
            owner.transform.Translate(60f*to_target.normalized*Time.deltaTime);
            
        }else{
            finished = true;
            owner.game_manager_script.movingFriendlyUnit_State.finished = true;//mark the game manager as having finished the move
        }
    }

    public void exit(){
        owner.transform.position = owner.home_base;
        this.owner.deselect_me();
    }

    public IState next_state(){
        if(!this.finished){
            return this;
        }
        else{
            return owner.idling_State;
        }
    }
}

public class Unit_movingToAttack_state : IState{
    UnitScript owner;
    public bool finished;
    
    public Unit_movingToAttack_state(UnitScript owner){
        this.owner = owner;
    }

    public void enter(){
        owner.deselect_me();
        owner.rend.material.color = Color.green;
        finished = false;

        owner.curr_state = "Moving to attack";
    }

    public void execute(){
        owner.deselect_me();
        Vector3 to_target = owner.target - owner.transform.position;
        if(to_target.magnitude > 0.1){
            owner.transform.Translate(60f*to_target.normalized*Time.deltaTime);
        }else{
            finished = true;
        }
    }

    public void exit(){
        owner.transform.position = owner.target;
    }

    public IState next_state(){
        if(!this.finished){
            return this;
        }
        else{
            return owner.returningHomFromAttack_State;
        }
    }
}

public class Unit_idling_state : IState{
    UnitScript owner;


    public Unit_idling_state(UnitScript owner){
        this.owner = owner;
    }

    public void enter(){
        owner.curr_state = "Idling";
    }

    public void execute(){
        if(owner.selected){
            owner.rend.material.color = Color.yellow;
        }else{
            owner.rend.material.color = Color.white;
        }
    }

    public void exit(){

    }

    public IState next_state(){
        return this;
    }
}

public class Unit_holdingForTime_state : IState{

    UnitScript owner;
    float start_time;
    float duration;

    float curr_time;

    public bool finished;

    public Unit_holdingForTime_state(UnitScript owner, float d){
        this.owner = owner;
        this.duration = d;
    }

    public void enter(){
        owner.deselect_me();
        owner.rend.material.color = Color.white;
        start_time = Time.time;
        curr_time = start_time;
        finished = false;

        owner.curr_state = "Holding for time";
    }

    public void execute(){
        owner.deselect_me();
        curr_time = Time.time;
        if(curr_time - duration > start_time){
            finished = true;
        }
    }

    public void exit(){

    }

    public IState next_state(){
        if(!this.finished){
            return this;
        }
        else{
            return owner.movingToAttack_State;
        }
    }
}

public class Unit_dying_state : IState{

    UnitScript owner;
    
    public bool finished;

    Vector3 target;

    public Unit_dying_state(UnitScript owner){
        this.owner = owner;
    }

    public void enter(){
        owner.deselect_me();
        target = owner.transform.position + 5*Vector3.up;
        finished = false;
        owner.curr_state = "Dying";
    }

    public void execute(){
        owner.deselect_me();
        owner.rend.material.color = Color.blue;
        Vector3 to_target = target - owner.transform.position;
        if(to_target.magnitude > 0.1){
            owner.transform.Translate(2.5f*to_target.normalized*Time.deltaTime);
        }else{
            finished = true;
        }
    }

    public void exit(){
        //this state will never exit since it never has a "next state"
    }

    public IState next_state(){
        if(finished){
            owner.deselect_me();
            owner.destroy_myself();//only things that inherit from monobehavior can call Destroy()
            return null;
        }
        return this;
    }
}