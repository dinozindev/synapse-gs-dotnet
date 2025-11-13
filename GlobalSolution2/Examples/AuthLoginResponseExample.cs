using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class AuthLoginResponseExample : IExamplesProvider<LoginResponseDto>
{
    public LoginResponseDto GetExamples()
    {
        var loginResponse = new LoginResponseDto()
        {
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InJvZHJpZ28ubmV2ZXMiLCJyb2xlIjoiZ2VyZW50ZSIsIm5iZiI6MTc2MTE2NzMzOSwiZXhwIjoxNzYxMTcwOTM5LCJpYXQiOjE3NjExNjczMzksImlzcyI6Ik1vdHR1TW90dGlvbkFQSSIsImF1ZCI6Ik1vdHR1TW90dGlvbkNsaWVudHMifQ.RUsg9P7MHebgXfe3NdhBTqL94Ce-rdnBo15mfDVUPhg",
            ExpiresIn = 3600,
            TokenType = "Bearer"
        };
            
        return loginResponse;
    }
}
      