using UnityEngine;

public class UI : MonoBehaviour
{
    public AvatarCropEditor avatarCrop;
    public SupabaseManager supabaseManager { get; private set; }
    public Favoritos favoritos { get; private set; }
    public Dashboard dashboard { get; private set; }
    public Enviados enviados { get; private set; }
    public Perfil perfil { get; private set; }
    public CadastroGeral geral { get; private set; }
    public CandidatoCadastro CadCandidato { get; private set; }
    public EmpresaCadastro CadEmpresa { get; private set; }

    public EmpresaSection empresa { get; private set; }
    public CandidatoSection candidato { get; private set; }

    private void Awake()
    {

        favoritos = GetComponentInChildren<Favoritos>(true);
        dashboard = GetComponentInChildren<Dashboard>(true);
        enviados = GetComponentInChildren<Enviados>(true);
        perfil = GetComponentInChildren<Perfil>(true);
        supabaseManager = FindFirstObjectByType<SupabaseManager>(
            FindObjectsInactive.Include);

        candidato = FindFirstObjectByType<CandidatoSection>(
            FindObjectsInactive.Include);

        empresa = FindFirstObjectByType<EmpresaSection>(
            FindObjectsInactive.Include);

        geral = FindFirstObjectByType<CadastroGeral>(
            FindObjectsInactive.Include);

        CadCandidato = FindFirstObjectByType<CandidatoCadastro>(
            FindObjectsInactive.Include);

        CadEmpresa = FindFirstObjectByType<EmpresaCadastro>(
            FindObjectsInactive.Include);

        avatarCrop = FindFirstObjectByType<AvatarCropEditor>(
            FindObjectsInactive.Include);


        Debug.Log($"Cadastro Geral: {geral}");
        Debug.Log($"Cadastro Candidato: {CadCandidato}");
        Debug.Log($"Cadastro Empresa: {CadEmpresa}");
        Debug.Log($"Empresa Section: {empresa}");
        Debug.Log($"Candidato Section: {candidato}");
        if (avatarCrop != null)
        {
            Debug.Log($"Crop encontrado: {avatarCrop.gameObject.name}");
        }
        else
        {
            Debug.LogError("AvatarCropEditor não foi encontrado na cena!");
        }
    }
    private void Update()
    {
        
    }

    private void Start()
    {
        try
        {
            dashboard.LoadDashboard();
            perfil.LoadProfile();
        }
        catch
        {
            Debug.LogAssertion("Falha ao inicializar");
        }
    }

    #region UI Event Changer Overrides

    #region Cadastro

    public virtual void desativarCadastros()
    {
        CadEmpresa.gameObject?.SetActive(false);
        CadCandidato.gameObject?.SetActive(false);
        geral.gameObject?.SetActive(false);
    }
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

    public virtual void ShowEmpresa()
    {
        empresa.gameObject.SetActive(true);
        candidato.gameObject?.SetActive(false);
        desativarCadastros();
    }
    public virtual void ShowCandidato()
    {
        candidato.gameObject.SetActive(true);
        empresa.gameObject?.SetActive(false);
        desativarCadastros();
    }
    public void AbrirPainelFoto()
    {
        if (avatarCrop == null)
        {
            Debug.LogError("Não é possível abrir: AvatarCropEditor está nulo.");
            return;
        }

        avatarCrop.gameObject.SetActive(true);
        Debug.Log("Painel de foto aberto.");
    }

    public void FecharPainelFoto()
    {
        if (avatarCrop == null)
        {
            Debug.LogError("Não é possível fechar: AvatarCropEditor está nulo.");
            return;
        }

        avatarCrop.gameObject.SetActive(false);
    }
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
