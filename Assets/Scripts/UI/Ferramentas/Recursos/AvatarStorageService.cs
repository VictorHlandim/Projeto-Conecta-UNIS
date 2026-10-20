
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Supabase;
using Supabase.Storage;

public class AvatarStorageService
{
    private readonly Supabase.Client supabase;

    public AvatarStorageService(Supabase.Client client)
    {
        supabase = client;
    }

    public async Task<string> UploadAvatarAsync(byte[] pngBytes)
    {
        var user = supabase.Auth.CurrentUser;

        if (user == null)
            throw new InvalidOperationException(
                "O usuario precisa estar autenticado.");

        if (pngBytes == null || pngBytes.Length == 0)
            throw new ArgumentException("A imagem esta vazia.");

        string localPath = Path.Combine(
            Application.persistentDataPath,
            "avatar_upload.png"
        );

        string storagePath = user.Id + "/profile.png";

        try
        {
            File.WriteAllBytes(localPath, pngBytes);

            await supabase.Storage
                .From("avatars")
                .Upload(
                    localPath,
                    storagePath,
                    new Supabase.Storage.FileOptions
                    {
                        Upsert = true,
                        ContentType = "image/png",
                        CacheControl = "3600"
                    },
                    inferContentType: false
                );

            return storagePath;
        }
        finally
        {
            if (File.Exists(localPath))
                File.Delete(localPath);
        }
    }
}
