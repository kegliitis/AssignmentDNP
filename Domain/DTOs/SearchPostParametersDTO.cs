namespace Domain.DTOs;

public class SearchPostParametersDTO
{
    public string? Username { get; }
    public string? TitleContains { get; }
    public string? BodyContains { get; }

    public SearchPostParametersDTO(string? username, string? title, string? body)
    {
        Username = username;
        TitleContains = title;
        BodyContains = body;
    }
}