using Domain.Models;

namespace Domain.DTOs;

public class PostCreationDTO
{
    public int OwnerId { get; }
    public string Title { get; }
    public string Body { get; }

    public PostCreationDTO(int ownerId, string title, string body)
    {
        OwnerId = ownerId;
        Title = title;
        Body = body;
    }
}