using Application.DAOInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace FileData.DAOs;

public class PostFileDAO : IPostDAO
{
    private readonly FileContext _fileContext;

    public PostFileDAO(FileContext fileContext)
    {
        _fileContext = fileContext;
    }
    public Task<Post> CreateAsync(Post post)
    {
        int id = 1;
        if (_fileContext.Posts.Any())
        {
            id = _fileContext.Posts.Max(p => p.Id);
            id++;
        }

        post.Id = id;
        
        _fileContext.Posts.Add(post);
        
        _fileContext.SaveChanges();

        return Task.FromResult(post);
    }

    public Task<IEnumerable<Post>> GetAsync(SearchPostParametersDTO searchParameters)
    {
        IEnumerable<Post> result = _fileContext.Posts.AsEnumerable();

        if (!string.IsNullOrEmpty(searchParameters.Username))
            result = _fileContext.Posts.Where(post =>
                post.Creator.Username.Equals(searchParameters.Username, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(searchParameters.TitleContains))
            result = result.Where(post =>
                post.Title.Contains(searchParameters.TitleContains, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrEmpty(searchParameters.BodyContains))
            result = result.Where(post =>
                post.Body.Contains(searchParameters.BodyContains, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(result);
    }

    public Task UpdateAsync(Post postToUpdate)
    {
        Post? existing = _fileContext.Posts.FirstOrDefault(post => post.Id == postToUpdate.Id);
        if (existing == null)
            throw new Exception($"Post with ID {postToUpdate.Id} does not exist!");

        _fileContext.Posts.Remove(existing);
        _fileContext.Posts.Add(postToUpdate);
        
        _fileContext.SaveChanges();

        return Task.CompletedTask;
    }

    public Task<Post> GetByIdAsync(int id)
    {
        Post? existing = _fileContext.Posts.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(existing);
    }

    public Task DeleteAsync(int id)
    {
        Post? existing = _fileContext.Posts.FirstOrDefault(post => post.Id == id);
        if (existing == null)
            throw new Exception($"Post with ID {id} does not exist!");

        _fileContext.Posts.Remove(existing);
        _fileContext.SaveChanges();
        
        return Task.CompletedTask;
    }
}