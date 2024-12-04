using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Linq;//needed to use the order by method



public class ManagerScript : MonoBehaviour
{

    public GameObject player_character;
    riderScript player_rider_script;


    public GameObject barrier_left;

    public GameObject barrier_right;

    public GameObject[] ai_riders_go;

    List<riderScript> all_riders = new List<riderScript>();

    
    public GameObject fadeout;

    IEnumerator fadeout_and_go_to_podium(){
        Renderer fadeout_rend = fadeout.GetComponent<Renderer>();
        Color c = fadeout_rend.material.color;

        int timescale = 240;
        for(int i=0; i<timescale; i++){
            c.a = (float)i/(float)timescale;
            fadeout_rend.material.color = c;
            yield return null;
        }

        //put the first, second, and third place riders into player prefs
        List<riderScript> riders_by_position = all_riders.OrderBy(o=>o.downtrack_pos).ToList();
        
        PlayerPrefs.SetInt("firstplace",riders_by_position[0].index);
        PlayerPrefs.SetInt("secondplace",riders_by_position[1].index);
        PlayerPrefs.SetInt("thirdplace",riders_by_position[2].index);


        //now load new scene
        SceneManager.LoadScene("PodiumScene");
    }

    IEnumerator RaceMainLoop(){
        
        Renderer fadeout_rend = fadeout.GetComponent<Renderer>();
        fadeout_rend.material.color = new Color(0f,0f,0f,0f);

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
        //StartCoroutine(fadeout_and_go_to_podium());

    }


    // Start is called before the first frame update
    void Start()
    {
        
        List<Color> jersey_colors = new List<Color>{Color.red,Color.green,Color.blue,Color.cyan,Color.white,Color.magenta};
        
        //get all ai characters and populate
        ai_riders_go = GameObject.FindGameObjectsWithTag("aiCharacter");
        

        player_rider_script = player_character.GetComponent<riderScript>();
        player_rider_script.strategy = -1;
        player_rider_script.index = -1;
        player_rider_script.jersey_color = Color.yellow;


        for(int i=0; i<ai_riders_go.Length; i++){
            ai_riders_go[i].GetComponent<riderScript>().strategy = i%3;
            ai_riders_go[i].GetComponent<riderScript>().index = i;
            ai_riders_go[i].GetComponent<riderScript>().jersey_color = jersey_colors[i];
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
