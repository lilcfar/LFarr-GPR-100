using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    private FlowerLifeCycle flowerLifeCycle;

    public int coins = 2;
   // public Text coinText;

    void Start()
    {
        flowerLifeCycle = GetComponent<FlowerLifeCycle>();
        //UpdateCoinText();
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
        UpdateCoinText();
    }

    public bool PurchaseFlower(int cost)
    {
        if (coins >= cost)
        {
            coins -= cost;
            Debug.Log("Flower purchased! Remaining coins: " + coins);
            UpdateCoinText();
            return true;

        }
        else
        {
            Debug.Log("Not enough coins to purchase flower.");
            return false;
        }
    }
    public void Update()
    {
        UpdateCoinText();
    }
    private void UpdateCoinText()
    {
        Text coinText = GameObject.Find("CoinText")?.GetComponent<Text>();
        if (coinText != null)
        {
            coinText.text = "Coins: " + coins.ToString();
        }
    }
}