using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorSwitcher : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public Image Image;
    public float CrossFadeFloat = 0.25f;
    [SerializeField]
    private WhatToSwitch WhatToSwitch;
    [SerializeField]
    private Color Target;
    [SerializeField]
    private Color OriginalColor;


    private void Awake()
    {
        Text = GetComponentInChildren<TextMeshProUGUI>();
        Image = GetComponentInChildren<Image>();
    }

    void Update()
    {
        Changer();
    }

    private void Changer()
    {
        switch (WhatToSwitch)
        {
            case WhatToSwitch.None:
                break;
            case WhatToSwitch.Text:
                ChangeTextColor();
                break;
            case WhatToSwitch.Image:
                ChangeImageColor();
                break;
            default:
                break;
        }
    } 

    private void ChangeTextColor()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            Text.color = Target;
        }
        else
        {
            Text.color = OriginalColor;
        }
    }
    private void ChangeImageColor()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            Image.CrossFadeColor(Target, CrossFadeFloat, true, true);
        }
        else
        {
            Image.CrossFadeColor(OriginalColor, CrossFadeFloat, true, true);
        }
    }
}
public enum WhatToSwitch
{
    None,
    Text,
    Image
}
