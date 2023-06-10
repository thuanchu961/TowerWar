using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

   
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        Debug.Log("play game");
    }

    public void Options()
    {

    }
}
