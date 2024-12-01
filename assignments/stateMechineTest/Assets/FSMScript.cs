using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState{
    public void enter();
    public void execute();
    public void exit();
    public IState next_state();
}

public class StateMachine{
    public IState curr_state;

    public void change_state(IState new_state){
        if(new_state == curr_state){
            return;
        }

        if(curr_state != null){
            curr_state.exit();
        }

        curr_state = new_state;
        curr_state.enter();
    }

    public void update(){
        if(curr_state != null){
            curr_state.execute();
        }
    }
}






public class FSMScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
