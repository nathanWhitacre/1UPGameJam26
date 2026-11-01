using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BodyPartPickup : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Pop")]
    [SerializeField] private float popDistance = 1.2f;
    [SerializeField] private float popDuration = 0.35f;
    [SerializeField] private Ease popEase = Ease.OutQuad;

    [Header("Fly To Player")]
    [SerializeField] private float flyDuration = 0.7f;
    [SerializeField] private Ease flyEase = Ease.InBack;
    [SerializeField] private float spinDegrees = 360f;

    private BodyPartEntry part;
    private Sequence sequence;
    private Vector3 flyStart;
    private bool flying;
    private bool collected;
    private int playerLayer;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    public void Setup(BodyPartEntry bodyPart)
    {
        part = bodyPart;
        spriteRenderer.sprite = part.sprite;
        CenterSprite();

        Vector3 baseScale = transform.localScale;
        Vector2 direction = Random.insideUnitCircle.normalized;
        Vector3 popTarget = transform.position + (Vector3)(direction * popDistance);

        sequence = Sequence.Create()
            .Group(Tween.Scale(transform, Vector3.zero, baseScale, popDuration, Ease.OutBack))
            .Group(Tween.Position(transform, popTarget, popDuration, popEase))
            .Chain(Tween.Custom(this, 0f, 1f, flyDuration, (pickup, t) => pickup.Fly(t), flyEase))
            .Group(Tween.LocalEulerAngles(spriteRenderer.transform, Vector3.zero, new Vector3(0f, 0f, spinDegrees), flyDuration, Ease.InQuad))
            .OnComplete(this, pickup => pickup.Collect());
    }

    private void CenterSprite()
    {
        Vector2[] vertices = part.sprite.vertices;
        Vector2 min = vertices[0];
        Vector2 max = vertices[0];
        for (int i = 1; i < vertices.Length; i++)
        {
            min = Vector2.Min(min, vertices[i]);
            max = Vector2.Max(max, vertices[i]);
        }

        Vector3 center = (min + max) * 0.5f;
        spriteRenderer.transform.localPosition = -Vector3.Scale(center, spriteRenderer.transform.localScale);
    }

    private void Fly(float t)
    {
        if (PlayerID.Instance == null) return;

        if (!flying)
        {
            flying = true;
            flyStart = transform.position;
        }

        Vector3 target = PlayerID.Instance.transform.position;
        target.z = flyStart.z;
        transform.position = Vector3.LerpUnclamped(flyStart, target, t);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != playerLayer) return;
        Collect();
    }

    private void Collect()
    {
        if (collected || part == null) return;
        collected = true;

        sequence.Stop();
        BodyPartInventory.Add(part);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        sequence.Stop();
    }
}
