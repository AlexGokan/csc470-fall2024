using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{


    public NavMeshAgent nma;

    float amount_rotated = 0f;

    Vector3 start_pos;

    public bool arrived_home;

    // Start is called before the first frame update
    void Start()
    {
        start_pos = transform.position;  

        StartCoroutine(chairBehavior()); 
    }


    void Update(){

    }

    void UpdateBad()//STUPID BAD 
    {
        
        
        
        GameObject nearest_chair = getNearestChair();


        nma.SetDestination(nearest_chair.transform.position);

        if(nearest_chair != null){
            float dist_to_chair = Vector3.Distance(transform.position,nearest_chair.transform.position);
            if(dist_to_chair < 0.3){
                Debug.Log("Arrived");
                //do a spin
                amount_rotated += 40*Time.deltaTime;
                transform.Rotate(0,40*Time.deltaTime,0);

                if(amount_rotated > 360f){
                    nma.SetDestination(start_pos);
                }
            }
        }
        
    }

    GameObject getNearestChair(){
        GameObject g = GameObject.FindGameObjectWithTag("Chair");

        return g;
    }

    IEnumerator chairBehavior(){
        GameObject nc = GameObject.FindGameObjectWithTag("Chair");
        float dist_to_chair = Vector3.Distance(transform.position,nc.transform.position);
        nma.SetDestination(nc.transform.position);

        Debug.Log(dist_to_chair);

        while(dist_to_chair > 2.5f){
            Debug.Log(dist_to_chair);
            yield return null;//I'm done with this function rn, check in RIGHT HERE after you finish this update cycle
            dist_to_chair = Vector3.Distance(transform.position,nc.transform.position);
        }

        Debug.Log("Arrived at chair");

        //do a spin
        amount_rotated = 0f;
        while(amount_rotated < 360f){
            amount_rotated += 90*Time.deltaTime;
            transform.Rotate(0,90*Time.deltaTime,0);

            yield return null;
        }

        Debug.Log("Done spinning");

        //go home
        float dist_to_start = Vector3.Distance(transform.position,start_pos);
        nma.SetDestination(start_pos);
        while(dist_to_start < 0.3f){
            yield return null;
            dist_to_start = Vector3.Distance(transform.position,start_pos);
        }

        Debug.Log("arrived home");
        arrived_home = true;

        yield return new WaitForSeconds(2);

        arrived_home = false;
    }

}


