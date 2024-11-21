using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI coinHarvest;
    public TextMeshProUGUI coinDuration;
    

    public void SetText(int cHarvest, int cDuration)
    {
        coinHarvest.text = "Harvest: " + cHarvest.ToString();
        coinDuration.text = "Harvest Time: " + cDuration.ToString();
       

    }
    private void Update()
    {
        Vector2 position = Input.mousePosition;

        transform.position = position;

    }

    

}
