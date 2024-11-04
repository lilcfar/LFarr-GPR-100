using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
    public static WaterSource Instance;
    private bool hasWater = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //  function called when  player clicks water sprite
    public void CollectWater()
    {
        hasWater = true;
        Debug.Log("Water collected!");
    }

    // function called to use water on a wilted flower
    public bool UseWater()
    {
        if (hasWater)
        {
            hasWater = false;
            Debug.Log("Water used!");
            return true;
        }
        return false;
    }

    // Check if player has water
    public bool HasWater()
    {
        return hasWater;
    }

    private void OnMouseDown()
    {
        Debug.Log("Watersource clicked!");
        CollectWater();
        //WaterSource.Instance.CollectWater();
    }

}
