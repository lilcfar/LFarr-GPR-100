using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelSource : MonoBehaviour
{
    public static ShovelSource Instance;
    private bool hasShovel = false;

    // For shovel collected feedback
    public GameObject shovelSpritePrefab;
    private GameObject shovelSpriteInstance;

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

    private void Start()
    {
        // Instantiate and hide the shovel sprite initially
        shovelSpriteInstance = Instantiate(shovelSpritePrefab);
        shovelSpriteInstance.SetActive(false);
    }

    public void CollectShovel()
    {
        hasShovel = true;
        shovelSpriteInstance.SetActive(true); 
        Debug.Log("Shovel collected!");
    }

    public bool HasShovel()
    {
        return hasShovel;
    }

    private void Update()
    {
        if (hasShovel)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Keep sprite on the same plane
            shovelSpriteInstance.transform.position = mousePosition;

            // Check for a click to use shovel on a flower
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("Flower"))
                {
                    FlowerLifeCycle flower = hit.collider.GetComponent<FlowerLifeCycle>();
                    if (flower != null && UseShovel())
                    {
                        flower.HarvestFlower();
                    }
                }
            }
        }
    }

    public bool UseShovel()
    {
        if (hasShovel)
        {
            //hasShovel = false;
            shovelSpriteInstance.SetActive(false); // Hide the shovel sprite after use
            Debug.Log("Shovel used!");

            return true;

        }
        return false;
    }


    private void OnMouseDown()
    {
        Debug.Log("Shovel source clicked!");
        CollectShovel();
    }
}
