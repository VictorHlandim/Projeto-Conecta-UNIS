using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UpponColorHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Cores")]
    [SerializeField]
    protected Color Target;
    [SerializeField]
    protected Color OriginalColor;

    protected void ChangeImgColor(Color TargetColor, UnityEngine.UI.Image img, float CrossFloat)
    {
        img.CrossFadeColor(TargetColor, CrossFloat, true, true);
    }
    protected void ChangeTextColor(Color TargetColor, TextMeshProUGUI txt)
    {
        txt.color = TargetColor;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {

    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }
}
