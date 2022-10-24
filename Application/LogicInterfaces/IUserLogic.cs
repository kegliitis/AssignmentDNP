using Domain.DTOs;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface IUserLogic
{
    Task<User> CreateAsync(UserCreationDTO dto);
    Task<IEnumerable<User>> GetAsync(SearchParametersDTO searchParameters);
}