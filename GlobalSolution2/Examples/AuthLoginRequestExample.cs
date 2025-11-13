using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class AuthLoginRequestExample : IExamplesProvider<LoginRequestDto>
{
    public LoginRequestDto GetExamples()
    {
        var loginRequest = new LoginRequestDto(
            NomeUsuario: "maria.silva",
            SenhaUsuario: "senha123"
            );
            
        return loginRequest;
    }

}