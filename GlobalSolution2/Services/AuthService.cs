namespace GlobalSolution2.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly JwtTokenService _jwt;

    public AuthService(AppDbContext db, JwtTokenService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public LoginResult Login(string nomeUsuario, string password)
    {
        var user = _db.Usuarios
            .FirstOrDefault(u => u.NomeUsuario == nomeUsuario);

        if (user == null)
            return new LoginResult { Success = false };
            
        if (password != user.SenhaUsuario)
            return new LoginResult { Success = false };

        var token = _jwt.GenerateToken(nomeUsuario, "gerente");
        return new LoginResult { Success = true, Token = token };
    }
}

public class LoginResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
}
