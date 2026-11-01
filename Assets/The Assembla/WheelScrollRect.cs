using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WheelScrollRect : ScrollRect
{
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
    }

    public override void OnDrag(PointerEventData eventData)
    {
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
    }

    public override void OnScroll(PointerEventData data)
    {
        Vector2 original = data.scrollDelta;
        data.scrollDelta = new Vector2(original.x, -original.y);
        base.OnScroll(data);
        data.scrollDelta = original;
    }
}
