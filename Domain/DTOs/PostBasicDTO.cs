namespace Domain.DTOs;

public class PostBasicDTO
{
    public int Id { get; }
    public string Username { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }

    public PostBasicDTO(int id, string username, string title, string body)
    {
        Id = id;
        Username = username;
        Title = title;
        Body = body;
    }
}