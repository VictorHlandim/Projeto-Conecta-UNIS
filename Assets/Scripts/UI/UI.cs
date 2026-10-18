using UnityEngine;

public class UI : MonoBehaviour
{
    public Favoritos favoritos { get; private set; }
    public Dashboard dashboard { get; private set; }
    public Enviados enviados { get; private set; }
    public Perfil perfil { get; private set; }

    private void Awake()
    {
        favoritos = GetComponentInChildren<Favoritos>(true);
        dashboard = GetComponentInChildren<Dashboard>(true);
        enviados = GetComponentInChildren<Enviados>(true);
        perfil = GetComponentInChildren<Perfil>(true);
    }

    public virtual void ShowFavoritos()
    {
        favoritos.gameObject.SetActive(true);
        perfil.gameObject.SetActive(false);
        dashboard.gameObject.SetActive(false);
        enviados.gameObject.SetActive(false);
    }
    public virtual void ShowDashboard()
    {
        favoritos.gameObject.SetActive(false);
        dashboard.gameObject.SetActive(true);
        perfil.gameObject.SetActive(false);
        enviados.gameObject.SetActive(false);
    }
    public virtual void ShowEnviados()
    {
        favoritos.gameObject.SetActive(false);
        perfil.gameObject.SetActive(false);
        dashboard.gameObject.SetActive(false);
        enviados.gameObject.SetActive(true);
    }
    public virtual void ShowPerfil()
    {
        favoritos.gameObject.SetActive(false);
        perfil.gameObject.SetActive(true);
        dashboard.gameObject.SetActive(false);
        enviados.gameObject.SetActive(false);
    }   
}
