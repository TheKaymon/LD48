using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string startingSceneName;

    public OptionsMenu menu;

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void StartGame()
    {
        SceneManager.LoadScene(startingSceneName);
    }

    public void ShowMenu()
    {
        Screen.fullScreen = !Screen.fullScreen;
        //menu.gameObject.SetActive(true);
    }

    public void HideMenu()
    {
       //menu.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
