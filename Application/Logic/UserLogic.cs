using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace Application.Logic;

public class UserLogic : IUserLogic
{
    private readonly IUserDAO _userDAO;

    public UserLogic(IUserDAO userDAO)
    {
        _userDAO = userDAO;
    }

    public async Task<User> CreateAsync(UserCreationDTO dto)
    {
        User? existing = await _userDAO.GetByUsernameAsync(dto.Username);
        if (existing != null)
            throw new Exception("Username already taken!");

        ValidateData(dto);

        var toBeCreated = new User
        {
            Username = dto.Username,
            Password = dto.Password
        };
        
        User created = await _userDAO.CreateAsync(toBeCreated);

        return created;
    }

    public Task<IEnumerable<User>> GetAsync(SearchParametersDTO searchParameters)
    {
        return _userDAO.GetAsync(searchParameters);
    }

    public Task<User?> ValidateUser(UserValidationDTO userValidationDto)
    {
        return _userDAO.ValidateUser(userValidationDto);
    }

    private static void ValidateData(UserCreationDTO userToCreate)
    {
        string username = userToCreate.Username;

        if (username.Length < 3)
            throw new Exception("Username must be at least 3 characters!");

        if (username.Length > 15)
            throw new Exception("Username must be less than 16 characters!");
    }
}