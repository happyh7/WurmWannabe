using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Hämta input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Skapa rörelsevektor
        Vector2 movement = new Vector2(moveX, moveY).normalized * moveSpeed;

        // Applicera rörelse
        rb.linearVelocity = movement;
    }
} 