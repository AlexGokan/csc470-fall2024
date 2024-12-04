using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class riderScript : MonoBehaviour
{
    public float speed;//the riders speed

    public float downtrack_pos;//down-track distance traveled
    public float crosstrack_pos;//cross-track position
    //these will be combined with the course information to generate xpos and ypos

    public GameObject legs;
    public GameObject front_wheel;
    public GameObject rear_wheel;

    public GameObject torso;

    public float turn_rad;

    public float draft_factor;

    public float current_effort = 0f;//instantaneous amount of effort being put in

    public float energy_remaining = 100f;

    public int strategy;//only used if the rider is an ai

    public CharacterController cc;

    public float speed_loss;

    public float delta_v;

    GameObject staminabar;
    GameObject effortbar;

    GameObject draftlight;
    Renderer draftlight_rend;

    public int index;

    public Color jersey_color;

    public float adjust_speed_for_curve(float speed, float rad){//when translating on y_distance, we will adjust based speed based on the radius of the curve
        return speed;
    }



    void Start()
    {
        staminabar = transform.Find("staminabar").gameObject;
        effortbar = transform.Find("effortbar").gameObject;
        draftlight = transform.Find("draftlight").gameObject;
        draftlight_rend = draftlight.GetComponent<Renderer>();
    }

    IEnumerator pick_steering_angle_randomly(){
        for(;;){
            this.turn_rad = Random.Range(-4,4);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator pick_effort_randomly(){
        for(;;){
            this.current_effort = Random.Range(0f,100f);
            yield return new WaitForSeconds(1f);
        }
    }

    public void start_race(){

        Renderer torso_rend = torso.GetComponent<Renderer>();
        torso_rend.material.color = this.jersey_color;

        this.speed = 0.1f;
        if(this.strategy == 0){
            StartCoroutine(pick_steering_angle_randomly());
        }if(this.strategy == 2){
            StartCoroutine(pick_effort_randomly());
        }
    }

    void spin_legs(){
        float rot_speed = 10f;
        float gear_ratio = 61/17;//my boy has quads

        legs.transform.Rotate(rot_speed*Time.deltaTime*this.speed,0f,0f);
        front_wheel.transform.Rotate(0f,-rot_speed*Time.deltaTime*this.speed*gear_ratio,0f);//want to rotate along local Y axis
        rear_wheel.transform.Rotate(0f,-rot_speed*Time.deltaTime*this.speed*gear_ratio,0f);
    }
    
    void select_steering_player(){
        float turn_speed = 7f;

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)){
            if(Input.GetKey(KeyCode.A)){
                turn_rad += turn_speed * Time.deltaTime;//want to make it a little softer at low turning radius
            }
            if(Input.GetKey(KeyCode.D)){
                turn_rad -= turn_speed * Time.deltaTime;
            }
        }else{
            this.turn_rad *= 0.92f;//decay the steering back towards the center
        }

        float max_turn = 8f;
        this.turn_rad = Mathf.Max(turn_rad,-max_turn);
        this.turn_rad = Mathf.Min(turn_rad,max_turn);
    }
    
    void select_steering_ai(){
        if(this.strategy == 2){
            this.turn_rad = 0f;
            return;
        }
        if(this.strategy == 1){//straight on
            this.turn_rad = 0f;
            return;
        }if(this.strategy == 0){
            //handled by the pick steering angle coroutine
            return;
        }

        this.turn_rad = 0f;//fallback case

    }

    void steer_rider(){
        

        //float current_fw_y_rot = front_wheel.transform.rotation.y;
        //front_wheel.transform.rotation = Quaternion.Euler(0f,current_fw_y_rot,turn_rad);//retain its Y(forward/back) rotation, while updating the X (left/right) rotation
        
        //front_wheel.transform.Rotate(transform.up,turn_rad);
        //doesnt seem to work WHY????

        this.transform.rotation = Quaternion.Euler(0f,0f,turn_rad*3f);//lean the whole bike+rider
        torso.transform.rotation = Quaternion.Euler(45f + this.current_effort * 0.3f,0f,0f);//make the torso lean forward at high effort

        //right now this just moves you left and right instead of actually turning you
        //I think for this particular game, its probably better that way

    }
    
    void update_draft_multiplier(){//could probably only check every .5 secs or something with a coroutine but idk if needed
        LayerMask lm = LayerMask.GetMask("Riders");
        RaycastHit hit;

        float drafting_distance = 4.5f;
        

        if(Physics.Raycast(transform.position,transform.forward,out hit,drafting_distance,lm)){
            this.draft_factor = 1f;
            draftlight_rend.material.color = Color.blue;
            
        }else{
            this.draft_factor = 0f;
            draftlight_rend.material.color = Color.gray;
        }
    }
    
    void select_effort_player(){
        float acceleration = 30f;
        float deceleration = 40f;


        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.LeftShift)){
            if(Input.GetKey(KeyCode.W)){
                this.current_effort = Mathf.Max(this.current_effort,0f);//set it to 0 if negative
                this.current_effort += acceleration * Time.deltaTime;
            }if(Input.GetKey(KeyCode.S)){
                this.current_effort = Mathf.Min(this.current_effort,0f);
                this.current_effort -= deceleration * Time.deltaTime;
            }if(Input.GetKey(KeyCode.LeftShift)){
                this.current_effort = 100f;
            }

        }else{
            this.current_effort *= 0.75f;//decay towards 0
        }

        this.current_effort = Mathf.Min(this.current_effort,100f);//clamp from -100 to +100
        this.current_effort = Mathf.Max(this.current_effort,-100f);
    }

    void select_effort_ai(){
        if(this.strategy == 0){//balanced
            this.current_effort = 30f + Random.Range(-10f,10f);
            return;
        }if(this.strategy == 1){//all-out
            this.current_effort = 100f;
            return;
        }if(this.strategy == 2){
            return;//handled by the pick random effort coroutine
        }

        this.current_effort = 30f;//fallback case
    }
    
    void move_rider_with_effort(){

        float effort_mult = 0.07f;
        this.delta_v = this.current_effort * effort_mult * Time.deltaTime;

        this.speed += this.delta_v;

    }

    void translate_rider(){
        //this.transform.position += this.speed * Vector3.forward * Time.deltaTime * 1f;
        //this.transform.position += this.turn_rad * Vector3.left * Time.deltaTime * 1.8f;

        this.cc.Move(this.speed * Vector3.forward * Time.deltaTime * 1f);
        this.cc.Move(this.turn_rad * Vector3.left * Time.deltaTime * 1.8f);

        //this.downtrack_pos += this.speed * Time.deltaTime;
        this.downtrack_pos = transform.position.z;
        this.crosstrack_pos += this.turn_rad * Time.deltaTime * 1.8f;
    }

    void update_energy(float e_quick, float e_slow){
        //if effort is <= 20, recover quickly
        //if effort is <= 40 recover slowly
        //otherwise drain energy

        float energy_delta;

        if(this.current_effort <= e_quick){//recover energy quickly if very low effort
            energy_delta = Time.deltaTime * 18f;
            energy_delta *= (1f+this.draft_factor);//recover energy faster in the draft
        }else if(this.current_effort <= e_slow){
            energy_delta = Time.deltaTime * 8f;
            energy_delta *= (1f+this.draft_factor);
        }else{
            energy_delta = -Time.deltaTime * (this.current_effort - e_slow) * 1f;
        }


        this.energy_remaining += energy_delta;

        if(this.energy_remaining <= 0f){
            this.energy_remaining = 0f;
            this.current_effort = 0f;
        }
        if(this.energy_remaining >= 100f){
            this.energy_remaining = 100f;
        }


        effortbar.GetComponent<healthbarScript>().set_health(this.current_effort,100f);
        staminabar.GetComponent<healthbarScript>().set_health(this.energy_remaining,100f);
    }

    void apply_draft(){
        this.speed_loss = Mathf.Pow(this.speed*0.1f,3f) * 0.001f;

        this.speed *= 1f-this.speed_loss;
    }

    public void controllable_update_step(){
        update_draft_multiplier();
        spin_legs();
        select_steering_player();
        steer_rider();
        apply_draft();
        select_effort_player();
        update_energy(20f,40f);//must be called before translate_rider and move_rider because we need to override effort if out of energy
        move_rider_with_effort();
        translate_rider();
    }

    public void ai_update_step(){
        update_draft_multiplier();
        spin_legs();
        select_steering_ai();
        steer_rider();
        apply_draft();
        select_effort_ai();
        update_energy(20,40);
        move_rider_with_effort();
        translate_rider();
    }
    

    void Update()
    {
        
    }
}
