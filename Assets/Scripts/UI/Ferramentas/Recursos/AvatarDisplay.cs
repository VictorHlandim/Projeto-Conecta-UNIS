
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Supabase;

public class AvatarDisplay : MonoBehaviour
{
    [Header("Todas as imagens de avatar da interface")]
    [SerializeField]
    private List<RawImage> avatarImages =
        new List<RawImage>();

    private Client supabase;
    private Texture2D currentTexture;

    public void Initialize(Client client)
    {
        supabase = client;
    }

    public IEnumerator LoadAvatar(string avatarPath)
    {
        if (supabase == null)
        {
            Debug.LogError("AvatarDisplay não foi inicializado.");
            yield break;
        }

        if (string.IsNullOrWhiteSpace(avatarPath))
        {
            Debug.LogWarning("O perfil não possui AvatarPath.");
            yield break;
        }

        string url = supabase.Storage
            .From("avatars")
            .GetPublicUrl(avatarPath);

        Debug.Log($"Carregando avatar. Caminho: {avatarPath}");
        Debug.Log($"URL do avatar: {url}");

        url += "?v=" +
            System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        using (UnityWebRequest request =
            UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    $"Erro ao carregar avatar: {request.error} | URL: {url}"
                );
                yield break;
            }

            Texture2D newTexture =
                DownloadHandlerTexture.GetContent(request);

            if (newTexture == null)
            {
                Debug.LogError("O download não retornou uma textura.");
                yield break;
            }

            if (currentTexture != null)
                Destroy(currentTexture);

            currentTexture = newTexture;

            int updated = 0;

            foreach (RawImage image in avatarImages)
            {
                if (image == null)
                    continue;

                image.texture = currentTexture;
                image.enabled = true;
                updated++;

                Debug.Log($"Avatar aplicado em: {image.gameObject.name}");
            }

            if (updated == 0)
            {
                Debug.LogError(
                    "Nenhum RawImage foi configurado na lista Avatar Images."
                );
            }
        }
    }

    public void SetAvatarLocally(Texture2D texture)
    {
        if (texture == null)
            return;

        foreach (RawImage image in avatarImages)
        {
            if (image != null)
                image.texture = texture;
        }
    }

    private void OnDestroy()
    {
        if (currentTexture != null)
            Destroy(currentTexture);
    }
}
