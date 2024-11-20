using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
    public static WaterSource Instance;
    private bool hasWater = false;

    // for water collected user feedback
    public GameObject waterSpritePrefab;
    private GameObject waterSpriteInstance;

    // to check if another tool is in use
    public static bool isOtherToolInUse = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // collect water user feedback stuff in this method
    private void Start()
    {
        // Instantiate and hide the water sprite initially
        waterSpriteInstance = Instantiate(waterSpritePrefab);
        waterSpriteInstance.SetActive(false);
    }

    // function called when  player clicks water sprite
    public void CollectWater()
    {
        if (ShovelSource.isOtherToolInUse || BouquetSource.isOtherToolInUse) // Check if shovel is being used
        {
            Debug.Log("Cannot collect water while shovel is in use!");
            return;
        }

        if (hasWater)
        {
            hasWater = false;
            isOtherToolInUse = false; // Allow other tools
            waterSpriteInstance.SetActive(false);
            Debug.Log("Water source deactivated!");
        }
        else
        {
            hasWater = true;
            isOtherToolInUse = true; // Block other tools
            waterSpriteInstance.SetActive(true);
            Debug.Log("Water collected!");
        }
    }

    // Function called to use water on a wilted flower
    public bool UseWater()
    {
        if (hasWater)
        {
            Debug.Log("Water used!");
            return true;
        }
        return false;
    }

    // Check if player has water
    public bool HasWater()
    {
        return hasWater;
    }

    // more stuff in this function so the watercan sprite moves w/cusour till flower is wateres
    private void Update()
    {
        // If holding water, follow the cursor
        if (hasWater)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Keep sprite on the same plane
            waterSpriteInstance.transform.position = mousePosition;

            // Check for a click to use water on a flower
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("Flower"))
                {
                    FlowerLifeCycle flower = hit.collider.GetComponent<FlowerLifeCycle>();
                    if (flower != null && UseWater())
                    {
                        flower.ReviveWithWater();
                    }
                }
            }
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Watersource clicked!");
        CollectWater();
    }
}