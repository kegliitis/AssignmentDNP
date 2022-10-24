namespace Domain.DTOs;

public class PostUpdateDTO
{
    public int Id { get; }
    public string Username { get; set; }

    public string? Title { get; set; }

    public string? Body { get; set; }
   
    public PostUpdateDTO(int id)
    {
        Id = id;
    }
}