using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextColorHover : UpponColorHover
{
    [SerializeField]
    private TextMeshProUGUI txt;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        ChangeTextColor(Target, txt);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        ChangeTextColor(OriginalColor, txt);
    }
}
