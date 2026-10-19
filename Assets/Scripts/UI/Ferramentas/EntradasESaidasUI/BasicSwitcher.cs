using UnityEngine;
using UnityEngine.EventSystems;

public class BasicSwitcher : MonoBehaviour, IPointerDownHandler
{
    public WhereToGo whereToGo;
    private UI ui;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
    }

    public void Switching()
    {
        switch (whereToGo)
        {
            case WhereToGo.Geral:
                ui.ShowCadGeral();
                break;
            case WhereToGo.Candidato:
                ui.ShowCadCandidato();
                break;
            case WhereToGo.Empresa:
                ui.ShowCadEmpresa();
                break;
        }

        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Switching();
    }
}
public enum WhereToGo
{
    Geral,
    Candidato,
    Empresa
}
