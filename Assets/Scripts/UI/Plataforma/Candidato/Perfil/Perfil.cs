
using TMPro;
using UnityEngine;
using System;

public class Perfil : UI
{
    public ProfileService profileService;

    [Header("Textos do perfil")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI infoText;

    private async void Awake()
    {
        LoadProfile();
    }
    public async void LoadProfile()
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
                nameText.text = perfil.Name ?? "";

            if (infoText != null)
                infoText.text = perfil.Presentation ?? "";

            Debug.Log($"Perfil carregado: {perfil.Name}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao carregar perfil: {ex}");
        }
    }
}
