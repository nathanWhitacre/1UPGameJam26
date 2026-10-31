using UnityEngine;

public class HumanHealth : MonoBehaviour
{
    [SerializeField] private float meatValue = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0f;
            isDead = true;
            PlayerID.Instance.meatSucked += meatValue;
        }
    }

    public void Heal(float healing)
    {
        if (isDead) return;
        currentHealth += healing;
    }

    public float GetHealthRatio()
    {
        return currentHealth / maxHealth;
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
