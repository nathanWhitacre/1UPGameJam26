using Unity.VisualScripting;
using UnityEngine;

public class HumanMeatSuckSpriteSuckManagerSucker : MonoBehaviour
{
    [SerializeField] private HumanHealth health;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite fullHealthSprite;
    [SerializeField] private float fullHealthScale;
    private bool isFullHealth = true;

    [SerializeField] private Sprite lowHealthSprite;
    [SerializeField] private float lowHealthScale;
    private bool isLowHealth = false;

    [SerializeField] private Sprite noHealthSprite;
    [SerializeField] private float noHealthScale;
    private bool isNoHealth = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.GetHealthRatio() < 0.5f && isFullHealth)
        {
            isFullHealth = false;
            isLowHealth = true;
            spriteRenderer.sprite = lowHealthSprite;
            transform.localScale = new Vector3(lowHealthScale, lowHealthScale, lowHealthScale);
        }

        if (health.GetHealthRatio() <= 0f && isLowHealth)
        {
            isLowHealth = false;
            isNoHealth = true;
            spriteRenderer.sprite = noHealthSprite;
            transform.localScale = new Vector3(noHealthScale, noHealthScale, noHealthScale);
        }
    }
}
