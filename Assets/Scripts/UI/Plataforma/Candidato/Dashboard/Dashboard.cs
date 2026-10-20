using System;
using TMPro;
using UnityEngine;

public class Dashboard : MonoBehaviour
{
    public ProfileService profileService;

    [Header("Textos do perfil")]
    [SerializeField] private TextMeshProUGUI nameText;

    private async void Awake()
    {
        LoadDashboard();
    }
    public async void LoadDashboard()
    {
        try
        {
            // Espera o Supabase terminar de inicializar.
            await SupabaseManager.Instance.InitializationTask;

            // Cria o serviço usando o cliente já inicializado.
            profileService = new ProfileService(
                SupabaseManager.Supabase
            );

            // Carrega os dados do usuário autenticado.
            Profile perfil = await profileService.LoadProfileAsync();

            if (perfil == null)
            {
                Debug.LogWarning("Perfil não encontrado.");
                return;
            }

            // Atualiza os textos da interface.
            if (nameText != null)
                nameText.text = "Eai " + perfil.Name + "!" ?? "";

            Debug.Log($"Perfil carregado: {perfil.Name}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao carregar perfil: {ex}");
        }
    }
}