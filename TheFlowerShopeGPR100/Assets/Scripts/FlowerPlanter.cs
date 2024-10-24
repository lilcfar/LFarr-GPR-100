using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// uses player's inventory to plant the seeds 
public class FlowerPlanter : MonoBehaviour
{
    public GameObject BaseFlowerOBJPrefab;

    public void TryPlantFlower(Vector2 position, string flowerType)
    {
        if (PlayerInventory.Instance.UseSeed(flowerType))
        {
            Instantiate(BaseFlowerOBJPrefab, position, Quaternion.identity);
            Debug.Log("Planted a " + flowerType);
        }
        else
        {
            Debug.Log("Not enough seeds to plant " + flowerType);
        }
    }

    //void Update()
    //{
    //    // press "P", to plant a flower at a specific position
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        Vector2 plantPosition = new Vector2(0, 0); // Replace later with a position in the shop scene 
    //        PlayerInventory.Instance.UseSeed("FlowerType"); // just base flower for now will add logic to choose types
    //        Instantiate(BaseFlowerOBJPrefab, plantPosition, Quaternion.identity);
    //    }
    //}
}
