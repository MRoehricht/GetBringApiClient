using GetBringApiClient.Exceptions;
using GetBringApiClient.Models;
using GetBringApiClient.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GetBringApiClient;

public class BringClient : IBringClient
{
    private readonly HttpClient _httpClient;
    public User? User { get; set; }

    public BringClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.getbring.com/rest/v2/")
        };

        _httpClient.DefaultRequestHeaders.Add("X-BRING-API-KEY", "cof4Nc6D8saplXjE3h3HXqHH8m7VU2i1Gs0g85Sp");
        _httpClient.DefaultRequestHeaders.Add("X-BRING-CLIENT", "webApp");
        _httpClient.DefaultRequestHeaders.Add("X-BRING-COUNTRY", "DE");
    }

    public async Task<User> Login(string email, string password)
    {
        var postParams = new Dictionary<string, string> { { "email", email }, { "password", password } };

        var response = await _httpClient.PostAsync("bringauth", new FormUrlEncodedContent(postParams));

        if (!response.IsSuccessStatusCode)
            throw new ResponseException(Constants.ErrorLogin);

        var responseBody = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(responseBody))
            throw new ResponseException(Constants.ErrorLogin);

        User = JsonConvert.DeserializeObject<User>(responseBody) ?? throw new ResponseException(Constants.ErrorLogin);
        _httpClient.DefaultRequestHeaders.Add("X-BRING-USER-UUID", User.Id);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {User.AccessToken}");
        return User;
    }

    public async Task<List<ListEnity>> GetLists()
    {
        if (User is null) throw new LoginException();
        var response = await _httpClient.GetAsync($"bringusers/{User.Id}/lists");
        if (!response.IsSuccessStatusCode)
            throw new ResponseException(Constants.ErrorLoadingLists);

        var responseBody = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(responseBody))
            throw new ResponseException(Constants.ErrorLoadingLists);

        var getListsResponse = JsonConvert.DeserializeObject<GetListsResponse>(responseBody) ?? throw new ResponseException(Constants.ErrorLoadingLists);

        return getListsResponse.ListEnities;
    }

    public async Task<string> CreateList(string name, string theme = "ch.publisheria.bring.theme.home")
    {
        if (User is null) throw new LoginException();

        var postParams = new Dictionary<string, string> { { "name", name }, { "theme", theme } }; ;

        var response = await _httpClient.PostAsync($"bringusers/{User.Id}/lists", new FormUrlEncodedContent(postParams));
        if (!response.IsSuccessStatusCode)
            throw new ResponseException(Constants.ErrorCreatingList);

        var responseBody = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(responseBody))
            throw new ResponseException(Constants.ErrorCreatingList);


        dynamic jsonObject = JObject.Parse(responseBody);

        string bringListUUID = jsonObject.bringListUUID;
        return bringListUUID;
    }


    public async Task<List<ListItem>> GetListItems(string listId)
    {
        if (User is null) throw new LoginException();
        var response = await _httpClient.GetAsync($"bringlists/{listId}");
        if (!response.IsSuccessStatusCode)
            throw new ResponseException(Constants.ErrorLoadingItems);

        var responseBody = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(responseBody))
            throw new ResponseException(Constants.ErrorLoadingItems);

        var getListItemsResponse = JsonConvert.DeserializeObject<GetListItemsResponse>(responseBody) ?? throw new ResponseException(Constants.ErrorLoadingItems);

        var listItems = new List<ListItem>();

        getListItemsResponse.Purchase.ForEach(_ => { _.IsPurchased = true; _.ListId = listId; });
        getListItemsResponse.Recently.ForEach(_ => { _.IsPurchased = false; _.ListId = listId; });
        listItems.AddRange(getListItemsResponse.Purchase);
        listItems.AddRange(getListItemsResponse.Recently);


        return listItems;
    }

    public async Task PurchaseListItem(string listId, string name, string specification)
    {
        var putParameters = new Dictionary<string, string> { { "uuid", listId }, { "purchase", name }, { "specification", specification } };
        await PutListItem(listId, putParameters);
    }

    public async Task RecentlyListItem(string listId, string name, string specification)
    {
        var putParameters = new Dictionary<string, string> { { "uuid", listId }, { "recently", name }, { "specification", specification } };
        await PutListItem(listId, putParameters);
    }

    public async Task RemoveListItem(string listId, string name)
    {
        var putParameters = new Dictionary<string, string> { { "uuid", listId }, { "remove", name } };
        await PutListItem(listId, putParameters);
    }

    public async Task ToggleListItem(ListItem listItem)
    {
        ArgumentNullException.ThrowIfNull(listItem);
        ArgumentNullException.ThrowIfNull(listItem.ListId);
        ArgumentNullException.ThrowIfNull(listItem.Name);
        ArgumentNullException.ThrowIfNull(listItem.Specification);

        var destination = listItem.IsPurchased ? "recently" : "purchase";
        var putParameters = new Dictionary<string, string> { { "uuid", listItem.ListId }, { destination, listItem.Name }, { "specification", listItem.Specification } };
        await PutListItem(listItem.ListId, putParameters);
        listItem.IsPurchased = !listItem.IsPurchased;
    }

    private async Task PutListItem(string listId, Dictionary<string, string> putParameters)
    {
        if (User is null) throw new LoginException();

        var response = await _httpClient.PutAsync($"bringlists/{listId}", new FormUrlEncodedContent(putParameters));
        if (!response.IsSuccessStatusCode)
            throw new ResponseException(Constants.ErrorChangingList);
    }
}
