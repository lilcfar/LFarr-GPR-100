using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// detects/manages players interaction with flower
public class FlowerInteraction : MonoBehaviour
{
    private FlowerLifeCycle flowerLifeCycle;

    void Start()
    {
        flowerLifeCycle = GetComponent<FlowerLifeCycle>();
    }

    void OnMouseDown()
    {
        Debug.Log("Flower clicked!");  // confirm if the click is detected

        int coinsCollected = flowerLifeCycle.CollectCoins();
        if (coinsCollected > 0)
        {
            PlayerInventory.Instance.AddCoins(coinsCollected);
        }
    }
}
