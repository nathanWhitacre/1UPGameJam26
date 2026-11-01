using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class BodyPartCard : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text label;
    [SerializeField] private float draggingAlpha = 0.3f;

    private BodyPartAssembler assembler;
    private CanvasGroup canvasGroup;
    private ScrollRect scrollRect;
    private bool scrolling;

    public Sprite Sprite { get; private set; }
    public BodyPartType Type { get; private set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private ScrollRect GetScrollRect()
    {
        if (scrollRect == null) scrollRect = GetComponentInParent<ScrollRect>();
        return scrollRect;
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (GetScrollRect() != null) scrollRect.OnInitializePotentialDrag(eventData);
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (GetScrollRect() != null) scrollRect.OnScroll(eventData);
    }

    public void Setup(BodyPartAssembler owner, Sprite sprite, Sprite iconSprite, BodyPartType type)
    {
        assembler = owner;
        Sprite = sprite;
        Type = type;

        if (icon != null) icon.sprite = iconSprite;
        if (label != null) label.text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sprite.name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.position - eventData.pressPosition;
        scrolling = GetScrollRect() != null && Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y);
        if (scrolling)
        {
            scrollRect.OnBeginDrag(eventData);
            return;
        }

        canvasGroup.alpha = draggingAlpha;
        canvasGroup.blocksRaycasts = false;
        assembler.BeginDrag(this, eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (scrolling)
        {
            scrollRect.OnDrag(eventData);
            return;
        }

        assembler.Drag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (scrolling)
        {
            scrolling = false;
            scrollRect.OnEndDrag(eventData);
            return;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        assembler.EndDrag(eventData.position);
    }
}
