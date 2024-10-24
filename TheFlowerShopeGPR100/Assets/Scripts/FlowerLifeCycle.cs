using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerLifeCycle : MonoBehaviour
{
    public enum FlowerState { Seed, Sprout, FullGrown }
    public FlowerState currentState;

    public float timeToGrowSprout = 10f;  
    public float timeToGrowFull = 20f;    
    public float coinProductionInterval = 60f;  // Time to generate one coin once full grown (will change currency later) 

    private float growthTimer = 0f;
    private bool isFullGrown = false;
    private bool hasCoinsReady = false; // check if coins are ready

    // need to add sprites for stage of the flower
    public Sprite seedSprite;
    public Sprite sproutSprite;
    public Sprite fullGrownSprite;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        currentState = FlowerState.Seed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = seedSprite;  // initial state as Seed for flower sprite
        StartCoroutine(GrowFlower());
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
                // full grown is true so call to start producing currency 
                StartCoroutine(ProduceCoins());
            }

            yield return null;
        }
    }

    IEnumerator ProduceCoins()
    {
        while (isFullGrown)
        {
            yield return new WaitForSeconds(coinProductionInterval);
            hasCoinsReady = true; // Coins are ready to be harvested
            Debug.Log("Coins ready to harvest!");
        }
    }

    // Function called when  player clicks on the flower to collect coins
    public int CollectCoins()
    {
        if (hasCoinsReady)
        {
            hasCoinsReady = false;
            Debug.Log("Coins collected!");
            return 1; // Return the number of coins collected 
        }
        return 0; // No coins to collect
    }

    // Will add a  Visual indicator to show coins are ready
    // will implement later once I'm ready to add currency / score system for player 
    // for now just debug log to check if it works - Lily 

}
