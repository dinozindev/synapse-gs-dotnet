using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class RecomendacaoProfissionalPostDtoExample : IExamplesProvider<RecomendacaoProfissionalPostDto>
{
    public RecomendacaoProfissionalPostDto GetExamples()
    {
        return new RecomendacaoProfissionalPostDto(
            "Vaga Front-end Pleno", 
            "Oportunidade para desenvolvedor front-end com anos de experiência",
            "IA me de uma vaga para um desenvolvedor com conhecimentos avançados em React, Tailwind e Mobile", 
            "Vaga", 
            "Front-end", 
            "LinkedIn", 
            1
        );
    }
}