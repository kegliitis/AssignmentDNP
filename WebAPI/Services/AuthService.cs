using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace WebAPI.Services;

public class AuthService : IAuthService
{
    private readonly IUserLogic _userLogic;

    public AuthService(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }
    
    public async Task<User> ValidateUser(string username, string password)
    {
        User? existingUser = await _userLogic.ValidateUser(new UserValidationDTO(username, password));

        if (existingUser == null)
            throw new Exception("Credentials invalid!");
        
        return await Task.FromResult(existingUser);
    }

    public async Task<Task> RegisterUser(UserCreationDTO user)
    {
        User? created = await _userLogic.CreateAsync(user);
            if(created == null)
                throw new Exception("Username already taken!");
            return Task.CompletedTask;
    }
}