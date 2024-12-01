using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unitscript : MonoBehaviour
{

    public StateMachine unit_state_machine = new StateMachine();

    public Vector3 target;

    public Vector3 home_base;

    public Unit_movingToTarget_state movingToTarget_state;
    public Unit_idling_state idling_state;

    public Unit_returningHome_state returningHome_state;

    public Unit_holdingingForTime_state holdingForTime_state_2sec;

    void Start(){
        movingToTarget_state = new Unit_movingToTarget_state(this);
        idling_state = new Unit_idling_state(this);
        returningHome_state = new Unit_returningHome_state(this);
        holdingForTime_state_2sec = new Unit_holdingingForTime_state(this,2f);



        unit_state_machine.change_state(movingToTarget_state);
        target = new Vector3(5f,0f,0f);
        home_base = transform.position;
    }

    void set_target(Vector3 t){
        this.target = t;
    }

    void Update(){

        IState next_state = unit_state_machine.curr_state.next_state();
        unit_state_machine.change_state(next_state);
                
        
        unit_state_machine.update();
    }

}

public class Unit_holdingingForTime_state : IState{
    Unitscript owner;
    float start_time;
    float curr_time;

    public bool finished;

    float duration;
    public Unit_holdingingForTime_state(Unitscript owner, float d){
        this.owner = owner;
        this.duration = d;
    }

    public void enter(){
        Debug.Log("entering holdingPattern");
        start_time = Time.time;
        curr_time = start_time;
        finished = false;
    }

    public void execute(){
        Debug.Log("executing holdingPattern");
        curr_time = Time.time;
        if(curr_time - duration > start_time){
            finished = true;
        }

    }

    public void exit(){
        Debug.Log("exiting holdingPattern");
    }

    public IState next_state(){
        if(this.finished){
            return owner.movingToTarget_state;
        }else{
            return owner.holdingForTime_state_2sec;
        }
    }
}

public class Unit_idling_state : IState{
    Unitscript owner;
    public Unit_idling_state(Unitscript owner){
        this.owner = owner;
    }

    public void enter(){
        Debug.Log("entering idle");
    }

    public void execute(){
        Debug.Log("executing idle");
        //do nothin
    }

    public void exit(){
        Debug.Log("exiting idle");
    }

    public IState next_state(){
        return owner.holdingForTime_state_2sec;
    }
}

public class Unit_returningHome_state : IState{
    Unitscript owner;

    public bool done_moving;
    public Unit_returningHome_state(Unitscript owner){
        this.owner = owner;
    }

    public void enter(){
        Debug.Log("entering returnHome");
        done_moving = false;
    }

    public void execute(){
        Debug.Log("executing returnHome");
        Vector3 to_target =  owner.home_base - owner.transform.position;

        Debug.Log(to_target);

        if(to_target.magnitude > 0.05){
            owner.transform.position += 3f * to_target.normalized * Time.deltaTime;
        }else{
            done_moving = true;
        }
    }

    public void exit(){
        Debug.Log("exiting returnHome");
        owner.transform.position = owner.home_base;
    }

    public IState next_state(){
        if(this.done_moving){
            return owner.idling_state;
        }else{
            return owner.returningHome_state;
        }
    }
}

public class Unit_movingToTarget_state : IState{
    Unitscript owner;
    public Unit_movingToTarget_state(Unitscript owner){
        this.owner = owner;
    }

    public bool done_moving;

    public void enter(){
        Debug.Log("entering moveToTarget");
        done_moving = false;
    }

    public void execute(){
        Debug.Log("executing moveToTarget");
        Vector3 to_target =  owner.target - owner.transform.position;

        Debug.Log(to_target);

        if(to_target.magnitude > 0.1){
            owner.transform.position += 3f * to_target.normalized * Time.deltaTime;
        }else{
            done_moving = true;
        }
    }

    public void exit(){
        Debug.Log("exiting moveToTarget");
        owner.transform.position = owner.target;
    }

    public IState next_state(){
        if(this.done_moving){
            return owner.returningHome_state;
        }else{
            return owner.movingToTarget_state;
        }
    }
}