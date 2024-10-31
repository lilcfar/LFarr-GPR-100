using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerLifeCycle : MonoBehaviour
{
    public enum FlowerState { Seed, Sprout, FullGrown, Wilted }
    public FlowerState currentState;

    public float timeToGrowSprout = 10f;
    public float timeToGrowFull = 20f;
    public float coinProductionInterval = 60f;
    public float timeToWilted = 120f;

    private float growthTimer = 0f;
    private bool isFullGrown = false;
    private bool isWilted = false;
    private bool hasCoinsReady = false;
    private float remainingWiltTime; // Tracks remaining wilt time

    public Sprite seedSprite;
    public Sprite sproutSprite;
    public Sprite fullGrownSprite;
    public Sprite wiltedSprite;

    public GameObject needWaterPrefab;  // Assign the "NeedWater" prefab in the Inspector
    public Vector3 needWaterOffset = new Vector3(0.65f, .85f, 0f);  // Set X to 0.85, Y to 1.2
    private GameObject needWaterInstance;  // Keep track of the instantiated prefab

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentState = FlowerState.Seed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = seedSprite;  // initial state as Seed for flower sprite
        remainingWiltTime = timeToWilted; // Initialize remaining time
        StartCoroutine(GrowFlower());
    }

    // detect player clicks
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

    // grows the flower and sets is full grown to true 
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
                //Vector3 positionAboveFlower = transform.position + new Vector3(.85f, 1.2f, 0); 
                //needWaterInstance = Instantiate(needWaterPrefab, positionAboveFlower, Quaternion.identity);
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

            remainingWiltTime = timeToWilted; // Reset the wilt timer
            StartCoroutine(ProduceCoins()); // Restart coin production
            StartCoroutine(WiltFlower()); // Restart wilting cycle
            Debug.Log("Flower revived with water!");
        }
    }

    // Will add a  Visual indicator to show coins are ready later well figure it out l8tr
}
