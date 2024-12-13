using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;



public class RiderScript : MonoBehaviour
{

    public GameObject t1_rot_center;
    public GameObject t2_rot_center;
    
    public CharacterController cc;

    public int course_state = 0;
    public float speed = 25f;

    public int laps = 0;

    public bool is_ai = false;

    public float delta_speed = 2.5f;

    public float top_pedaling_speed = 40f;

    public int ai_steer_direction;
    public int ai_throttle;

    public GameObject race_manager_go;

    RaceManagerScript race_manager;

    public Camera main_cam;

    public Vector3 cam_home_position;//"home base" for camera

    public Vector3 camera_shake_vector = new Vector3(0f,0f,0f);//updates every 0.2secs, camera pos it always home+shake

    public bool is_drafting;

    public bool is_being_drafted;

    public TMP_Text lap_counter;

    public GameObject front_wheel;
    public GameObject rear_wheel;


    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {//https://discussions.unity.com/t/rotate-a-vector-around-a-certain-point/81225/3
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    IEnumerator pick_random_throttle(){
        for(;;){
            float r = UnityEngine.Random.Range(0f,1f);
            if(r > 0.25f){
                this.ai_throttle = 1;
            }else if(r > 0.125){
                this.ai_throttle = 0;
            }else{
                this.ai_throttle = -1;
            }

            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator pick_random_steer(){
        for(;;){
            float r = UnityEngine.Random.Range(0f,1f);//too lazy to figure out how to pick a random int
            if(r < 0.333f){
                this.ai_steer_direction = -1;
            }else if(r>0.666){
                this.ai_steer_direction = 1;
            }else{
                this.ai_steer_direction = 0;
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    

    void Start()
    {

        Debug.Log("starting");
        
        if(!is_ai){
            cam_home_position = main_cam.transform.localPosition;
        }

        race_manager = race_manager_go.GetComponent<RaceManagerScript>();

        //TODO: WHY CAN I NOT SET THE TMP_TEXT AT THE START OF THE GAME???????


        if(!is_ai){
            Debug.Log("am NOT ai");
            this.top_pedaling_speed = 34f + PlayerPrefs.GetInt("rider_speed");
            this.delta_speed = 3.1f + PlayerPrefs.GetInt("rider_speed") * 0.1f;
            StartCoroutine(get_new_camera_jitter());
        }else{
            Debug.Log("am an ai");
            StartCoroutine(pick_random_steer());
        }

    }

    float draft_multiplier(){
        float draft_distance = 10f;
        
        RaycastHit hit;
        LayerMask lm = LayerMask.GetMask("Draftbox");



        if(Physics.Raycast(transform.position,transform.forward,out hit, draft_distance,lm)){
            Debug.DrawRay(transform.position,draft_distance*transform.forward,Color.green);
            is_drafting = true;

            GameObject g = hit.collider.gameObject;
            //g.GetComponent<RiderScript>().is_being_drafted = true;//this line causes WEIRD effects - don't uncomment

            return 1.25f;
        }else{
            Debug.DrawRay(transform.position,draft_distance*transform.forward,Color.red);
            is_drafting = false;
            return 1f;
        }

    }


    void do_control_independent_move(){
        
        if(this.course_state == 0){
            Vector3 to_move = this.speed * draft_multiplier() * Time.deltaTime * transform.forward;
            float delta_y = transform.position.y - 0.54f;
            to_move += Vector3.up * delta_y * -1f;
            
            cc.Move(to_move);

            Vector3 north = Vector3.forward;
            Vector3 south = -1f * Vector3.forward;
            float dn = Vector3.Dot(north,transform.forward);
            float ds = Vector3.Dot(south,transform.forward);


            float traveling_angle = Vector3.SignedAngle(north,transform.forward,Vector3.up);
            if(Mathf.Abs(traveling_angle - 0f) < 60f){//lock the facing direction to north/south in case high speeds cause floating point error to make him a bit off angle
                transform.Rotate(Vector3.up,-traveling_angle);
            }
            if(Mathf.Abs(traveling_angle - 180f) < 60f){
                transform.Rotate(Vector3.up,180-traveling_angle);
            }


            return;
        }

        if(this.course_state == 1){
            /*
            V_t = r*omega ---> V_t=tangent velocity, r=radius, omega=angular velocity
            omega = V_t/r = this.speed/30f (in rads/s)

            */

            float turn_radius = (t1_rot_center.transform.position - this.transform.position).magnitude;

            float omega_r = this.speed * draft_multiplier() / turn_radius;
            float omega_d = omega_r * 180f/Mathf.PI;
            float amount_to_rotate = Time.deltaTime * omega_d * -1f;

            Vector3 new_pos = RotatePointAroundPivot(transform.position,t1_rot_center.transform.position,new Vector3(0f,amount_to_rotate,0f));
            Vector3 to_move = new_pos - transform.position;
            float delta_y = transform.position.y - 0.54f;
            to_move += Vector3.up * delta_y * -1f;
            cc.Move(to_move);

            this.transform.Rotate(Vector3.up,amount_to_rotate);
            return;
        }

         if(this.course_state == 2){
            float turn_radius = (t2_rot_center.transform.position - this.transform.position).magnitude;
            
            float omega_r = this.speed * draft_multiplier() / turn_radius;
            float omega_d = omega_r * 180f/Mathf.PI;
            float amount_to_rotate = Time.deltaTime * omega_d * -1f;

            Vector3 new_pos = RotatePointAroundPivot(transform.position,t2_rot_center.transform.position,new Vector3(0f,amount_to_rotate,0f));
            Vector3 to_move = new_pos - transform.position;
            float delta_y = transform.position.y - 0.54f;
            to_move += Vector3.up * delta_y * -1f;
            cc.Move(to_move);

            this.transform.Rotate(Vector3.up,amount_to_rotate);
            return;
        }
    }

    void speed_update(){
        int throttle;
        if(is_ai){
            throttle = get_ai_throttle();
        }else{
            throttle = get_user_throttle();
        }
        
        
        if(throttle == 1){
            if(this.speed <= top_pedaling_speed){
                    this.speed += Time.deltaTime * delta_speed;
            }
            return;
        }
        
        if(throttle == -1){
            this.speed -= Time.deltaTime * delta_speed;
            this.speed = Mathf.Max(this.speed,0f);
            return;
        }


        //if throttle == 0 do nothing
    }
    
    void turn_left(float side_move_speed, float speed_loss){
        
        Vector3 to_move = this.speed * Time.deltaTime * transform.right * -1f * side_move_speed;
        

        RaycastHit hit;
        LayerMask lm = LayerMask.GetMask("Sidewall");
        float wall_check_distance = 2f;
        if(Physics.Raycast(transform.position,-1*transform.right,out hit,wall_check_distance,lm)){
            
        }else{
            //this.speed /= 1f - (speed_loss * Time.deltaTime);
            //to_move += Vector3.up * 0.12f * -1f * Time.deltaTime * this.speed;//this shit will not stop bugging so I'm REMOVING IT    
        }
        cc.Move(to_move); 
        
    }

    void turn_right(float side_move_speed, float speed_loss){
        Vector3 to_move = this.speed * Time.deltaTime * transform.right * side_move_speed;       


        RaycastHit hit;
        LayerMask lm = LayerMask.GetMask("Sidewall");
        float wall_check_distance = 2f;
        if(Physics.Raycast(transform.position,transform.right,out hit,wall_check_distance,lm)){

        }else{
            //to_move += Vector3.up * 0.12f * 1f * Time.deltaTime * this.speed;
            //this.speed *= 1f - (speed_loss * Time.deltaTime);
        }
        cc.Move(to_move);
        
    }


    int get_user_steer(){
        if(Input.GetKey(KeyCode.A)){
            return -1;
        }if(Input.GetKey(KeyCode.D)){
            return 1;
        }

        return 0;
    }

    int get_user_throttle(){
        if(Input.GetKey(KeyCode.W)){
            return 1;
        }

        if(Input.GetKey(KeyCode.S)){
            return -1;
        }

        return 0;
    }

    int get_ai_steer(){
        return this.ai_steer_direction;
    }

    int get_ai_throttle(){
        return this.ai_throttle;
    }




    void controlled_move(){
        
        /*
        if we are on the straightway, our turning is constrained by the walls, no special logic check
        if we are in a curve (track_state == 1 or 2) we need to make sure the smaller center distance never gets bigger than the track width
        */
        
        
        int steer = 0;
        if(is_ai){
            steer = get_ai_steer();
        }else{
            steer = get_user_steer();
        }
        

        if(this.course_state == 0){
            if(steer == 1){
                turn_right(1f,0.20f);
            }

            if(steer == -1){
                turn_left(1f,0.20f);
            }
        }

        else{
            float d1 = (t1_rot_center.transform.position - this.transform.position).magnitude;
            float d2 = (t2_rot_center.transform.position - this.transform.position).magnitude;
            float d = Mathf.Min(d1,d2);//get distance of closest turn point
            
            //Debug.Log(d);
            if(d < 69.5f){
                if(steer == 1){//only need to disallow moving right since the left side is constrained by wall
                    turn_right(1f,0.20f);
                }
            }

            if(steer == -1){
                turn_left(1f,0.20f);
            }

        }
        
        

        /*
        if(!cc.isGrounded){
            Debug.Log("ungrounded player");
        }



        if(!cc.isGrounded){
            RaycastHit hit;
            LayerMask lm = LayerMask.GetMask("Ground");
        
            if(Physics.Raycast(transform.position,-1*Vector3.up,out hit, Mathf.Infinity,lm)){
                if(!is_ai){
                    Vector3 snap_point = hit.point;
                    Debug.Log("hit the ground" + snap_point);
                    cc.transform.position = snap_point + 1f*Vector3.up;
                    this.transform.position = snap_point + 1f*Vector3.up;
                }
            }
        }
        */

    }

    IEnumerator get_new_camera_jitter(){
        for(;;){
            camera_shake_vector = UnityEngine.Random.insideUnitSphere * 0.08f * Mathf.Exp((this.speed/3.5f)-10);
            yield return new WaitForSeconds(0.03f);
        }
    }
    
    void update_camera(){
        if(!is_ai){
            if(Input.GetKey(KeyCode.LeftShift)){
                main_cam.transform.localPosition = cam_home_position + 4f*transform.forward + -0.5f*transform.up;//this doesn't work idk
                main_cam.transform.localRotation = Quaternion.Euler(0,180,0);
            }else{
                main_cam.transform.localPosition = cam_home_position + camera_shake_vector;//only camera shake when looking forward
                main_cam.transform.localRotation = Quaternion.Euler(0,0,0);
            }
            
        }
    }

    void update_particles(){
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();        
        ParticleSystem.EmissionModule em =  ps.emission;
        
        em.enabled = this.is_drafting;

    }
   
    void Update()
    {
        this.is_being_drafted = false;

        speed_update();
        controlled_move();
        do_control_independent_move();
        update_camera();
        update_particles();

    }

    void OnTriggerEnter(Collider c){
        
        GameObject g = c.gameObject;
        if(g.CompareTag("Turn1Coll")){
            this.course_state = 1;
            return;
        }

        if(g.CompareTag("Turn2Coll")){
            this.course_state = 2;
            return;
        }

        if(g.CompareTag("StraightawayColl")){
            this.course_state = 0;
            return;
        }

        if(g.CompareTag("FinishLine")){
            this.laps++;
            if(!is_ai){
                lap_counter.text = "Laps: " + this.laps + "/" + race_manager.num_laps;
            }

            return;
        }


        //Debug.Log("Collided with untagged object " + g.name);
    }
}
