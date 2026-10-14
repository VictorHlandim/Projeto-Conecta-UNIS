using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageColorHover : UpponColorHover
{
    [SerializeField]
    private Image img;
    public float CrossFadeDuration = 0.25f;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        ChangeImgColor(Target, img, CrossFadeDuration);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        ChangeImgColor(OriginalColor, img, CrossFadeDuration);
    }
}
