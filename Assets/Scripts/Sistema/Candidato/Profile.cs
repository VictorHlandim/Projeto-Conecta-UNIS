
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("profiles")]
public class Profile : BaseModel
{
    [PrimaryKey("id", false)]
    public string Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("phone")]
    public string Phone { get; set; }

    [Column("interest_sector")]
    public string InterestSector { get; set; }

    [Column("education")]
    public string Education { get; set; }

    [Column("location")]
    public string Location { get; set; }

    [Column("presentation")]
    public string Presentation { get; set; }

    [Column("avatar_path")]
    public string AvatarPath { get; set; }

    [Column("updated_at")]
    public System.DateTime UpdatedAt { get; set; }
}
