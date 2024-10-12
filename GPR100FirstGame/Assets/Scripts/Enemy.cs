using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float respawnX;
    float respawnY = 6;
    public int damageAmount = 1;
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        respawnX = transform.position.x;

    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        Vector2 newPos = new Vector2(respawnX, respawnY);
        transform.position = newPos;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().Respawn();
        }
    }

    // attach to plyr and health 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.SetParent(collision.transform);
            
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}


