namespace MGAware.Database.DTO;
public class TokenConfigurationsDto
{
    public string? Audience { get; set; }
    public string? Issuer { get; set; }
    public int Seconds { get; set; }
    public string? SecretJwtKey { get; set; }
}