
using Supabase;
using System;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class ProfileService
{
    private readonly Client supabase;

    public ProfileService(Client supabaseClient)
    {
        supabase = supabaseClient
            ?? throw new ArgumentNullException(nameof(supabaseClient));
    }

    private Supabase.Gotrue.User GetAuthenticatedUser()
    {
        var user = supabase.Auth.CurrentUser;

        if (user == null)
        {
            throw new InvalidOperationException(
                "O usuário precisa estar autenticado.");
        }

        return user;
    }

    // Cria ou salva o perfil do usuário autenticado.
    public async Task SaveProfileAsync(
        string name,
        string interestSector,
        string education,
        string location,
        string presentation,
        string phone = "",
        string avatarPath = null)
    {
        var user = GetAuthenticatedUser();

        var profile = new Profile
        {
            Id = user.Id,
            Name = name,
            Email = user.Email,
            Phone = phone,
            InterestSector = interestSector,
            Education = education,
            Location = location,
            Presentation = presentation,
            AvatarPath = avatarPath,
            UpdatedAt = DateTime.UtcNow
        };

        await supabase
            .From<Profile>()
            .Upsert(profile);
    }

    // Carrega o perfil do usuário autenticado.
    public async Task<Profile> LoadProfileAsync()
    {
        var user = GetAuthenticatedUser();

        var result = await supabase
            .From<Profile>()
            .Where(p => p.Id == user.Id)
            .Get();

        return result.Models.Count > 0
            ? result.Models[0]
            : null;
    }

    // Atualiza os dados de um perfil que já existe.
    public async Task UpdateProfileAsync(
        string name,
        string interestSector,
        string education,
        string location,
        string presentation,
        string phone = "")
    {
        var profile = await LoadProfileAsync();

        if (profile == null)
        {
            throw new InvalidOperationException(
                "Perfil não encontrado. Salve o perfil antes de atualizá-lo.");
        }

        profile.Name = name;
        profile.InterestSector = interestSector;
        profile.Education = education;
        profile.Location = location;
        profile.Presentation = presentation;
        profile.Phone = phone;
        profile.UpdatedAt = DateTime.UtcNow;

        await supabase
            .From<Profile>()
            .Update(profile);
    }

    // Atualiza somente o caminho da foto do perfil.
    public async Task UpdateAvatarPathAsync(string avatarPath)
    {
        var profile = await LoadProfileAsync();

        if (profile == null)
        {
            throw new InvalidOperationException(
                "Perfil não encontrado. Crie o perfil antes de enviar a foto.");
        }

        profile.AvatarPath = avatarPath;
        profile.UpdatedAt = DateTime.UtcNow;

        await supabase
            .From<Profile>()
            .Update(profile);
    }
}
