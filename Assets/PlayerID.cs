using UnityEngine;

public class PlayerID : MonoBehaviour
{

    public static PlayerID Instance;

    public float meatSucked = 0f;
    public float timeSuckin = 0f;
    public float timeToSuck = 45f;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToSuck -= Time.deltaTime;
        timeSuckin += Time.deltaTime;
    }
}
