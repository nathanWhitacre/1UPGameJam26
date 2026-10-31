using UnityEngine;

public class HumanMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxDistance = 10f;

    private HumanHealth health;
    private Rigidbody2D rb;
    private Vector2 movementVector;

    private void Awake()
    {
        health = GetComponent<HumanHealth>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, PlayerID.Instance.transform.position) < maxDistance)
        {
            movementVector = (transform.position - PlayerID.Instance.transform.position).normalized;
            movementVector.Normalize();
        }
        else
        {
            movementVector = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementVector * speed * health.GetHealthRatio();
    }
}
