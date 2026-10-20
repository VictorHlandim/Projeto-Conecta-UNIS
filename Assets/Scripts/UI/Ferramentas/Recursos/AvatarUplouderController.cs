
using System;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Supabase;

public class AvatarUploaderController : MonoBehaviour
{
    [SerializeField] private AvatarDisplay avatarDisplay;
    [SerializeField] private AvatarCropEditor cropEditor;
    [SerializeField] private TMP_Text statusText;

    private AvatarStorageService storageService;
    private ProfileService profileService;

    // Chame este metodo depois que o SupabaseManager
    // terminar de inicializar o cliente.
    public void Initialize(Client client)
    {
        if (client == null)
        {
            Debug.LogError("Cliente Supabase não foi fornecido.");
            return;
        }

        if (cropEditor == null)
        {
            Debug.LogError("AvatarCropEditor não foi atribuído no Inspector.");
            return;
        }

        storageService = new AvatarStorageService(client);
        profileService = new ProfileService(client);

        cropEditor.OnAvatarReady -= OnAvatarReady;
        cropEditor.OnAvatarReady += OnAvatarReady;

        if (avatarDisplay != null)
            avatarDisplay.Initialize(client);
        else
            Debug.LogWarning("AvatarDisplay não foi atribuído no Inspector.");

        Debug.Log("AvatarUploaderController inicializado.");
    }

    private void OnAvatarReady(byte[] pngBytes)
    {
        _ = SaveAvatarAsync(pngBytes);
    }


    private async Task SaveAvatarAsync(byte[] pngBytes)
    {
        try
        {
            statusText.text = "Enviando foto...";

            string path = await storageService.UploadAvatarAsync(
                pngBytes
            );

            Debug.Log("Upload concluído: " + path);

            statusText.text = "Atualizando perfil...";

            await profileService.UpdateAvatarPathAsync(path);

            Debug.Log("Perfil atualizado no banco.");

            // Atualiza a foto visível na interface.
            if (avatarDisplay != null)
            {
                StartCoroutine(avatarDisplay.LoadAvatar(path));
            }

            statusText.text = "Foto de perfil salva!";
        }
        catch (Exception e)
        {
            statusText.text = "Erro ao salvar a foto.";
            Debug.LogException(e);
        }
    }


    private void OnDestroy()
    {
        if (cropEditor != null)
            cropEditor.OnAvatarReady -= OnAvatarReady;
    }
}
