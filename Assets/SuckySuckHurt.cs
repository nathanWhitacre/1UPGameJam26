using UnityEngine;

public class SuckySuckHurt : MonoBehaviour
{
    [SerializeField] private HumanHealth health;
    [SerializeField] private float damagePerTick;
    private bool isGettingMeatSucked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) != "Vaccuum") return;

        health.Damage(damagePerTick);
    }
}
