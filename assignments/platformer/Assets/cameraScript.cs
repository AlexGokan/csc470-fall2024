using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    

    public TextMeshProUGUI gui_tmp;//UGUI???
    public GameObject player;

    characterScript cs;

    public GameObject blood;

    Renderer blood_renderer;

    void Start()
    {
    
    GameObject canvas = transform.GetChild(0).gameObject;
    cs = player.GetComponent<characterScript>();

    blood_renderer = blood.GetComponent<Renderer>();
    
    }

    void Update()
    {
        string final_gui_text = "";
        string dash_string = "Dash (shift) cooldown: ";

        float dash_cooldown_pct = cs.dash_cooldown / cs.dash_cooldown_max;
        dash_cooldown_pct = Mathf.Max(0.0f,dash_cooldown_pct);//bring it to 0 if its below
        int number_of_dashes = (int)(dash_cooldown_pct * 20);
        string dash_dashes = new string('-',number_of_dashes);

        dash_string = dash_string + dash_dashes + '\n';
        final_gui_text += dash_string;

        string fuel_string = "Rocket fuel (ctrl): ";
        float f = cs.fuel_time / cs.fuel_time_max;
        number_of_dashes = (int)(f*20);
        string fuel_dashes = new string('-',number_of_dashes);

        fuel_string += fuel_dashes + "\n";
        final_gui_text += fuel_string;



        string keys_string = "Keys collected: ";
        int k =  cs.keys_collected;
        keys_string += k.ToString();
        keys_string += "/" + cs.num_keys_needed.ToString();
        keys_string += "\n";

        final_gui_text += keys_string;


        gui_tmp.SetText(final_gui_text);


        float blood_transparency = cs.time_since_death / 1.5f;
        blood_transparency = Mathf.Min(blood_transparency,1.0f);
        blood_transparency = 1f - blood_transparency;

        blood_renderer.material.color = new Color(1f,0f,0f,blood_transparency);
    }
}
