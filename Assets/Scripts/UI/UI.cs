using UnityEngine;

public class UI : MonoBehaviour
{
    public Favoritos favoritos { get; private set; }
    public Dashboard dashboard { get; private set; }
    public Enviados enviados { get; private set; }
    public Perfil perfil { get; private set; }
    public CadastroGeral geral { get; private set; }
    public CandidatoCadastro CadCandidato { get; private set; }
    public EmpresaCadastro CadEmpresa { get; private set; }

    private void Awake()
    {
        favoritos = GetComponentInChildren<Favoritos>(true);
        dashboard = GetComponentInChildren<Dashboard>(true);
        enviados = GetComponentInChildren<Enviados>(true);
        perfil = GetComponentInChildren<Perfil>(true);

        geral = FindFirstObjectByType<CadastroGeral>(
            FindObjectsInactive.Include);

        CadCandidato = FindFirstObjectByType<CandidatoCadastro>(
            FindObjectsInactive.Include);

        CadEmpresa = FindFirstObjectByType<EmpresaCadastro>(
            FindObjectsInactive.Include);

        Debug.Log($"Cadastro Geral: {geral}");
        Debug.Log($"Cadastro Candidato: {CadCandidato}");
        Debug.Log($"Cadastro Empresa: {CadEmpresa}");
    }
    private void Update()
    {
        
    }

    #region UI Event Changer Overrides

    #region Cadastro
    public virtual void ShowCadEmpresa(){
        CadEmpresa.gameObject?.SetActive(true);
        CadCandidato.gameObject?.SetActive(false);
        geral.gameObject?.SetActive(false);
    }
    public virtual void ShowCadCandidato(){
        CadEmpresa.gameObject?.SetActive(false);
        CadCandidato.gameObject?.SetActive(true);
        geral.gameObject?.SetActive(false);
    }
    public virtual void ShowCadGeral(){
        CadEmpresa.gameObject?.SetActive(false);
        CadCandidato.gameObject?.SetActive(false);
        geral.gameObject.SetActive(true);
    }
    #endregion



    #region Candidato
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
    #endregion


    #endregion
}
