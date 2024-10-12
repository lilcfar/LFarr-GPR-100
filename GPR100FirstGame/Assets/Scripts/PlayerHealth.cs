using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public GameObject CanvasDisplay;
    public int health = 4;
    public Text HealthText;

    public Text LoseMessageText;

    private void Start()
    {
        UpdateHealthUI();
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHealthUI();
        Debug.Log("Player health: " + health);

        if (health <= 0)
        {
            Debug.Log("No health left");
            TriggerGameOver();
        }
    }

    private void UpdateHealthUI()
    {
        if (HealthText != null)
        {
            HealthText.text = "Player Health/4: " + health.ToString();
        }
    }

    private void TriggerGameOver()
    {
        if (LoseMessageText != null)
        {
            LoseMessageText.text = "YOU LOSE!";
            LoseMessageText.gameObject.SetActive(true);
            SceneManager.LoadScene("LoseScene");
        }
    }
}
