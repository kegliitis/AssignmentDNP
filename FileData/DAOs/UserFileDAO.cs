using Application.DAOInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace FileData.DAOs;

public class UserFileDAO : IUserDAO
{
    private readonly FileContext _fileContext;

    public UserFileDAO(FileContext fileContext)
    {
        _fileContext = fileContext;
    }


    public Task<User> CreateAsync(User user)
    {
        int userId = 1;
        if (_fileContext.Users.Any())
        {
            userId = _fileContext.Users.Max(u => u.Id);
            userId++;
        }

        user.Id = userId;
        
        _fileContext.Users.Add(user);
        _fileContext.SaveChanges();

        return Task.FromResult(user);
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        User? existing =
            _fileContext.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(existing);
    }

    public Task<IEnumerable<User>> GetAsync(SearchParametersDTO searchParameters)
    {
        IEnumerable<User> users = _fileContext.Users.AsEnumerable();

        if (searchParameters.UsernameContains != null)
            users = _fileContext.Users.Where(u =>
                u.Username.Contains(searchParameters.UsernameContains, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(users);
    }

    public Task<User?> GetByIdAsync(int id)
    {
        User? existing = _fileContext.Users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(existing);
    }

    public Task<User?> ValidateUser(UserValidationDTO dto)
    {
        User? user = null;
        User? existing = _fileContext.Users.FirstOrDefault(u => u.Username.Equals(dto.Username));

        if (existing != null && existing.Username.Equals(dto.Username) && existing.Password.Equals(dto.Password))
        {
            user = new User
            {
                Id = existing.Id,
                Username = existing.Username,
                Password = existing.Password
            };
        }

        return Task.FromResult(user);
    }
}