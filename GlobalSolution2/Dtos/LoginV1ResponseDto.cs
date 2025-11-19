using System.Text.Json.Serialization;
using GlobalSolution2.Models;

namespace GlobalSolution2.Dtos;

public class LoginV1ResponseDto
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }

    [JsonPropertyName("usuario")]
    public required string Usuario { get; set; }
}