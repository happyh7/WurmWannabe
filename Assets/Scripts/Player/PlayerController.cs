using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    /// <summary>
    /// Initierar Rigidbody2D-komponenten.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Hanterar spelarens rörelse varje frame.
    /// </summary>
    void Update()
    {
        if (rb == null) return;
        // Hämta input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        // Skapa rörelsevektor
        Vector2 movement = new Vector2(moveX, moveY).normalized * moveSpeed;
        // Applicera rörelse
        rb.linearVelocity = movement;
    }
} 