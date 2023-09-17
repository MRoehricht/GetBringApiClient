using Newtonsoft.Json;

namespace GetBringApiClient.Models;

public class ListItem
{
    public string? ListId { get; set; }

    [JsonProperty("specification")]
    public string? Specification { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    public bool IsPurchased { get; set; }
}
