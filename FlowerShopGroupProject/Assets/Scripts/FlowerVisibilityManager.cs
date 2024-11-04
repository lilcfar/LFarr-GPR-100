using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowerVisibilityManager : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Scene2")
        {
            SetFlowersActive(true);
        }
        else
        {
            SetFlowersActive(false);
        }
    }

    private void SetFlowersActive(bool isActive)
    {
        GameObject[] flowers = GameObject.FindGameObjectsWithTag("Flower");
        foreach (var flower in flowers)
        {
            flower.SetActive(isActive);
        }
    }
}
