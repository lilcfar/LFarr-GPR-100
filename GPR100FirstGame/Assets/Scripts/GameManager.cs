using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Text itemCounterText;
    public Text resetCounterText;

    public int itemCount = 0; 
    public int totalKeysRequired = 3;
    public int resetCount = 0;
    public int allowedAttempts = 8;
    private bool touchDoor = false;

    void start()
    {
        updateCounter();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }

    }


    public void AddResets(int count)
    {
        resetCount += count;
        // updateCounter();
        Debug.Log("Reset count: " + resetCount);
        if (resetCount >= allowedAttempts)
        {
            Debug.Log("You lose!");
            SceneManager.LoadScene("LoseScene");
        }
    }

  

    public void AddItem(int value)
    {
        itemCount += value;
        updateCounter();
        if (itemCount >= totalKeysRequired)
        {
            UnlockNextLevel();
        }
    }

    private void updateCounter()
    {
        itemCounterText.text = "KEYS/3: " + itemCount;
        // resetCounterText.text = "RESETS: " + resetCount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            touchDoor = true;
            // Call the function to load the win scene or show the win UI
            UnlockNextLevel();
        }
    }

    private void UnlockNextLevel()
    {
        if (touchDoor == true && itemCount >= totalKeysRequired)
        {
            Debug.Log("Next level unlocked.");
            SceneManager.LoadScene("LvlTwoScene");
        }
        else
        {
            touchDoor = false;
        }
    }

   
}
