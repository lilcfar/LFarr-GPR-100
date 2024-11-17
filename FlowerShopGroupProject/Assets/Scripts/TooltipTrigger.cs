using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int cHarvest;
    public int cDuration;


    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Show(cHarvest, cDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Hide();
    }
}
