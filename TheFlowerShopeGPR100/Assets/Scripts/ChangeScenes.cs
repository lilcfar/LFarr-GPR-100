using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class ChangeScenes : MonoBehaviour
{
    public GameObject optionsPanel;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionsMenu();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("FlowerScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }    

    public void LeftLoadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        int nextSceneIndex = scene.buildIndex - 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void RightLoadScene()
    {
        Scene scene =SceneManager.GetActiveScene();
        int nextSceneIndex = scene.buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void OptionsMenu()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseMenu()
    {
        optionsPanel.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Start Screen");
    }
}
