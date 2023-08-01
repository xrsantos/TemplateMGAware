using System.Text.Json.Serialization;

namespace MGAware.Database.DAL;

public class Person
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    // Propriedade de navegação
    public virtual ICollection<Contact>? Contacts { get; set; }
}