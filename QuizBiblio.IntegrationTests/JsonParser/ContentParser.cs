using System.Text.Json;

namespace QuizBiblio.IntegrationTests.JsonParser;

public static class ContentParser
{
    public readonly static JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };

    public static async Task<T?> GetRequestContent<T>(HttpResponseMessage responseMessage) => 
        JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), SerializerOptions);
}
