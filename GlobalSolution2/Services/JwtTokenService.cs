using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GlobalSolution2.Services;

public class JwtTokenService
{
    private readonly IConfiguration _config;
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtTokenService(IConfiguration config)
    {
        _config = config;
        
        _secret = Environment.GetEnvironmentVariable("JwtSettings__Secret") 
                  ?? _config["JwtSettings:Secret"]
                  ?? throw new InvalidOperationException(
                      "JWT Secret não configurado. Configure 'JwtSettings__Secret' nas variáveis de ambiente ou appsettings.json");
        
        _issuer = Environment.GetEnvironmentVariable("JwtSettings__Issuer")
                  ?? _config["JwtSettings:Issuer"]
                  ?? "ProjetoGlobalSolutionAPI";
        
        _audience = Environment.GetEnvironmentVariable("JwtSettings__Audience")
                    ?? _config["JwtSettings:Audience"]
                    ?? "ProjetoGlobalSolutionUsers";
        
        if (_secret.Length < 32)
        {
            throw new InvalidOperationException(
                "JWT Secret deve ter no mínimo 32 caracteres para segurança adequada");
        }
    }

    public string GenerateToken(string username, string role = "user")
    {
        var key = Encoding.ASCII.GetBytes(_secret);
        var handler = new JwtSecurityTokenHandler();

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }
}