using Newtonsoft.Json;

namespace GetBringApiClient.Models;

public class ListEnity
{
    [JsonProperty("listUuid")]
    public string? Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("theme")]
    public string? Theme { get; set; }
}
