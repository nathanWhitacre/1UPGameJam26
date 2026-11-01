using UnityEngine;

public class HumanHealth : MonoBehaviour
{
    [SerializeField] private float meatValue = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private HumanData humanData;
    [SerializeField] private BodyPartPickup bodyPartPickupPrefab;
    private bool isDead = false;
    private HumanMeatSpawnerSoonToBeSuckedMeatMeat spawner;

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
            if (spawner != null) spawner.MeatSucked();
            DropBodyPart();
        }
    }

    private void DropBodyPart()
    {
        if (humanData == null || bodyPartPickupPrefab == null) return;
        if (!humanData.TryRollBodyPart(out BodyPartEntry part)) return;

        BodyPartPickup pickup = Instantiate(bodyPartPickupPrefab, transform.position, Quaternion.identity);
        pickup.Setup(part);
    }

    public void Heal(float healing)
    {
        if (isDead) return;
        currentHealth += healing;
    }

    public void SetHumanData(HumanData data)
    {
        humanData = data;
    }

    public void SetSpawner(HumanMeatSpawnerSoonToBeSuckedMeatMeat owner)
    {
        spawner = owner;
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
