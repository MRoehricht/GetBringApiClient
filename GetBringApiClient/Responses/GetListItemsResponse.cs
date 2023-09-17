using GetBringApiClient.Models;
using Newtonsoft.Json;

namespace GetBringApiClient.Responses;

internal class GetListItemsResponse
{
    [JsonProperty("uuid")]
    public required string ListId { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("purchase")]
    public List<ListItem> Purchase { get; set; } = new List<ListItem>();

    [JsonProperty("recently")]
    public List<ListItem> Recently { get; set; } = new List<ListItem>();
}
