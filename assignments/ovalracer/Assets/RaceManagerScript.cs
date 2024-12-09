using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManagerScript : MonoBehaviour
{

    public GameObject[] all_riders_go;

    public List<RiderScript> podium_riders;
    
    public bool done = false;

    public float cheer_amount;

    public int num_laps = 5;

    IEnumerator fade_out_and_go_to_menu(){
        StopAllCoroutines();

        SceneManager.LoadScene("GameOverScene");
        yield return null;
    }

    void Start()
    {
        all_riders_go = GameObject.FindGameObjectsWithTag("Rider");
        Debug.Log("number of riders: " + all_riders_go.Length);
    }

    // Update is called once per frame
    void Update()
    {



        int highest_lap_count = 0;

        for(int i=0; i<all_riders_go.Length; i++){
            highest_lap_count = Mathf.Max(highest_lap_count,all_riders_go[i].GetComponent<RiderScript>().laps);

            if(all_riders_go[i].GetComponent<RiderScript>().laps >= num_laps){
                podium_riders.Add(all_riders_go[i].GetComponent<RiderScript>());
            }
        }

        this.cheer_amount = ((float)highest_lap_count/(float)num_laps);
        
        if(podium_riders.Count >= 3 && done == false){
            StartCoroutine(fade_out_and_go_to_menu());
            done = true;
        }

    }
}
