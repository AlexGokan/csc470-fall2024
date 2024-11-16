using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlatformerController : MonoBehaviour
{

    public CharacterController cc;

    float rot_speed = 90;
    float move_speed = 12;

    float yvel = 0;

    float gravity = -9.8f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float haxis = Input.GetAxis("Horizontal");
        float vaxis = Input.GetAxis("Vertical");

        transform.Rotate(0,rot_speed*haxis*Time.deltaTime,0);

        
        if(!cc.isGrounded){//in the air
            yvel += gravity* Time.deltaTime;

            if(Input.GetKeyUp(KeyCode.Space) && yvel>0){
                yvel = 0;
            }
        }else{//grounded
            yvel = -2f;//force it to constantly check collision against the ground
            if(Input.GetKeyDown(KeyCode.Space)){
                yvel = 10.0f;
            }
        }
        UnityEngine.Vector3 m = new UnityEngine.Vector3(0f,0f,0f);
        m += transform.forward * move_speed * vaxis * Time.deltaTime;
        m.y += yvel * Time.deltaTime;

        Debug.Log(m);

        cc.Move(m);

    }
}
