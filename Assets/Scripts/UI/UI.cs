using UnityEngine;

public class UI : MonoBehaviour
{
    public Favoritos favoritos { get; private set; }
    public Dashboard dashboard { get; private set; }
    public Enviados enviados { get; private set; }

    private void Awake()
    {
        favoritos = GetComponentInChildren<Favoritos>(true);
        dashboard = GetComponentInChildren<Dashboard>(true);
        enviados = GetComponentInChildren<Enviados>(true);
    }

    public void ShowFavoritos()
    {
        favoritos.gameObject.SetActive(true);
        dashboard.gameObject.SetActive(false);
        enviados.gameObject.SetActive(false);
    }
    public void ShowDashboard()
    {
        favoritos.gameObject.SetActive(false);
        dashboard.gameObject.SetActive(true);
        enviados.gameObject.SetActive(false);
    }
    public void ShowEnviados()
    {
        favoritos.gameObject.SetActive(false);
        dashboard.gameObject.SetActive(false);
        enviados.gameObject.SetActive(true);
    }
}
