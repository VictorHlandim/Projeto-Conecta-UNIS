
using Newtonsoft.Json;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using UnityEngine;

public class UnitySupabaseSessionHandler
    : IGotrueSessionPersistence<Session>
{
    private const string SessionKey = "SUPABASE_AUTH_SESSION";

    public void SaveSession(Session session)
    {
        if (session == null)
            return;

        string json = JsonConvert.SerializeObject(session);

        PlayerPrefs.SetString(SessionKey, json);
        PlayerPrefs.Save();

        Debug.Log("Sessão Supabase salva.");
    }

    public Session LoadSession()
    {
        if (!PlayerPrefs.HasKey(SessionKey))
            return null;

        string json = PlayerPrefs.GetString(SessionKey);

        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            return JsonConvert.DeserializeObject<Session>(json);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Erro ao restaurar sessão: {ex.Message}");
            return null;
        }
    }

    public void DestroySession()
    {
        PlayerPrefs.DeleteKey(SessionKey);
        PlayerPrefs.Save();

        Debug.Log("Sessão Supabase removida.");
    }
}
