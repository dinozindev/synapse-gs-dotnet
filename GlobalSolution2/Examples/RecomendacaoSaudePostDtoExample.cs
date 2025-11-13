using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class RecomendacaoSaudePostDtoExample : IExamplesProvider<RecomendacaoSaudePostDto>
{
    public RecomendacaoSaudePostDto GetExamples()
    {
        return new RecomendacaoSaudePostDto(
            "Melhorar sono", 
            "Optar por dormir em um horário antes da Meia-noite para uma melhor noite de sono.",
            "IA me de uma sugestão de como ajustar meu horário de sono para melhorar minha energia e estresse durante o dia.", 
            "Sono", 
            "Moderado", 
            "Estabeleça rotina de sono consistente", 
            1
        );
    }
}