using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MMGameManagerScript : MonoBehaviour
{
    
    public Button start_game_button;

    public Button speed_up_button, speed_down_button;
    public int rider_speed = 4;

    public TMP_Text speed_display;

    
    IEnumerator blackout_and_load_scene(){
        SceneManager.LoadScene("RaceScene");
        yield return null;
    }
    
    void Start()
    {
        start_game_button.onClick.AddListener(start_race);  
        speed_up_button.onClick.AddListener(speed_up); 
        speed_down_button.onClick.AddListener(speed_down); 

        speed_display.text = " " + rider_speed;
    }

    void start_race(){
        PlayerPrefs.SetInt("rider_speed",rider_speed);

        StartCoroutine(blackout_and_load_scene());
        //SceneManager.LoadScene("RaceScene");
    }

    void speed_down(){
        rider_speed--;
        rider_speed = Mathf.Max(1,rider_speed);
        Debug.Log(rider_speed);

        speed_display.text = " " + rider_speed;
    }

    void speed_up(){
        rider_speed++;
        rider_speed = Mathf.Min(rider_speed,6);
        Debug.Log(rider_speed);

        speed_display.text = " " + rider_speed;
    }
    
    void Update()
    {
        
    }
}
