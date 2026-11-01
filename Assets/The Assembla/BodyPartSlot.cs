using UnityEngine;

public enum BodyPartHighlight
{
    None,
    Hint,
    Hover,
    Wrong
}

[RequireComponent(typeof(SpriteRenderer))]
public class BodyPartSlot : MonoBehaviour
{
    [SerializeField] private BodyPartType type;
    [SerializeField] private Color hintColor = new Color(0.8f, 1f, 0.85f, 1f);
    [SerializeField] private Color hoverColor = new Color(0.45f, 1f, 0.6f, 1f);
    [SerializeField] private Color wrongColor = new Color(1f, 0.45f, 0.45f, 1f);

    private SpriteRenderer spriteRenderer;
    private Color baseColor;

    public BodyPartType Type => type;
    public Sprite CurrentSprite => spriteRenderer.sprite;
    public SpriteRenderer Renderer => spriteRenderer;
    public int SortingOrder => spriteRenderer.sortingOrder;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
    }

    public Sprite Attach(Sprite sprite)
    {
        Sprite old = spriteRenderer.sprite;
        spriteRenderer.sprite = sprite;
        return old;
    }

    public void SetHighlight(BodyPartHighlight highlight)
    {
        switch (highlight)
        {
            case BodyPartHighlight.Hint:
                spriteRenderer.color = baseColor * hintColor;
                break;
            case BodyPartHighlight.Hover:
                spriteRenderer.color = baseColor * hoverColor;
                break;
            case BodyPartHighlight.Wrong:
                spriteRenderer.color = baseColor * wrongColor;
                break;
            default:
                spriteRenderer.color = baseColor;
                break;
        }
    }

    public bool Contains(Vector3 worldPoint, float padding)
    {
        if (spriteRenderer.sprite == null) return false;
        Bounds bounds = GetTightBounds(spriteRenderer.sprite, transform);
        bounds.Expand(new Vector3(padding * 2f, padding * 2f, 0f));
        worldPoint.z = bounds.center.z;
        return bounds.Contains(worldPoint);
    }

    public static Bounds GetTightBounds(Sprite sprite, Transform target)
    {
        Vector2[] vertices = sprite.vertices;
        Vector2 min = vertices[0];
        Vector2 max = vertices[0];
        for (int i = 1; i < vertices.Length; i++)
        {
            min = Vector2.Min(min, vertices[i]);
            max = Vector2.Max(max, vertices[i]);
        }

        Vector3 a = target.TransformPoint(min);
        Vector3 b = target.TransformPoint(max);
        Bounds bounds = new Bounds(a, Vector3.zero);
        bounds.Encapsulate(b);
        return bounds;
    }
}
