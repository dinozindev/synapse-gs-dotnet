using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class LoginV1ResponseDtoExample : IExamplesProvider<LoginV1ResponseDto>
{
    public LoginV1ResponseDto GetExamples()
    {
        var loginResponse = new LoginV1ResponseDto()
        {
            Message = "Login realizado com sucesso!",
            Usuario = "maria.silva"
        };
            
        return loginResponse;
    }
}
   