
using System;
using UnityEngine;

public class SessionPersistence : MonoBehaviour
{
    [Header("Painéis da interface")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject dashboardPanel;

    private async void Start()
    {
        // Evita mostrar a tela errada enquanto verifica a sessão.
        if (loginPanel != null)
            loginPanel.SetActive(false);

        if (dashboardPanel != null)
            dashboardPanel.SetActive(false);

        try
        {
            await SupabaseManager.Instance.InitializationTask;

            var user = SupabaseManager.Supabase.Auth.CurrentUser;

            if (user != null)
            {
                Debug.Log("Usuário autenticado: " + user.Email);
                dashboardPanel.SetActive(true);
            }
            else
            {
                Debug.Log("Nenhuma sessão ativa.");
                loginPanel.SetActive(true);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Erro ao verificar sessão: " + ex.Message);

            loginPanel.SetActive(true);
        }
    }
}
