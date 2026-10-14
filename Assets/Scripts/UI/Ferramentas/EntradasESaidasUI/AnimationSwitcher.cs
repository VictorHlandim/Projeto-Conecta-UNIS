using UnityEngine;
using UnityEngine.EventSystems;

public class AnimationSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private bool highlight;

    [SerializeField]
    private string hightlightAnimationName;
    [SerializeField]
    private string clickAnimationName;

    [SerializeField]
    private Animator anim;

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool(hightlightAnimationName, true);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        anim.SetTrigger(clickAnimationName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool(hightlightAnimationName, false);
    }
}
