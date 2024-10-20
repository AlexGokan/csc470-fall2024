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


    // Start is called before the first frame update
    void Start()
    {
        dash_amount = 0.0f;
        speed_up = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float haxis = Input.GetAxis("Horizontal");
        float vaxis = Input.GetAxis("Vertical");


        if(cc.isGrounded){
            num_jumps = 2;
            speed_up = -1f;//force it to check the ground
        }else{
            speed_up = speed_up - gravity * Time.deltaTime;
        }

        
        if(Input.GetKeyDown(KeyCode.Space) && (num_jumps > 0)){
            num_jumps--;
            speed_up = jump_speed;
        }
        



        if(Input.GetKeyDown(KeyCode.LeftShift)){
            dash_amount = dash_mult;
        }
        float dash_speed_mult = 1.0f + dash_amount;
        dash_amount *= 0.95f;

        float speed_forward = vaxis * Time.deltaTime * base_speed * -1 * dash_speed_mult;
        float speed_side = haxis * Time.deltaTime * base_speed * dash_speed_mult;



        Vector3 to_move = new Vector3(speed_forward,speed_up,speed_side);
        


        Transform visuals_transform = transform.Find("visuals");//the child "visuals" object
        visuals_transform.rotation = Quaternion.Euler(speed_side*20.0f,0,-1*speed_forward*20.0f);
        

        cc.Move(to_move);

        Vector3 horizontal_move = to_move; horizontal_move.y = 0;
        double h_speed = horizontal_move.magnitude;
        

        if((h_speed > 0.5) && (!dirt_visible)){
            dirt_visible = true;
            dirt_instance = Instantiate<GameObject>(dirt_trail);
        }
        if(h_speed < 0.1){
            dirt_visible = false;
            Destroy(dirt_instance);
        }

        if(dirt_visible){
            dirt_instance.transform.position = transform.position;
            float dirt_angle = Mathf.Atan2(horizontal_move.z,-horizontal_move.x);//returns in radians
            dirt_angle = dirt_angle*360f/(2f*Mathf.PI);

            dirt_instance.transform.rotation = Quaternion.Euler(0f,dirt_angle,0f);

            Debug.Log(dirt_angle);

        }

        



    }
}
