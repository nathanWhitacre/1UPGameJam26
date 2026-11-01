using Unity.VisualScripting;
using UnityEngine;

public class HumanMeatSuckSpriteSuckManagerSucker : MonoBehaviour
{
    [SerializeField] private HumanHealth health;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite fullHealthSprite;
    [SerializeField] private float fullHealthScale;
    private bool isFullHealth = true;

    [SerializeField] private float lowHealthThreshold = 0.5f;
    [SerializeField] private Sprite lowHealthSprite;
    [SerializeField] private float lowHealthScale;
    [SerializeField] private AudioSource lowHealthSound;
    private bool isLowHealth = false;

    [SerializeField] private Sprite noHealthSprite;
    [SerializeField] private float noHealthScale;
    [SerializeField] private AudioSource noHealthSound;
    private bool isNoHealth = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.GetHealthRatio() < lowHealthThreshold && isFullHealth)
        {
            isFullHealth = false;
            isLowHealth = true;
            spriteRenderer.sprite = lowHealthSprite;
            transform.localScale = new Vector3(lowHealthScale, lowHealthScale, lowHealthScale);
            lowHealthSound.Play();
        }

        if (health.GetHealthRatio() <= 0f && isLowHealth)
        {
            isLowHealth = false;
            isNoHealth = true;
            spriteRenderer.sprite = noHealthSprite;
            transform.localScale = new Vector3(noHealthScale, noHealthScale, noHealthScale);
            noHealthSound.Play();
            lowHealthSound.Stop();
        }
    }
}
