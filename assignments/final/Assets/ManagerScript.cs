using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/*

The game manager will use coroutines to handle the large-scale structure of the race
One coroutine will handle the race
One coroutine will handle the podium


*/



public class ManagerScript : MonoBehaviour
{

    public GameObject player_character;
    riderScript player_rider_script;


    public GameObject barrier_left;

    public GameObject barrier_right;

    public GameObject[] ai_riders_go;

    List<riderScript> all_riders = new List<riderScript>();

    IEnumerator RaceMainLoop(){
        
        for(int i=0; i<all_riders.Count; i++){
            all_riders[i].GetComponent<riderScript>().start_race();
        }
        
        
        float best_distance = 0f;
        
        while(best_distance < 250f){
            player_rider_script.controllable_update_step();
            best_distance = Mathf.Max(best_distance,player_rider_script.downtrack_pos);

            for(int i=0; i<ai_riders_go.Length; i++){
                riderScript rs = ai_riders_go[i].GetComponent<riderScript>();
                rs.ai_update_step();
                best_distance = Mathf.Max(best_distance,rs.downtrack_pos);
            }


            yield return null;
        }

        //we've finished the race

        Debug.Log("race is finished");
    }


    // Start is called before the first frame update
    void Start()
    {
        
        //get all ai characters and populate
        ai_riders_go = GameObject.FindGameObjectsWithTag("aiCharacter");
        

        player_rider_script = player_character.GetComponent<riderScript>();
        player_rider_script.strategy = -1;

        for(int i=0; i<ai_riders_go.Length; i++){
            ai_riders_go[i].GetComponent<riderScript>().strategy = i%3;
            all_riders.Add(ai_riders_go[i].GetComponent<riderScript>());
        }
        all_riders.Add(player_rider_script);//create a list with every rider
        



        StartCoroutine(RaceMainLoop());


        //instantiate a row of cubes at the barriers for me to look at for speed reference
        for(int i=0; i<150; i++){
            GameObject bl = Instantiate(barrier_left);
            bl.transform.position += i * new Vector3(0f,0f,1f) * 14;

            GameObject br = Instantiate(barrier_right);
            br.transform.position += i * new Vector3(0f,0f,1f) * 14;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
