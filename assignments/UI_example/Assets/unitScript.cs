using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



//https://discussions.unity.com/t/a-guide-on-using-the-new-ai-navigation-package-in-unity-2022-lts-and-above/371872

public class unitScript : MonoBehaviour
{

    public string name;
    public string bio;
    public string stats;

    public bool selected = false;

    public Color selected_color;
    public Color deselected_color;

    public Renderer rend;

    float rot_speed;

    public GameObject wallseeing_sphere;


    void OnEnable(){
        managerScript.instance.SpacebarPressed += changeToRandomColor;//weird syntax. Means to call changeToRandomColor when managerscript sends SpaceBarPressed
        managerScript.instance.UnitClicked += managerSaysUnitClicked;
    }

    void changeToRandomColor(){
        rend.material.color = new Color(Random.value,Random.value,Random.value);
    }

    void managerSaysUnitClicked(unitScript unit){
        if(unit == this){
            selected = true;
            rend.material.color = selected_color;
        }else{
            selected = false;
            rend.material.color = deselected_color;
        }
    }

    void OnDisable(){
        managerScript.instance.SpacebarPressed -= changeToRandomColor;//have to unsubscribe when we disable the object
        managerScript.instance.UnitClicked += managerSaysUnitClicked;
    }

    // Start is called before the first frame update
    void Start()
    {
        managerScript.instance.name_text = "";

        managerScript.instance.units.Add(this);

        transform.Rotate(0,Random.Range(0,360),0);

        rot_speed = Random.Range(40f,70f);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,rot_speed * Time.deltaTime, 0f);

        Vector3 ray_start = transform.position + 1.5f*Vector3.up;
        Vector3 ray_dir = transform.forward * 5;
        Color ray_color;

        LayerMask lm = LayerMask.GetMask("Wall");

        RaycastHit hit;
        bool coll = Physics.Raycast(ray_start,transform.forward,out hit,float.PositiveInfinity,lm);//this tells it to only check for raycasts against the wall

        if(coll){//we hit something            
            if(hit.collider.gameObject.CompareTag("wall")){
                wallseeing_sphere.SetActive(true);
                wallseeing_sphere.transform.position = hit.point;
            }else{
                //Debug.Log(hit.collider.gameObject.GetComponent<unitScript>().name);
            }

            ray_color = Color.red;
        }else{
            wallseeing_sphere.SetActive(false);
            ray_color = Color.black;
        }

        Debug.DrawRay(ray_start,transform.forward*5,ray_color);
    }

    void OnMouseDown(){
        managerScript.instance.OpenCharacterSheet(this);
        managerScript.instance.selectUnit(this);
    }

    void OnDestroy(){
        managerScript.instance.units.Remove(this);
    }


}
