using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public TMP_Text xpText;
    public static int playerXP = 0;

    public void Update()
    {
        UpdateXPDisplay();
    }

    private void UpdateXPDisplay()
    {
        if (xpText != null)
        {
            xpText.text = "XP:" + playerXP;
        }
    }
}
