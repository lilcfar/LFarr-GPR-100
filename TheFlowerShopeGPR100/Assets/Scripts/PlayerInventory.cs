using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    private FlowerLifeCycle flowerLifeCycle;

    public int coins = 0;

    void Start()
    {
        flowerLifeCycle = GetComponent<FlowerLifeCycle>();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log("Coins in inventory: " + coins);
    }

    public bool PurchaseFlower(int cost)
    {
        if (coins >= cost)
        {
            coins -= cost;
            Debug.Log("Flower purchased! Remaining coins: " + coins);
            return true;
        }
        else
        {
            Debug.Log("Not enough coins to purchase flower.");
            return false;
        }
    }
}