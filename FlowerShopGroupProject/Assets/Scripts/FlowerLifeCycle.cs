using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerLifeCycle : MonoBehaviour
{
    public enum FlowerState { Seed, Sprout, FullGrown, Wilted }
    public FlowerState currentState;

    // Harvest stuff
    public float timeToHarvest = 60f; // Time until the flower is ready to be harvested
    private bool isReadyToHarvest = false;

    private PlayerInventory playerInventory;

    // Growth cycle (grow wilt produce coins)
    public float timeToGrowSprout = 10f;
    public float timeToGrowFull = 20f;
    public float coinProductionInterval = 60f;
    public float timeToWilted = 120f;

    private float growthTimer = 0f;
    private bool isFullGrown = false;
    private bool isWilted = false;
    private bool hasCoinsReady = false;
    private float remainingWiltTime; // Tracks remaining wilt time

    // flower state sprites
    public Sprite seedSprite;
    public Sprite sproutSprite;
    public Sprite fullGrownSprite;
    public Sprite wiltedSprite;

    // need water feed back
    public GameObject needWaterPrefab;
    public Vector3 needWaterOffset = new Vector3(0.65f, .85f, 0f);
    private GameObject needWaterInstance;

    // coin ready feedback 
    public GameObject coinReadyPrefab;
    public Vector3 coinReadyOffset = new Vector3(0.65f, .85f, 0f);
    private GameObject coinReadyInstance;

    // Harvestable feedback
    public GameObject harvestablePrefab;
    public Vector3 harvestableOffset = new Vector3(0.65f, .85f, 0f);
    private GameObject harvestableInstance;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentState = FlowerState.Seed;
        playerInventory = FindObjectOfType<PlayerInventory>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = seedSprite;  // initial state as Seed for flower sprite
        remainingWiltTime = timeToWilted; // Initialize remaining time
        StartCoroutine(GrowFlower());
    }

    void Update()
    {
        if (isReadyToHarvest)
        {
            if (harvestableInstance == null)
            {
                Vector3 positionAboveFlower = transform.position + harvestableOffset;
                harvestableInstance = Instantiate(harvestablePrefab, positionAboveFlower, Quaternion.identity);
            }
        }
        if (isReadyToHarvest && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                HarvestFlower();
            }
        }
    }

    private void OnMouseDown()
    {
        if (isWilted)
        {
            ReviveWithWater();
        }
        else
        {
            CollectCoins(); // Collect coins if not wilted
        }
    }

    // grows the flower and sets is full grown to true and waits / sets harvestable true
    // maybe just add some if spade from tool bar selected then you can harvest so its optional?
    IEnumerator GrowFlower()
    {
        while (!isFullGrown)
        {
            growthTimer += Time.deltaTime;

            if (currentState == FlowerState.Seed && growthTimer >= timeToGrowSprout)
            {
                currentState = FlowerState.Sprout;
                spriteRenderer.sprite = sproutSprite;
            }
            else if (currentState == FlowerState.Sprout && growthTimer >= timeToGrowFull)
            {
                currentState = FlowerState.FullGrown;
                spriteRenderer.sprite = fullGrownSprite;
                isFullGrown = true;
                // full grown is true so call to start producing currency and wilting cycle
                StartCoroutine(ProduceCoins());
                StartCoroutine(WiltFlower());
            }
            yield return null;
        }
        // once full grown, set flower to be harvestable after a delay
        yield return new WaitForSeconds(timeToHarvest);
        isReadyToHarvest = true;
    }

    IEnumerator ProduceCoins()
    {
        while (isFullGrown)
        {
            // Wait for the coin production interval only if the flower is not wilted
            while (isWilted)
            {
                yield return null; // Pauses the coroutine while the flower is wilted
            }

            yield return new WaitForSeconds(coinProductionInterval);

            if (!isWilted) // Coins only ready if flower  not wilted
            {
                hasCoinsReady = true;
                Debug.Log("Coins ready to harvest!");

                // Instantiate the coinReadyPrefab above the flower if it hasn't been instantiated yet
                if (coinReadyInstance == null)
                {
                    Vector3 positionAboveFlower = transform.position + coinReadyOffset;
                    coinReadyInstance = Instantiate(coinReadyPrefab, positionAboveFlower, Quaternion.identity);
                }
            }
        }
    }

    IEnumerator WiltFlower()
    {
        while (isFullGrown && !isWilted)
        {
            yield return new WaitForSeconds(remainingWiltTime);
            currentState = FlowerState.Wilted;
            spriteRenderer.sprite = wiltedSprite;
            isWilted = true;
            Debug.Log("flower wilted and needs water");

            if (needWaterInstance == null)
            {
                Vector3 positionAboveFlower = transform.position + needWaterOffset;
                needWaterInstance = Instantiate(needWaterPrefab, positionAboveFlower, Quaternion.identity);
                // hasCoinsReady = false; /// this is where must do thing set active false? 
            }
        }
    }

    // Function called when  player clicks on the flower to collect coins
    public void CollectCoins()
    {
        if (hasCoinsReady)
        {
            hasCoinsReady = false;
            Debug.Log("Coins collected!");
            int coinsCollected = 1;
            PlayerInventory.Instance.AddCoins(coinsCollected); // Use Instance to add coins

            // Destroy the coinReadyInstance after collecting coins
            if (coinReadyInstance != null)
            {
                Destroy(coinReadyInstance);
                coinReadyInstance = null;
            }
        }
    }

    // Function to revive wilted flower
    public void ReviveWithWater()
    {
        if (isWilted && WaterSource.Instance.UseWater())
        {
            isWilted = false;
            isFullGrown = true; // Flower returns to full-grown state
            currentState = FlowerState.FullGrown;
            spriteRenderer.sprite = fullGrownSprite;

            if (needWaterInstance != null)
            {
                Destroy(needWaterInstance);
                needWaterInstance = null;
                // add sm logic here to handle redisplaying coins (or maybe setactive true?)
            }

            remainingWiltTime = timeToWilted; // Reset the wilt timer
            StartCoroutine(ProduceCoins()); // Restart coin production
            StartCoroutine(WiltFlower()); // Restart wilting cycle
            Debug.Log("Flower revived with water!");
        }
    }

    private void HarvestFlower()
    {
        if (isReadyToHarvest)
        {
            // Removea "Clone" part of the name to match the inventory keys
            string flowerType = this.gameObject.name.Replace("(Clone)", "").Trim();
            // Adds flower to inventory
            playerInventory.AddFlowerToInventory(flowerType);
            // destroy flower feedback after harvesting 
            Destroy(coinReadyInstance);
            Destroy(needWaterInstance);
            if (harvestableInstance != null)
            {
                Destroy(harvestableInstance);
                harvestableInstance = null;
            }
            Destroy(this.gameObject);
        }
    }
}

//public class FlowerLifeCycle : MonoBehaviour
//{
//    public enum FlowerState { Seed, Sprout, FullGrown, Wilted }
//    public FlowerState currentState;

//    public float timeToGrowSprout = 10f;
//    public float timeToGrowFull = 20f;
//    public float coinProductionInterval = 60f;
//    public float timeToWilted = 120f;

//    private float growthTimer = 0f;
//    private bool isFullGrown = false;
//    private bool isWilted = false;
//    private bool hasCoinsReady = false;
//    private float remainingWiltTime; // Tracks remaining wilt time

//    public Sprite seedSprite;
//    public Sprite sproutSprite;
//    public Sprite fullGrownSprite;
//    public Sprite wiltedSprite;

//    public GameObject needWaterPrefab; 
//    public Vector3 needWaterOffset = new Vector3(0.65f, .85f, 0f);  
//    private GameObject needWaterInstance;  // Keep track of the instantiated prefab

//    // for feedback that coins ready 
//    public GameObject coinReadyPrefab;
//    public Vector3 coinReadyOffset = new Vector3(0.65f, .85f, 0f);
//    private GameObject coinReadyInstance;


//    private SpriteRenderer spriteRenderer;

//    void Start()
//    {
//        DontDestroyOnLoad(this.gameObject); // Make flower persistent across scenes
//        currentState = FlowerState.Seed;
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        spriteRenderer.sprite = seedSprite;  // initial state as Seed for flower sprite
//        remainingWiltTime = timeToWilted; // Initialize remaining time
//        StartCoroutine(GrowFlower());
//    }

//    // detect player clicks
//    private void OnMouseDown()
//    {
//        if (isWilted)
//        {
//            ReviveWithWater();
//        }
//        else
//        {
//            CollectCoins(); // Collect coins if not wilted
//        }
//    }

//    // grows the flower and sets is full grown to true 
//    IEnumerator GrowFlower()
//    {
//        while (!isFullGrown)
//        {
//            growthTimer += Time.deltaTime;

//            if (currentState == FlowerState.Seed && growthTimer >= timeToGrowSprout)
//            {
//                currentState = FlowerState.Sprout;
//                spriteRenderer.sprite = sproutSprite;
//            }
//            else if (currentState == FlowerState.Sprout && growthTimer >= timeToGrowFull)
//            {
//                currentState = FlowerState.FullGrown;
//                spriteRenderer.sprite = fullGrownSprite;
//                isFullGrown = true;
//                // full grown is true so call to start producing currency and wilting cycle
//                StartCoroutine(ProduceCoins());
//                StartCoroutine(WiltFlower());
//            }
//            yield return null;
//        }
//    }

//    IEnumerator ProduceCoins()
//    {
//        while (isFullGrown)
//        {
//            // Wait for the coin production interval only if the flower is not wilted
//            while (isWilted)
//            {
//                yield return null; // Pauses the coroutine while the flower is wilted
//            }

//            yield return new WaitForSeconds(coinProductionInterval);

//            if (!isWilted) // Coins only ready if flower  not wilted
//            {
//                hasCoinsReady = true;
//                Debug.Log("Coins ready to harvest!");

//                // Instantiate the coinReadyPrefab above the flower if it hasn't been instantiated yet
//                if (coinReadyInstance == null)
//                {
//                    Vector3 positionAboveFlower = transform.position + coinReadyOffset;
//                    coinReadyInstance = Instantiate(coinReadyPrefab, positionAboveFlower, Quaternion.identity);
//                }
//            }
//        }
//    }

//    IEnumerator WiltFlower()
//    {
//        while (isFullGrown && !isWilted)
//        {
//            yield return new WaitForSeconds(remainingWiltTime);
//            currentState = FlowerState.Wilted;
//            spriteRenderer.sprite = wiltedSprite;
//            isWilted = true;
//            Debug.Log("flower wilted and needs water");

//            if (needWaterInstance == null)
//            {
//                Vector3 positionAboveFlower = transform.position + needWaterOffset;
//                needWaterInstance = Instantiate(needWaterPrefab, positionAboveFlower, Quaternion.identity);
//            }
//        }
//    }

//    // Function called when  player clicks on the flower to collect coins
//    public void CollectCoins()
//    {
//        if (hasCoinsReady)
//        {
//            hasCoinsReady = false;
//            Debug.Log("Coins collected!");
//            int coinsCollected = 1;
//            PlayerInventory.Instance.AddCoins(coinsCollected); // Use Instance to add coins

//            // Destroy the coinReadyInstance after collecting coins
//            if (coinReadyInstance != null)
//            {
//                Destroy(coinReadyInstance);
//                coinReadyInstance = null;
//            }
//        }
//    }

//    // Function to revive wilted flower
//    public void ReviveWithWater()
//    {
//        if (isWilted && WaterSource.Instance.UseWater())
//        {
//            isWilted = false;
//            isFullGrown = true; // Flower returns to full-grown state
//            currentState = FlowerState.FullGrown;
//            spriteRenderer.sprite = fullGrownSprite;

//            if (needWaterInstance != null)
//            {
//                Destroy(needWaterInstance);
//                needWaterInstance = null;
//            }

//            remainingWiltTime = timeToWilted; // Reset the wilt timer
//            StartCoroutine(ProduceCoins()); // Restart coin production
//            StartCoroutine(WiltFlower()); // Restart wilting cycle
//            Debug.Log("Flower revived with water!");
//        }
//    }
//}
