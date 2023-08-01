using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MGAware.Database.DAL;

public class Contact
{
    public int Id { get; set; }
    [MaxLength(150),Required]
    public required string Name { get; set; }
    [MaxLength(150)]
    public string? Email { get; set; }
    [MaxLength(50)]
    public string? FoneNumber { get; set; }

    [JsonIgnore]
    public virtual Person? Person { get; set; }
}