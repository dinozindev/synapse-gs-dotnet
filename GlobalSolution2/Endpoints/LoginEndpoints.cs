using System.Security.Claims;
using Asp.Versioning.Builder;
using GlobalSolution2.Dtos;
using GlobalSolution2.Services;

namespace GlobalSolution2.Endpoints;

public static class LoginEndpoints
{
    public static void MapLoginEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/auth")
            .WithApiVersionSet(apiVersionSet)
            .WithTags("Auth");

        group.MapPost("/login", (LoginRequestDto request, AuthService authService) =>
        {
            var result = authService.Login(request.NomeUsuario, request.SenhaUsuario);

            if (!result.Success)
                return Results.Unauthorized();

            return Results.Ok(new
            {
                message = "Login realizado com sucesso!",
                usuario = request.NomeUsuario
            });
        })
        .MapToApiVersion(1.0)
        .WithName("Faz login e retorna usuário (V1)")
        .WithSummary("Login simples sem JWT (V1)")
        .Produces<LoginV1ResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Accepts<LoginRequestDto>("application/json")
        .AllowAnonymous();

        group.MapPost("/login", HandleLogin)
            .WithName("LoginV2")
            .WithSummary("Faz login e retorna um token JWT (V2)")
            .WithDescription("Autentica um usuário e retorna um token JWT válido por 1 hora")
            .MapToApiVersion(2, 0)
            .Produces<LoginResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Accepts<LoginRequestDto>("application/json")
            .AllowAnonymous();

        group.MapPost("/refresh-token", HandleRefreshToken)
            .WithName("RefreshTokenV2")
            .WithSummary("Renova um token JWT expirado (V2)")
            .MapToApiVersion(2, 0)
            .Produces<LoginResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireRateLimiting("login")
            .RequireAuthorization();
    }

    private static IResult HandleLogin(LoginRequestDto request, AuthService authService, ILogger<AuthService> logger)
    {
        try
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(request.NomeUsuario))
                return Results.BadRequest(new { message = "Nome de usuário é obrigatório" });

            if (string.IsNullOrWhiteSpace(request.SenhaUsuario))
                return Results.BadRequest(new { message = "Senha é obrigatória" });

            logger.LogInformation($"Tentativa de login para usuário: {request.NomeUsuario}");

            // Tentar autenticar
            var result = authService.Login(request.NomeUsuario, request.SenhaUsuario);

            if (!result.Success)
            {
                logger.LogWarning($"Falha de autenticação para usuário: {request.NomeUsuario}");
                return Results.Unauthorized();
            }

            logger.LogInformation($"Login bem-sucedido para usuário: {request.NomeUsuario}");

            return Results.Ok(new LoginResponseDto
            {
                Token = result.Token,
                ExpiresIn = 3600,
                TokenType = "Bearer"
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao fazer login");
            return Results.Problem($"Erro ao fazer login: {ex.Message}", statusCode: 500);
        }
    }

    private static IResult HandleRefreshToken(
        HttpContext context,
        JwtTokenService jwtService,
        ILogger<AuthService> logger)
    {
        try
        {
            var username = context.User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
                return Results.Unauthorized();

            var newToken = jwtService.GenerateToken(username,
                context.User.FindFirst(ClaimTypes.Role)?.Value ?? "user");

            return Results.Ok(new LoginResponseDto
            {
                Token = newToken,
                ExpiresIn = 3600,
                TokenType = "Bearer"
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao renovar token");
            return Results.Problem(statusCode: 500);
        }
    }
}