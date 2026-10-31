using UnityEngine;

public class VaccuumRotator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector3 lookPosition = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 lookDirection = lookPosition - transform.position;

            float angle = Vector2.SignedAngle(Vector2.down, lookDirection);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
    }
}
