using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Post
{
    [Key]
    public int Id { get; set; }
    public User Creator { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }

    public Post(User creator, string title, string body)
    {
        Creator = creator;
        Title = title;
        Body = body;
    }

    public Post()
    {
        
    }
}