using GetBringApiClient.Models;

namespace GetBringApiClient;

public interface IBringClient
{
    User? User { get; set; }

    Task<string> CreateList(string name, string theme = "ch.publisheria.bring.theme.home");
    Task<List<ListItem>> GetListItems(string listId);
    Task<List<ListEnity>> GetLists();
    Task<User> Login(string email, string password);
    Task PurchaseListItem(string listId, string name, string specification);
    Task RecentlyListItem(string listId, string name, string specification);
    Task RemoveListItem(string listId, string name);
    Task ToggleListItem(ListItem listItem);
}