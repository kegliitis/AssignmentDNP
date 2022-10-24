namespace Domain.DTOs;

public class SearchParametersDTO
{
    public string? UsernameContains { get; }

    public SearchParametersDTO(string? usernameContains)
    {
        UsernameContains = usernameContains;
    }
}