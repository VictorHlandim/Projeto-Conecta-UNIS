
using Supabase;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class SupabaseManager : MonoBehaviour
{
    public static SupabaseManager Instance { get; private set; }
    public static Client Supabase { get; private set; }

    public Task InitializationTask { get; private set; }

    private UI ui;

    private const string Url =
        "https://ijrynrqsppznhwnwhgox.supabase.co";

    private const string Key =
        "sb_publishable_AWFfW64ExTFcxNS0Hq05Dg_En9MKbk1";

    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializationTask = InitializeAsync();

        try
        {
            await InitializationTask;

            Debug.Log("Supabase inicializado com sucesso.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Falha ao inicializar Supabase: {ex}");

            // Não tentar acessar Auth se a inicialização falhou.
            return;
        }

        var avatarController =
            FindFirstObjectByType<AvatarUploaderController>();

        if (avatarController != null)
            avatarController.Initialize(Supabase);

        ui = FindFirstObjectByType<UI>(
            FindObjectsInactive.Include
        );

        if (ui == null)
        {
            Debug.LogError("Não encontrei o controlador de UI.");
            return;
        }

        var user = Supabase.Auth.CurrentUser;

        if (user != null)
        {
            Debug.Log($"Sessão encontrada: {user.Email}");
            ui.desativarCadastros();
            ui.ShowCandidato();
        }
        else
        {
            Debug.Log("Nenhum usuário autenticado.");
            ui.ShowCadGeral();
        }
    }

    private async Task InitializeAsync()
    {
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = false,
            SessionHandler = new UnitySupabaseSessionHandler()
        };

        Supabase = new Client(Url, Key, options);

        Supabase.Auth.LoadSession();

        await Supabase.InitializeAsync();
    }

    [ContextMenu("Limpar Sessão Local")]
    public void LimparSessaoLocal()
    {
        Supabase?.Auth.SignOut();

        PlayerPrefs.DeleteKey("SUPABASE_AUTH_SESSION");
        PlayerPrefs.Save();

        Debug.Log("Solicitada a limpeza da sessão.");
    }
}
