using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StudioPause : MonoBehaviour
{
    public GameObject Menu;
    bool MenuOpen;
    // Start is called before the first frame update
    private void Start()
    {
        Menu.SetActive(false);
        MenuOpen = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MenuOpen == false)
            {
                Menu.SetActive(true);
                MenuOpen = true;
            }
            else
            {
                Menu.SetActive(false);
                MenuOpen = false;
            }
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Start");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Continue()
    {
        SceneManager.LoadScene("Studio");
        Menu.SetActive(false);
    }
}
