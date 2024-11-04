using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string scene1 = "Scene1";
    public string scene2 = "Scene2";

    public void ToggleScene()
    {
        // Checks active scene and switches to the other one
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == scene1)
        {
            SceneManager.LoadScene(scene2);
        }
        else
        {
            SceneManager.LoadScene(scene1);
        }
    }
}
