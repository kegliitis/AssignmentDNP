using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Domain.DTOs;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations;

public class PostHttpClient : IPostService
{
    private readonly HttpClient _client;

    public PostHttpClient(HttpClient client)
    {
        _client = client;
    }

    public async Task CreateAsync(PostCreationDTO dto)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("/posts", dto);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    public async Task<ICollection<Post>> GetAsync(string? username, string? title, string? body)
    {
        string query = ConstructQuery(username, title, body);
        HttpResponseMessage response = await _client.GetAsync("/posts" + query);
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        ICollection<Post> posts = JsonSerializer.Deserialize<ICollection<Post>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return posts;
    }

    public async Task<PostBasicDTO> GetByIdAsync(int id)
    {
        HttpResponseMessage response = await _client.GetAsync($"/posts/{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        PostBasicDTO post = JsonSerializer.Deserialize<PostBasicDTO>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return post;
    }

    public async Task UpdateAsync(PostUpdateDTO dto)
    {
        string dtoAsJson = JsonSerializer.Serialize(dto);
        StringContent body = new StringContent(dtoAsJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PatchAsync("/posts", body);

        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage response = await _client.DeleteAsync($"Posts/{id}");
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    private static string ConstructQuery(string? username,string? titleContains, string? bodyContains)
    {
        string query = "";
        if (!string.IsNullOrEmpty(username))
        {
            query += $"?username={username}";
        }
        
        if (!string.IsNullOrEmpty(titleContains))
        {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"titlecontains={titleContains}";
        }
        
        if (!string.IsNullOrEmpty(bodyContains))
        {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"bodycontains={bodyContains}";
        }

        return query;
    }
}