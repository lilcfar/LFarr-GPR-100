using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public int itemValue = 1; // counts amount of consumabelss

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddItem(itemValue);
            Destroy(gameObject); // gets rid of key sprite after consumebed/collidesd
        }

    }

}
