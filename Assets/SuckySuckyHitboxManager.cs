using UnityEngine;

public class SuckySuckyHitboxManager : MonoBehaviour
{

    private Collider2D collider;
    private bool isSuckingOff = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !isSuckingOff)
        {
            collider.enabled = true;
            isSuckingOff = true;
        }
        else if (!Input.GetMouseButton(0) && isSuckingOff)
        {
            collider.enabled = false;
            isSuckingOff = false;
        }
    }
}
