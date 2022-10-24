using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace Application.Logic;

public class PostLogic : IPostLogic
{
    private readonly IPostDAO _postDao;
    private readonly IUserDAO _userDao;

    public PostLogic(IPostDAO postDao, IUserDAO userDao)
    {
        _postDao = postDao;
        _userDao = userDao;
    }

    public async Task<Post> CreateAsync(PostCreationDTO dto)
    {
        User? user = await _userDao.GetByIdAsync(dto.OwnerId);
        if (user == null)
            throw new Exception($"User with ID {dto.OwnerId} was not found.");

        ValidatePost(dto);

        Post post = new Post(user, dto.Title, dto.Body);
        Post created = await _postDao.CreateAsync(post);
        return created;
    }

    public Task<IEnumerable<Post>> GetAsync(SearchPostParametersDTO searchParameters)
    {
        return _postDao.GetAsync(searchParameters);
    }

    public async Task UpdateAsync(PostUpdateDTO dto)
    {
        Post? existing = await _postDao.GetByIdAsync(dto.Id);

        if (existing == null)
            throw new Exception($"Post with ID {dto.Id} not found!");

        User? user = null;
        if (dto.Id != null)
        {
            user = await _userDao.GetByIdAsync((int)dto.Id);
            if (user == null)
                throw new Exception($"User with ID {dto.Id} was not found!");
        }

        User userToUse = user ?? existing.Creator;
        string titleToUse = dto.Title ?? existing.Title;
        string bodyToUse = dto.Body ?? existing.Body;

        Post updated = new(userToUse, titleToUse, bodyToUse)
        {
            Id = existing.Id
        };
        
        ValidatePost(updated);

        await _postDao.UpdateAsync(updated);
    }

    public async Task DeleteAsync(int id)
    {
        Post? post = await _postDao.GetByIdAsync(id);
        if (post == null)
            throw new Exception($"Post with ID {id} was not found!");

        await _postDao.DeleteAsync(id);
    }

    public async Task<PostBasicDTO> GetByIdAsync(int id)
    {
        Post? post = await _postDao.GetByIdAsync(id);
        if (post == null)
            throw new Exception($"Post with ID {id} not found!");
        return new PostBasicDTO(post.Id, post.Creator.Username, post.Title, post.Body);
    }

    private void ValidatePost(PostCreationDTO dto)
    {
        if (string.IsNullOrEmpty(dto.Title))
            throw new Exception("Title cannot be empty!");
        if (string.IsNullOrEmpty(dto.Body))
            throw new Exception("Body cannot be empty!");
    }
    
    private void ValidatePost(Post post)
    {
        if (string.IsNullOrEmpty(post.Title)) 
            throw new Exception("Title cannot be empty!");
        if (string.IsNullOrEmpty(post.Body))
            throw new Exception("Post cannot be empty!");
    }
}