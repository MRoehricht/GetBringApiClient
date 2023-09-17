using Newtonsoft.Json;

namespace GetBringApiClient.Models;

public class User
{
    [JsonProperty("uuid")]
    public string? Id { get; set; }

    [JsonProperty("publicUuid")]
    public string? PublicUuid { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("photoPath")]
    public string? PhotoPath { get; set; }

    [JsonProperty("bringListUUID")]
    public string? BringListUuid { get; set; }

    [JsonProperty("access_token")]
    public string? AccessToken { get; set; }

    [JsonProperty("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonProperty("token_type")]
    public string? TokenType { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
}


