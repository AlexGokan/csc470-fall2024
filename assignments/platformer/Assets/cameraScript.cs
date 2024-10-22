using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    

    public TextMeshProUGUI gui_tmp;//UGUI???
    public GameObject player;

    characterScript cs;

    void Start()
    {
    
    GameObject canvas = transform.GetChild(0).gameObject;
    cs = player.GetComponent<characterScript>();


    }

    void Update()
    {
        string final_gui_text = "";
        string dash_string = "Dash cooldown: ";

        float dash_cooldown_pct = cs.dash_cooldown / cs.dash_cooldown_max;
        dash_cooldown_pct = Mathf.Max(0.0f,dash_cooldown_pct);//bring it to 0 if its below
        int number_of_dashes = (int)(dash_cooldown_pct * 20);
        string dash_dashes = new string('-',number_of_dashes);

        dash_string = dash_string + dash_dashes + '\n';
        final_gui_text += dash_string;


        gui_tmp.SetText(final_gui_text);
    }
}
