using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterScript : MonoBehaviour
{

    public CharacterController cc;

    public GameObject dirt_trail;

    float base_speed = 15;
    float dash_mult = 5.0f;

    float jump_speed = 0.6f;

    float dash_amount;

    int num_jumps = 2;

    float gravity = 2.5f;
    float speed_up;

    bool dirt_visible = false;
    GameObject dirt_instance = null;

    public float dash_cooldown_max = 0.1f;

    public float dash_cooldown;

    Vector3 platform_delta = Vector3.zero;

    public int keys_collected;

    public float fuel_time = 2.5f;

    public float fuel_time_max = 2.5f;

    public float time_since_death;

    public int num_keys_needed = 4;

    Vector3 respawn_loc = new Vector3(-0.1f,0f,0f);


    // Start is called before the first frame update
    void Start()
    {
        dash_amount = 0.0f;
        speed_up = 0.0f;
        dash_cooldown = 0.0f;

        dash_cooldown_max = 0.7f;

        keys_collected = 0;

        time_since_death = float.MaxValue;

        num_keys_needed = 4;
    }

    void reset_player(){
        cc.enabled = false;
        //transform.position = new Vector3(-0.1f,0f,0f);
        transform.position = respawn_loc;
        cc.enabled = true;

        time_since_death = 0f;
    }


    void Update()
    {

        time_since_death += Time.deltaTime;

        dash_amount *= 0.90f;
        dash_cooldown -= Time.deltaTime;


        float haxis = Input.GetAxisRaw("Horizontal");//"raw" version has no ramp up/down
        float vaxis = Input.GetAxisRaw("Vertical");


        if(cc.isGrounded){
            num_jumps = 2;
            speed_up = -0.2f;//force it to check the ground

            fuel_time += (float)(Time.deltaTime) * 0.75f;//recover at 75% rate
            fuel_time = Mathf.Min(fuel_time,fuel_time_max);//cap it at the max amount

        }else{
            speed_up = speed_up - (gravity * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.P)){
            reset_player();
        }

        
        if(Input.GetKeyDown(KeyCode.Space) && (num_jumps > 0)){
            num_jumps--;
            speed_up = jump_speed;
        }
        

        if(Input.GetKey(KeyCode.LeftControl) && (fuel_time > 0f)){
            if(cc.isGrounded){
                speed_up = 0f;
            }
            else{
                speed_up += gravity * 1.05f * Time.deltaTime;//only be 5% stronger than gravity
            }
            
            fuel_time -= Time.deltaTime;
        }
        

        if(Input.GetKeyDown(KeyCode.LeftShift) && (dash_cooldown < 0.0f)){
            dash_amount = dash_mult;
            dash_cooldown = dash_cooldown_max;
            speed_up = 0.1f;
        }
        float dash_speed_mult = 1.0f + dash_amount;
        

        
        float speed_forward = vaxis * Time.deltaTime * base_speed * -1 * dash_speed_mult;
        float speed_side = haxis * Time.deltaTime * base_speed * dash_speed_mult;

        


        Vector3 to_move = new Vector3(speed_forward,speed_up,speed_side);
        to_move += platform_delta;
        

        float squash_scale = 1f;
        if(num_jumps < 2 || fuel_time < fuel_time_max){//you are in the air because you have jumped or rocket boosted
            if(Mathf.Abs(speed_up) < 0.3f){
                squash_scale = (Mathf.Abs(speed_up) + 0.06f) * 3f;
            }
        }

        Transform visuals_transform = transform.Find("visuals");//the child "visuals" object
        visuals_transform.localScale = new Vector3(1f,squash_scale,1f);
        
        visuals_transform.rotation = Quaternion.Euler(speed_side*45.0f,0,-1*speed_forward*45.0f);
        //visuals_transform.rotation = Quaternion.LookRotation(to_move);


        cc.Move(to_move);

        Vector3 horizontal_move = to_move; horizontal_move.y = 0;
        double h_speed = horizontal_move.magnitude;
        

        if((dash_amount > dash_mult * 0.5f) && (!dirt_visible)){
            dirt_visible = true;
            dirt_instance = Instantiate<GameObject>(dirt_trail);
        }
        if(dash_amount < dash_mult * 0.05){
            dirt_visible = false;
            Destroy(dirt_instance);
        }

        if(dirt_visible){
            dirt_instance.transform.position = transform.position;
            float dirt_angle = Mathf.Atan2(horizontal_move.z,-horizontal_move.x);//returns in radians
            dirt_angle = dirt_angle*360f/(2f*Mathf.PI);

            dirt_instance.transform.rotation = Quaternion.Euler(0f,dirt_angle,0f);


        }


    }

    void OnTriggerEnter(Collider other){//this one for the spikes
        if(other.gameObject.CompareTag("death")){
            reset_player();
        }

        if(other.gameObject.CompareTag("key")){
            keys_collected++;
            respawn_loc = other.gameObject.transform.position;
            Destroy(other.gameObject);
        }
    }

    
    void OnControllerColliderHit(ControllerColliderHit other){//this one for the ground
        GameObject otro = other.collider.gameObject;
        if(otro.CompareTag("death")){
            reset_player();
        }

    }
    

    void OnTriggerStay(Collider other){
        if(other.gameObject.CompareTag("movingplatform")){
            movingPlatformScript platform_script = other.gameObject.GetComponent<movingPlatformScript>();
            platform_delta = platform_script.delta;
            
        }
        
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("movingplatform")){
            platform_delta = Vector3.zero;
            
        }
    }

    

}
