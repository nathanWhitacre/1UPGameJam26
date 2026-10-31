using UnityEngine;

public class SpriteMovementRotator : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool isFlippedByDefault;
    public bool stopRotating = false;

    private SpriteRenderer spriteRenderer;
    private Vector2 movementVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopRotating) return;

        movementVector = rb.linearVelocity;
        if (movementVector.x < 0)
        {
            spriteRenderer.flipX = !isFlippedByDefault;
        }
        else if (movementVector.x > 0)
        {
            spriteRenderer.flipX = isFlippedByDefault;
        }
    }
}
