using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rule_script : MonoBehaviour
{

    public int responds_to;
    public bool result;


    public Renderer c1_renderer;
    public Renderer c2_renderer;
    public Renderer c4_renderer;
    public Renderer cr_renderer;

    public Color zero_color = new Color(0f,0f,0f);
    public Color one_color = new Color(255f,255f,255f);


    // Start is called before the first frame update
    void Start()
    {
        update_colors();

    }

    void update_colors(){
        if(result){
            cr_renderer.material.color = one_color;
        }else{
            cr_renderer.material.color = zero_color;
        }


        string binary_rep = System.Convert.ToString(responds_to,2);
        binary_rep = binary_rep.PadLeft(3,'0');

        if(binary_rep[0] == '0'){
            c4_renderer.material.color = zero_color;
        }else{
            c4_renderer.material.color = one_color;
        }

        if(binary_rep[1] == '0'){
            c2_renderer.material.color = zero_color;
        }else{
            c2_renderer.material.color = one_color;
        }

        if(binary_rep[2] == '0'){
            c1_renderer.material.color = zero_color;
        }else{
            c1_renderer.material.color = one_color;
        }


    }

    public void swap_output(){
        result = !result;
        update_colors();
    }

    void OnMouseDown(){
        swap_output();
    }

    

    // Update is called once per frame
    void Update()
    {

    }
}
