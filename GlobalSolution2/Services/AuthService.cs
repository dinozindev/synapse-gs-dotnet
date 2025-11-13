namespace GlobalSolution2.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly JwtTokenService _jwt;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext db, JwtTokenService jwt, ILogger<AuthService> logger)
    {
        _db = db;
        _jwt = jwt;
        _logger = logger;
    }

    public LoginResult Login(string nomeUsuario, string password)
    {
        _logger.LogInformation("Tentativa de login para usuário {User}", nomeUsuario);

        var user = _db.Usuarios
            .FirstOrDefault(u => u.NomeUsuario == nomeUsuario);

        if (user == null)
        {
            _logger.LogWarning("Usuário {User} não encontrado", nomeUsuario);
            return new LoginResult { Success = false };
        }
            
        if (password != user.SenhaUsuario)
        {
            _logger.LogWarning("Senha incorreta para o usuário {User}", nomeUsuario);
            return new LoginResult { Success = false };
        }

        var token = _jwt.GenerateToken(nomeUsuario, "gerente");
        _logger.LogInformation("Login bem-sucedido para usuário {User}", nomeUsuario);
        
        return new LoginResult { Success = true, Token = token };
    }
}

public class LoginResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
}
