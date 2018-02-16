using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheLastHero.Models;

[assembly: Xamarin.Forms.Dependency(typeof(TheLastHero.Services.MockDataStore))]
namespace TheLastHero.Services
{

    public class MockDataStore : IDataStore<Item>
    {
        const string HEAD = "HEAD";
        const string BODY = "BODY";
        const string FEET = "FEET";
        const string LEFTHAND = "LEFTHAND";
        const string RIGHTHAND = "RIGHTHAND";
        const string LEFTFINGER = "LEFTFINGER";
        const string RIGHTFINGER = "RIGHTFINGER";

        List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>();
            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Name = "Bulletproof Helmet", HP=0, MP=5, Type = HEAD, Lvl=3, Atk=0,Def=10, Spd=1, Luk=1,SpecialAbility="", EquippableLocation="", EquippedBy=""},
                new Item { Id = Guid.NewGuid().ToString(), Name = "Victoria's Secret", HP=2, MP=20, Type = BODY, Lvl=1, Atk=0,Def=5, Spd=1, Luk=1,SpecialAbility="", EquippableLocation="", EquippedBy=""},
                new Item { Id = Guid.NewGuid().ToString(), Name = "AJ9", HP=10, MP=5, Type = FEET, Lvl=1, Atk=0,Def=2, Spd=5, Luk=1,SpecialAbility="", EquippableLocation="", EquippedBy=""},
                new Item { Id = Guid.NewGuid().ToString(), Name = "Santoku", HP=0, MP=0, Type = LEFTHAND, Lvl=1, Atk=100,Def=2, Spd=5, Luk=1,SpecialAbility="", EquippableLocation="", EquippedBy=""},
                new Item { Id = Guid.NewGuid().ToString(), Name = "M9", HP=0, MP=0, Type = RIGHTHAND, Lvl=1, Atk=30,Def=2, Spd=1, Luk=1,SpecialAbility="", EquippableLocation="", EquippedBy=""},
                new Item { Id = Guid.NewGuid().ToString(), Name = "NBA Champion Ring", HP=0, MP=5, Type = LEFTFINGER, Lvl=1, Atk=2,Def=5, Spd=1, Luk=1,SpecialAbility="", EquippableLocation="", EquippedBy=""},
                new Item { Id = Guid.NewGuid().ToString(), Name = "Chipotle Ring", HP=0, MP=5, Type = RIGHTFINGER, Lvl=1, Atk=1,Def=2, Spd=1, Luk=1,SpecialAbility="", EquippableLocation="", EquippedBy=""}, };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Item item)
        {
            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}