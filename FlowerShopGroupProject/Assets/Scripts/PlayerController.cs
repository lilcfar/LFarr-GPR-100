using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player movement
    public Vector2 minBounds;    // Minimum x and y coordinates
    public Vector2 maxBounds;    // Maximum x and y coordinates

    private bool canMove = false; // Control movement availability

    void Update()
    {
        if (canMove)
        {
            MovePlayer();
            RestrictPlayerToBounds();
        }
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.position += new Vector3(moveX, moveY, 0f);

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (moveX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveX < 0)
        {
            spriteRenderer.flipX=true;
        }
    }

    private void RestrictPlayerToBounds()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public void EnableMovement(bool enable)
    {
        canMove = enable;
    }
}
