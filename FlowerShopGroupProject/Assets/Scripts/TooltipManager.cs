using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System.Transactions;
using UnityEngine.EventSystems;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager current;

    public Tooltip tooltip;

    public void Awake()
    {
        current = this;
    }

    public static void Show(int cHarvest, int cDuration)
    {
        current.tooltip.SetText(cHarvest, cDuration);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }


}
