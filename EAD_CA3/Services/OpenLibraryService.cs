
using EAD_CA3.Pages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace EAD_CA3.Services
{
    public class OpenLibraryService
    {
        private readonly HttpClient _httpClient;

        public OpenLibraryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OpenLibraryResponse?> SearchBooksAsync(string query, int page = 1)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<OpenLibraryResponse>($"search.json?q={query}&page={page}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from OpenLibrary API: {ex.Message}");
                return null;
            }
        }
        public async Task<BookDetails?> GetBookDetailsAsync(string key)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<BookDetails>($"works/{key}.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching book details: {ex.Message}");
                return null;
            }
        }

        public async Task<AuthorDetails?> GetAuthorDetailsAsync(string key)
        {
            try
            {
                var url = $"authors/{key}.json";
                Console.WriteLine($"Fetching author details from: {url}");
                return await _httpClient.GetFromJsonAsync<AuthorDetails>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching author details: {ex.Message}");
                return null;
            }
        }


    }
    public class AuthorDetails
    {
        public string Name { get; set; } = string.Empty;
    }
    public class BookDetails
    {
        [JsonConverter(typeof(DescriptionConverter))]
        public string? Description { get; set; }
        public string[]? Subjects { get; set; }
        public string Title { get; set; } = string.Empty;

        public List<AuthorRole>? Authors { get; set; }
        public List<int>? Covers { get; set; }
    }

    public class AuthorRole
    {
        [JsonPropertyName("author")]
        public AuthorReference? Author { get; set; }
    }

    public class AuthorReference
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;
    }



    public class OpenLibraryResponse
    {
        public int? NumFound { get; set; } = 0;
        public Doc[]? Docs { get; set; } = Array.Empty<Doc>();
    }

    public class Doc
    {
        public string? Title { get; set; } = string.Empty;
        public string[]? AuthorName { get; set; } = Array.Empty<string>();
        public int? FirstPublishYear { get; set; }
        public string? Key { get; set; } = string.Empty; 
    }



}