using System.Net.Mail;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Supabase;
using TMPro;

public class CandidatoCadastro : MonoBehaviour
{
    private UI ui;
    private ProfileService profileService;

    [Header("Paineis")]

    [SerializeField] private GameObject cadastroPanel;
    [SerializeField] private GameObject dashboardPanel;

    [Header("Campos do cadastro")]
    [SerializeField] private GameObject nomeInputOBJ;
    [SerializeField] private GameObject emailInputOBJ;
    [SerializeField] private GameObject senhaInputOBJ;
    [SerializeField] private GameObject confirmarSenhaInputOBJ;
    [SerializeField] private GameObject localInputOBJ;
    [SerializeField] private GameObject phoneInputOBJ;
    [SerializeField] private GameObject apresentacaoInputOBJ;
    [SerializeField] private GameObject setorDropOBJ;
    [SerializeField] private GameObject escolaridadeDropOBJ;

    private TMP_InputField nomeInput;
    private TMP_InputField emailInput;
    private TMP_InputField senhaInput;
    private TMP_InputField confirmarSenhaInput;
    private TMP_InputField localInput;
    private TMP_InputField phoneInput;
    private TMP_InputField apresentacaoInput;

    private TMP_Dropdown setorDrop;
    private TMP_Dropdown escolaridadeDrop;

    private bool cadastrando;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();

        nomeInput = nomeInputOBJ != null
            ? nomeInputOBJ.GetComponent<TMP_InputField>() : null;

        emailInput = emailInputOBJ != null
            ? emailInputOBJ.GetComponent<TMP_InputField>() : null;

        localInput = localInputOBJ != null
            ? localInputOBJ.GetComponent<TMP_InputField>() : null;

        phoneInput = phoneInputOBJ != null
            ? phoneInputOBJ.GetComponent<TMP_InputField>() : null;

        senhaInput = senhaInputOBJ != null
            ? senhaInputOBJ.GetComponent<TMP_InputField>() : null;

        confirmarSenhaInput = confirmarSenhaInputOBJ != null
            ? confirmarSenhaInputOBJ.GetComponent<TMP_InputField>() : null;

        apresentacaoInput = apresentacaoInputOBJ != null
            ? apresentacaoInputOBJ.GetComponent<TMP_InputField>() : null;

        setorDrop = setorDropOBJ != null
            ? setorDropOBJ.GetComponent<TMP_Dropdown>() : null;

        escolaridadeDrop = escolaridadeDropOBJ != null
            ? escolaridadeDropOBJ.GetComponent<TMP_Dropdown>() : null;

        Debug.Log(
            $"nome: {nomeInput != null}, " +
            $"email: {emailInput != null}, " +
            $"senha: {senhaInput != null}, " +
            $"confirmarSenha: {confirmarSenhaInput != null}, " +
            $"local: {localInput != null}, " +
            $"telefone: {phoneInput != null}, " +
            $"apresentacao: {apresentacaoInput != null}, " +
            $"setor: {setorDrop != null}, " +
            $"escolaridade: {escolaridadeDrop != null}"
        );
    }


    //private void Update()
    //{
    //    Debug.Log(nomeInput.text);
    //}

    public async void SalvarPerfil()
    {
        if (cadastrando)
            return;

        if (nomeInput == null ||
            emailInput == null ||
            senhaInput == null ||
            confirmarSenhaInput == null ||
            localInput == null ||
            phoneInput == null ||
            apresentacaoInput == null ||
            setorDrop == null ||
            escolaridadeDrop == null)
        {
            Debug.LogError(
                "Um ou mais campos do cadastro não foram encontrados. " +
                "Confira o Inspector e os componentes TMP."
            );
            return;
        }

        string nome = nomeInput.text.Trim();
        string email = emailInput.text.Trim();
        string senha = senhaInput.text.Trim();
        string confirmarSenha = confirmarSenhaInput.text.Trim();
        string local = localInput.text.Trim();
        string phone = phoneInput.text.Trim();
        string apresentacao = apresentacaoInput.text.Trim();
        string setor = setorDrop.options[setorDrop.value].text;
        string escolaridade =
            escolaridadeDrop.options[escolaridadeDrop.value].text;

        if (string.IsNullOrWhiteSpace(nome) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(local))
        {
            Debug.LogWarning(
                "Preencha nome, e-mail e localizacao."
            );
            return;
        }

        if (string.IsNullOrWhiteSpace(senha) || senha.Length < 6)
        {
            Debug.LogWarning(
                "A senha deve ter pelo menos 6 caracteres."
            );
            return;
        }

        if (senha != confirmarSenha)
        {
            Debug.LogWarning("As senhas nao coincidem.");
            return;
        }

        if (SupabaseManager.Supabase == null)
        {
            Debug.LogError("Supabase ainda nao foi inicializado.");
            return;
        }
        try
        {
            var endereco = new MailAddress(email);

            if (endereco.Address != email)
            {
                Debug.LogWarning("Informe um e-mail válido.");
                return;
            }
        }
        catch (FormatException)
        {
            Debug.LogWarning(
                $"E-mail inválido: '{email}'. " +
                "Informe um endereço como usuario@exemplo.com."
            );
            return;
        }

        cadastrando = true;

        try
        {
            var supabase = SupabaseManager.Supabase;

            // 1. Criar a conta no Supabase Auth.
            await supabase.Auth.SignUp(email, senha);

            // 2. Confirmar que existe uma sessao autenticada.
            // Com confirmacao de e-mail habilitada, pode nao
            // existir sessao ate o usuario confirmar e entrar.
            var user = supabase.Auth.CurrentUser;

            if (user == null)
            {
                Debug.Log(
                    "Conta criada. Verifique o e-mail, confirme " +
                    "a conta e faca login para salvar o perfil."
                );

                // Nao navegar para a dashboard sem sessao.
                // O fluxo de confirmacao/login deve continuar aqui.
                return;
            }

            // 3. Salvar os dados do perfil.
            profileService = new ProfileService(supabase);

            await profileService.SaveProfileAsync(
                nome,
                setor,
                escolaridade,
                local,
                apresentacao,
                phone
            );

            Debug.Log("Conta e perfil criados com sucesso!");

            // 4. Abrir a dashboard somente apos o sucesso.
            if (cadastroPanel != null)
                cadastroPanel.SetActive(false);

            if (dashboardPanel != null)
                dashboardPanel.SetActive(true);
        }
        catch (Exception ex)
        {
            Debug.LogError(
                $"Erro no cadastro: {ex.Message}"
            );
        }
        finally
        {
            cadastrando = false;
        }
    }
}
