using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class sphere_contact_script : MonoBehaviour
{

    int score = 0;
    public TMP_Text score_text;//drag this in from the unity UI
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Score: "+score);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other){//gets called if the trigger check is active on a collider
        Debug.Log("Hello");
        score++;
        Debug.Log("Score: "+score);

        Destroy(other.gameObject);

        score_text.text = "Score: "+score;//modify the score text objects string value
    }

    public void OnCollisionEnter(Collision col){
        return;
    }
}
