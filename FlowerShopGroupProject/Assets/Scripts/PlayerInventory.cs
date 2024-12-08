using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    private FlowerLifeCycle flowerLifeCycle;

    public GameObject bouquetSlotPrefab;
    public Transform bouquetSlotTransform;
    private int bouquetCount = 0;
    private GameObject bouquetSlot;
    private bool hasBouquet = false; // for later dont worry 

    public GameObject flowerSlotPrefab;
    public Transform flowerGridTransform; // Reference to FlowerGrid in the InventoryPanel
    private Dictionary<string, int> flowerInventory = new Dictionary<string, int>();
    private Dictionary<string, GameObject> flowerSlots = new Dictionary<string, GameObject>();

    public Sprite cactus1;
    public Sprite daisy1;
    public Sprite monstera1;
    public Sprite orchid1;
    public Sprite sunflower1;
    public Sprite venusflytrap1;


    public Text feedbackText; // Reference to a UI Text component for feedback messages
    public int coins = 10;

    //************************  Get things started&Initialized ************************//
    void Start()
    {
        flowerLifeCycle = GetComponent<FlowerLifeCycle>();

        InitializeFlowerSlot("Cactus", cactus1);
        InitializeFlowerSlot("Daisy", daisy1);
        InitializeFlowerSlot("Monstera", monstera1);
        InitializeFlowerSlot("Orchid", orchid1);
        InitializeFlowerSlot("SunFlower", sunflower1);
        InitializeFlowerSlot("VenusFlyTrap", venusflytrap1);

        // Initializes bouquet slot as a single slot
        InitializeBouquetSlot();
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
        TextMeshProUGUI coinText = GameObject.Find("CoinText")?.GetComponent<TextMeshProUGUI>();
        if (coinText != null)
        {
            coinText.text = " " + coins.ToString();
        }
    }

    ///////*************************** Add to Inventory ***************************///////
    private void InitializeFlowerSlot(string flowerType, Sprite icon)
    {
        // Instantiate the slot and add it to the inventory grid
        GameObject newFlowerSlot = Instantiate(flowerSlotPrefab, flowerGridTransform);
        flowerSlots[flowerType] = newFlowerSlot;

        // slot's icon and initial quantity text
        Image iconImage = newFlowerSlot.GetComponentInChildren<Image>();
        Text quantityText = newFlowerSlot.GetComponentInChildren<Text>();

        iconImage.sprite = icon;
        quantityText.text = "0";

        // Initializes the flower count to 0 in the dictionary
        flowerInventory[flowerType] = 0;
    }

    // for bouquet slot
    private void InitializeBouquetSlot()
    {
        bouquetSlot = Instantiate(bouquetSlotPrefab, bouquetSlotTransform);

        Text quantityText = bouquetSlot.GetComponentInChildren<Text>();
        quantityText.text = "0";
    }
    //

    public void AddFlowerToInventory(string flowerType)
    {
        // Increment the flower count and update the quantity text
        if (flowerInventory.ContainsKey(flowerType))
        {
            flowerInventory[flowerType]++;
            UpdateFlowerSlot(flowerType);
        }
        else
        {
            Debug.LogWarning($"Flower type {flowerType} does not have a slot in the inventory.");
        }
    }

    private void UpdateFlowerSlot(string flowerType)
    {
        if (flowerSlots.TryGetValue(flowerType, out GameObject slot))
        {
            Text quantityText = slot.GetComponentInChildren<Text>();
            quantityText.text = flowerInventory[flowerType].ToString();
        }
    }

    // ------------------------------- bouquet logic ---------------------------------------- //

    private void UpdateBouquetSlot()
    {
        Text quantityText = bouquetSlot.GetComponentInChildren<Text>();
        quantityText.text = bouquetCount.ToString();
    }

    public void TryCreateBouquet()
    {
        List<string> availableFlowers = new List<string>();

        foreach (var flower in flowerInventory)
        {
            if (flower.Value > 0)
            {
                availableFlowers.Add(flower.Key);
            }
        }
        // adjusted amount of flowers needed 
        if (availableFlowers.Count >= 6)
        {
            for (int i = 0; i < 4; i++)
            {
                string flowerType = availableFlowers[i];
                flowerInventory[flowerType]--;
                UpdateFlowerSlot(flowerType);
            }

            bouquetCount++;
            UpdateBouquetSlot();

            DisplayFeedback("Bouquet created successfully!");
        }
        else
        {
            DisplayFeedback("Not enough different flowers to create a bouquet.");
        }

        // Notify BouquetSource to update the toolbar
        BouquetSource.Instance?.UpdateBouquetCountText();
    }

    private void DisplayFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            CancelInvoke("ClearFeedback");
            Invoke("ClearFeedback", 2f); // Clears the feedback message after 2 seconds
        }
    }

    private void ClearFeedback()
    {
        feedbackText.text = "";
    }

    // stuff for using bouquet
    public bool HasBouquet()
    {
        return bouquetCount > 0;
    }

    public void UseBouquet()
    {
        if (bouquetCount > 0)
        {
            bouquetCount--;
            UpdateBouquetSlot();
            Debug.Log("Bouquet used! Remaining bouquets: " + bouquetCount);
        }
        else
        {
            Debug.LogWarning("No bouquets available to use!");
        }
        // Notify BouquetSource to update the toolbar
        BouquetSource.Instance?.UpdateBouquetCountText();
    }

    // for toolbar 
    public int BouquetCount
    {
        get { return bouquetCount; }
    }
}