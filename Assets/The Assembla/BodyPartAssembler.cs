using System;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartAssembler : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera worldCamera;
    [SerializeField] private List<BodyPartEntry> startingParts = new List<BodyPartEntry>();
    [SerializeField] private bool returnReplacedToTray = true;
    [SerializeField] private float dropPadding = 0.35f;

    [Header("Tray")]
    [SerializeField] private RectTransform tray;
    [SerializeField] private RectTransform trayContent;
    [SerializeField] private BodyPartCard cardPrefab;

    [Header("Misc")]
    [SerializeField] private int miscSortingOrder = 10;

    [Header("Drag")]
    [SerializeField] private int ghostSortingOrder = 100;
    [SerializeField] private Color ghostColor = new Color(1f, 1f, 1f, 0.9f);

    private readonly List<BodyPartSlot> slots = new List<BodyPartSlot>();
    private readonly Dictionary<Sprite, Sprite> iconCache = new Dictionary<Sprite, Sprite>();
    private SpriteRenderer ghost;
    private BodyPartCard draggedCard;

    private void Awake()
    {
        if (worldCamera == null) worldCamera = Camera.main;
        if (canvas == null) canvas = FindFirstObjectByType<Canvas>();

        slots.AddRange(FindObjectsByType<BodyPartSlot>(FindObjectsSortMode.None));

        BuildGhost();

        IEnumerable<BodyPartEntry> parts = BodyPartInventory.RunFinished ? BodyPartInventory.Collected : startingParts;
        foreach (BodyPartEntry entry in parts)
        {
            if (entry.sprite != null) AddCard(entry.sprite, entry.type);
        }
    }

    public void BeginDrag(BodyPartCard card, Vector2 screenPosition)
    {
        draggedCard = card;

        BodyPartSlot match = card.Type == BodyPartType.Misc ? (slots.Count > 0 ? slots[0] : null) : FindMatchingSlot(card.Type);
        ghost.transform.localScale = match != null ? match.transform.lossyScale : Vector3.one;
        if (match != null) ghost.sharedMaterial = match.Renderer.sharedMaterial;
        ghost.sprite = card.Sprite;
        ghost.gameObject.SetActive(true);

        Drag(screenPosition);
    }

    public void Drag(Vector2 screenPosition)
    {
        if (draggedCard == null) return;

        if (TryGetWorldPoint(screenPosition, out Vector3 worldPoint))
        {
            ghost.transform.position = worldPoint;
            Bounds bounds = BodyPartSlot.GetTightBounds(ghost.sprite, ghost.transform);
            Vector3 offset = worldPoint - bounds.center;
            offset.z = 0f;
            ghost.transform.position += offset;
        }

        BodyPartSlot hovered = IsOverTray(screenPosition) ? null : FindSlotAt(screenPosition, draggedCard.Type);
        bool misc = draggedCard.Type == BodyPartType.Misc;
        foreach (BodyPartSlot slot in slots)
        {
            bool fits = misc || slot.Type == draggedCard.Type;
            if (slot == hovered)
                slot.SetHighlight(fits ? BodyPartHighlight.Hover : BodyPartHighlight.Wrong);
            else
                slot.SetHighlight(fits && !misc ? BodyPartHighlight.Hint : BodyPartHighlight.None);
        }
    }

    public void EndDrag(Vector2 screenPosition)
    {
        if (draggedCard == null) return;

        BodyPartCard card = draggedCard;
        draggedCard = null;
        Vector3 ghostPosition = ghost.transform.position;
        Vector3 ghostScale = ghost.transform.localScale;
        ghost.gameObject.SetActive(false);

        foreach (BodyPartSlot slot in slots) slot.SetHighlight(BodyPartHighlight.None);

        if (IsOverTray(screenPosition)) return;

        BodyPartSlot target = FindSlotAt(screenPosition, card.Type);
        if (target == null) return;

        if (card.Type == BodyPartType.Misc)
        {
            PlaceMisc(card.Sprite, target, ghostPosition, ghostScale);
            Destroy(card.gameObject);
            return;
        }

        if (target.Type != card.Type) return;

        Sprite old = target.Attach(card.Sprite);
        Destroy(card.gameObject);

        if (returnReplacedToTray && old != null && old != card.Sprite) AddCard(old, target.Type);
    }

    private BodyPartSlot FindSlotAt(Vector2 screenPosition, BodyPartType preferred)
    {
        if (!TryGetWorldPoint(screenPosition, out Vector3 worldPoint)) return null;

        BodyPartSlot fallback = null;
        foreach (BodyPartSlot slot in slots)
        {
            if (!slot.Contains(worldPoint, dropPadding)) continue;
            if (slot.Type == preferred) return slot;
            if (fallback == null || slot.SortingOrder > fallback.SortingOrder) fallback = slot;
        }
        return fallback;
    }

    private void PlaceMisc(Sprite sprite, BodyPartSlot target, Vector3 position, Vector3 scale)
    {
        GameObject miscObject = new GameObject(sprite.name, typeof(SpriteRenderer));
        SpriteRenderer miscRenderer = miscObject.GetComponent<SpriteRenderer>();
        miscRenderer.sprite = sprite;
        miscRenderer.sharedMaterial = target.Renderer.sharedMaterial;
        miscRenderer.sortingOrder = miscSortingOrder;

        miscObject.transform.SetPositionAndRotation(position, Quaternion.identity);
        miscObject.transform.localScale = scale;
        miscObject.transform.SetParent(target.transform, true);
    }

    private BodyPartSlot FindMatchingSlot(BodyPartType type)
    {
        foreach (BodyPartSlot slot in slots)
        {
            if (slot.Type == type) return slot;
        }
        return null;
    }

    private bool TryGetWorldPoint(Vector2 screenPosition, out Vector3 worldPoint)
    {
        float planeZ = slots.Count > 0 ? slots[0].transform.position.z : 0f;
        Plane plane = new Plane(Vector3.forward, new Vector3(0f, 0f, planeZ));
        Ray ray = worldCamera.ScreenPointToRay(screenPosition);

        if (plane.Raycast(ray, out float distance))
        {
            worldPoint = ray.GetPoint(distance);
            return true;
        }

        worldPoint = Vector3.zero;
        return false;
    }

    private bool IsOverTray(Vector2 screenPosition)
    {
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        return RectTransformUtility.RectangleContainsScreenPoint(tray, screenPosition, uiCamera);
    }

    private void BuildGhost()
    {
        GameObject ghostObject = new GameObject("Body Part Ghost", typeof(SpriteRenderer));
        ghost = ghostObject.GetComponent<SpriteRenderer>();
        ghost.sortingOrder = ghostSortingOrder;
        ghost.color = ghostColor;
        ghostObject.SetActive(false);
    }

    private void AddCard(Sprite sprite, BodyPartType type)
    {
        BodyPartCard card = Instantiate(cardPrefab, trayContent);
        card.name = "Card " + sprite.name;
        card.Setup(this, sprite, GetIcon(sprite), type);
    }

    private Sprite GetIcon(Sprite source)
    {
        if (iconCache.TryGetValue(source, out Sprite cached)) return cached;

        Vector2[] vertices = source.vertices;
        Vector2 min = vertices[0];
        Vector2 max = vertices[0];
        for (int i = 1; i < vertices.Length; i++)
        {
            min = Vector2.Min(min, vertices[i]);
            max = Vector2.Max(max, vertices[i]);
        }

        float ppu = source.pixelsPerUnit;
        Rect rect = source.rect;
        Vector2 pivot = source.pivot;
        float xMin = Mathf.Clamp(rect.x + pivot.x + min.x * ppu, rect.xMin, rect.xMax);
        float yMin = Mathf.Clamp(rect.y + pivot.y + min.y * ppu, rect.yMin, rect.yMax);
        float xMax = Mathf.Clamp(rect.x + pivot.x + max.x * ppu, rect.xMin, rect.xMax);
        float yMax = Mathf.Clamp(rect.y + pivot.y + max.y * ppu, rect.yMin, rect.yMax);

        Sprite icon = Sprite.Create(source.texture, Rect.MinMaxRect(xMin, yMin, xMax, yMax), new Vector2(0.5f, 0.5f), ppu, 0, SpriteMeshType.FullRect);
        icon.name = source.name;
        iconCache[source] = icon;
        return icon;
    }
}
