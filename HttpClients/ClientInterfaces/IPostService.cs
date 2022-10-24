using Domain.DTOs;
using Domain.Models;

namespace HttpClients.ClientInterfaces;

public interface IPostService
{
    Task CreateAsync(PostCreationDTO dto);
    Task<ICollection<Post>> GetAsync(string? username, string? title, string? body);
    Task<PostBasicDTO> GetByIdAsync(int id);
    Task UpdateAsync(PostUpdateDTO dto);
    Task DeleteAsync(int id);
}