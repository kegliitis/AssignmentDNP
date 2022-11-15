using Application.DAOInterfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDataAccess.DAOs;

public class PostEfcDAO : IPostDAO
{
    private readonly PostContex context;

    public PostEfcDAO(PostContex context)
    {
        this.context = context;
    }

    public async Task<Post> CreateAsync(Post post)
    {
        EntityEntry<Post> added = await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task<IEnumerable<Post>> GetAsync(SearchPostParametersDTO searchParameters)
    {
        IQueryable<Post> query = context.Posts.Include(post => post.Creator).AsQueryable();

        if (!string.IsNullOrEmpty(searchParameters.Username))
            query = query.Where(post => post.Creator.Username.ToLower().Equals(searchParameters.Username.ToLower()));

        if (!string.IsNullOrEmpty(searchParameters.TitleContains))
            query = query.Where(post => post.Title.ToLower().Equals(searchParameters.TitleContains.ToLower()));

        if (!string.IsNullOrEmpty(searchParameters.BodyContains))
            query = query.Where(post => post.Body.ToLower().Equals(searchParameters.BodyContains.ToLower()));

        List<Post> result = await query.ToListAsync();
        return result;
    }

    public async Task UpdateAsync(Post post)
    {
        context.ChangeTracker.Clear();
        context.Posts.Update(post);
        await context.SaveChangesAsync();
    }

    public async Task<Post> GetByIdAsync(int id)
    {
        Post? found = await context.Posts.Include(p => p.Creator).FirstOrDefaultAsync(p => p.Id == id);
        return found;
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await GetByIdAsync(id);
        if (existing == null)
            throw new Exception($"Post with ID {id} was not found");
        context.Posts.Remove(existing);
        await context.SaveChangesAsync();
    }
}