using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int coinsCollected; // coins collected from each flower


    // Harvestable feedback
    public GameObject harvestablePrefab;
    public Vector3 harvestableOffset = new Vector3(0.65f, .85f, 0f);
    private GameObject harvestableInstance;

    //progressBar feedback
    public GameObject progressBarPrefab;
    private float progressBarTimer = 0f;
    public Image progressBarImage;
    

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

        if (currentState == FlowerState.FullGrown || currentState == FlowerState.Wilted)
        {
            progressBarPrefab.SetActive(true);
            progressBarTimer += Time.deltaTime;
            progressBarImage.fillAmount = progressBarTimer / timeToHarvest;
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
            Debug.Log("Flower wilted and needs water");

            if (needWaterInstance == null)
            {
                Vector3 positionAboveFlower = transform.position + needWaterOffset;
                needWaterInstance = Instantiate(needWaterPrefab, positionAboveFlower, Quaternion.identity);
            }

            // Destroy coin feedback when the flower wilts
            if (coinReadyInstance != null)
            {
                Destroy(coinReadyInstance);
                coinReadyInstance = null;
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
            }

            // Recreate or reactivate the coin feedback when the flower is revived
            if (coinReadyInstance == null && hasCoinsReady)
            {
                Vector3 positionAboveFlower = transform.position + coinReadyOffset;
                coinReadyInstance = Instantiate(coinReadyPrefab, positionAboveFlower, Quaternion.identity);
            }

            remainingWiltTime = timeToWilted; // Reset the wilt timer
            StartCoroutine(ProduceCoins()); // Restart coin production
            StartCoroutine(WiltFlower()); // Restart wilting cycle
            Debug.Log("Flower revived with water!");
        }
    }

    public void HarvestFlower() // changed to public from private so i could call in update of shovelsource
    {
        if (isReadyToHarvest && ShovelSource.Instance.HasShovel())
        {
            string flowerType = this.gameObject.name.Replace("(Clone)", "").Trim();
            playerInventory.AddFlowerToInventory(flowerType);

            // Destroy feedback instances and the flower GameObject
            Destroy(coinReadyInstance);
            Destroy(needWaterInstance);
            Destroy(harvestableInstance);
            Destroy(this.gameObject);
        }
    }
}