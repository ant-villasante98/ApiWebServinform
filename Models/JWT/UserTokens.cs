namespace Servirform.Models.JWT;

public enum Roles
{
    administrador = 1,
    usuario = 2
};
public class UserTokens
{
    public string? Token { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public TimeSpan Validity { get; set; }
    public string? RefreshToken { get; set; }
    public Guid GuidId { get; set; }
    public DateTime ExpiredTime { get; set; }
    public Roles Rol { get; set; }
}
