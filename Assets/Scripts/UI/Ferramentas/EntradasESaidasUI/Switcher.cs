using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Switcher : MonoBehaviour, IPointerDownHandler
{
    private UI ui;

    [SerializeField]
    private WhereToGo whereToGo;
    [SerializeField]
    private float FadeTime = .25f;

    [SerializeField]
    private Color Target;
    [SerializeField]
    private Color OgColor;

    [Space]
    [Header("NavBar")]
    public Image Fav;
    public Image Env;
    public Image Vag;
    public Image Per;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
    }
    private void Update()
    {
        NavBarHandle();
    }
    private void NavBarHandle()
    {
        if(Fav == null || Env == null || Vag == null || Per == null)
        {
            return;
        }
        if (ui.enviados.gameObject.activeInHierarchy)
        {
            Fav.CrossFadeColor(OgColor, FadeTime, true, true);
            Env.CrossFadeColor(Target, FadeTime, true, true);
            Per.CrossFadeColor(OgColor, FadeTime, true, true);
            Vag.CrossFadeColor(OgColor, FadeTime, true, true);
        }
        else if(ui.dashboard.gameObject.activeInHierarchy)
        {
            Fav.CrossFadeColor(OgColor, FadeTime, true, true);
            Env.CrossFadeColor(OgColor, FadeTime, true, true);
            Vag.CrossFadeColor(Target, FadeTime, true, true);
            Per.CrossFadeColor(OgColor, FadeTime, true, true);
        }
        else if (ui.favoritos.gameObject.activeInHierarchy)
        {
            Fav.CrossFadeColor(Target, FadeTime, true, true);
            Env.CrossFadeColor(OgColor, FadeTime, true, true);
            Vag.CrossFadeColor(OgColor, FadeTime, true, true);
            Per.CrossFadeColor(OgColor, FadeTime, true, true);
        }
        else if (ui.perfil.gameObject.activeInHierarchy)
        {
            Fav.CrossFadeColor(OgColor, FadeTime, true, true);
            Env.CrossFadeColor(OgColor, FadeTime, true, true);
            Vag.CrossFadeColor(OgColor, FadeTime, true, true);
            Per.CrossFadeColor(Target, FadeTime, true, true);
        }
    }
    public void Switching()
    {
        switch (whereToGo)
        {
            case WhereToGo.Favoritos:
                ui.ShowFavoritos();
                break;
            case WhereToGo.Vagas:
                ui.ShowDashboard();
                break;
            case WhereToGo.Enviados:
                ui.ShowEnviados();
                break;
            case WhereToGo.Perfil:
                ui.ShowPerfil();
                break;
            case WhereToGo.CadastroGeral:
                ui.ShowCadGeral();
                break;
            case WhereToGo.CadastroEmpresa:
                    ui.ShowCadEmpresa();
                    break;
            default:
                Debug.LogWarning("To sem valor papai (Switch case)");
                break;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Switching();
    }

    public enum WhereToGo
    {
        None,
        Favoritos,
        Vagas,
        Enviados,
        Perfil,
        CadastroGeral,
        CadastroEmpresa,
        CadastroCandidato
    }
}