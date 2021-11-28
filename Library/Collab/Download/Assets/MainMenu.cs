using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Map map;
    // Start is called before the first frame update
    public void StartGame(){
        map.generateMap();
        SceneManager.LoadScene(1);
    }
    
    public void Exit(){
        Debug.Log("Quitting");
        Application.Quit();
        
    }
}
