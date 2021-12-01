using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Simple main menu code that either starts or ends the game depending on what button you press.
public class MainMenu : MonoBehaviour
{
    public void StartGame(){
        SceneManager.LoadScene(1);
    }
    
    public void Exit(){
        Debug.Log("Quitting");
        Application.Quit();        
    }
}
