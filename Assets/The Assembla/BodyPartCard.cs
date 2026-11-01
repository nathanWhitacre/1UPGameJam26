using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class BodyPartCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text label;
    [SerializeField] private float draggingAlpha = 0.3f;

    private BodyPartAssembler assembler;
    private CanvasGroup canvasGroup;

    public Sprite Sprite { get; private set; }
    public BodyPartType Type { get; private set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
        canvasGroup.alpha = draggingAlpha;
        canvasGroup.blocksRaycasts = false;
        assembler.BeginDrag(this, eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        assembler.Drag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        assembler.EndDrag(eventData.position);
    }
}
