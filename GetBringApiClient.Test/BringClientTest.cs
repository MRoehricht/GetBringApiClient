using GetBringApiClient.Models;
using NUnit.Framework;

namespace GetBringApiClient.Test;

public class BringClientTest
{
    private BringClient _bringClient;

    [SetUp]
    public async Task Setup()
    {
        _bringClient = new BringClient();
        await _bringClient.Login("", "");

        await Console.Out.WriteLineAsync(_bringClient?.User?.AccessToken);
    }

    [Test]
    public void LoginTest()
    {
        Assert.That(_bringClient.User, Is.Not.Null);
        Assert.Pass();
    }

    [Test]
    public async Task GetListsTest()
    {
        var lists = await _bringClient.GetLists();
        Assert.That(lists, Is.Not.Null);
        Assert.That(lists, Is.Not.Empty);
    }

    [Test]
    public async Task GetListItemsTest()
    {
        var listItems = await _bringClient.GetListItems("00000000-0000-0000-0000-000000000000");
        Assert.That(listItems, Is.Not.Null);
        Assert.That(listItems, Is.Not.Empty);
    }

    [Test]
    public async Task PurchaseListItemTest()
    {
        await _bringClient.PurchaseListItem("00000000-0000-0000-0000-000000000000", "Bananen", "5");
    }

    [Test]
    public async Task RecentlyListItemTest()
    {
        await _bringClient.RemoveListItem("00000000-0000-0000-0000-000000000000", "Karotten");

        await _bringClient.RecentlyListItem("00000000-0000-0000-0000-000000000000", "Karotten", "35");
    }

    [Test]
    public async Task ToggleListItemTest()
    {
        var listItem = new ListItem { Name = "Karotten", IsPurchased = true, ListId = "00000000-0000-0000-0000-000000000000", Specification = "12" };

        await _bringClient.ToggleListItem(listItem);

        Assert.That(listItem.IsPurchased, Is.False);
    }

    [Test]
    public async Task CreateListTest()
    {
        var listsBefor = await _bringClient.GetLists();

        var id = await _bringClient.CreateList("TestList");

        Assert.That(id, Is.Not.Null);

        var listsAfter = await _bringClient.GetLists();

        Assert.That(listsAfter, Has.Count.Not.EqualTo(listsBefor.Count));

    }
}