using Application.DAOInterfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDataAccess.DAOs;

public class UserEfcDAO : IUserDAO
{
    private readonly PostContex context;

    public UserEfcDAO(PostContex context)
    {
        this.context = context;
    }

    public async Task<User> CreateAsync(User user)
    {
        EntityEntry<User> newUser = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return newUser.Entity;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        User? existing = await context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));
        return existing;
    }

    public async Task<IEnumerable<User>> GetAsync(SearchParametersDTO searchParameters)
    {
        IQueryable<User> usersQuery = context.Users.AsQueryable();

        if (searchParameters.UsernameContains != null)
            usersQuery = usersQuery.Where(u =>
                u.Username.ToLower().Contains(searchParameters.UsernameContains.ToLower()));
        IEnumerable<User> result = await usersQuery.ToListAsync();
        return result;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        User? user = await context.Users.FindAsync(id);
        return user;
    }

    public async Task<User?> ValidateUser(UserValidationDTO dto)
    {
        User? user = null;
        User? validation = await context.Users.FirstOrDefaultAsync(u => u.Username.Equals(dto.Username));

        if (validation != null && validation.Username.Equals(dto.Username) && validation.Password.Equals(dto.Password))
        {
            user = new User
            {
                Id = validation.Id,
                Username = validation.Username,
                Password = validation.Password
            };
        }
        return await Task.FromResult(user);
    }
}