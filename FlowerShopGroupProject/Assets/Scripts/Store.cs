using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Store : MonoBehaviour
{
    public GameObject storePanel;

    public GameObject[] flowerPrefabs; // Array of flower prefab variants
    public int[] flowerCosts;          // Costs for each type

    public GameObject selectedFlower;  // The flower prefab selected for placement set to pub from priv.

    // more stuff for saving an retriving flower data for scene switches
    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void BuyFlower(int flowerIndex)
    {
        if (flowerIndex >= 0 && flowerIndex < flowerPrefabs.Length)
        {
            int cost = flowerCosts[flowerIndex];
            if (PlayerInventory.Instance.PurchaseFlower(cost))
            {
                selectedFlower = Instantiate(flowerPrefabs[flowerIndex]);
                Debug.Log("Flower selected for placement.");
            }
        }
    }

    void Update()
    {
        if (selectedFlower != null)
        {
            MoveFlowerWithCursor();
            if (Input.GetMouseButtonDown(0))
            {
                PlaceFlower();
            }
        }
    }

    void MoveFlowerWithCursor()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // so flower stays in 2D 
        selectedFlower.transform.position = mousePosition;
    }

    void PlaceFlower()
    {
        // to only place on objects in ground layer
        int groundLayer = LayerMask.GetMask("Ground");
        Collider2D hit = Physics2D.OverlapPoint(selectedFlower.transform.position, groundLayer);

        if (hit != null)
        {
            Debug.Log("Collider detected: " + hit.name);
            if (hit.CompareTag("ground"))
            {
                selectedFlower = null; // Remove reference after placing
                Debug.Log("Flower placed on ground.");
                return;
            }
            else
            {
                Debug.Log("Detected collider does not have 'ground' tag: " + hit.tag);
            }
        }
        else
        {
            Debug.Log("No collider detected at flower position.");
        }

        Debug.Log("Invalid placement. Please place on ground.");
    }

    public void OpenStore()
    {
        if (storePanel.activeInHierarchy == false)
        {
            storePanel.SetActive(true);
        }
        else if (storePanel.activeInHierarchy == true)
        {
            storePanel.SetActive(false);
        }
    }
}
