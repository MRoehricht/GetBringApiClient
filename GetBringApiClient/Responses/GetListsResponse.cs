using GetBringApiClient.Models;
using Newtonsoft.Json;

namespace GetBringApiClient.Responses;

internal class GetListsResponse
{
    [JsonProperty("lists")]
    public List<ListEnity> ListEnities { get; set; } = new List<ListEnity>();
}
