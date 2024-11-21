using UnityEngine;

public class ManagerScript : MonoBehaviour
{

    public GameObject unit_prefab;
    public GameObject board_square_prefab;

    public StateMachine game_state_machine;

    GameObject[,] squares = new GameObject[4,4];

    public bool test_check = false;

    public UnitScript selected_unit;

    public Game_waitingForUnitSelection_state waitingForUnitSelection_State;
    public Game_waitingForDestinationSelection_state waitingForDestinationSelection_State;

    public Game_movingFriendlyUnit_state movingFriendlyUnit_State;

    void Start()
    {
        //--------<state machine setup>---------
        game_state_machine = new StateMachine();
        
        waitingForDestinationSelection_State = new Game_waitingForDestinationSelection_state(this);
        waitingForUnitSelection_State = new Game_waitingForUnitSelection_state(this);
        movingFriendlyUnit_State = new Game_movingFriendlyUnit_state(this);

        game_state_machine.set_state_abrupt(waitingForUnitSelection_State);

        //------<other game objects setup>------
        
        for(int i=0; i<4; i++){
            GameObject u1 = Instantiate(unit_prefab);
            u1.GetComponent<UnitScript>().set_pos_at_start(new Vector3(-3f,0f,i*2f));
            u1.GetComponent<UnitScript>().set_target(new Vector3(0f,0f,-1.5f));
            u1.GetComponent<UnitScript>().set_friendly_status(true);

        }


        //<------game board setup>------------
        for(int x=0; x<4; x++){
            for(int y=0; y<4; y++){
                GameObject go = Instantiate(board_square_prefab);
                BoardSqaureScript s = go.GetComponent<BoardSqaureScript>();
                s.setup_square(x,y);

                this.squares[x,y] = go;
            }
        }

    }

    void Update()
    {
        IState next_state = game_state_machine.curr_state.next_state();
        game_state_machine.change_state(next_state);

        game_state_machine.update();
    }

}

public class Game_waitingForUnitSelection_state : IState{
    ManagerScript owner;

    public Game_waitingForUnitSelection_state(ManagerScript owner){
        this.owner = owner;
    }

    public void enter(){
        if(this.owner.selected_unit != null){
            this.owner.selected_unit.selected = false;
            this.owner.selected_unit = null;
        }
    }

    public void execute(){

    }

    public void exit(){

    }

    public IState next_state(){
        return this;
    }
}


public class Game_waitingForMoveValidation_state : IState{
    ManagerScript owner;

    public Game_waitingForMoveValidation_state(ManagerScript owner){
        this.owner = owner;
    }

    public void enter(){
    }

    public void execute(){
        
    }

    public void exit(){

    }

    public IState next_state(){
        if(owner.selected_unit.checkingMoveValidity_State.valid){
            return owner.movingFriendlyUnit_State;
        }else{
            return owner.waitingForUnitSelection_State;
        }
    }
}


public class Game_waitingForDestinationSelection_state : IState{
    ManagerScript owner;

    public Game_waitingForDestinationSelection_state(ManagerScript owner){
        this.owner = owner;
    }

    public void enter(){

    }

    public void execute(){

    }

    public void exit(){

    }

    public IState next_state(){
        return this;
    }
}

public class Game_movingFriendlyUnit_state : IState{
    ManagerScript owner;

    public bool finished;

    public Game_movingFriendlyUnit_state(ManagerScript owner){
        this.owner = owner;
    }

    public void enter(){
        this.finished = false;
    }

    public void execute(){
    }

    public void exit(){

    }

    public IState next_state(){
        if(this.finished){
            return owner.waitingForUnitSelection_State;
        }else{
            return this;
        }
    }
}
