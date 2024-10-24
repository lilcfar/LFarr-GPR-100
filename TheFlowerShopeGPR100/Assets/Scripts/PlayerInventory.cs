using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public int coins = 0; // Stores collected coins
    public Dictionary<string, int> flowerSeeds = new Dictionary<string, int>(); // To store seeds (flower type and quantity)

    void Awake()
    {
        // Ensures one instance of the inventory (Singleton)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // So the inventory persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add coins to the player's inventory
    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log("Coins in inventory: " + coins);
    }

    // Add seeds to the player's inventory
    public void AddSeeds(string flowerType, int amount)
    {
        if (flowerSeeds.ContainsKey(flowerType))
        {
            flowerSeeds[flowerType] += amount;
        }
        else
        {
            flowerSeeds[flowerType] = amount;
        }
        Debug.Log(flowerType + " seeds in inventory: " + flowerSeeds[flowerType]);
    }

    // Remove seeds from the player's inventory (when planting)
    public bool UseSeed(string flowerType)
    {
        if (flowerSeeds.ContainsKey(flowerType) && flowerSeeds[flowerType] > 0)
        {
            flowerSeeds[flowerType]--;
            Debug.Log(flowerType + " seeds remaining: " + flowerSeeds[flowerType]);
            return true; // Successfully used a seed
        }
        else
        {
            Debug.Log("No seeds of type " + flowerType + " left.");
            return false; // No seeds to use
        }
    }
}
