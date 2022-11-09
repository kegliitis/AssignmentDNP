using Domain.DTOs;
using Domain.Models;

namespace Application.DAOInterfaces;

public interface IUserDAO
{
    Task<User> CreateAsync(User user);
    Task<User?> GetByUsernameAsync(string username);
    Task<IEnumerable<User>> GetAsync(SearchParametersDTO searchParameters);
    Task<User?> GetByIdAsync(int id);
    Task<User?> ValidateUser(UserValidationDTO userValidationDto);
}