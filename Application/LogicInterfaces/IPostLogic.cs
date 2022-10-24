using Domain.DTOs;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface IPostLogic
{
    Task<Post> CreateAsync(PostCreationDTO dto);
    Task<IEnumerable<Post>> GetAsync(SearchPostParametersDTO searchParameters);

    Task UpdateAsync(PostUpdateDTO dto);
    Task DeleteAsync(int id);

    Task<PostBasicDTO> GetByIdAsync(int id);
}