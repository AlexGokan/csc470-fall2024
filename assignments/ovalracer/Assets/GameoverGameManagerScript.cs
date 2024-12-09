using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameoverGameManagerScript : MonoBehaviour
{
    public Button return_to_menu_button;
    
    // Start is called before the first frame update
    void Start()
    {
        return_to_menu_button.onClick.AddListener(back_to_menu);
    }

    void back_to_menu(){
        SceneManager.LoadScene("MainMenuScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
