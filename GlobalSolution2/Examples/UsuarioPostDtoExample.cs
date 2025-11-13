using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class UsuarioPostDtoExample : IExamplesProvider<UsuarioPostDto>
{
    public UsuarioPostDto GetExamples()
    {
        return new UsuarioPostDto(
            "jorge.roberto",
            "jorge12345",
            "Frentista",
            "Back-end Java",
            "Transição para Aplicações Back-end com Java e Spring Boot",
            "Nenhuma"
        );
    }
}