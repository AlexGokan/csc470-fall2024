using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class controlScript : MonoBehaviour
{

    public GameObject camera_obj;//reference to the camera which will follow the plane
    
    public Terrain ground;

    public TMP_Text score_display;
    public TMP_Text gameover_display;

    public GameObject explosion;

    float prevtime;
    float forward_speed = 0.5f;
    float rot_speed = 15.0f;

    float time_left = 5;
    int score_needed = 6;

    Quaternion starting_rot;

    int score = 0;

    bool game_is_over;

    // Start is called before the first frame update
    void Start()
    {
        score_display.text = "Score: 0\nTime: "+time_left;
        starting_rot = transform.rotation;

        game_is_over = false;
    }

    

    // Update is called once per frame
    void Update()
    {

    time_left -= Time.deltaTime;

    float terrain_height = ground.SampleHeight(transform.position) + ground.GetPosition().y;
    float height_diff = transform.position.y - terrain_height;
    Debug.Log("height diff: "+height_diff);

    score_display.text = "Score: "+score+"\nTime: "+time_left;

    if(height_diff <= 0){//the player crashed
        transform.position = new Vector3(0.0f,0.0f,0.0f);//back to the start
        transform.rotation = starting_rot;//reset to what it was when the game started
    }

    if(time_left < 0 && score < score_needed){
        game_is_over = true;
        gameover_display.text = "GAME OVER!!!";

        explosion.transform.localScale = new Vector3(1f,1f,1f) * 400.0f;
    }

    bool roll_left = false;
    bool roll_right = false;


    if(Input.GetKey(KeyCode.RightShift)){
        forward_speed = -10f;
    }
    else if(Input.GetKey(KeyCode.LeftShift)){
        forward_speed = 2.0f;
    }
    else{
        forward_speed = 10f;
    }

    if(Input.GetKey(KeyCode.Q)){
        roll_left = true;
    }
    if(Input.GetKey(KeyCode.E)){
        roll_right = true;
    }

    float h_axis = Input.GetAxis("Horizontal");
    float v_axis = Input.GetAxis("Vertical");


    //Debug.Log("Score: "+score);
    if(!game_is_over){
        Vector3 to_rotate = new Vector3(0,0,0);
        to_rotate.x = v_axis * rot_speed;
        to_rotate.y = h_axis * rot_speed;

        float roll_speed = 10.0f;
        if(roll_left && !roll_right){
            to_rotate.z = roll_speed;
        }
        if(roll_right && !roll_left){
            to_rotate.z = - roll_speed;
        }

        to_rotate *= Time.deltaTime;
        
        transform.Rotate(to_rotate,Space.Self);
        transform.position += transform.forward*forward_speed*Time.deltaTime;


        
        
        //position the camera with the plane, and lock its rotation
        
        Vector3 camera_pos = transform.position;
        camera_pos += -transform.forward * 0.8f;//move back
        camera_pos += Vector3.up*0.4f;//move up
        camera_obj.transform.position = camera_pos;

        //camera_obj.transform.Rotate(0.0f,0.0f,transform.rotation.z);//make the camera copy the z roration of the plane (roll)
        //doesnt seem to be working


        Vector3 lookat_offset = 10f * transform.forward;
        Vector3 to_look_at = lookat_offset + transform.position;
        camera_obj.transform.LookAt(to_look_at);
        //camera_obj.transform.LookAt(to_look_at,transform.rotation.eulerAngles);
    }    

    
    
    

    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Coin")){
            Destroy(other.gameObject);
            score += 1;
        }
    }
}
