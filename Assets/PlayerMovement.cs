using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 movementVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movementVector = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movementVector += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementVector += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementVector += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementVector += Vector2.right;
        }
        movementVector.Normalize();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementVector * speed;
    }
}
